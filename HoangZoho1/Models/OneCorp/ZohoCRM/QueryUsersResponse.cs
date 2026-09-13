using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class QueryUsersResponse
    {

        public UserData[] data { get; set; }

        public UserInfo info { get; set; }

    }

    public class UserData
    {

        public string id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string time_zone { get; set; }

        public string status { get; set; }

    }

    public class UserInfo
    {

        public int? count { get; set; }

        public bool? more_records { get; set; }

    }

}
