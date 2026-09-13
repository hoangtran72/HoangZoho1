using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoWorkdrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{
    public interface IOneCorpWorkdriveService
    {

        Task<ApiResultDto<UploadFileResponse>> UploadFile(UploadFileRequest uploadRequest);

        Task<ApiResultDto<ListFilesFoldersResponse>> ListFilesFolders(string parentId, string filterBy);

        Task<ApiResultDto<SearchAcrossFolderResponse>> SearchAcrossFolder(SearchAcrossFolderRequest searchRequest);

        Task<ApiResultDto<CreateFolderResponse>> CreateFolder(CreateFolderRequest createFolderRequest);

        Task<ApiResultDto<MoveFolderResponse>> MoveFolder(MoveFolderRequest moveFolderRequest);


    }
}
