using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.XeroAuth
{
    public interface IXeroAuthService
    {
        Task<string> GetAccessToken(string clientName, string companyName);
    }
}
