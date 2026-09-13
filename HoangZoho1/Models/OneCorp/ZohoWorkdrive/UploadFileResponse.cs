using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class UploadFileResponse
    {
        public UploadFileData[] data { get; set; }
    }

    public class UploadFileData
    {

        public Attributes attributes { get; set; }

        public string type { get; set; }

    }

    public class Attributes
    {

        public string Permalink { get; set; }

        public FileINFO FileINFO { get; set; }

        public string parent_id { get; set; }

        public string FileName { get; set; }

        public string resource_id { get; set; }

    }

    public class FileINFO
    {

        public string RESOURCE_ID { get; set; }

        public string LIBRARY_ID { get; set; }

        public string PARENT_MODEL_ID { get; set; }

        public string PARENT_ID { get; set; }

        public int RESOURCE_TYPE { get; set; }

        public string WMS_SENT_TIME { get; set; }

        public string OWNER { get; set; }

        public string RESOURCE_GROUP { get; set; }

        public string PARENT_MODEL_NAME { get; set; }

        public string OPERATION { get; set; }

        public string EVENT_ID { get; set; }

        public AUDIT_INFO AUDIT_INFO { get; set; }

        public int ZUID { get; set; }

        public string TEAM_ID { get; set; }

    }

    public class AUDIT_INFO
    {

        public Resource resource { get; set; }

        public Parentinfo parentInfo { get; set; }

        public Libraryinfo libraryInfo { get; set; }

        public string statusCode { get; set; }

    }

    public class Resource
    {

        public string owner { get; set; }

        public long created_time { get; set; }

        public string creator { get; set; }

        public int service_type { get; set; }

        public string extension { get; set; }

        public int resource_type { get; set; }

        public string name { get; set; }

    }

    public class Parentinfo
    {

        public string parentName { get; set; }

        public string parentId { get; set; }

        public int parentType { get; set; }

    }

    public class Libraryinfo
    {

        public string libraryName { get; set; }

        public string libraryId { get; set; }

        public int libraryType { get; set; }

    }

}
