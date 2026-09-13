namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class ListFilesFoldersResponse
    {

        public FileFolderData[] data { get; set; }

    }

    public class FileFolderData
    {

        public string id { get; set; }

        public string type { get; set; }

        public FileFolderAttributes attributes { get; set; }

        public Relationships relationships { get; set; }

        public LinksSelf links { get; set; }

    }

    public class FileFolderAttributes
    {

        public string modified_by_zuid { get; set; }

        public bool? is_locked { get; set; }

        public int? conv_engine_type { get; set; }

        public bool? is_fillable_resource { get; set; }

        public bool? is_published { get; set; }

        public string destination_id { get; set; }

        public Storage_Info storage_info { get; set; }

        public string type { get; set; }

        public string created_time_i18 { get; set; }

        public object[] associated_data_templates { get; set; }

        public long? modified_time_in_millisecond { get; set; }

        public string status_change_time { get; set; }

        public string download_url { get; set; }

        public int? comment_badge_count { get; set; }

        public bool? is_app_associated { get; set; }

        public string created_time { get; set; }

        public int? lock_status { get; set; }

        public bool? is_folder { get; set; }

        public int? resource_type { get; set; }

        public bool? is_email_in_upload { get; set; }

        public string display_attr_name { get; set; }

        public string created_by { get; set; }

        public string display_html_name { get; set; }

        public object[] labels { get; set; }

        public string parent_id { get; set; }

        public string name { get; set; }

        public long? status_change_time_in_millisecond { get; set; }

        public string permalink { get; set; }

        public bool? favorite { get; set; }

        public int? new_badge_count { get; set; }

        public int? status { get; set; }

        public string modified_time_i18 { get; set; }

        public string extn { get; set; }

        public bool? customized { get; set; }

        public View_Pref view_pref { get; set; }

        public string shortcut_link { get; set; }

        public string status_change_time_i18 { get; set; }

        public string description { get; set; }

        public long? uploaded_time_in_millisecond { get; set; }

        public string thumbnail_url { get; set; }

        public string title { get; set; }

        public Files_View_Pref files_view_pref { get; set; }

        public string modified_time { get; set; }

        public string library_id { get; set; }

        public string icon_class { get; set; }

        public long? created_time_in_millisecond { get; set; }

        public string owner { get; set; }

        public string creator { get; set; }

        public Capabilities capabilities { get; set; }

        public string uploaded_time_i18 { get; set; }

        public bool? is_external_upload { get; set; }

        public Watch_Preference watch_preference { get; set; }

        public int? opened_time_in_millisecond { get; set; }

        public int? edit_badge_count { get; set; }

        public object[] share_data { get; set; }

        public string uploaded_time { get; set; }

        public bool? has_folders { get; set; }

        public string service_type { get; set; }

        public string display_url_name { get; set; }

        public bool? is_unread { get; set; }

        public string modified_by { get; set; }

        public string creator_avatar_url { get; set; }

        public Embed_Props embed_props { get; set; }

    }

}
