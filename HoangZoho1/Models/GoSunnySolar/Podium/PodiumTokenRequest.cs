using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.Podium
{

    public class PodiumTokenRequest
    {

        public string refresh_token { get; set; }

        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string grant_type { get; set; }

    }

}
