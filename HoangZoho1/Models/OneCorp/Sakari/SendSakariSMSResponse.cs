using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class SendSakariSMSResponse
    {

        public bool? success { get; set; }

        public SendSMSResponseData data { get; set; }

    }

    public class SendSMSResponseData
    {

        public string jobId { get; set; }

        public SakariJob job { get; set; }

        public int? requested { get; set; }

        public int? valid { get; set; }
        
        public decimal? estimatedPrice { get; set; }

        public SakariMessage[] messages { get; set; }

        public int? batches { get; set; }

        public object[] invalid { get; set; }

    }

    public class SakariJob
    {

        public string id { get; set; }

        public decimal? estimatedPrice { get; set; }

        public string status { get; set; }
        
        public decimal? price { get; set; }

        public object[] invalid { get; set; }

        public Created created { get; set; }

        public Updated updated { get; set; }

    }

    public class Created
    {

        public DateTime? at { get; set; }

        public By by { get; set; }

    }

    public class By
    {

        public string name { get; set; }

        public string source { get; set; }

        public string subSource { get; set; }

    }

    public class Updated
    {

        public DateTime? at { get; set; }

        public By by { get; set; }

    }

    public class SakariMessage
    {

        public string id { get; set; }

        public Contact contact { get; set; }

        public MessageJob job { get; set; }

        public int? jobBatch { get; set; }

        public string type { get; set; }

        public string template { get; set; }

        public string status { get; set; }

        public bool? outgoing { get; set; }

        public string country { get; set; }

        public int? segments { get; set; }

        public decimal? price { get; set; }

        public bool? read { get; set; }

        public object[] media { get; set; }

        public SakariConversation conversation { get; set; }

        public Created created { get; set; }
        
        public Updated updated { get; set; }
    
    }

    public class Contact
    {

        public string id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public SakariMobile mobile { get; set; }

        public string email { get; set; }

        public Created created { get; set; }

    }

    public class MessageJob
    {

        public string id { get; set; }

    }

    public class SakariConversation
    {

        public string id { get; set; }
        
        public DateTime? closed { get; set; }

        public PhoneNumber phoneNumber { get; set; }

    }

    public class PhoneNumber
    {

        public string id { get; set; }
        
        public string number { get; set; }
        
        public string country { get; set; }
    
    }

}
