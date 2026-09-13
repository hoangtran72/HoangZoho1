using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetNotesResponse
    {

        public GetNotesResponse()
        {

            data = new List<NoteData>();
        
        }

        public List<NoteData> data { get; set; }
        
        public NoteInfo info { get; set; }
    
    }

    public class NoteInfo
    {

        public int? per_page { get; set; }
        
        public int? count { get; set; }
        
        public int? page { get; set; }
        
        public bool? more_records { get; set; }
    
    }

    public class NoteData
    {

        public NoteOwner Owner { get; set; }
        
        public DateTime? Modified_Time { get; set; }

        public object attachments { get; set; }

        public object field_states { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        public Parent_Id Parent_Id { get; set; }
        
        public bool? editable { get; set; }
        
        public string se_module { get; set; }
        
        public bool? is_shared_to_client { get; set; }
        
        public Modified_By Modified_By { get; set; }
        
        public object size { get; set; }
        
        public string state { get; set; }
        
        public bool? voice_note { get; set; }
        
        public string id { get; set; }
        
        public Created_By Created_By { get; set; }
        
        public string Note_Title { get; set; }
        
        public string Note_Content { get; set; }
    
    }

    public class NoteOwner
    {

        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }
    
    }

    public class Parent_Id
    {

        public string name { get; set; }
        
        public string id { get; set; }
    
    }

}
