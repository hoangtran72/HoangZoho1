using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class GetSakariMessagesResponse
    {

        public GetSakariMessagesResponse()
        {

            data = new List<MessageData>();
        
        }

        public bool? success { get; set; }

        public List<MessageData> data { get; set; }

        public Pagination pagination { get; set; }

    }

    public class MessageData
    {

        public string id { get; set; }

        public Conversation conversation { get; set; }
        
        public MessageGroup group { get; set; }
        
        public Contact contact { get; set; }
        
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
        
        public SakariCreated created { get; set; }

        public SakariUpdated updated { get; set; }

        public int? jobBatch { get; set; }
        
        public string template { get; set; }
        
        public Error error { get; set; }

    
    }

}
