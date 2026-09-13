using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.PinjarraBakery
{
    public interface IPinjarraProjectService
    {
        #region Projects

        Task<ApiResultDto<GetProjectsResponse>> GetProjectDetails(string projectId);

        #endregion

        #region Tasks

        Task<ApiResultDto<GetTasksResponse>> GetAllTasksInProject(string projectId);

        Task<ApiResultDto<GetTasksResponse>> GetTaskById(string projectId, string taskId);

        Task<ApiResultDto<CreateTaskResponse>> UpdateTask(string projectId, string taskId, TaskForUpdation taskForUpdation);

        Task<ApiResultDto<AddCommentResponse>> AddComment(string projectId, string taskId, AddCommentRequest addComment);

        #endregion

    }
}
