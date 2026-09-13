using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Sakari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.SakariAuth
{

    public interface ISakariAuthService
    {

        Task<string> GetSakariToken(string clientId, string clientSecret);

    }

}
