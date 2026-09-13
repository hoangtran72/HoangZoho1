using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZohoAuth
{
    public interface IZohoAuthService
    {
        Task<string> GetAccessToken(string clientName, string platformName);
    }
}
