using ClosedXML.Excel;
using Google.Ads.GoogleAds.V17.Resources;
using Google.Api.Gax.ResourceNames;
using Grpc.Core;
using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
using HoangZoho1.Models.WclSolutions;
using HoangZoho1.Models.ZoRaw.Custom;
using HoangZoho1.Models.ZoRaw.Freightcom;
using HoangZoho1.Models.ZoRaw.SalesData;
using HoangZoho1.Models.ZoRaw.Stallion;
using HoangZoho1.Models.ZoRaw.ZohoCRM;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using HoangZoho1.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Compression.Zip;
using Syncfusion.DocIO.DLS;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public class ZoRawCustomService : IZoRawCustomService
    {

        private readonly IZoRawStallionService _zoRawStallionService;
        private readonly IZoRawFreightcomService _zoRawFreightcomService;
        private readonly IZoRawInventoryService _zoRawInventoryService;
        private readonly IZoRawCrmService _zoRawCrmService;
        private readonly ICommonService _commonService;

        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public ZoRawCustomService(IZoRawStallionService zoRawStallionService,
            IZoRawFreightcomService zoRawFreightcomService,
            IZoRawInventoryService zoRawInventoryService,
            IZoRawCrmService zoRawCrmService = null,
            ICommonService commonService = null)
        {
            _zoRawStallionService = zoRawStallionService;
            _zoRawFreightcomService = zoRawFreightcomService;
            _zoRawInventoryService = zoRawInventoryService;
            _zoRawCrmService = zoRawCrmService;
            _commonService = commonService;
        }

        public List<TransformedRecord> ReadExcelToTransformedRecords(
            string inputFile,
            ExcelMapping mapping,
            bool exportExcel = false,
            string outputPath = null)
        {
            var workbook = new XLWorkbook(inputFile);
            var ws = workbook.Worksheets.First();

            int lastCol = ws.LastColumnUsed().ColumnNumber();
            int lastRow = ws.LastRowUsed().RowNumber();

            // 1. Read fixed headers from FixedHeaderRow
            int fixedColCount = mapping.FixedColumns.Count;

            // 2. Read month-year headers from DynamicHeaderRow
            var dynamicColumns = new List<(int QtyColIndex, int AmountColIndex, string MonthName)>();
            for (int c = fixedColCount + 1; c <= lastCol; c += 2)
            {
                var monthCell = ws.Row(mapping.DynamicHeaderRow).Cell(c);
                string monthName;
                if (monthCell.DataType == XLDataType.DateTime)
                    monthName = monthCell.GetDateTime().ToString("MMM yyyy");
                else
                    monthName = monthCell.GetValue<string>().Trim();

                dynamicColumns.Add((QtyColIndex: c, AmountColIndex: c + 1, MonthName: monthName));
            }

            var transformedRecords = new List<TransformedRecord>();

            // 3. Loop through data rows
            for (int r = mapping.StartRow; r <= lastRow; r++)
            {
                var row = ws.Row(r);

                // Skip row if the ignore column matches ignore values
                var cellValue = row.Cell(mapping.IgnoreColumnIndex).GetValue<string>().Trim();
                if (mapping.IgnoreValues.Any(v => cellValue.Equals(v, StringComparison.OrdinalIgnoreCase)))
                    continue;

                foreach (var dyn in dynamicColumns)
                {
                    var qtyStr = row.Cell(dyn.QtyColIndex).GetValue<string>();
                    var amtStr = row.Cell(dyn.AmountColIndex).GetValue<string>();

                    // Convert to decimal for checking zero
                    decimal qty = StringHelpers.ParseCurrency(qtyStr);
                    decimal amt = StringHelpers.ParseCurrency(amtStr);

                    // Skip if both Qty and Amount are zero
                    if (qty == 0 && amt == 0)
                        continue;

                    var record = new TransformedRecord
                    {
                        MonthYear = dyn.MonthName,
                        Qty = qtyStr,
                        Amount = amtStr
                    };

                    // Add fixed fields
                    int colIndex = 1;
                    foreach (var kvp in mapping.FixedColumns)
                    {
                        record.FixedFields[kvp.Key] = row.Cell(colIndex).GetValue<string>();
                        colIndex++;
                    }

                    transformedRecords.Add(record);
                }
            }

            // 4. Optional: Export to Excel
            if (exportExcel && !string.IsNullOrEmpty(outputPath))
            {
                ExportTransformedRecordsToExcel(transformedRecords, outputPath);
            }

            return transformedRecords;
        }

        public static void ExportTransformedRecordsToExcel(List<TransformedRecord> records, string outputFile)
        {
            if (records == null || !records.Any())
                return;

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Data");

            int colIndex = 1;

            // Fixed headers
            foreach (var key in records.First().FixedFields.Keys)
            {
                ws.Cell(1, colIndex).Value = key;
                colIndex++;
            }

            // Dynamic headers
            ws.Cell(1, colIndex++).Value = "MonthYear";
            ws.Cell(1, colIndex++).Value = "Qty";
            ws.Cell(1, colIndex++).Value = "Amount";

            // Data rows
            int rowIndex = 2;
            foreach (var rec in records)
            {
                colIndex = 1;
                foreach (var value in rec.FixedFields.Values)
                    ws.Cell(rowIndex, colIndex++).Value = value;

                ws.Cell(rowIndex, colIndex++).Value = rec.MonthYear;
                ws.Cell(rowIndex, colIndex++).Value = rec.Qty;
                ws.Cell(rowIndex, colIndex++).Value = rec.Amount;

                rowIndex++;
            }

            ws.Columns().AdjustToContents();
            workbook.SaveAs(outputFile);
        }


        #region APIs for Zoho Inventory Widget

        public async Task<ApiResultDto<List<CommonShipmentRate>>> GetRatesFromWidget
            (GetRatesFromWidgetRequest request)
        {

            var apiResult = new ApiResultDto<List<CommonShipmentRate>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GRFW_400
            };

            try
            {

                var getStallionRateRequest = new GetStallionRatesRequest();
                var stallionItems = new List<StallionRateItem>();
                var locationDict = new Dictionary<string, Location>();

                // STEP 2: Get data from request body
                string soId = request.SalesOrderId;
                string packageId = request.PackageId;

                var rateBoxes = request.Boxes;

                decimal length = 0;
                decimal width = 0;
                decimal height = 0;
                decimal weight = 0;

                if (rateBoxes.Count == 1)
                {

                    var firstBox = rateBoxes[0];
                    length = firstBox.Length.Value;
                    width = firstBox.Width.Value;
                    height = firstBox.Height.Value;
                    weight = firstBox.Weight.Value;

                }

                string expectedShipDateStr = request.ExpectedShipDate;
                string readyAtHourStr = request.ReadyAtHour;
                string readyAtMinuteStr = request.ReadyAtMinute;
                string readyUntilHourStr = request.ReadyUntilHour;
                string readyUntilMinuteStr = request.ReadyUntilMinute;
                bool residentialAddress = request.ResidentialAddress.HasValue ? request.ResidentialAddress.Value : false;

                string attention = string.Empty;
                string toAddressLine1 = string.Empty;
                string toAddressLine2 = string.Empty;
                string toCity = string.Empty;
                string toState = string.Empty;
                string toProvinceCode = string.Empty;
                string toPostalCode = string.Empty;
                string toCountry = string.Empty;
                string toCountryCode = string.Empty;
                string toPhone = string.Empty;
                string toPhoneExtension = string.Empty;
                string toEmail = string.Empty;

                // STEP 3: Get Sales Order by Id
                var getSalesOrderByIdResponse = await _zoRawInventoryService
                    .GetSalesOrderById(soId);

                var salesOrderDetails = getSalesOrderByIdResponse.Data.salesorder;

                string salesChannel = salesOrderDetails.sales_channel;
                var contactPersonDetails = salesOrderDetails.contact_person_details;
                var firstContactPerson = contactPersonDetails != null && contactPersonDetails.Length > 0 ? contactPersonDetails[0] : null;

                string firstContactPersonFullName = firstContactPerson != null ? $"{firstContactPerson.first_name} {firstContactPerson.last_name}" : string.Empty;
                string currencyCode = salesOrderDetails.currency_code;
                var soLineItems = salesOrderDetails.line_items;

                var locationDetails = new Location();
                string locationName = string.Empty;
                string locationContactName = string.Empty;
                var locationAddress = new LocationAddress();
                string locationAttention = string.Empty;
                string locationAddress1 = string.Empty;
                string locationAddress2 = string.Empty;
                string locationCity = string.Empty;
                string locationState = string.Empty;
                string locationStateCode = string.Empty;
                string locationPostalCode = string.Empty;
                string locationCountry = string.Empty;
                string locationCountryCode = string.Empty;
                string locationPhone = string.Empty;
                string locationEmail = string.Empty;

                if (string.IsNullOrEmpty(packageId))
                {
                    apiResult.Message = ZoRawConstants.GRFW_E02;
                    return apiResult;
                }

                // STEP 4: Get Package by Id
                var getPackageByIdResponse = await _zoRawInventoryService
                    .GetPackageById(packageId);
                var packageDetails = getPackageByIdResponse.Data.package;

                // Step 4.2: Extract To Location
                var shippingAddress = packageDetails.shipping_address;
                attention = shippingAddress.attention;
                string soCustomer = packageDetails.customer_name;
                if (!string.IsNullOrEmpty(attention))
                {
                    attention = soCustomer;
                }
                toAddressLine1 = shippingAddress.address;
                toAddressLine2 = shippingAddress.street2;
                toCity = shippingAddress.city;
                string state = shippingAddress.state;
                string stateCode = string.Empty;
                string country = shippingAddress.country;
                string countryCode = string.Empty;
                if (country.Equals("Canada",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    stateCode = AddressHelpers.GetCanadaProvinceCode(state);
                    countryCode = "CA";
                }
                else if (country.Contains("US", StringComparison.InvariantCultureIgnoreCase)
                    || country.Contains("United State", StringComparison.InvariantCultureIgnoreCase)
                    || country.Contains("U.S.A", StringComparison.InvariantCultureIgnoreCase))
                {
                    stateCode = AddressHelpers.GetUSStateCode(state);
                    countryCode = "US";
                }    
                toProvinceCode = stateCode;
                toPostalCode = shippingAddress.zip;
                toCountryCode = countryCode;
                toPhone = shippingAddress.phone;

                // Step 4.3: Handle To Phone in case it's EMPTY
                if (string.IsNullOrEmpty(toPhone))
                {
                    toPhone = firstContactPerson.phone;
                    var phoneResult = StringHelpers.ExtractPhoneAndExtension(toPhone);
                    toPhone = phoneResult.PhoneNumber;
                    toPhoneExtension = phoneResult.Extension;
                }

                if (string.IsNullOrEmpty(toEmail))
                {
                    toEmail = firstContactPerson.email;
                }    

                // Step 4.4: Handle Line Items
                string soLocationId = "";
                var packageLineItems = packageDetails.line_items;
                foreach (var item in packageLineItems)
                {
                    string lineItemId = item.line_item_id;
                    string soLineItemId = item.so_line_item_id;

                    string itemName = item.name;
                    // Handle Line Item Rate
                    decimal itemRate = 0;
                    foreach (var soItem in soLineItems)
                    {
                        if (soItem.line_item_id.Equals(soLineItemId))
                        {
                            itemRate = soItem.rate.Value;
                            break;
                        }
                    }
                    string itemSku = item.sku;
                    string itemDescription = item.description;
                    decimal quantity = item.quantity.Value;
                    soLocationId = item.location_id;

                    string manufacturerAddress = locationAddress1;
                    if (!string.IsNullOrEmpty(locationAddress2))
                    {
                        manufacturerAddress += $", {locationAddress2}";
                    }

                    if (!locationDict.ContainsKey(soLocationId))
                    {
                        var getLocationByIdResponse = await 
                            _zoRawInventoryService.GetLocationById(soLocationId);
                        locationDetails = getLocationByIdResponse.Data.location;
                        locationDict.Add(soLocationId, locationDetails);
                    }
                    else
                    {
                        locationDetails = locationDict[soLocationId];
                    }   
                    
                    locationName = locationDetails.location_name;
                    locationContactName = locationDetails.contact_name;
                    locationPhone = locationDetails.phone;
                    locationEmail = locationDetails.email;
                    locationAddress = locationDetails.address;
                    locationAttention = locationAddress.attention;
                    locationAddress1 = locationAddress.street_address1;
                    locationAddress2 = locationAddress.street_address2;
                    locationCity = locationAddress.city;
                    locationState = locationAddress.state;
                    locationStateCode = locationAddress.state_code;
                    locationPostalCode = locationAddress.postal_code;
                    locationCountry = locationAddress.country;
                    if (locationCountry.Equals("Canada",
                    StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(locationStateCode))
                        {
                            locationStateCode = AddressHelpers.GetCanadaProvinceCode(locationState);
                        }
                        locationCountryCode = "CA";
                    }
                    else if (locationCountry.Contains("US", StringComparison.InvariantCultureIgnoreCase)
                        || locationCountry.Contains("United State", StringComparison.InvariantCultureIgnoreCase)
                        || locationCountry.Contains("U.S.A", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(locationStateCode))
                        {
                            locationStateCode = AddressHelpers.GetUSStateCode(locationState);
                        }
                        locationCountryCode = "US";
                    }

                    var stallionItem = new StallionRateItem();

                    stallionItem.description = itemName;
                    //stallionItem.sku = itemSku;
                    stallionItem.value = itemRate;
                    stallionItem.quantity = quantity;
                    stallionItem.currency = currencyCode;
                    //stallionItem.manufacturer_name = locationName;
                    //stallionItem.manufacturer_address1 = manufacturerAddress;
                    //stallionItem.manufacturer_city = locationCity;
                    //stallionItem.manufacturer_province_code = locationStateCode;
                    //stallionItem.manufacturer_postal_code = locationPostalCode;
                    //stallionItem.manufacturer_country_code = locationCountryCode;
                    //stallionItem.country_of_origin = locationCountryCode;
                    stallionItems.Add(stallionItem);

                }

                // STEP 5: Handle Return Address
                var returnAddress = new Return_Address()
                {
                    name = locationName,
                    address1 = locationAddress1,
                    address2 = locationAddress2,
                    city = locationCity,
                    province_code = locationStateCode,
                    postal_code = locationPostalCode,
                    country_code = locationCountryCode,
                    phone = locationPhone,
                    email = locationEmail
                };
                getStallionRateRequest.return_address = returnAddress;

                var allCommonRates = new List<CommonShipmentRate>();

                if (salesChannel.Equals("shopify", StringComparison.InvariantCultureIgnoreCase) && rateBoxes.Count == 1)
                {
                    // STEP 6.1: Handle Get Stallion Rate Request
                    getStallionRateRequest.is_return = false;
                    getStallionRateRequest.weight_unit = "kg";
                    getStallionRateRequest.weight = weight;
                    getStallionRateRequest.size_unit = "in";
                    getStallionRateRequest.length = length;
                    getStallionRateRequest.width = width;
                    getStallionRateRequest.height = height;
                    getStallionRateRequest.package_type = "Parcel";

                    var toAddress = new To_Address()
                    {
                        name = attention,
                        address1 = toAddressLine1,
                        address2 = toAddressLine2,
                        city = toCity,
                        province_code = toProvinceCode,
                        postal_code = toPostalCode,
                        country_code = toCountryCode,
                        phone = toPhone,
                    };
                    getStallionRateRequest.to_address = toAddress;
                    getStallionRateRequest.items = stallionItems;

                    // STEP 6.2: Call Stallion Get Rates API
                    var getStallionRatesResponse = await _zoRawStallionService
                        .GetRates(getStallionRateRequest);

                    if (getStallionRatesResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZoRawConstants.GRFW_E01;
                        return apiResult;
                    }

                    var stallionRates = getStallionRatesResponse.Data.rates;
                    foreach (var stallionRate in stallionRates)
                    {
                        var commonRate = ConvertHelpers.Stallion_Convert2CommonRate(stallionRate);
                        commonRate.GetRatesDetails = getStallionRateRequest;
                        allCommonRates.Add(commonRate);
                    }
                }

                // STEP 7.1: Handle Get Freightcom Rate Request
                var requestRateEstimateRequest = new RequestRateEstimateRequest();
                var freightcomShipmentDetails = new FreightcomShipmentDetails();

                // Step 7.1.1: Handle Freightcom Origin
                var freightcomOrigin = new FreightcomOrigin
                {
                    name = locationName
                };
                freightcomOrigin.contact_name = "Gigi Gill";
                var originAddress = new FreightcomAddress
                {
                    address_line_1 = locationAddress1,
                    address_line_2 = locationAddress2,
                    city = locationCity,
                    region = locationStateCode,
                    postal_code = locationPostalCode,
                    country = locationCountryCode
                };
                freightcomOrigin.address = originAddress;
                freightcomOrigin.tailgate_required = false;
                if (!string.IsNullOrEmpty(locationContactName))
                {
                    freightcomOrigin.contact_name = locationContactName;
                }
                if (!string.IsNullOrEmpty(locationPhone))
                {
                    freightcomOrigin.phone_number = new FreightcomPhoneNumber()
                    {
                        number = locationPhone
                    };
                }
                if (!string.IsNullOrEmpty(locationEmail))
                {
                    freightcomOrigin.email_addresses = new List<string>()
                    {
                        locationEmail
                    };
                }
                freightcomOrigin.receives_email_updates = true;
                freightcomShipmentDetails.origin = freightcomOrigin;

                // Step 7.1.2: Handle Freightcom Destination
                var freightcomDestination = new FreightcomDestination
                {
                    name = attention
                };
                var destinationAddress = new FreightcomAddress
                {
                    address_line_1 = toAddressLine1,
                    address_line_2 = toAddressLine2,
                    city = toCity,
                    region = toProvinceCode,
                    country = toCountryCode,
                    postal_code = toPostalCode,
                };
                freightcomDestination.address = destinationAddress;
                freightcomDestination.residential = residentialAddress;
                freightcomDestination.tailgate_required = false;
                if (!string.IsNullOrEmpty(firstContactPersonFullName))
                {
                    freightcomDestination.contact_name = firstContactPersonFullName;                }
                else
                {
                    freightcomDestination.contact_name = attention;
                }    
                freightcomDestination.phone_number = new FreightcomPhoneNumber()
                {
                    number = toPhone,
                    extension = toPhoneExtension
                };
                freightcomDestination.email_addresses = new List<string>()
                {
                    toEmail
                };
                freightcomDestination.receives_email_updates = true;
                var readyAt = new FreightcomReadyAt
                {
                    hour = int.Parse(readyAtHourStr),
                    minute = int.Parse(readyAtMinuteStr)
                };
                var readyUntil = new FreightcomReadyUntil
                {
                    hour = int.Parse(readyUntilHourStr),
                    minute = int.Parse(readyUntilMinuteStr)
                };
                freightcomDestination.signature_requirement = "not-required";
                freightcomShipmentDetails.destination = freightcomDestination;

                // TODO: Step 7.1.2: Handle Expected Ship Date

                // Step 7.1.3: Handle Freightcom Packaging
                freightcomShipmentDetails.packaging_type = "package";

                var packagingProperties = new FreightcomPackagingProperties();
                packagingProperties.includes_return_label = false;
                packagingProperties.has_dangerous_goods = false;

                var packages = new List<FreightcomPackage>();

                foreach (var box in rateBoxes)
                {

                    length = box.Length.Value;
                    width = box.Width.Value;
                    height = box.Height.Value;
                    weight = box.Weight.Value;

                    var package = new FreightcomPackage();

                    var packageMeasurements = new FreightcomMeasurements();
                    var packageWeight = new FreightcomWeight();
                    packageWeight.unit = "kg";
                    packageWeight.value = weight;
                    packageMeasurements.weight = packageWeight;

                    var packageCuboid = new FreightcomCuboid();
                    packageCuboid.unit = "in";
                    packageCuboid.l = length;
                    packageCuboid.w = width;
                    packageCuboid.h = height;
                    packageMeasurements.cuboid = packageCuboid;

                    package.measurements = packageMeasurements;
                    package.description = "Chocolate";
                    package.special_handling_required = false;
                    packages.Add(package);

                }
               
                packagingProperties.packages = packages;
                freightcomShipmentDetails.packaging_properties = packagingProperties;

                // STEP 7.1.4: Handle Expected Ship Date
                var freightcomExpectedShipDate = new FreightcomExpectedShipDate();
                var expectedShipDate = DateTime.ParseExact
                    (expectedShipDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var expectedDateYear = expectedShipDate.Year;
                var expectedDateMonth = expectedShipDate.Month;
                var expectedDateDay = expectedShipDate.Day;

                freightcomExpectedShipDate.year = expectedDateYear;
                freightcomExpectedShipDate.month = expectedDateMonth;
                freightcomExpectedShipDate.day = expectedDateDay;
                freightcomShipmentDetails.expected_ship_date = freightcomExpectedShipDate;

                freightcomShipmentDetails.packaging_properties = packagingProperties;
                requestRateEstimateRequest.details = freightcomShipmentDetails;

                string requestRateEstimateStr = JsonConvert.SerializeObject(requestRateEstimateRequest);

                // STEP 7.2: Call API to get Freightcom Rates
                var requestRateEstimateResponse = await _zoRawFreightcomService
                    .RequestRateEstimate(requestRateEstimateRequest);

                if (requestRateEstimateResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = requestRateEstimateResponse.Message;
                    return apiResult;
                }

                // STEP 7.3: Call API to retrieve a rate by id
                var rateId = requestRateEstimateResponse.Data.request_id;

                Thread.Sleep(3000);
                var retrieveARateByIdResponse = await _zoRawFreightcomService
                    .RetrieveARate(rateId);

                if (retrieveARateByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = retrieveARateByIdResponse.Message;
                    return apiResult;
                }

                var retrieveDone = retrieveARateByIdResponse.Data.status.done;

                var maxTrial = 5;
                var trialCount = 0;

                while (!retrieveDone.HasValue || !retrieveDone.Value)
                {
                    trialCount++;
                    Thread.Sleep(1000); // Wait for 1 second
                    retrieveARateByIdResponse = await _zoRawFreightcomService
                        .RetrieveARate(rateId);
                    if (trialCount >= maxTrial)
                    {
                        break;
                    }
                }


                var freightcomRates = retrieveARateByIdResponse.Data.rates;
                foreach (var freightcomRate in freightcomRates)
                {
                    var commonRate = ConvertHelpers.Freightcom_Convert2CommonRate(freightcomRate);
                    commonRate.GetRatesDetails = requestRateEstimateRequest;
                    allCommonRates.Add(commonRate);
                }

                var sortedRates = allCommonRates.OrderBy(r => r.Total).ToList();

                // STEP 8: Handle Common Rates
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZoRawConstants.GRFW_200;
                apiResult.Data = sortedRates;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> BookShipmentFromWidget
            (BookShipmentFromWidgetRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CSFW_400
            };

            string labelFilePath = string.Empty;

            try
            {

                // STEP 1: Extract data from request body
                string soId = request.SalesOrderId;
                string packageId = request.PackageId;

                var getPackageByIdResponse = await 
                    _zoRawInventoryService.GetPackageById(packageId);

                var packageDetails = getPackageByIdResponse.Data.package;
                string packageNumber = packageDetails.package_number;

                string serviceId = request.ServiceId;

                // STEP 1.1: Split Service Id to get Provider and Service Name

                var serviceSplits = serviceId.Split("|||");
                string provider = serviceSplits[0];
                string serviceName = serviceSplits[1];

                string comment = request.Comment;
                //float length = request.Length.Value;
                //float width = request.Width.Value;
                //float weight = request.Weight.Value;
                string getRatesDetails = request.GetRatesDetails;

                // STEP 1.2: Search Delivery Orders

                var searchDeliveryOrdersResponse = await _zoRawInventoryService
                    .SearchOrderFulfillments(soId);

                var deliveryOrders = searchDeliveryOrdersResponse.Data.module_records;

                string fulfillmentId = string.Empty;
                if (deliveryOrders != null && deliveryOrders.Count() > 0)
                {

                    var firstDeliveryOrder = deliveryOrders[0];
                    fulfillmentId = firstDeliveryOrder.module_record_id;
                }

                #region Stallion

                if (provider == "STL")
                {
                    // STEP 2: Create Shipment Order and Upload File
                    var getStallionRatesRequest = 
                        JsonConvert.DeserializeObject<GetStallionRatesRequest>(getRatesDetails);

                    var createStallionShipmentRequest = ConvertHelpers
                        .Stallion_ConvertGetRatesToShipment(getStallionRatesRequest);

                    createStallionShipmentRequest.postage_type = serviceName;

                    var createStallionShipmentResponse = await _zoRawStallionService
                        .CreateShipment(createStallionShipmentRequest);

                    if (createStallionShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = createStallionShipmentResponse.Message;
                        return apiResult;
                    }

                    var createStallionDetails = createStallionShipmentResponse.Data;
                    string label = createStallionDetails.label;

                    var rateDetails = createStallionDetails.rate;
                    decimal totalRate = decimal.Parse(rateDetails.rate);
                    string postageType = rateDetails.postage_type;

                    var shipmentDetails = createStallionDetails.shipment;
                    string trackingCode = shipmentDetails.tracking_code;
                    string shipCode = shipmentDetails.ship_code;

                    // STEP 2.1: Get Stallion Shipment Url
                    var trackShipmentResponse = await _zoRawStallionService
                        .TrackShipment(trackingCode);

                    string trackingUrl = string.Empty;
                    if (trackShipmentResponse.Code == ResultCode.OK)
                    {
                        var trackShipmentDetails = trackShipmentResponse.Data;
                        var trackDetails = trackShipmentDetails.details;
                        trackingUrl = trackDetails.url;
                    }

                    var currentTimeUtc = DateTime.UtcNow;
                    var torontoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    var torontoTime = TimeZoneInfo.ConvertTime(currentTimeUtc, torontoTimeZone);

                    string torontoTimeStr = torontoTime.ToString("yyyy-MM-dd");

                    // Step 3.1: Create Shipment Order
                    var splitService = StringHelpers.ParseShippingMethod(serviceName);

                    string carrier = splitService.Carrier;
                    string shippingType = splitService.ShippingType;

                    var createShipmentRequest = new CreateShipmentRequest()
                    {
                        date = torontoTimeStr,
                        reference_number = shipCode,
                        tracking_number = trackingCode,
                        tracking_link = trackingUrl,
                        shipping_charge = totalRate,
                        exchange_rate = 1,
                        notes = comment
                    };

                    if (!string.IsNullOrEmpty(carrier))
                    {
                        createShipmentRequest.delivery_method = carrier;
                    }
                    else
                    {
                        createShipmentRequest.delivery_method = postageType;
                    }

                    var shipmentCustomFields = new List<Custom_Field>();

                    var createShipmentResponse = await
                        _zoRawInventoryService.CreateShipment(soId, packageId, createShipmentRequest);

                    if (createShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = createShipmentResponse.Message;
                        return apiResult;
                    }

                    var createShipmentDetails = createShipmentResponse.Data.shipmentorder;

                    string shipmentId = createShipmentDetails.shipment_id;

                    // Step 3.2: Convert Label to File

                    string fileName = $"{trackingCode}.pdf";
                    labelFilePath = Path.Combine(DocumentPath, fileName);
                    if (!Directory.Exists(DocumentPath))
                    {
                        Directory.CreateDirectory(DocumentPath);
                    }

                    FileHelpers.SaveBase64PdfToFile(label, labelFilePath);

                    // Step 3.3: Upload File to Shipment Order
                    var uploadFile2ShipmentResponse = await
                        _zoRawInventoryService.UploadFileToRecord(labelFilePath, "shipmentorders", shipmentId);

                    if (uploadFile2ShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZoRawConstants.UF2S_400;
                        return apiResult;
                    }

                    // Step 3.4: Upload File to Delivery Order
                    if (!string.IsNullOrEmpty(fulfillmentId))
                    {

                        var uploadFile2DeliveryOrderResponse = await
                            _zoRawInventoryService.UploadFileToRecord(labelFilePath, "cm_delivery_order", fulfillmentId);

                        if (uploadFile2DeliveryOrderResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = ZoRawConstants.UF2D_400;
                            return apiResult;
                        }

                        var getOrderAttachmentsResponse = await _zoRawInventoryService
                            .GetRecordAttachments("cm_delivery_order", fulfillmentId);

                        var getOrderAttachmentsDetails = getOrderAttachmentsResponse.Data;
                        var uploadDocuments = getOrderAttachmentsDetails.documents;

                        string uploadDocumentId = string.Empty;

                        foreach (var document in uploadDocuments)
                        {
                            string documentName = document.file_name;
                            string documentId = document.document_id;
                            if (documentName == fileName)
                            {
                                uploadDocumentId = documentId;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(uploadDocumentId))
                        {

                            string printShippingUrl = ZoRawConstants.PrintShippingLabelUrl
                                .Replace("$OrderFulfillmentId$", fulfillmentId)
                                .Replace("$DocumentId$", uploadDocumentId);

                            var updateFulfillmentRequest = new UpdateOrderFulfillmentRequest()
                            {
                                cf_step_4_print_shipping_label = printShippingUrl
                            };

                            var updateFulfillmentResponse = await _zoRawInventoryService
                                .UpdateOrderFulfillment(fulfillmentId, updateFulfillmentRequest);

                            if (updateFulfillmentResponse.Code != ResultCode.OK)
                            {
                                apiResult.Message = ZoRawConstants.UOF_400;
                                return apiResult;
                            }

                        }
                    }

                }

                #endregion

                #region Freightcom

                if (provider == "FRC")
                {

                    // STEP 4: Create Freightcom Order and Upload File
                    var freightcomDetails = JsonConvert
                        .DeserializeObject<RequestRateEstimateRequest>(getRatesDetails);

                    var createFreightcomShipmentRequest = ConvertHelpers
                        .Freightcom_ConvertGetRatesToShipment(freightcomDetails);

                    // STEP 4.1: Handle Payment Method Id
                    var getPaymentMethodsResponse = await _zoRawFreightcomService
                        .GetPaymentMethods();

                    var paymentMethods = getPaymentMethodsResponse.Data;

                    string paymentMethodId = "";

                    foreach (var paymentMethod in paymentMethods)
                    {
                        string methodType = paymentMethod.type;
                        if (methodType == "credit-card")
                        {
                            paymentMethodId = paymentMethod.id;
                            break;
                        }
                    }
                    createFreightcomShipmentRequest.payment_method_id = 
                        paymentMethodId;
                    createFreightcomShipmentRequest.service_id = serviceName;
                    createFreightcomShipmentRequest.unique_id = packageNumber;

                    string createFreightcomShipmentRequestStr = 
                        JsonConvert.SerializeObject(createFreightcomShipmentRequest);

                    var createFreightcomShipmentResponse = await _zoRawFreightcomService
                        .CreateFreightcomShipment(createFreightcomShipmentRequest);

                    if (createFreightcomShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = createFreightcomShipmentResponse.Message;
                        return apiResult;
                    }

                    var freightcomShipmentId = createFreightcomShipmentResponse.Data.id;

                    // Step 5: Retrieve Freightcom Shipment Details
                    const int maxWaitSeconds = 180;   // 3 minutes
                    const int retryDelayMs = 5000;    // 5 seconds
                    var stopwatch = Stopwatch.StartNew();

                    ApiResultDto<RetrieveShipmentDetailsResponse> retrieveResponse = null;

                    while (stopwatch.Elapsed.TotalSeconds < maxWaitSeconds)
                    {
                        retrieveResponse = await _zoRawFreightcomService.RetrieveShipmentDetails(freightcomShipmentId);

                        // Check if data is available and valid
                        if (retrieveResponse?.Data?.shipment != null)
                        {
                            break;
                        }

                        // Wait before next poll
                        await Task.Delay(retryDelayMs);
                    }

                    var retriveShipmentDetails = retrieveResponse.Data.shipment;

                    var freightcomRate = retriveShipmentDetails.rate;

                    string trackingCode = retriveShipmentDetails.primary_tracking_number;
                    string trackingUrl = retriveShipmentDetails.tracking_url;
                    string shipCode = retriveShipmentDetails.id;

                    var totalRate = freightcomRate.total;
                    string totalValueStr = totalRate.value.ToString();
                    decimal totalValueFloat = decimal.Parse(totalValueStr);

                    var currentTimeUtc = DateTime.UtcNow;
                    var torontoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    var torontoTime = TimeZoneInfo.ConvertTime(currentTimeUtc, torontoTimeZone);

                    string torontoTimeStr = torontoTime.ToString("yyyy-MM-dd");

                    // Step 5.1: Create Shipment Order
                    var createShipmentRequest = new CreateShipmentRequest()
                    {
                        date = torontoTimeStr,
                        reference_number = freightcomShipmentId,
                        tracking_number = trackingCode,
                        tracking_link = trackingUrl,
                        shipping_charge = totalValueFloat / 100,
                        exchange_rate = 1,
                        notes = comment
                    };

                    var splitService = StringHelpers.ParseShippingMethod(serviceName);
                    string carrier = splitService.Carrier;
                    string shippingType = splitService.ShippingType;

                    var customField = new Custom_Field()
                    {
                        customfield_id = ZoRawConstants.FreightcomShipmentId_CustomFieldId,
                        value = freightcomShipmentId
                    };

                    var customFields = new List<Custom_Field>
                    {
                        customField
                    };

                    if (!string.IsNullOrEmpty(carrier))
                    {
                        createShipmentRequest.delivery_method = carrier;
                    }
                    else
                    {
                        createShipmentRequest.delivery_method = serviceName;
                    }

                    if (!string.IsNullOrEmpty(shippingType))
                    {
                        customField = new Custom_Field()
                        {
                            customfield_id = ZoRawConstants.ShippingType_CustomFieldId,
                            value = shippingType
                        };
                        customFields.Add(customField);
                    }

                    createShipmentRequest.shipmentorder_custom_fields = customFields;

                    var createShipmentResponse = await
                        _zoRawInventoryService.CreateShipment(soId, packageId, createShipmentRequest);

                    if (createShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = createShipmentResponse.Message;
                        return apiResult;
                    }

                    var createShipmentDetails = createShipmentResponse.Data.shipmentorder;

                    string shipmentId = createShipmentDetails.shipment_id;

                    // Step 5.2: Retrieve Labels from Shipment Details
                    var freightcomLabels = retriveShipmentDetails.labels;
                    string finalLabelUrl = string.Empty;
                    foreach (var label in freightcomLabels)
                    {

                        string labelSize = label.size;
                        string labelFormat = label.format;
                        string labelUrl = label.url;
                        bool padded = label.padded.Value;

                        if (labelSize == "a6" && labelFormat == "pdf" && !padded)
                        {
                            finalLabelUrl = labelUrl;
                            break;
                        }
                    }

                    // Step 5.3: Convert Label to File
                    string fileName = $"{trackingCode}.pdf";
                    labelFilePath = Path.Combine(DocumentPath, fileName);
                    if (!Directory.Exists(DocumentPath))
                    {
                        Directory.CreateDirectory(DocumentPath);
                    }
                    var downloadLabelResponse = await _commonService
                        .DownloadFileFromUrl(finalLabelUrl, labelFilePath);

                    // Step 5.4: Upload File to Shipment Order
                    var uploadFile2ShipmentResponse = await
                        _zoRawInventoryService.UploadFileToRecord(labelFilePath, "shipmentorders", shipmentId);

                    if (uploadFile2ShipmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZoRawConstants.UF2S_400;
                        return apiResult;
                    }

                    // Step 5.5: Upload File to Delivery Order
                    if (!string.IsNullOrEmpty(fulfillmentId))
                    {
                        var uploadFile2DeliveryOrderResponse = await
                            _zoRawInventoryService.UploadFileToRecord(labelFilePath, "cm_delivery_order", fulfillmentId);

                        if (uploadFile2DeliveryOrderResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = ZoRawConstants.UF2D_400;
                            return apiResult;
                        }

                        var getOrderDocumentsResponse = await _zoRawInventoryService
                            .GetRecordAttachments("cm_delivery_order", fulfillmentId);

                        var getOrderDocumentsDetails = getOrderDocumentsResponse.Data;
                        var uploadDocuments = getOrderDocumentsDetails.documents;

                        string uploadDocumentId = string.Empty;

                        foreach (var document in uploadDocuments)
                        {
                            string documentName = document.file_name;
                            string documentId = document.document_id;
                            if (documentName == fileName)
                            {
                                uploadDocumentId = documentId;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(uploadDocumentId))
                        {

                            string printShippingUrl = ZoRawConstants.PrintShippingLabelUrl.Replace("$OrderFulfillmentId$", fulfillmentId)
                                .Replace("$DocumentId$", uploadDocumentId);

                            var updateFulfillmentRequest = new UpdateOrderFulfillmentRequest()
                            {
                                cf_step_4_print_shipping_label = printShippingUrl
                            };

                            var updateFulfillmentResponse = await _zoRawInventoryService
                                .UpdateOrderFulfillment(fulfillmentId, updateFulfillmentRequest);

                            if (updateFulfillmentResponse.Code != ResultCode.OK)
                            {
                                apiResult.Message = ZoRawConstants.UOF_400;
                                return apiResult;
                            }

                        }
                    }
                }

                // STEP 4: Sync Order Fulfillment from Sales Order
                var syncFulfillmentRequest = new SyncOrderFulfillmentFromSalesOrderRequest()
                {
                    SalesOrderId = soId
                };
                var syncFulfillmentResponse = await _zoRawCrmService
                    .SyncOrderFulfillmentFromSalesOrder(syncFulfillmentRequest);

                if (syncFulfillmentResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = "Sync Order Fulfillment (Custom Module) from Sales Order FAILED";
                    return apiResult;
                }

                #endregion

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZoRawConstants.CSFW_200;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
            finally
            {
                // Delete the file after uploading
                if (File.Exists(labelFilePath))
                {
                    File.Delete(labelFilePath);
                }

            }

        }

        public async Task<ApiResultDto<string>> CreatePackageFromWidget
            (CreatePackageFromWidgetRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CPFW_400
            };

            try
            {

                // STEP 1: Extract data from request body
                string salesOrderId = request.SalesOrderId;
                var scannedItems = request.ScannedItems;

                var utcNow = DateTime.UtcNow;

                // Get Canada Eastern Time Zone (Toronto, Montreal, etc.)
                TimeZoneInfo canadaEasternTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                var canadaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, canadaEasternTime);

                string currentTime = canadaTime.ToString("yyyy-MM-dd");

                // STEP 2: Convert from Scanned Items to Package Line Items

                var createPackageRequest = new CreatePackageRequest();
                createPackageRequest.date = currentTime;

                var packageLineItems = new List<CreatePackageLineItem>();

                foreach (var scannedItem in scannedItems)
                {

                    string lineItemId = scannedItem.line_item_id;
                    float quantity = scannedItem.quantity.Value;

                    string batchInfo = scannedItem.batchInfo;
                    string batchName = string.Empty;
                    string batchId = string.Empty;
                    string batchInId = string.Empty;

                    if (!string.IsNullOrEmpty(batchInfo))
                    {

                        var batchSplits = batchInfo.Split("|");
                        batchName = batchSplits[0];
                        batchId = batchSplits[1];
                        batchInId = batchSplits[2];

                    }

                    var mappedItems = scannedItem.mapped_items;

                    // STEP 2.1: Check if so_line_item_id exists
                    var packageItem = packageLineItems.Where(i => i.so_line_item_id == lineItemId)
                        .FirstOrDefault();

                    if (packageItem == null)
                    {

                        packageItem = new CreatePackageLineItem()
                        {
                            so_line_item_id = lineItemId,
                            quantity = quantity
                        };

                        var batches = new List<CreatePackageBatch>();
                        var batch = new CreatePackageBatch();

                        if (!string.IsNullOrEmpty(batchId) && !string.IsNullOrEmpty(batchInId))
                        {
                            batch.batch_id = batchId;
                            batch.out_quantity = quantity.ToString();
                            batch.batch_in_id = batchInId;
                            batches.Add(batch);
                            packageItem.batches = batches;
                        }
                        else
                        {
                            packageItem.batches = null;
                        }

                        if (mappedItems != null && mappedItems.Count > 0)
                        {
                            packageItem.quantity = 0;
                            var packageMappedItems = new List<CreatePackageMappedItem>();

                            foreach (var mappedItem in mappedItems)
                            {

                                var packageMappedItem = new CreatePackageMappedItem()
                                {
                                    so_line_item_id = mappedItem.line_item_id,
                                    quantity = quantity * mappedItem.quantity
                                };
                                string mappedBatchInfo = mappedItem.batchInfo;
                                if (!string.IsNullOrEmpty(mappedBatchInfo))
                                {
                                    var mappedBatchSplits = mappedBatchInfo.Split("|");
                                    string mappedBatchName = mappedBatchSplits[0];
                                    string mappedBatchId = mappedBatchSplits[1];
                                    string mappedBatchInId = mappedBatchSplits[2];
                                    var mappedBatches = new List<CreatePackageBatch>();
                                    var mappedBatch = new CreatePackageBatch();

                                    if (!string.IsNullOrEmpty(mappedBatchId) && 
                                        !string.IsNullOrEmpty(mappedBatchInId))
                                    {
                                        mappedBatch.batch_id = mappedBatchId;
                                        mappedBatch.out_quantity = (quantity * mappedItem.quantity).ToString();
                                        mappedBatch.batch_in_id = mappedBatchInId;
                                        mappedBatches.Add(mappedBatch);
                                        packageMappedItem.batches = mappedBatches;
                                    }
                                }
                                packageMappedItems.Add(packageMappedItem);
                            }

                            packageItem.mapped_items = packageMappedItems;

                        }

                        packageLineItems.Add(packageItem);

                    }
                    else
                    {

                        packageItem.quantity += quantity;
                        var currentMappedItems = packageItem.mapped_items;

                        var packageBatches = packageItem.batches;

                        bool batchExist = false;

                        if (packageBatches != null && packageBatches.Count > 0)
                        {
                            foreach (var batch in packageBatches)
                            {

                                string currentBatchId = batch.batch_id;
                                string currentBatchInId = batch.batch_in_id;

                                if (currentBatchId == batchId && currentBatchInId == batchInId)
                                {
                                    batchExist = true;
                                    batch.out_quantity = (float.Parse(batch.out_quantity) + quantity).ToString();
                                    break;
                                }

                            }
                        }

                        if (!batchExist && !string.IsNullOrEmpty(batchId) && !string.IsNullOrEmpty(batchInId))
                        {
                            var batch = new CreatePackageBatch()
                            {
                                batch_id = batchId,
                                out_quantity = quantity.ToString(),
                                batch_in_id = batchInId
                            };

                            packageBatches.Add(batch);
                            packageItem.batches = packageBatches;
                        }
                    
                        if (mappedItems != null && mappedItems.Count > 0)
                        {

                            packageItem.quantity = 0;

                            foreach (var mappedItem in mappedItems)
                            {

                                string mappedItemLineItemId = mappedItem.line_item_id;

                                if (currentMappedItems != null)
                                {
                                    var existMappedItem = currentMappedItems
                                    .Where(mi => mi.so_line_item_id == mappedItemLineItemId)
                                    .FirstOrDefault();

                                    if (existMappedItem != null)
                                    {
                                        existMappedItem.quantity += quantity * mappedItem.quantity;
                                        if (!string.IsNullOrEmpty(mappedItem.batchInfo))
                                        {
                                            var mappedBatchSplits = mappedItem.batchInfo.Split("|");
                                            string mappedBatchName = mappedBatchSplits[0];
                                            string mappedBatchId = mappedBatchSplits[1];
                                            string mappedBatchInId = mappedBatchSplits[2];
                                            var mappedBatches = existMappedItem.batches;
                                            if (mappedBatches == null)
                                            {
                                                mappedBatches = new List<CreatePackageBatch>();
                                            }
                                            var mappedBatch = new CreatePackageBatch();
                                            if (!string.IsNullOrEmpty(mappedBatchId) &&
                                                !string.IsNullOrEmpty(mappedBatchInId))
                                            {
                                                mappedBatch.batch_id = mappedBatchId;
                                                mappedBatch.out_quantity = (quantity * mappedItem.quantity).ToString();
                                                mappedBatch.batch_in_id = mappedBatchInId;
                                                mappedBatches.Add(mappedBatch);
                                                existMappedItem.batches = mappedBatches;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var packageMappedItem = new CreatePackageMappedItem()
                                        {
                                            so_line_item_id = mappedItemLineItemId,
                                            quantity = quantity * mappedItem.quantity
                                        };
                                        string mappedBatchInfo = mappedItem.batchInfo;
                                        if (!string.IsNullOrEmpty(mappedBatchInfo))
                                        {
                                            var mappedBatchSplits = batchInfo.Split("|");
                                            string mappedBatchName = mappedBatchSplits[0];
                                            string mappedBatchId = mappedBatchSplits[1];
                                            string mappedBatchInId = mappedBatchSplits[2];
                                            var mappedBatches = new List<CreatePackageBatch>();
                                            var mappedBatch = new CreatePackageBatch();
                                            if (!string.IsNullOrEmpty(mappedBatchId) &&
                                                !string.IsNullOrEmpty(mappedBatchInId))
                                            {
                                                mappedBatch.batch_id = batchId;
                                                mappedBatch.out_quantity = quantity.ToString();
                                                mappedBatch.batch_in_id = batchInId;
                                                mappedBatches.Add(mappedBatch);
                                                packageMappedItem.batches = mappedBatches;
                                            }
                                        }
                                        currentMappedItems.Add(packageMappedItem);
                                    }
                                }
                                

                            }
                        }
                    
                    }
                }

                createPackageRequest.line_items = packageLineItems;

                string createPackageRequestStr = JsonConvert.SerializeObject(createPackageRequest);

                // STEP 3: Create Package in Zoho Inventory
                var createPackageResponse = await _zoRawInventoryService.CreatePackage
                    (salesOrderId, createPackageRequest);

                if (createPackageResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = createPackageResponse.Message;
                }

                // STEP 4: Sync Order Fulfillment from Sales Order
                var syncFulfillmentRequest = new SyncOrderFulfillmentFromSalesOrderRequest()
                {
                    SalesOrderId = salesOrderId
                };
                var syncFulfillmentResponse = await _zoRawCrmService
                    .SyncOrderFulfillmentFromSalesOrder(syncFulfillmentRequest);

                if (syncFulfillmentResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = "Sync Order Fulfillment (Custom Module) from Sales Order FAILED";
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZoRawConstants.CPFW_200;
                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        #endregion

    }

}
