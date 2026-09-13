using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class UpsertContactResponse
    {

        public bool success { get; set; }

        public UpsertContactData data { get; set; }

    }

    public class UpsertContactData
    {

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public ContactTag[] tags { get; set; }

        public bool valid { get; set; }

        public object error { get; set; }

        public string keyId { get; set; }

        public string id { get; set; }

        public ContactMobile mobile { get; set; }

        public Created created { get; set; }

        public Updated updated { get; set; }

        public object[] lists { get; set; }

    }

}
