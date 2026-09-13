using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshExpiredTime { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessExpiredTime { get; set; }
    }
}
