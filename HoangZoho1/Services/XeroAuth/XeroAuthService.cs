using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Xero;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.XeroAuth
{
    public class XeroAuthService : IXeroAuthService
    {
        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<string> GetAccessToken(string clientName, string companyName)
        {
            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                string refreshToken = string.Empty;
                if (clientName == DiamondMindsConstants.DiamondMinds)
                {
                    if (companyName == "Demo Company")
                    {
                        clientId = DiamondMindsConstants.Demo_ClientId;
                        clientSecret = DiamondMindsConstants.Demo_ClientSecret;
                        refreshToken = DiamondMindsConstants.Demo_RefreshToken;
                    }
                    else if (companyName == "AA Fisher")
                    {
                        clientId = DiamondMindsConstants.Demo_ClientId;
                        clientSecret = DiamondMindsConstants.Demo_ClientSecret;
                        refreshToken = DiamondMindsConstants.Fisher_RefreshToken;
                    }
                }

                // Step 2: Get Refresh Tokens
                RefreshTokenModel tokenModel = null;
                string tokenFile = $"{clientName}_Xero_{companyName}.txt";
                string tokenPath = $"{DocumentPath}/{tokenFile}";
                if (!Directory.Exists(DocumentPath))
                {
                    Directory.CreateDirectory(DocumentPath);
                }
                if (File.Exists(tokenPath)) 
                {
                    string text = File.ReadAllText(tokenPath);
                    if (!string.IsNullOrEmpty(text))
                    {
                        tokenModel = JsonConvert.DeserializeObject<RefreshTokenModel>(text);
                        if (DateTime.UtcNow < tokenModel.AccessExpiredTime)
                        {
                            return tokenModel.AccessToken;
                        }
                        else
                        {
                            refreshToken = tokenModel.RefreshToken;
                        }
                    }
                }

                string grantType = "refresh_token";
                var data = new[]
                {
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                };

                string endpoint = CommonConstants.Xero_AuthEndpoint;

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(data)
                };

                using var httpClient = new HttpClient();
                var basicAuth = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(basicAuth)}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseData);

                var refreshTokenForSaving = new RefreshTokenModel()
                {
                    AccessToken = responseObj.access_token,
                    AccessExpiredTime = DateTime.UtcNow.AddMinutes(15),
                    RefreshToken = responseObj.refresh_token,
                    RefreshExpiredTime = DateTime.UtcNow.AddDays(30)
                };
                File.WriteAllText(tokenPath, JsonConvert.SerializeObject(refreshTokenForSaving));
                
                return refreshTokenForSaving.AccessToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
