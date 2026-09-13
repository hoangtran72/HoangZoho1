using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoogleAPI
{
    public interface IGoogleAuthService
    {

        Task<string> GetAccessToken(string clientName);

    }
}
