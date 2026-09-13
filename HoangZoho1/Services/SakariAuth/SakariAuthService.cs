using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Sakari;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.SakariAuth
{

    public class SakariAuthService : ISakariAuthService
    {

        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<string> GetSakariToken(string clientId, string clientSecret)
        {
            try
            {
                string refreshToken = string.Empty;
                string authEndpoint = string.Empty;

                authEndpoint = OneCorpConstants.Sakari_AuthEndpoint;

                string accessTokenFile = $"Sakari_access_token.txt";

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

                var getTokenRequest = new GetSakariTokenRequest()
                {
                    client_id = clientId,
                    client_secret = clientSecret,
                    grant_type = "client_credentials"
                };
                string endpoint = $"{authEndpoint}";

                string requestBody = JsonConvert.SerializeObject(getTokenRequest);

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
                var responseObj = JsonConvert.DeserializeObject<GetSakariTokenResponse>(responseData);

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
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
