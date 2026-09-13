using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    public class ConvertTimeStampRequest
    {

        public double? TimeStamp { get; set; }

        public int? Type { get; set; }

        public string TimeZone { get; set; }

        public string ReturnedFormat { get; set; }

    }

}
