using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CreateContactRequest
    {

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Title { get; set; }

        public string Company_Name { get; set; }

        public string Specialty { get; set; }

        public string Country_select { get; set; }

        public bool? Onboarding_Course_Finished { get; set; }

    }
}
