using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoogleAPI
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<string> GetAccessToken(string clientName)
        {
            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                string refreshToken = string.Empty;
                string authEndpoint = GoogleAPIConstants.TokenEndpoint;
                string grantType = GoogleAPIConstants.GrantType_RefreshToken;
                string platformName = "Google API";

                if (clientName == REOConstants.RestaurantEquipmentOnline)
                {
                    clientId = REOConstants.Google_ClientId;
                    clientSecret = REOConstants.Google_ClientSecret;
                    refreshToken = REOConstants.Google_RefreshToken;
                }

                string accessTokenFile = $"{clientName}_{platformName}_access_token.txt";

                AccessTokenModel tokenModel = null;
                string txtFilePath = $"{DocumentPath}\\{accessTokenFile}";
                if (!Directory.Exists(DocumentPath))
                {
                    Directory.CreateDirectory(DocumentPath);
                }
                if (File.Exists(txtFilePath))
                {
                    string text = File.ReadAllText(txtFilePath);
                    if (!string.IsNullOrEmpty(text))
                    {
                        tokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(text);
                        if (DateTime.UtcNow < tokenModel.ExpiredTime)
                        {
                            return tokenModel.AccessToken;
                        }
                    }
                }

                string endpoint = authEndpoint;

                var encodeContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                };

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                { 
                    Content = new FormUrlEncodedContent(encodeContent)
                };

                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<GoogleTokenResponse>(responseData);

                string accessToken = responseObj.access_token;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var accessTokenForSaving = new AccessTokenModel()
                    {
                        AccessToken = accessToken,
                        ExpiredTime = DateTime.UtcNow.AddMinutes(10),
                    };
                    File.WriteAllText(txtFilePath, JsonConvert.SerializeObject(accessTokenForSaving));
                }
                return accessToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
