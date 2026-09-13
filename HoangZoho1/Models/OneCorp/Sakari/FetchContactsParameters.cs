using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class FetchContactsParameters
    {

        public int? offset { get; set; }

        public int? limit { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public string tags { get; set; }

    }

}
