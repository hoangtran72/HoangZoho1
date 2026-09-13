using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.Podium;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZohoAuth
{

    public class PodiumAuthService : IPodiumAuthService
    {

        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<string> GetAccessToken(string clientName)
        {

            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                string refreshToken = string.Empty;
                string authEndpoint = string.Empty;

                if (clientName == GoSunnySolarConstants.GoSunnySolar)
                {
                    authEndpoint = GoSunnySolarConstants.PodiumAuthEndpoint;
                    clientId = GoSunnySolarConstants.PodiumClientId;
                    clientSecret = GoSunnySolarConstants.PodiumClientSecret;
                    refreshToken = GoSunnySolarConstants.PodiumRefreshToken;
                }

                string accessTokenFile = $"{clientName}_Podium_access_token.txt";
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
                string grantType = "refresh_token";
                string endpoint = $"{authEndpoint}?refresh_token={refreshToken}&client_id={clientId}&client_secret={clientSecret}&grant_type={grantType}";

                var requestPodiumToken = new PodiumTokenRequest()
                {
                    client_id = clientId,
                    client_secret = clientSecret,
                    refresh_token = refreshToken,
                    grant_type = "refresh_token"
                };

                string requestBody = JsonConvert.SerializeObject(requestPodiumToken);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<ZohoTokenResponse>(responseData);

                string accessToken = responseObj.access_token;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var accessTokenForSaving = new AccessTokenModel()
                    {
                        AccessToken = accessToken,
                        ExpiredTime = DateTime.UtcNow.AddMinutes(30),
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
