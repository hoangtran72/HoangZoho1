using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.GHL
{

    public class GetContactByIdResponse
    {
        public Contact contact { get; set; }
    }

    public class Contact
    {
        public string id { get; set; }
        public string locationId { get; set; }
        public string firstName { get; set; }
        public string address1 { get; set; }
        public object[] attachments { get; set; }
        public string timezone { get; set; }
        public DateTime dateAdded { get; set; }
        public string state { get; set; }
        public string emailLowerCase { get; set; }
        public string fingerprint { get; set; }
        public string postalCode { get; set; }
        public string fullNameLowerCase { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public string lastNameLowerCase { get; set; }
        public string firstNameLowerCase { get; set; }
        public string assignedTo { get; set; }
        public string[] tags { get; set; }
        public string lastName { get; set; }
        public string type { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string website { get; set; }
        public string source { get; set; }
        public CustomField[] customField { get; set; }
        public AttributionSource attributionSource { get; set; }
        public LastAttributionSource lastAttributionSource { get; set; }
    }

    public class AttributionSource
    {
        public object gclid { get; set; }
        public object msclkid { get; set; }
        public string sessionSource { get; set; }
        public object utmMedium { get; set; }
        public object ip { get; set; }
        public object userAgent { get; set; }
        public string url { get; set; }
        public object utmSource { get; set; }
        public object utmTerm { get; set; }
        public object utmContent { get; set; }
        public object referrer { get; set; }
    }

    public class LastAttributionSource
    {
        public string fbEventId { get; set; }
        public object utmContent { get; set; }
        public object utmTerm { get; set; }
        public object referrer { get; set; }
        public object gclid { get; set; }
        public string ip { get; set; }
        public string sessionSource { get; set; }
        public string userAgent { get; set; }
        public object utmSource { get; set; }
        public object msclkid { get; set; }
        public string fbp { get; set; }
        public string url { get; set; }
        public object utmMedium { get; set; }
    }

    public class CustomField
    {
        public string id { get; set; }
        public object value { get; set; }
    }

}
