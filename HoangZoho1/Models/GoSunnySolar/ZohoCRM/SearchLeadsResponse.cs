using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.ZohoCRM
{

    public class SearchLeadsResponse
    {
        public LeadDetail[] data { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public int? per_page { get; set; }
        public int? count { get; set; }
        public int? page { get; set; }
        public bool? more_records { get; set; }
    }
}
