namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class CreateFolderResponse
    {

        public CreateFolderData data { get; set; }

    }

    public class CreateFolderData
    {

        public string id { get; set; }

        public string type { get; set; }

        public CreateFolderAttributes attributes { get; set; }

        public Relationships relationships { get; set; }

        public LinksSelf links { get; set; }

    }

    public class CreateFolderAttributes
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

        public string org_id { get; set; }

        public string parent_id { get; set; }

        public string name { get; set; }

        public long status_change_time_in_millisecond { get; set; }

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

        public bool? is_collection_associated { get; set; }

        public Capabilities capabilities { get; set; }

        public string uploaded_time_i18 { get; set; }

        public bool? is_external_upload { get; set; }

        public Watch_Preference watch_preference { get; set; }

        public int? opened_time_in_millisecond { get; set; }

        public int? edit_badge_count { get; set; }

        public object[] share_data { get; set; }

        public string data_template_id_to_mandate { get; set; }

        public string uploaded_time { get; set; }

        public bool? has_folders { get; set; }

        public string service_type { get; set; }

        public string display_url_name { get; set; }

        public bool? is_unread { get; set; }

        public string modified_by { get; set; }

        public string creator_avatar_url { get; set; }

        public Embed_Props embed_props { get; set; }

    }

    public class Storage_Info
    {

        public string size { get; set; }

        public string storage_used { get; set; }

        public int? files_count { get; set; }

        public int? folders_count { get; set; }

        public int? size_in_bytes { get; set; }

        public int? storage_used_in_bytes { get; set; }

        public int? storage_used_by_workdrive_in_bytes { get; set; }

        public int? storage_used_by_app_in_bytes { get; set; }

    }

    public class View_Pref
    {

        public string sort_by { get; set; }

        public string sort_order { get; set; }

        public string filtered_by { get; set; }

        public string layout { get; set; }

    }

    public class Files_View_Pref
    {

        public string sort_by { get; set; }

        public string sort_order { get; set; }

        public string filtered_by { get; set; }

        public string layout { get; set; }

    }

    public class Capabilities
    {

        public bool? can_read { get; set; }

        public bool? can_share { get; set; }

        public bool? can_remove_share { get; set; }

        public bool? can_delete { get; set; }

        public bool? can_edit { get; set; }

        public bool? can_create_files { get; set; }

        public bool? can_upload_files { get; set; }

        public bool? can_trash { get; set; }

        public bool? can_rename { get; set; }

        public bool? can_restore { get; set; }

        public bool? can_copy { get; set; }

        public bool? can_move { get; set; }

        public bool? can_zip { get; set; }

        public bool? can_download { get; set; }

        public bool? can_emailattach { get; set; }

        public bool? can_publish { get; set; }

        public bool? can_create_task { get; set; }

        public bool? can_share_support { get; set; }

        public bool? can_label { get; set; }

        public bool? can_delist_file { get; set; }

        public bool? can_associate_data_template { get; set; }

        public bool? can_mandate_data_template { get; set; }

        public bool? can_favorite { get; set; }

        public bool? can_customize { get; set; }

        public bool? can_trash_files { get; set; }

        public bool? can_open_with_addons { get; set; }

        public bool? has_app_permission_alone { get; set; }

    }

    public class Watch_Preference
    {

        public bool? watch { get; set; }

        public bool? notifyemail { get; set; }

        public bool? notifybell { get; set; }

    }

    public class Embed_Props
    {
    }

    public class Relationships
    {

        public CustomPermissions custompermissions { get; set; }

        public Folders folders { get; set; }

        public Records records { get; set; }

        public Unzip unzip { get; set; }

        public AccessChartData accesschartdata { get; set; }

        public ResourceProperty resourceproperty { get; set; }

        public Shortcut shortcut { get; set; }

        public ImportFile importfile { get; set; }

        public Permissions permissions { get; set; }

        public SaveAsTemplate saveastemplate { get; set; }

        public Links links { get; set; }

        public Copy copy { get; set; }

        public PreviewZip previewzip { get; set; }

        public Tasks tasks { get; set; }

        public CustomMetadata custommetadata { get; set; }

        public Comments comments { get; set; }

        public PreviewInfo previewinfo { get; set; }

        public ApprovedVersions approvedversions { get; set; }

        public PublicLink publiclink { get; set; }

        public ParentFolders parentfolders { get; set; }

        public Versions versions { get; set; }

        public SupportShare supportshare { get; set; }

        public RecordSuggestions recordsuggestions { get; set; }

        public Timeline timeline { get; set; }

        public Files files { get; set; }

        public AccessData accessdata { get; set; }

        public Breadcrumbs breadcrumbs { get; set; }

        public Entity entity { get; set; }

        public Statistics statistics { get; set; }

        public AppData appdata { get; set; }

        public JourneyInstances journeyinstances { get; set; }

    }

    public class CustomPermissions
    {

        public Links links { get; set; }

    }

    public class Links
    {

        public string self { get; set; }

        public string related { get; set; }

    }

    public class LinksSelf
    {

        public string self { get; set; }

    }

    public class Folders
    {

        public Links links { get; set; }

    }

    public class Records
    {

        public Links links { get; set; }

    }

    public class Unzip
    {

        public Links links { get; set; }

    }

    public class AccessChartData
    {

        public Links links { get; set; }

    }

    public class ResourceProperty
    {

        public Links links { get; set; }

    }

    public class Shortcut
    {

        public Links links { get; set; }

    }

    public class ImportFile
    {

        public Links links { get; set; }

    }

    public class Permissions
    {

        public Links links { get; set; }

    }

    public class SaveAsTemplate
    {

        public Links links { get; set; }

    }

    public class Copy
    {

        public Links links { get; set; }

    }

    public class PreviewZip
    {

        public Links links { get; set; }

    }

    public class Tasks
    {

        public Links links { get; set; }

    }

    public class CustomMetadata
    {

        public Links links { get; set; }

    }

    public class Comments
    {

        public Links links { get; set; }

    }

    

    public class PreviewInfo
    {

        public Links links { get; set; }

    }

    public class ApprovedVersions
    {

        public Links links { get; set; }

    }

    public class PublicLink
    {

        public Links links { get; set; }

    }

    public class ParentFolders
    {

        public Links links { get; set; }

    }

    public class Versions
    {

        public Links links { get; set; }

    }

    public class SupportShare
    {

        public Links links { get; set; }

    }

    public class RecordSuggestions
    {

        public Links links { get; set; }

    }

    public class Timeline
    {

        public Links links { get; set; }

    }

    public class Files
    {

        public Links links { get; set; }

    }

    public class AccessData
    {

        public Links links { get; set; }

    }

    public class Breadcrumbs
    {

        public Links links { get; set; }

    }

    public class Entity
    {

        public Links links { get; set; }

    }

    public class Statistics
    {

        public Links links { get; set; }

    }

    public class AppData
    {

        public Links links { get; set; }

    }

    public class JourneyInstances
    {

        public Links links { get; set; }

    }

}
