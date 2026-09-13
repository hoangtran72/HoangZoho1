using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class GetRecordAttachmentsResponse
    {

        public int? code { get; set; }
        
        public string message { get; set; }
        
        public List<RecordDocument> documents { get; set; }
    
    }

    public class RecordDocument
    {
        
        public bool? can_send_in_mail { get; set; }
        
        public bool? is_custom_field_document { get; set; }
        
        public string file_name { get; set; }
        
        public int? attachment_order { get; set; }
        
        public string source { get; set; }
        
        public string document_id { get; set; }
        
        public string file_size { get; set; }
        
        public string source_formatted { get; set; }
        
        public string uploaded_by { get; set; }
        
        public string file_type { get; set; }
        
        public string file_size_formatted { get; set; }
        
        public string uploaded_on { get; set; }
        
        public string uploaded_by_id { get; set; }
        
        public string alter_text { get; set; }
        
        public string uploaded_on_date_formatted { get; set; }
    
    }

}
