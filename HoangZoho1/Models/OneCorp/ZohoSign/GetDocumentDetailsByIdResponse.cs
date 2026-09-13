using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoSign
{

    public class GetDocumentDetailsByIdResponse
    {

        public int? code { get; set; }

        public DocumentRequests requests { get; set; }

        public string message { get; set; }

        public string status { get; set; }

    }

    public class DocumentRequests
    {

        public string request_status { get; set; }

        public string notes { get; set; }

        public object[] attachments { get; set; }

        public int? reminder_period { get; set; }

        public string owner_id { get; set; }

        public string description { get; set; }

        public string request_name { get; set; }

        public long? modified_time { get; set; }

        public long? action_time { get; set; }

        public bool? is_deleted { get; set; }

        public int? expiration_days { get; set; }

        public bool? is_sequential { get; set; }

        public long? sign_submitted_time { get; set; }

        public string owner_first_name { get; set; }

        public decimal? sign_percentage { get; set; }

        public long? expire_by { get; set; }

        public string owner_email { get; set; }

        public long? created_time { get; set; }

        public bool? email_reminders { get; set; }

        public bool? self_sign { get; set; }

        public DocumentFields[] document_fields { get; set; }

        public bool? in_process { get; set; }

        public decimal? validity { get; set; }

        public string request_type_name { get; set; }

        public VisibleSignSettings visible_sign_settings { get; set; }

        public string request_id { get; set; }

        public string zsdocumentid { get; set; }

        public string request_type_id { get; set; }

        public string owner_last_name { get; set; }

        public string custom_data { get; set; }

        public DocumentAction[] actions { get; set; }

        public int? attachment_size { get; set; }

    }

    public class VisibleSignSettings
    {

        public bool? visible_sign { get; set; }

        public bool? allow_reason_visible_sign { get; set; }

    }

    public class DocumentPage
    {

        public string image_string { get; set; }

        public int? page { get; set; }

        public bool? is_thumbnail { get; set; }

    }

    public class DocumentFields
    {

        public string document_id { get; set; }

        public object[] fields { get; set; }

    }

    public class DocumentAction
    {

        public bool? verify_recipient { get; set; }

        public string recipient_countrycode_iso { get; set; }

        public string action_type { get; set; }

        public string private_notes { get; set; }

        public string cloud_provider_name { get; set; }

        public bool? has_payment { get; set; }

        public string recipient_email { get; set; }

        public bool? send_completed_document { get; set; }

        public bool? allow_signing { get; set; }

        public string recipient_phonenumber { get; set; }

        public bool? is_bulk { get; set; }

        public string action_id { get; set; }

        public bool? is_revoked { get; set; }

        public bool? is_embedded { get; set; }

        public int? cloud_provider_id { get; set; }

        public int? signing_order { get; set; }

        public DocumentField[] fields { get; set; }

        public string recipient_name { get; set; }

        public string delivery_mode { get; set; }

        public string action_status { get; set; }

        public string recipient_countrycode { get; set; }

    }

    public class DocumentField
    {

        public string field_id { get; set; }

        public int? x_coord { get; set; }

        public string field_type_id { get; set; }

        public int? abs_height { get; set; }

        public string field_category { get; set; }

        public string field_label { get; set; }

        public bool? is_mandatory { get; set; }

        public int? page_no { get; set; }

        public string document_id { get; set; }

        public bool? is_draggable { get; set; }

        public string field_name { get; set; }

        public float? y_value { get; set; }

        public int? abs_width { get; set; }

        public string action_id { get; set; }

        public float? width { get; set; }

        public int? y_coord { get; set; }

        public string field_type_name { get; set; }

        public string description_tooltip { get; set; }

        public bool is_resizable { get; set; }

        public float? x_value { get; set; }

        public float? height { get; set; }

        public DocumentTextProperty text_property { get; set; }

        public int? time_zone_offset { get; set; }

        public string time_zone { get; set; }

        public string field_value { get; set; }

        public string date_format { get; set; }

    }

    public class DocumentTextProperty
    {

        public bool? is_italic { get; set; }

        public int? max_field_length { get; set; }

        public bool? is_underline { get; set; }

        public string font_color { get; set; }

        public bool? is_fixed_width { get; set; }

        public int? font_size { get; set; }

        public bool? is_fixed_height { get; set; }

        public bool? is_read_only { get; set; }

        public bool? is_bold { get; set; }

        public string font { get; set; }

    }

}
