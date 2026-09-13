using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GetUnik.ZohoCRM
{

    public class GetTemporaryRecordByIdResponse
    {
        public TemporaryRecordData[] data { get; set; }
    }

    public class TemporaryRecordData
    {

        public Owner Owner { get; set; }
        
        public string currency_symbol { get; set; }
        
        //public object field_states { get; set; }
        
        public Review_Process review_process { get; set; }

        public Layout_Id layout_id { get; set; }

        //public string sharing_permission { get; set; }
        
        public string Name { get; set; }
        
        public object Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }
        
        //public object review { get; set; }
        
        //public object Unsubscribed_Mode { get; set; }
        
        //public bool process_flow { get; set; }
        
        public string Related_Function { get; set; }
        
        //public bool locked_for_me { get; set; }
        
        public string id { get; set; }
        
        //public object zia_visions { get; set; }
        
        public string Temporary_Record_Status { get; set; }
        
        //public Approval approval { get; set; }
        
        public DateTime? Modified_Time { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        //public object Unsubscribed_Time { get; set; }
        
        //public object wizard_connection_path { get; set; }
        
        //public bool editable { get; set; }
        
        //public string Record_Status__s { get; set; }
        
        //public bool orchestration { get; set; }
        
        //public bool in_merge { get; set; }
        
        //public bool Locked__s { get; set; }
        
        public string Payload { get; set; }

        public string Payload_2 { get; set; }

        public Created_By Created_By { get; set; }
        
        public object[] Tag { get; set; }
        
        //public string zia_owner_assignment { get; set; }
        
        //public string approval_state { get; set; }
        
        //public bool pathfinder { get; set; }
        public string Temporary_Record_Id { get; set; }
    
    }

    public class Owner
    {

        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }
    
    }

    public class Review_Process
    {

        public bool? approve { get; set; }
        
        public bool? reject { get; set; }
        
        public bool? resubmit { get; set; }
    
    }

    public class Layout_Id
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Modified_By
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }
    
    }

    public class Approval
    {
        
        public bool? _delegate { get; set; }
        
        public bool? takeover { get; set; }
        
        public bool? approve { get; set; }
        
        public bool? reject { get; set; }
        
        public bool? resubmit { get; set; }
    
    }

    public class Created_By
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }

    }

}
