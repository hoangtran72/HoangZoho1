using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.ZohoProjects;
using HoangZoho1.Models.GetUnik.ZohoBooks;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Getunik
{
    
    public interface IGetunikProjectsService
    {

        Task<ApiResultDto<GetProjectByIdResponse>> GetProjectById(string projectId);

        Task<ApiResultDto<GetProjectTasklistsResponse>> GetProjectTasklists(string projectId);

        Task<ApiResultDto<GetProjectUsersResponse>> GetProjectUsers();

        Task<ApiResultDto<CreateTaskResponse>> CreateTask(string projectId, CreateTaskRequest createTaskRequest);

    }

}
