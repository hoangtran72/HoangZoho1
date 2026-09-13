using Newtonsoft.Json;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{
    public class LeadForUpdation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Business_Duration_Setup_Stage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Intended_Use_Dishes_or_Services { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Brand_Preference { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Priority_Delivery_or_Price { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Urgent_Delivery_Option { get; set; }

    }
}
