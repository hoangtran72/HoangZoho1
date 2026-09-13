using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoangZoho1.Models.Getunik.ZohoProjects
{

    public class CreateTaskRequest
    {

        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParentalInfo parental_info { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TasklistForCreation tasklist { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TaskStatus status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string priority { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string start_date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string billing_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OwnersAndWork owners_and_work { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string account_number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string billing_method { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? autocreated_from_crm { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zcrm_product_id { get; set; }

    }

    public class TasklistForCreation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? id { get; set; }

    }

    public class TaskStatus
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? id { get; set; }

    }

    public class OwnersAndWork
    {

        public OwnersAndWork()
        {
            
            owners = new List<TaskOwner>();

        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string work_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string unit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? copy_task_duration { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TaskOwner> owners { get; set; }
    
    }

    public class ParentalInfo
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? parent_task_id { get; set; }

    }

    public class TaskOwner
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zpuid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string work_values { get; set; }

    }

}
