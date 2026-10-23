using Newtonsoft.Json;

namespace HoangZoho1.Models.LocalingTours
{
    public class DrChipsRecord
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("Post_Tour_Image_Uploaded")]
        public bool Post_Tour_Image_Uploaded { get; set; }

        [JsonProperty("WorkDrive_Folder_ID")]
        public string WorkDrive_Folder_ID { get; set; }
    }
}
