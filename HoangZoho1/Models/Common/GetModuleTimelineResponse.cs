using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    public class GetModuleTimelineResponse
    {

        public Timeline[] __timeline { get; set; }

        public TimelineInfo info { get; set; }

    }

    public class TimelineInfo
    {

        public int? per_page { get; set; }

        public string next_page_token { get; set; }

        public int? count { get; set; }

        public int? page { get; set; }

        public string previous_page_token { get; set; }

        public bool? more_records { get; set; }

    }

    public class Timeline
    {

        public Done_By done_by { get; set; }

        public Related_Record related_record { get; set; }

        public Automation_Details automation_details { get; set; }

        public Record record { get; set; }

        public DateTime? audited_time { get; set; }

        public string action { get; set; }

        public string id { get; set; }

        public string source { get; set; }

        public Field_History[] field_history { get; set; }

    }

    public class Done_By
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Related_Record
    {

        public Module module { get; set; }

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Module
    {

        public string api_name { get; set; }

        public string id { get; set; }

    }

    public class Automation_Details
    {

        public Rule rule { get; set; }

        public string type { get; set; }

    }

    public class Rule
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Record
    {

        public Module module { get; set; }

        public string name { get; set; }

        public string id { get; set; }

        public Recipents recipents { get; set; }

    }

    public class Recipents
    {

        public Cc[] cc { get; set; }

        public To[] to { get; set; }

    }

    public class Cc
    {

        public string email_id { get; set; }

        public string name { get; set; }

        public string id { get; set; }

    }

    public class To
    {

        public string email_id { get; set; }

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Field_History
    {

        public string api_name { get; set; }

        public FieldValue _value { get; set; }

        public string id { get; set; }

    }

    public class FieldValue
    {

        [JsonProperty("new")]
        public string _new { get; set; }

        public string old { get; set; }

    }

}
