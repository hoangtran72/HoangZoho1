using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZohoAuth
{
    public class ZohoAuthService : IZohoAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public ZohoAuthService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<string> GetAccessToken(string clientName, string platformName)
        {
            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                string refreshToken = string.Empty;
                string authEndpoint = string.Empty;

                if (clientName == null || platformName == null)
                {
                    return null;
                }

                if (clientName.Equals(PinjarraBakeryConstants.PinjarraBakery,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = PinjarraBakeryConstants.ZohoAuthEndpoint;
                    clientId = PinjarraBakeryConstants.ZohoClientId;
                    clientSecret = PinjarraBakeryConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoProjects)
                    {
                        refreshToken = PinjarraBakeryConstants.ZohoProjects_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = PinjarraBakeryConstants.ZohoCRM_RefreshToken;
                    }
                }
                else if (clientName.Equals(GoSunnySolarConstants.GoSunnySolar,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = GoSunnySolarConstants.ZohoAuthEndpoint;
                    clientId = GoSunnySolarConstants.ZohoClientId;
                    clientSecret = GoSunnySolarConstants.ZohoClientSecret;
                    refreshToken = GoSunnySolarConstants.ZohoCRM_RefreshToken;
                }
                else if (clientName.Equals(REOConstants.RestaurantEquipmentOnline,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = REOConstants.ZohoAuthEndpoint;
                    clientId = REOConstants.ZohoClientId;
                    clientSecret = REOConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = REOConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoDesk)
                    {
                        refreshToken = REOConstants.ZohoDesk_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoAnalytics)
                    {
                        refreshToken = REOConstants.ZohoAnalytics_RefreshToken;
                    }
                }
                else if (clientName.Equals(OneCorpConstants.OneCorp,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        authEndpoint = OneCorpConstants.ZohoAuthEndpoint;
                        clientId = OneCorpConstants.ZohoClientId;
                        clientSecret = OneCorpConstants.ZohoClientSecret;
                        refreshToken = OneCorpConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoProjects)
                    {
                        authEndpoint = OneCorpConstants.ZohoAuthEndpoint;
                        clientId = OneCorpConstants.ZohoClientId;
                        clientSecret = OneCorpConstants.ZohoClientSecret;
                        refreshToken = OneCorpConstants.ZohoProjects_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoSign)
                    {
                        authEndpoint = OneCorpConstants.ZohoAuthEndpoint;
                        clientId = OneCorpConstants.ZohoClientId;
                        clientSecret = OneCorpConstants.ZohoClientSecret;
                        refreshToken = OneCorpConstants.ZohoSign_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoWorkDrive)
                    {
                        authEndpoint = OneCorpConstants.ZohoAuthEndpoint;
                        clientId = OneCorpConstants.ZohoClientId;
                        clientSecret = OneCorpConstants.ZohoClientSecret;
                        refreshToken = OneCorpConstants.ZohoWorkdrive_RefreshToken;
                    }
                }
                else if (clientName.Equals(LumicareConstants.Lumicare,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = LumicareConstants.ZohoAuthEndpoint;
                    clientId = LumicareConstants.ZohoClientId;
                    clientSecret = LumicareConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = LumicareConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoWorkDrive)
                    {
                        refreshToken = LumicareConstants.ZohoWorkdrive_RefreshToken;
                    }
                }
                else if (clientName.Equals(ZipfoxConstants.Zipfox,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = ZipfoxConstants.ZohoAuthEndpoint;
                    clientId = ZipfoxConstants.ZohoClientId;
                    clientSecret = ZipfoxConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = ZipfoxConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoDesk)
                    {
                        refreshToken = ZipfoxConstants.ZohoDesk_RefreshToken;
                    }
                }
                else if (clientName.Equals(OrattoConstants.Oratto,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = OrattoConstants.ZohoAuthEndpoint;
                    clientId = OrattoConstants.ZohoClientId;
                    clientSecret = OrattoConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = OrattoConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoMail)
                    {
                        clientId = OrattoConstants.ZohoMail_ClientId;
                        clientSecret = OrattoConstants.ZohoMail_ClientSecret;
                        refreshToken = OrattoConstants.ZohoMail_RefreshToken;
                    }
                }
                else if (clientName.Equals(EnvioCoreConstants.EnvioCore,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = EnvioCoreConstants.ZohoAuthEndpoint;
                    clientId = EnvioCoreConstants.ZohoClientId;
                    clientSecret = EnvioCoreConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = EnvioCoreConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoFSM)
                    {
                        refreshToken = EnvioCoreConstants.ZohoFSM_RefreshToken;
                    }
                }
                else if (clientName.Equals(MarcoInteriorsConstants.MarcoInteriors,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = MarcoInteriorsConstants.ZohoAuthEndpoint;
                    clientId = MarcoInteriorsConstants.ZohoClientId;
                    clientSecret = MarcoInteriorsConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = MarcoInteriorsConstants.ZohoCRM_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoPeople)
                    {
                        refreshToken = MarcoInteriorsConstants.ZohoPeople_RefreshToken;
                    }
                }
                else if (clientName.Equals(LegendaryFundingConstants.LegendaryFundingGroup,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = LegendaryFundingConstants.ZohoAuthEndpoint;
                    clientId = LegendaryFundingConstants.ZohoClientId;
                    clientSecret = LegendaryFundingConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoWorkDrive)
                    {
                        refreshToken = LegendaryFundingConstants.ZohoWorkDrive_RefreshToken;
                    }
                }
                else if (clientName.Equals(CascadiaWebServicesConstants.CascadiaWebServices,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = CascadiaWebServicesConstants.ZohoAuthEndpoint;
                    clientId = CascadiaWebServicesConstants.ZohoClientId;
                    clientSecret = CascadiaWebServicesConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoRecruit)
                    {
                        refreshToken = CascadiaWebServicesConstants.ZohoRecruit_RefreshToken;
                    }
                }
                else if (clientName.Equals(SpacificConstants.Spacific,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = SpacificConstants.ZohoAuthEndpoint;
                    clientId = SpacificConstants.ZohoClientId;
                    clientSecret = SpacificConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoProjects)
                    {
                        refreshToken = SpacificConstants.ZohoProjects_RefreshToken;
                    }
                }
                else if (clientName.Equals(WclConstants.WCLSolutions,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = WclConstants.ZohoAuthEndpoint;
                    clientId = WclConstants.ZohoClientId;
                    clientSecret = WclConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoInventory)
                    {
                        refreshToken = WclConstants.ZohoInventory_RefreshToken;
                    }
                }
                else if (clientName.Equals(OneBudgetConstants.OneBudget,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = OneBudgetConstants.ZohoAuthEndpoint;
                    clientId = OneBudgetConstants.ZohoClientId;
                    clientSecret = OneBudgetConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = OneBudgetConstants.ZohoCRM_RefreshToken;
                    }
                }
                else if (clientName.Equals(GetunikConstants.GetUnik,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = GetunikConstants.ZohoAuthEndpoint;
                    clientId = GetunikConstants.ZohoClientId;
                    clientSecret = GetunikConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = GetunikConstants.ZohoCRM_RefreshToken;
                    }
                    if (platformName == CommonConstants.ZohoBooks)
                    {
                        refreshToken = GetunikConstants.ZohoBooks_RefreshToken;
                    }
                    if (platformName == CommonConstants.ZohoProjects)
                    {
                        refreshToken = GetunikConstants.ZohoProjects_RefreshToken;
                    }
                }
                else if (clientName.Equals(SignaramaConstants.Signarama,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = SignaramaConstants.ZohoAuthEndpoint;
                    clientId = SignaramaConstants.ZohoClientId;
                    clientSecret = SignaramaConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoWorkDrive)
                    {
                        refreshToken = SignaramaConstants.ZohoWorkDrive_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoBooks)
                    {
                        refreshToken = SignaramaConstants.ZohoBooks_RefreshToken;
                    }
                }
                else if (clientName.Equals(ZoRawConstants.ZoRawChocolates,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = ZoRawConstants.ZohoAuthEndpoint;
                    clientId = ZoRawConstants.ZohoClientId;
                    clientSecret = ZoRawConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoInventory)
                    {
                        refreshToken = ZoRawConstants.ZohoInventory_RefreshToken;
                    }
                    else if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = ZoRawConstants.ZohoCRM_RefreshToken;
                    }
                }
                else if (clientName.Equals(MetroManhattanConstants.MetroManhattan,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    authEndpoint = MetroManhattanConstants.ZohoAuthEndpoint;
                    clientId = MetroManhattanConstants.ZohoClientId;
                    clientSecret = MetroManhattanConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = MetroManhattanConstants.ZohoCRM_RefreshToken;
                    }
                }
                else if (clientName.Equals(GGInsuranceConstants.GGInsurance))
                {
                    authEndpoint = GGInsuranceConstants.ZohoAuthEndpoint;
                    clientId = GGInsuranceConstants.ZohoClientId;
                    clientSecret = GGInsuranceConstants.ZohoClientSecret;
                    if (platformName == CommonConstants.ZohoCRM)
                    {
                        refreshToken = GGInsuranceConstants.ZohoCRM_RefreshToken;
                    }
                }

                if (string.IsNullOrWhiteSpace(authEndpoint) ||
                    string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return null;
                }

                string cacheKey = $"zoho-access-token:{clientName}:{platformName}";
                if (_cache.TryGetValue(cacheKey, out string cachedAccessToken))
                {
                    return cachedAccessToken;
                }

                using var request = new HttpRequestMessage(HttpMethod.Post, authEndpoint)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["refresh_token"] = refreshToken,
                        ["client_id"] = clientId,
                        ["client_secret"] = clientSecret,
                        ["grant_type"] = "refresh_token"
                    })
                };
                var httpClient = _httpClientFactory.CreateClient();
                using var response = await httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<ZohoTokenResponse>(responseData);

                string accessToken = responseObj?.access_token;
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    var lifetime = responseObj.expires_in > 120
                        ? TimeSpan.FromSeconds(responseObj.expires_in - 60)
                        : TimeSpan.FromMinutes(30);
                    _cache.Set(cacheKey, accessToken, lifetime);
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
