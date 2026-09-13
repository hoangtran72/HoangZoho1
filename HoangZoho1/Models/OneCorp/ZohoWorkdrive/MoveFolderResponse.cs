using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class MoveFolderResponse
    {

        public MoveFolderData data { get; set; }

    }

    public class MoveFolderData
    {

        public string id { get; set; }

        public string type { get; set; }

        public Attributes attributes { get; set; }

        public Relationships relationships { get; set; }

        public LinksSelf links { get; set; }

    }

}
