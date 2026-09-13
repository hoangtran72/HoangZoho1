using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{
    public class GetSakariTokenRequest
    {

        public GetSakariTokenRequest()
        {

            grant_type = "client_credentials";
        
        }

        public string grant_type { get; set; }

        public string client_id { get; set; }

        public string client_secret { get; set; }

    }
}
