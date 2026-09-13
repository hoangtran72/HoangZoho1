using HoangZoho1.Models.GGInsurance;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GGInsurance
{
    public interface IRingCentralService
    {

        public Task<string> SendMmsAsync(RingCentralMmsRequest requestData);

    }
}
