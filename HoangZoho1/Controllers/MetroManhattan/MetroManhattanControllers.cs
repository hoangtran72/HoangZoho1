using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.IO;
using HoangZoho1.Services.MetroManhattan;

namespace HoangZoho1.Controllers.MetroManhattan
{

    [Route("api/metro-manhattan")]
    [ApiController]
    public class MetroManhattanControllers : ControllerBase
    {

        private readonly IMetroManhattanLlmService _llmService;

        public MetroManhattanControllers(IMetroManhattanLlmService llmService)
        {
            _llmService = llmService;
        }

        [HttpPost("openai/chat/completions")]
        public async Task<IActionResult> ChatCompletions()
        {
            
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
            };

            using var reader = new StreamReader(Request.Body);

            string userMessage = await reader.ReadToEndAsync();

            apiResult = await _llmService.ChatCompletions(userMessage);

            if (apiResult.Code == ResultCode.OK)
            {
                return Ok(apiResult);
            }    

            return BadRequest(apiResult);

        }

        [HttpPost("claude/messages")]
        public async Task<IActionResult> CreateClaudeMessages()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
            };

            using var reader = new StreamReader(Request.Body);

            string userMessage = await reader.ReadToEndAsync();

            apiResult = await _llmService.CallClaudeAsync(userMessage);

            if (apiResult.Code == ResultCode.OK)
            {
                return Ok(apiResult);
            }

            return BadRequest(apiResult);

        }

    }

}
