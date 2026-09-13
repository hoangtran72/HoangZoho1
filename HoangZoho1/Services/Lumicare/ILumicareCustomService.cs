using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.ZohoCRM;
using HoangZoho1.Models.Lumicare.ZohoForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Lumicare
{
    public interface ILumicareCustomService
    {

        Task<ApiResultDto<string>> HandleTrainingForm(TrainingFormRequest trainingFormRequest);

        Task<ApiResultDto<string>> ApproveClientToEnroll(string contactId);

        Task<ApiResultDto<string>> CheckDistributorCredential(CheckDistributorRequest checkDistributorRequest);

    }
}
