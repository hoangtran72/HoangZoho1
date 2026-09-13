using System.Text;
using System;
using HoangZoho1.Models.ZoRaw.Custom;
using HoangZoho1.Models.ZoRaw.Stallion;
using System.Collections.Generic;
using HoangZoho1.Models.ZoRaw.Freightcom;

namespace HoangZoho1.Helpers
{

    public class ConvertHelpers
    {

        public static CommonShipmentRate Stallion_Convert2CommonRate
            (StallionRate stallionRate)
        {

            var commonRate = new CommonShipmentRate
            {
                Provider = "Stallion",
                ProviderShort = "STL",
                ServiceId = stallionRate.postage_type.ToString(),
                CarrierName = stallionRate.postage_type,
                Tax = stallionRate.tax,
                Base = stallionRate.base_rate,
                Currency = stallionRate.currency,
                DeliveryDays = stallionRate.delivery_days,
                Total = stallionRate.total
            };

            var stallionAddOns = stallionRate.add_ons;
            float addOnRate = 0;
            var commonAddOns = new List<CommonAddOn>();
            foreach (var stallionAddOn in stallionAddOns)
            {
                string name = stallionAddOn.name;
                string type = stallionAddOn.type;

                addOnRate += stallionAddOn.cost.Value;

                var commonAddOn = new CommonAddOn
                {
                    Name = $"{stallionAddOn.name} ({stallionAddOn.type})",
                    Cost = stallionAddOn.cost,
                    Currency = stallionAddOn.currency
                };
                commonAddOns.Add(commonAddOn);
            }
            commonRate.AddOnRate = (float) Math.Round(addOnRate, 2);
            commonRate.AddOns = commonAddOns;

            return commonRate;
        
        }

        public static CommonShipmentRate Freightcom_Convert2CommonRate
            (FreightcomRate freightcomRate)
        {

            var commonRate = new CommonShipmentRate
            {
                Provider = "Freightcom",
                ProviderShort = "FRC",
                ServiceId = freightcomRate.service_id,
            };

            string carrierName = freightcomRate.carrier_name;
            string serviceName = freightcomRate.service_name;
            if (!string.IsNullOrEmpty(serviceName))
            {
                carrierName = $"{carrierName} ({serviceName})";
            }
            commonRate.CarrierName = carrierName;

            // Handle Delivery Days
            var transitTimeDays = freightcomRate.transit_time_days;
            if (transitTimeDays != null)
            {
                commonRate.DeliveryDays = transitTimeDays.ToString();
            }
            else
            {
                commonRate.DeliveryDays = "N/A"; // or some default value
            }

            // Handle Base and Currency
            var freightcomBase = freightcomRate._base;
            commonRate.Base = float.Parse(freightcomBase.value) / 100;
            commonRate.Currency = freightcomBase.currency;

            // Handle Tax
            float tax = 0;
            var freightcomTaxes = freightcomRate.taxes;
            foreach (var freightcomTax in freightcomTaxes)
            {
                var taxAmount = freightcomTax.amount;
                var taxValue = float.Parse(taxAmount.value);
                tax += taxValue;
            }
            commonRate.Tax = tax / 100;

            // Handle Total
            var freightcomTotal = freightcomRate.total;
            commonRate.Total = float.Parse(freightcomTotal.value) / 100;

            // Handle Add Ons
            var freightcomAddOns = freightcomRate.surcharges;
            var commonAddOns = new List<CommonAddOn>();
            float addOnRate = 0;
            foreach (var freightcomAddOn in freightcomAddOns)
            {
                var freightcomAddOnAmountObj = freightcomAddOn.amount;

                float freightcomAddonAmount = float.Parse(freightcomAddOnAmountObj.value);
                string freightcomAddonCurrency = freightcomAddOnAmountObj.currency;

                addOnRate += freightcomAddonAmount / 100;

                var commonAddOn = new CommonAddOn
                {
                    Name = freightcomAddOn.type,
                    Cost = freightcomAddonAmount / 100,
                    Currency = freightcomAddonCurrency
                };
                commonAddOns.Add(commonAddOn);
            }
            commonRate.AddOnRate = (float) Math.Round(addOnRate, 2);
            commonRate.AddOns = commonAddOns;

            return commonRate;

        }

        public static CreateStallionShipmentRequest Stallion_ConvertGetRatesToShipment
            (GetStallionRatesRequest getStallionRatesRequest)
        {

            var createStallionShipmentRequest = new CreateStallionShipmentRequest()
            {
                to_address = getStallionRatesRequest.to_address,
                return_address = getStallionRatesRequest.return_address,
                is_return = getStallionRatesRequest.is_return,
                weight_unit = getStallionRatesRequest.weight_unit,
                weight = getStallionRatesRequest.weight,
                length = getStallionRatesRequest.length,
                width = getStallionRatesRequest.width,
                height = getStallionRatesRequest.height,
                size_unit = getStallionRatesRequest.size_unit,
                package_type = getStallionRatesRequest.package_type,
                signature_confirmation = getStallionRatesRequest.signature_confirmation,
                label_format = "pdf",
                is_fba = false,
                is_draft = false,
                insured = false,
            };

            var createItems = new List<ShipmentItem>();

            foreach (var rateItem in getStallionRatesRequest.items)
            {
                var shipmentItem = new ShipmentItem
                {
                    description = rateItem.description,
                    sku = rateItem.sku,
                    quantity = rateItem.quantity,
                    value = rateItem.value,
                    currency = rateItem.currency,
                    country_of_origin = rateItem.country_of_origin,
                    hs_code = rateItem.hs_code,
                };
                createItems.Add(shipmentItem);
            }
            createStallionShipmentRequest.items = createItems;

            return createStallionShipmentRequest;
        }

        public static CreateFreightcomShipmentRequest Freightcom_ConvertGetRatesToShipment
            (RequestRateEstimateRequest requestRateEstimateRequest)
        {

            var createFreightcomShipmentRequest = new CreateFreightcomShipmentRequest()
            {

                details = requestRateEstimateRequest.details,

            };

            return createFreightcomShipmentRequest;

        }

    }

}
