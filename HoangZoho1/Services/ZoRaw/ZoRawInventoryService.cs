using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoBooks;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using HoangZoho1.Services.ZohoAuth;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;
using System.Collections.Generic;
using HoangZoho1.Models.WclSolutions;

namespace HoangZoho1.Services.ZoRaw
{

    public class ZoRawInventoryService : IZoRawInventoryService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZoRawInventoryService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<GetSalesOrderByIdResponse>> 
            GetSalesOrderById(string salesOrderId)
        {

            var apiResult = new ApiResultDto<GetSalesOrderByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GSBI_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/salesorders/{salesOrderId}?" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetSalesOrderByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.GSBI_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            
            }

        }

        public async Task<ApiResultDto<ListSalesOrdersResponse>>
            GetSalesOrderByNumber(string salesOrderNumber)
        {

            var apiResult = new ApiResultDto<ListSalesOrdersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GSBI_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/salesorders" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListSalesOrdersResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.GSBI_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }
        public async Task<ApiResultDto<GetPackageByIdResponse>> GetPackageById(string packageId)
        {

            var apiResult = new ApiResultDto<GetPackageByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GPBI_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/packages/{packageId}?" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetPackageByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.GPBI_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<CreatePackageResponse>> CreatePackage(string salesOrderId, 
            CreatePackageRequest createPackageRequest)
        {

            var apiResult = new ApiResultDto<CreatePackageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CPK_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                // Extract Request Body
                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/packages?" +
                    $"salesorder_id={salesOrderId}" +
                    $"&organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";

                string requestBody = JsonConvert.SerializeObject(createPackageRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreatePackageResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.CPK_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetLocationByIdResponse>> GetLocationById(string locationId)
        {

            var apiResult = new ApiResultDto<GetLocationByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GLBI_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/locations/{locationId}?" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetLocationByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.GLBI_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<ListAllLocationsResponse>> ListAllLocations()
        {

            var apiResult = new ApiResultDto<ListAllLocationsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.LAL_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/locations?" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK 
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListAllLocationsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.LAL_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<CreateShipmentResponse>> CreateShipment(string salesOrderId, string packageId, CreateShipmentRequest createShipmentRequest)
        {

            var apiResult = new ApiResultDto<CreateShipmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CSO_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                // Extract Request Body
                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/shipmentorders?" +
                    $"salesorder_id={salesOrderId}" +
                    $"&package_ids={packageId}" +
                    $"&organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";

                string requestBody = JsonConvert.SerializeObject(createShipmentRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateShipmentResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.CSO_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }    

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<UploadFileToRecordResponse>> 
            UploadFileToRecord(string filePath, string recordModule, string recordId)
        {

            var apiResult = new ApiResultDto<UploadFileToRecordResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.UF2S_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                // Extract Request Body
                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/{recordModule}/{recordId}" +
                    $"/attachment?organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";

                var fileName = Path.GetFileName(filePath);

                // Detect MIME type
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out string contentType))
                {
                    contentType = "application/octet-stream"; // fallback
                }

                var content = new MultipartFormDataContent();

                var fileBytes = await File.ReadAllBytesAsync(filePath);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                
                content.Add(fileContent, "attachment", fileName);
                content.Add(new StringContent(ZoRawConstants.ZohoInventory_OrganizationId), "organization_id");

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = content
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <UploadFileToRecordResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.CSO_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception ex)
            {
                
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }
        
        public async Task<ApiResultDto<GetItemByIdResponse>> GetItemById(string itemId)
        {

            var apiResult = new ApiResultDto<GetItemByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GIBI_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/items/{itemId}?" +
                    $"organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <GetItemByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.GIBI_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<SearchBatchesResponse>> 
            SearchBatches(SearchBatchesRequest searchBatchesRequest)
        {

            var apiResult = new ApiResultDto<SearchBatchesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SBR_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string itemId = searchBatchesRequest.item_id;
                string locationId = searchBatchesRequest.location_id;

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/items/batches?" +
                    $"page=1&per_page=100&sort_column=batch_number&sort_order=A" +
                    $"&organization_id={ZoRawConstants.ZohoInventory_OrganizationId}&item_id={itemId}" +
                    $"&location_id={locationId}&include_empty_batches=false";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <SearchBatchesResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.SBR_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<SearchDeliveryOrdersResponse>> SearchOrderFulfillments
            (string salesOrderId)
        {
            var apiResult = new ApiResultDto<SearchDeliveryOrdersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SBR_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/cm_delivery_order" +
                    $"?organization_id={ZoRawConstants.ZohoInventory_OrganizationId}" +
                    $"&cf_order_id={salesOrderId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <SearchDeliveryOrdersResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.SBR_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpdateOrderFulfillmentResponse>> UpdateOrderFulfillment(
            string fulfillmentId,
            UpdateOrderFulfillmentRequest updateDeliveryOrderRequest)
        {
            var apiResult = new ApiResultDto<UpdateOrderFulfillmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.UOF_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                // Extract Request Body
                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/cm_delivery_order/{fulfillmentId}" +
                    $"?organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";

                string requestBody = JsonConvert.SerializeObject(updateDeliveryOrderRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <UpdateOrderFulfillmentResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.UOF_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

        public async Task<ApiResultDto<GetRecordAttachmentsResponse>> GetRecordAttachments(string recordModule, string recordId)
        {

            var apiResult = new ApiResultDto<GetRecordAttachmentsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.UF2S_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                // Handle Endpoint
                string endpoint = $"{ZoRawConstants.ZohoInventory_EndpointV1}/{recordModule}/{recordId}" +
                    $"/attachment?organization_id={ZoRawConstants.ZohoInventory_OrganizationId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <GetRecordAttachmentsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.CSO_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

    }

}
