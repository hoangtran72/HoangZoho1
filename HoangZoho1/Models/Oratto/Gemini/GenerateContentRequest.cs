using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.Gemini
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GenerateContentRequest
    {

        public GenerateContentRequest()
        {

            contents = new List<Content>();

        }

        public List<Content> contents { get; set; }
    
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Content
    {

        public Content()
        {

            parts = new List<Part>();

        }

        public List<Part> parts { get; set; }
    
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Part
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public File_Data file_data { get; set; }
    
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class File_Data
    {

        public string mime_type { get; set; }
        
        public string file_uri { get; set; }
    
    }

}
