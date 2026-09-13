using HoangZoho1.Models.Common;
using System.Threading.Tasks;

namespace HoangZoho1.Services.MetroManhattan
{

    public interface IMetroManhattanLlmService
    {

        Task<ApiResultDto<string>> ChatCompletions(string userContent);

        Task<ApiResultDto<string>> CallClaudeAsync(string userMessage);

    }

}
