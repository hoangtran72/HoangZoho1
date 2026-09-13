using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.OpenAI
{

    public class ChatCompletionRequest
    {

        public ChatCompletionRequest()
        {

            messages = new List<OpenAiMessage>();
        
        }

        public string model { get; set; }
        
        public List<OpenAiMessage> messages { get; set; }
        
        // public decimal? temperature { get; set; }
    
    }

    public class OpenAiMessage
    {

        public string role { get; set; }
        
        public string content { get; set; }
    
    }

}
