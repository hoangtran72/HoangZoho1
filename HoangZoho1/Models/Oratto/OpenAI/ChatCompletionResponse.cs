using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.OpenAI
{

    public class ChatCompletionResponse
    {

        public ChatCompletionResponse()
        {

            choices = new List<Choice>();

        }

        public string id { get; set; }
        
        [JsonProperty("object")]
        public string _object { get; set; }

        public int? created { get; set; }

        public string model { get; set; }
        
        public List<Choice> choices { get; set; }
        
        public OpenAiUsage usage { get; set; }
        
        public string system_fingerprint { get; set; }
    
    }

    public class OpenAiUsage
    {
        
        public int? prompt_tokens { get; set; }
        
        public int? completion_tokens { get; set; }
        
        public int? total_tokens { get; set; }
        
        public Prompt_Tokens_Details prompt_tokens_details { get; set; }
        
        public Completion_Tokens_Details completion_tokens_details { get; set; }
    
    }

    public class Prompt_Tokens_Details
    {
        
        public int? cached_tokens { get; set; }
    
    }

    public class Completion_Tokens_Details
    {
        
        public int? reasoning_tokens { get; set; }
    
    }

    public class Choice
    {
        
        public int? index { get; set; }
        
        public Message message { get; set; }
        
        public string finish_reason { get; set; }
    
    }

    public class Message
    {
        
        public string role { get; set; }
        
        public string content { get; set; }
        
        public object refusal { get; set; }
    
    }

}
