using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UpsertRequest<T>
    {

        public UpsertRequest()
        {

            data = new List<T>();

            trigger = new List<string>();

        }

        public List<T> data { get; set; }

        public List<string> trigger { get; set; }

    }

}
