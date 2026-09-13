using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Services.ZohoAuth;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.RestaurantOnline
{

    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private readonly IZohoAuthService _zohoAuthService;

        public CommonController(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        [HttpGet("sleep/{time}")]
        public async Task<IActionResult> Sleep(string time)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.OK,
                Message = CommonConstants.MSG_200,
            };

            try
            {
                if (!int.TryParse(time, out int delay))
                {
                    delay = 1000;
                }

                // Optional safety limit (VERY IMPORTANT)
                delay = Math.Min(delay, 3000); // max 5 seconds

                await Task.Delay(delay);

                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = CommonConstants.MSG_400;
                apiResult.Data = $"{ex.Message}";
                return BadRequest(apiResult);
            }
        }


        [HttpPost("extract-meeting-link")]
        public IActionResult ExtractMeetingLink(ExtractMeetingRequest extractMeetingRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.EML_400,
            };

            try
            {

                string description = extractMeetingRequest.Description;

                string meetingLink = StringHelpers.ExtractMeetingLink(description);

                apiResult.Code = 0;
                apiResult.Message = CommonConstants.EML_200;
                apiResult.Data = meetingLink;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                return Ok(apiResult);
            }
        }

        [HttpPost("extract-first-number")]
        public IActionResult ExtractFirstNumber(ExtractFirstNumberRequest extractRequest)
        {

            try
            {

                string invoiceName = extractRequest.InvoiceName;
                
                string invoiceNumber = StringHelpers.ExtractInvoiceNumber(invoiceName);

                return Ok(invoiceNumber);

            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpPost("convert-number")]
        public IActionResult ConvertNumber2Currency(ConvertNumberRequest numberRequest)
        {

            string result = "0";
            try
            {

                var number = numberRequest.number;
                string currency = numberRequest.currency;

                if (number.HasValue)
                {
                    result = StringHelpers.ConvertNumberToFormat(number.Value, currency);
                }    
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(result);
            }
        }

        #region DateTime

        [HttpPost("get-first-and-last-day-of-month")]
        public IActionResult GetFirstAndLastDayOfMonth
            ([FromBody] GetFirstAndLastDayOfMonthRequest request)
        {
            var apiResult = new ApiResultDto<GetFirstAndLastDayOfMonthResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GFLM_400,
            };
            try
            {
                string dateStr = request.Date;

                bool canParse = DateTime.TryParseExact(dateStr, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime parsedDate);

                if (!canParse)
                {
                    apiResult.Code = ResultCode.BadRequest;
                    apiResult.Message = CommonConstants.MSG_400;
                    return BadRequest(apiResult);
                }

                var response = DateTimeHelpers.GetFirstAndLastDayOfMonth(parsedDate);
                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.GFLM_200;
                apiResult.Data = response;

                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("convert-timestamp")]
        public IActionResult ConvertTimeStamp([FromBody] ConvertTimeStampRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = CommonConstants.CTS_200,
            };
            try
            {

                string text = DateTimeHelpers.UnixTimeStampToDateTime(request);
                apiResult.Data = text;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = CommonConstants.MSG_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("adjust-zoho-time")]
        public IActionResult AdjustZohoTime([FromBody] AdjustZohoTimeRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.AZT_400,
            };
            try
            {

                string text = DateTimeHelpers.AdjustZohoTime(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.AZT_200;
                apiResult.Data = text;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("convert-zoho-time-to-text")]
        public IActionResult ConvertZohoTime2Text(
            [FromBody] ConvertZohoTime2Text request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CZ2T_400,
            };
            try
            {

                string text = DateTimeHelpers.ConvertZohoTimeToText(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CZ2T_200;
                apiResult.Data = text;

                return Ok(apiResult);

            }
            catch (Exception)
            {

                return BadRequest(apiResult);
            }

        }

        [HttpPost("convert-zoho-time-to-unix-epoch")]
        public IActionResult ConvertZohoTime2UnixEpoch(
            [FromBody] ConvertZohoTime2UnixEpochRequest request)
        {

            var apiResult = new ApiResultDto<long>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CZ2U_400,
            };
            try
            {

                long unixEpoch = DateTimeHelpers.ZohoTimeToUnixEpoch(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CZ2U_200;
                apiResult.Data = unixEpoch;

                return Ok(apiResult);

            }
            catch (Exception)
            {
                
                return BadRequest(apiResult);
            }

        }

        [HttpPost("get-current-time")]
        public IActionResult GetCurrentTime(GetCurrentTimeRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GCT_400,
            };

            try
            {

                string dateTimeNow = DateTimeHelpers.GetCurrentTime(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.GCT_200;
                apiResult.Data = dateTimeNow;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("get-middle-date")]
        public IActionResult GetMiddleDate(GetMiddleDateRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GCT_400,
            };

            try
            {

                string middleDate = DateTimeHelpers.GetMiddleDate(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.GCT_200;
                apiResult.Data = middleDate;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("get-utc-offset")]
        public IActionResult GetUtcOffset(GetUtcOffsetRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GUO_400,
            };

            try
            {

                string timeZoneId = request.TimeZoneId;

                string utcOffset = DateTimeHelpers.GetUtcOffset(timeZoneId);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.GCT_200;
                apiResult.Data = utcOffset;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("convert-seconds-to-text")]
        public IActionResult ConvertSeconds2Text(
            ConvertSeconds2TextRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CS2T_400,
            };

            try
            {

                int totalSeconds = 0;
                if (request.TotalSeconds.HasValue)
                {
                    totalSeconds = request.TotalSeconds.Value;
                }

                string text = StringHelpers.ConvertSecondsToText(totalSeconds);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CS2T_200;
                apiResult.Data = text;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("process-name")]
        public IActionResult ProcessCustomerName(
            ProcessNameRequest request)
        {

            var apiResult = new ApiResultDto<ProcessNameResponse>()
            {
                Code = ResultCode.OK,
                Message = CommonConstants.MSG_200,
            };

            string firstName = request.FirstName;
            string lastName = request.LastName;
            string email = request.Email;

            var (cleanFirst, cleanLast) = 
                StringHelpers.GetBetterName(firstName, lastName, email);

            var processNameResponse = new ProcessNameResponse()
            {
                CleanFirstName = cleanFirst,
                CleanLastName = cleanLast
            };

            apiResult.Data = processNameResponse;

            return Ok(apiResult);

        }

        #endregion

        #region File Helpers

        [HttpPost("convert-label")]
        public IActionResult ConvertLabelToFile
            ([FromBody] ConvertLabelRequest convertLabelRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CL2P_400
            };

            try
            {

                string label = convertLabelRequest.Label;
                string outputPath = convertLabelRequest.OutputPath;

                FileHelpers.SaveBase64PdfToFile(label, outputPath);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CL2P_200;

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }

            }
            catch (Exception ex)
            {
                
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            
            }

        }

        #endregion

        #region HTML

        [HttpPost("scrape")]
        public async Task<IActionResult> ScrapeWebsite()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.SW_400,
            };

            try
            {

                // var url = "https://www.zillow.com/homes/2469%20S.%20Kittredge%20Way,%20Aurora,%20CO%2080013";
                // var web = new HtmlWeb();
                // web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36";

                using var reader = new StreamReader(HttpContext.Request.Body);
                var body = await reader.ReadToEndAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(body);

                var value = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]");

                return Ok(value);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("html-to-text")]
        public async Task<IActionResult> ConvertToPlainText()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CH2T_400,
            };

            try
            {

                // var url = "https://www.zillow.com/homes/2469%20S.%20Kittredge%20Way,%20Aurora,%20CO%2080013";
                // var web = new HtmlWeb();
                // web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36";

                using var reader = new StreamReader(HttpContext.Request.Body);
                var body = await reader.ReadToEndAsync();

                string text = HtmlUtilities.ConvertToPlainText(body);

                return Ok(text);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("check-valid-url")]
        public IActionResult CheckValidUrl([FromBody] CheckUrlRequest request)
        {

            var apiResult = new ApiResultDto<bool>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CVU_400,
            };

            try
            {

                string inputUrl = request.InputUrl;

                bool isValid = StringHelpers.IsValidURL(inputUrl);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CVU_200;
                apiResult.Data = isValid;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("get-page-title")]
        public async Task<IActionResult> GetPageTitle([FromBody] CheckUrlRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GPT_400,
            };

            try
            {

                string inputUrl = request.InputUrl;

                string pageTitle = await StringHelpers.GetPageTitleAsync(inputUrl);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.GPT_200;
                apiResult.Data = pageTitle;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        [HttpPost("clean-url")]
        public IActionResult CleanUrl([FromBody] CheckUrlRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CU_400,
            };

            try
            {

                string inputUrl = request.InputUrl;

                string cleanUrl = StringHelpers.CleanUrl(inputUrl);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.CU_200;
                apiResult.Data = cleanUrl;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }

        }

        #endregion

        #region String Helpers

        [HttpPost("convert-to-title-case")]
        public IActionResult Convert2TitleCase(ConvertTextRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.EJS_400,
            };

            try
            {

                string jsonString = request.InputText;
                string convertedText = StringHelpers.ToTitleCase(jsonString);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.EJS_200;
                apiResult.Data = convertedText;

                return Ok(apiResult);

            }
            catch (Exception)
            {

                return BadRequest(apiResult);

            }

        }

        #endregion

        [HttpPost("extract-json-data")]
        public IActionResult ExtractJsonData(ExtractJsonDataRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.EJS_400,
            };

            try
            {

                string jsonString = request.JsonString;
                string readableText = StringHelpers.ExtractJsonData(jsonString);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.EJS_200;
                apiResult.Data = readableText;

                return Ok(apiResult);

            }
            catch (Exception)
            {

                return BadRequest(apiResult);

            }

        }

        [HttpPost("clean-php-text")]
        public IActionResult CleanPhpText(CleanPhpTextRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.CPT_400,
            };

            try
            {

                string phpText = request.PhpText;
                string readableText = StringHelpers.CleanPhpText(phpText);

                if (!string.IsNullOrEmpty(readableText))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.CPT_200;
                    apiResult.Data = readableText;
                }

                return Ok(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);

            }

        }

        #region Address

        [HttpPost("canadian-postal-code")]
        public IActionResult FormatCanadianPostalCode
            (FormatCanadianPostalCodeRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.FCPC_400,
            };

            try
            {

                string postalCode = request.PostalCode;
                string formattedPostalCode = StringHelpers
                    .FormatCanadianPostalCode(postalCode);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.FCPC_200;
                apiResult.Data = formattedPostalCode;

                return Ok(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);

            }

        }

        #endregion

        [HttpPost("jaro-winkler-distance")]
        public IActionResult JaroWinklerDistance(JaroWinklerRequest request)
        {

            var apiResult = new ApiResultDto<double>()
            {
                Code = ResultCode.OK,
                Message = CommonConstants.JWD_200,
            };

            try
            {

                string text1 = request.Text1;
                string text2 = request.Text2;
                double distance = 100 * StringHelpers.Proximity(text1, text2);

                distance = Math.Round(distance, 2);

                apiResult.Data = distance;

                return Ok(apiResult);

            }
            catch (Exception)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = CommonConstants.JWD_400;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("extract-australia-address")]
        public IActionResult ExtractAustralianAddress(ExtractAddressRequest request)
        {

            try
            {

                string fullAddress = request.FullAddress;

                var extractResponse = AddressHelpers
                    .ExtractAustralianAddress(fullAddress);

                return Ok(extractResponse);

            }
            catch (Exception)
            {
                
                return BadRequest();

            }

        }

        [HttpPost("parse-creator-address")]
        public IActionResult ParseCreatorAddress(ExtractAddressRequest request)
        {

            try
            {

                string fullAddress = request.FullAddress;

                var parseCreatorAddress = new ParseCreatorAddress();

                var extractResponse = AddressHelpers
                    .ParseCreatorAddress(fullAddress);

                return Ok(extractResponse);

            }
            catch (Exception)
            {

                return BadRequest();

            }

        }

        [HttpPost("zoho-token")]
        public async Task<IActionResult> GetZohoToken([FromBody] GetZohoTokenRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.GZT_400,
            };

            try
            {

                string clientName = request.ClientName;
                string platformName = request.PlatformName;

                string token = await _zohoAuthService.GetAccessToken(clientName, platformName);

                if (!string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.GZT_200;
                    apiResult.Data = token;
                }

                return Ok(apiResult);

            }
            catch (Exception)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = CommonConstants.JWD_400;
                return BadRequest(apiResult);
            }

        }

    }
}
