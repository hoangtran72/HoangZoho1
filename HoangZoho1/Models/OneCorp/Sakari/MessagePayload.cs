using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{
    public class MessagePayload
    {

        public string accountId { get; set; }

        public string eventType { get; set; }

        public Payload payload { get; set; }

    }

    public class Payload
    {

        public string accountId { get; set; }

        public Conversation conversation { get; set; }

        public PhoneGroup group { get; set; }

        public MessageContact contact { get; set; }

        public Job job { get; set; }

        public string type { get; set; }

        public string message { get; set; }

        public string status { get; set; }

        public bool? outgoing { get; set; }

        public string country { get; set; }

        public decimal? segments { get; set; }

        public decimal? price { get; set; }

        public bool? read { get; set; }

        public bool? operational { get; set; }

        public object[] media { get; set; }

        public string id { get; set; }

        public MessageCreated created { get; set; }

        public MessageUpdated updated { get; set; }

    }

    public class Conversation
    {

        public string id { get; set; }

        public DateTime? closed { get; set; }

        public MessageGroup group { get; set; }

        public MessagePhoneNumber phoneNumber { get; set; }

    }

    public class MessageGroup
    {

        public string id { get; set; }

    }

    public class MessagePhoneNumber
    {

        public string id { get; set; }

        public string number { get; set; }

        public string country { get; set; }

    }

    public class PhoneGroup
    {

        public string id { get; set; }

        public string name { get; set; }

    }

    public class MessageContact
    {

        public string id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public Mobile mobile { get; set; }

        public string email { get; set; }

        public Created created { get; set; }

    }

    public class MessageMobile
    {

        public string number { get; set; }

        public string country { get; set; }

    }

    public class MessageCreated
    {

        public DateTime? at { get; set; }

        public MessageBy by { get; set; }

    }

    public class Job
    {

        public string id { get; set; }

    }

    public class MessageBy
    {

        public string id { get; set; }

        public string name { get; set; }

        public string source { get; set; }

        public string subSource { get; set; }

    }

    public class MessageUpdated
    {

        public DateTime? at { get; set; }

        public MessageBy by { get; set; }

    }
}
