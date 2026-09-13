using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.Podium
{

    public class PodiumTokenResponse
    {

        public string access_token { get; set; }
        
        public int? expires_in { get; set; }
        
        public string refresh_token { get; set; }
        
        public string scope { get; set; }
        
        public string token_type { get; set; }
    
    }


}
