using DotnetGeminiSDK;
using HoangZoho1.Constants;
using HoangZoho1.Services.Common;
using HoangZoho1.Services.DaviesImagingGroup;
using HoangZoho1.Services.DoAbility;
using HoangZoho1.Services.Getunik;
using HoangZoho1.Services.GetUnik;
using HoangZoho1.Services.GGInsurance;
using HoangZoho1.Services.GoogleAPI;
using HoangZoho1.Services.GoSunnySolar;
using HoangZoho1.Services.LocalingTours;
using HoangZoho1.Services.Lumicare;
using HoangZoho1.Services.MetroManhattan;
using HoangZoho1.Services.OneBudget;
using HoangZoho1.Services.OneCorp;
using HoangZoho1.Services.Oratto;
using HoangZoho1.Services.PinjarraBakery;
using HoangZoho1.Services.RestaurantEquipmentOnline;
using HoangZoho1.Services.SakariAuth;
using HoangZoho1.Services.Signarama;
using HoangZoho1.Services.Twilio;
using HoangZoho1.Services.WclSolutions;
using HoangZoho1.Services.XeroAuth;
using HoangZoho1.Services.Zipfox;
using HoangZoho1.Services.ZohoAuth;
using HoangZoho1.Services.ZoRaw;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddGeminiClient(config =>
            {
                config.ApiKey = EnvironmentConstants.Get("SHARED_GEMINI_API_KEY");
                config.ImageBaseUrl = "";
                config.TextBaseUrl = "";
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder
                                      .WithOrigins("https://lumicare-website--lumicare-preview-snuuwpi1.web.app",
                                        "https://lumicare-website--lumicare-preview-huno2iqo.web.app",
                                        "http://localhost:3000",
                                        "https://lumicare.one",
                                        "https://www.lumicare.one",
                                        // OneCorp
                                        "https://47467cfe-e5d0-412c-b756-3616a69a74db.zappsusercontent.com.au",
                                        "https://9b03e878-6ffc-4fbb-9a79-94efc36d53df.zappsusercontent.com.au",
                                        "https://595c8120-4226-4431-bcdf-e3cc74b78748.zappsusercontent.com.au",
                                        "https://9c15e07f-bd3d-40d2-a2d9-f8e44a2c139f.zappsusercontent.com.au",
                                        "https://981312cc-c27e-4647-ac4c-c695f90233c7.zappsusercontent.com.au",
                                        // Zipfox
                                        "https://a34595f2-20f7-4e78-a6a6-e1037acb2972.zappsusercontent.com",
                                        "https://d5f9f796-c701-43e8-8e45-fb7dfe6d2600.zappsusercontent.com",
                                        // Restaurant Equipment Online
                                        "https://69fe8a36-cdae-4d16-8803-75e88169f77d.zappsusercontent.com",
                                        "https://6ab79ddb-4b87-45a7-902f-2ed7e00f8f3e.zappsusercontent.com",
                                        "https://ef7dd859-f889-48ee-a7d2-6af222d2f385.zappsusercontent.com",
                                        "https://0ad3a75c-f0ec-4c95-b2d7-d8529fba642d.zappsusercontent.com",
                                        "https://4568b61d-f841-4fd8-9867-5864b2aa4592.zappsusercontent.com",
                                        // Oratto
                                        "https://450012c1-a77d-4a09-ba51-e3b3c258d8e4.zappsusercontent.eu",
                                        "https://8a8c587c-c8c2-444f-97dc-227412d7e0b4.zappsusercontent.eu",
                                        // WCL Solution
                                        "https://5fa583af-9a0f-4d40-959f-a5816a5c8f4a.zappsusercontent.eu",
                                        // Go Sunny
                                        "https://c6c9bb3b-c892-4b68-8030-d313016bd9f9.zappsusercontent.com.au",
                                        "https://7282b264-79de-461c-ba32-dac44f949595.zappsusercontent.com.au",
                                        "https://bb1b2417-2c5d-49b3-bbc8-2dc2c4370869.zappsusercontent.com.au",
                                        // ZoRaw Chocolate
                                        "https://cf6d33d2-045b-4556-b585-c723224c8734.zappsusercontent.ca",
                                        "https://zoraw-chocolates.web.app",
                                        // getunik
                                        "https://6b629e60-a7b0-4f4d-b070-acaf7119258f.zappsusercontent.com",
                                        // Davies Imaging Group
                                        "https://frameflow.daviesimaging.com")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            services.AddControllers().AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
                setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Add this line
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddScoped<ITwilioService, TwilioService>();
            services.AddScoped<IZohoAuthService, ZohoAuthService>();
            services.AddScoped<IPodiumAuthService, PodiumAuthService>();
            services.AddScoped<IXeroAuthService, XeroAuthService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IGmailService, GmailService>();

            #region Common Services

            services.AddScoped<ICommonService, CommonService>();

            #endregion

            #region Pinjarra Bakery Services

            services.AddScoped<IPinjarraCrmService, PinjarraCrmService>();
            services.AddScoped<IPinjarraProjectService, PinjarraProjectService>();
            services.AddScoped<IPinjarraCustomService, PinjarraCustomService>();
            
            #endregion

            #region Go Sunny Solar Services
            
            services.AddScoped<ISolarCrmService, SolarCrmService>();
            services.AddScoped<ISolarGhlService, SolarGhlService>();
            services.AddScoped<ISolarPodiumService, SolarPodiumService>();
            services.AddScoped<ISolarCustomService, SolarCustomService>();

            #endregion

            #region Restaurant Equipment Online Services

            services.AddScoped<IReoCrmService, ReoCrmService>();
            services.AddScoped<IReoJustCallService, ReoJustCallService>();
            services.AddScoped<IReoShopifyService, ReoShopifyService>();
            services.AddScoped<IReoTnzService, ReoTnzService>();
            services.AddScoped<IReoCustomService, ReoCustomService>();

            #endregion

            #region OneCorp Services

            services.AddScoped<IOneCorpCrmService, OneCorpCrmService>();
            services.AddScoped<IOneCorpSignService, OneCorpSignService>();
            services.AddScoped<IOneCorpWorkdriveService, OneCorpWorkdriveService>();
            services.AddScoped<IOneCorpOnceHubService, OneCorpOnceHubService>();
            services.AddScoped<IOneCorpProjectsService, OneCorpProjectsService>();
            services.AddScoped<IOneCorpCustomService, OneCorpCustomService>();
            services.AddScoped<IOneCorpSakariService, OneCorpSakariService>();
            services.AddScoped<IOneCorpTwilioService, OneCorpTwilioService>();
            services.AddScoped<ISakariAuthService, SakariAuthService>();

            #endregion

            #region OneBudget Services

            services.AddScoped<IOneBudgetCrmService, OneBudgetCrmService>();
            services.AddScoped<IOneBudgetCustomService, OneBudgetCustomService>();

            #endregion

            #region Talent LMS

            services.AddScoped<ITalentLMSService, TalentLMSService>();

            #endregion

            #region Lumicare Services 

            services.AddScoped<ILumicareCrmService, LumicareCrmService>();
            services.AddScoped<ILumicareCustomService, LumicareCustomService>();

            #endregion

            #region Zipfox Services

            services.AddScoped<IZipfoxWhatsAppService, ZipfoxWhatsAppService>();

            services.AddScoped<IZipfoxCrmService, ZipfoxCrmService>();

            services.AddScoped<IZipfoxDeskService, ZipfoxDeskService>();

            services.AddScoped<IZipfoxCustomService, ZipfoxCustomService>();

            #endregion

            #region Oratto Services

            services.AddScoped<IOrattoCrmService, OrattoCrmService>();

            services.AddScoped<IOrattoMailService, OrattoMailService>();

            services.AddScoped<IOrattoOpenAIService, OrattoOpenAiService>();

            services.AddScoped<IOrattoGeminiService, OrattoGeminiService>();

            services.AddScoped<IOrattoDocumentService, OrattoDocumentService>();

            services.AddScoped<IOrattoCustomService, OrattoCustomService>();

            #endregion

            #region Wcl Services

            services.AddScoped<IWclInventoryService, WclInventoryService>();

            services.AddScoped<IWclWooService, WclWooService>();

            services.AddScoped<IWclCustomService, WclCustomService>();

            #endregion

            #region getunik Services

            services.AddScoped<IGetunikCustomService, GetunikCustomService>();

            services.AddScoped<IGetunikCrmService, GetunikCrmService>();

            services.AddScoped<IGetunikBooksService, GetunikBooksService>();

            services.AddScoped<IGetunikProjectsService, GetunikProjectsService>();

            #endregion

            #region Signarama Novi and Flint Services

            services.AddScoped<ISignaramaWorkDriveService, SignaramaWorkDriveService>();

            services.AddScoped<ISignaramaGeminiService, SignaramaGeminiService>();

            services.AddScoped<ISignaramaBooksService, SignaramaBooksService>();

            services.AddScoped<ISignaramaCustomService, SignaramaCustomService>();

            #endregion

            #region ZoRaw Services

            services.AddScoped<IZoRawStallionService, ZoRawStallionService>();

            services.AddScoped<IZoRawCrmService, ZoRawCrmService>();

            services.AddScoped<IZoRawInventoryService, ZoRawInventoryService>();

            services.AddScoped<IZoRawFreightcomService, ZoRawFreightcomService>();

            services.AddScoped<IZoRawCustomService, ZoRawCustomService>();

            #endregion

            #region DoAbility Services

            services.AddScoped<IGoogleAdsService, GoogleAdsService>();

            #endregion

            #region Localing Tours Services

            services.AddScoped<ILocalingToursCustomService, LocalingToursCustomService>();

            #endregion

            #region MetroManhattan Services

            services.AddScoped<IMetroManhattanLlmService, MetroManhattanLlmService>();

            #endregion

            #region GG-Insurance Services

            services.AddScoped<IGGInsuranceCrmService, GGInsuranceCrmService>();

            services.AddScoped<IGGInsuranceCustomService, GGInsuranceCustomService>();

            services.AddScoped<IRingCentralService, RingCentralService>();

            #endregion

            #region Davies Imaging Group

            services.AddScoped<IDigCustomService, DigCustomService>();

            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hoang ZOHO #1 API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "Hoang ZOHO #1 API");
                    // serve UI at root
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
