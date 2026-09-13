namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class SearchAcrossFolderResponse
    {

        public FileFolderData[] data { get; set; }

        public SearchMeta meta { get; set; }

    }

    public class SearchMeta {

        public int? search_result_count { get; set; }

    }

}
