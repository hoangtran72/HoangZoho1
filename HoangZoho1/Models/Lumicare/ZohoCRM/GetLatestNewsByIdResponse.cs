using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    public class GetLatestNewsByIdResponse
    {

        public NewsData[] data { get; set; }

    }

    public class NewsData
    {

        public Owner Owner { get; set; }

        public string Headline_Image_URL { get; set; }

        public string Name { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public string News_Date { get; set; }

        public string News_Headline { get; set; }

        public string id { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Images_Information[] Images_Information { get; set; }

        public string WorkDrive_Folder_Id { get; set; }

        public string News_Content { get; set; }

        public Created_By Created_By { get; set; }

    }

    public class Images_Information
    {

        public string Image_ID { get; set; }

        public string Image_URL { get; set; }

        public DateTime? Created_Time { get; set; }

        public Parent_Id Parent_Id { get; set; }

        public string id { get; set; }

        public Created_By Created_By { get; set; }

        public string LinkingModule12_Serial_Number { get; set; }

    }

    public class Parent_Id
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
