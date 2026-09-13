using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.WclSolutions
{
    public class WclCustomService : IWclCustomService
    {

        private IWclWooService _wclWooService;

        public WclCustomService(IWclWooService wclWooService)
        {
            _wclWooService = wclWooService;
        }

        public async Task<GetMasterItemsResponse> GetMasterItems()
        {

            var apiResult = new GetMasterItemsResponse();

            try
            {

                string endpoint = WclConstants.GetMasterItemsAndBatchesUrl;

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint);
                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jObject = JObject.Parse(responseData);
                    string masterItemsStr = (string) jObject["details"]["output"];
                    apiResult = JsonConvert.DeserializeObject<GetMasterItemsResponse>(masterItemsStr);
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                return apiResult;
            }

        }

        public async Task<string> SyncWooOrder2ZohoResponse(SyncWooOrdersFromWidgetRequest request)
        {

            string resultMessage = string.Empty;

            try
            {

                string wooOrderNumbers = request.WooOrderIds;

                var wooOrderSplits = wooOrderNumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                resultMessage = "<ol>";

                foreach (string wooNumber in wooOrderSplits)
                {

                    string wooOrderNumber = wooNumber.Trim();

                    var syncWooOrderRequest = new SyncWooOrderRequest()
                    {
                        OrderNumber = wooOrderNumber
                    };

                    string syncOrderToInventoryUrl = WclConstants.WooSyncOrderToInventoryUrl;

                    using (var httpClient = new HttpClient())
                    {
                        var requestMessage = new HttpRequestMessage(
                            HttpMethod.Post,
                            syncOrderToInventoryUrl)
                        {
                            Content = new StringContent(
                                JsonConvert.SerializeObject(syncWooOrderRequest),
                                    System.Text.Encoding.UTF8,
                                    "application/json")
                        };

                        // Send request
                        var response = await httpClient.SendAsync(requestMessage);

                        // Get response as string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Check status
                        if (response.IsSuccessStatusCode && 
                            responseBody.Contains("Sync Order from Woo to Inventory SUCCESSFULLY", 
                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            resultMessage += $"<li>{wooOrderNumber}: ✅ Successfully synced orders</li>";
                        }
                        else
                        {
                            resultMessage += $"<li>{wooOrderNumber}: ⚠️ Failed to sync orders</li>";
                        }
                    }

                }

                resultMessage += "</ol>";

                return resultMessage;

            }
            catch (Exception ex)
            {
                
                if (!resultMessage.Contains("</ol>"))
                {
                    resultMessage += "</ol>";
                }

                resultMessage += "<br><b>Error:</b> " + ex.Message + "<br>";
                resultMessage += "<br><b>Stack Trace:</b> " + ex.StackTrace + "<br>";

                return resultMessage;

            }
        }

    }
}
