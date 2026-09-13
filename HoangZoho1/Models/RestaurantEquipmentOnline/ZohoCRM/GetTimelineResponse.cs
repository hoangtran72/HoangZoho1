using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetTimelineResponse
    {

        public __Timeline[] __timeline { get; set; }
        
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

    public class __Timeline
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
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public Path_Finder path_finder { get; set; }
        
        public Workflow workflow { get; set; }
    
    }

    public class Rule
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public State state { get; set; }
    
    }

    public class State
    {
        
        public Field field { get; set; }
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Field
    {
        
        public string api_name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Path_Finder
    {
        
        public bool? process_entry { get; set; }
        
        public bool? process_exit { get; set; }
        
        public State1 state { get; set; }
    
    }

    public class State1
    {
        
        public string trigger_type { get; set; }
        
        public string name { get; set; }
        
        public bool? is_last_state { get; set; }
        
        public string id { get; set; }
    
    }

    public class Workflow
    {
        
        public Field_Update_Action[] field_update_action { get; set; }
    
    }

    public class Field_Update_Action
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Record
    {
        
        public Module module { get; set; }
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Field_History
    {
        
        public _Value _value { get; set; }
        
        public string id { get; set; }
        
        public string api_name { get; set; }
    
    }

    public class _Value
    {
        
        public string _new { get; set; }
        
        public object old { get; set; }
    
    }

}
