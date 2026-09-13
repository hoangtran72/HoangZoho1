using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetZPCommentByIdResponse
    {
        public ZPComment[] data { get; set; }
    }

    public class ZPComment
    {

        public Owner Owner { get; set; }

        public DateTime? Comment_Created_Time { get; set; }

        public DateTime? Comment_Last_Modified_Time { get; set; }

        public string Project_Name { get; set; }

        public string Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Comment_Id { get; set; }

        public bool? Has_Attachment { get; set; }

        public string Task_URL { get; set; }

        public string id { get; set; }

        public Comment_Attachments[] Comment_Attachments { get; set; }

        public string Project_URL { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Related_Deal Related_Deal { get; set; }

        public string Task_Name { get; set; }

        public string Content { get; set; }

        public Related_Contact Related_Contact { get; set; }

        public string Added_Person { get; set; }

        public Created_By Created_By { get; set; }

    }

    public class Comment_Attachments
    {

        public string File_Name { get; set; }

        public DateTime? Created_Time { get; set; }

        public Parent_Id Parent_Id { get; set; }

        public string LinkingModule16_Serial_Number { get; set; }

        public string App_Name { get; set; }

        public string Preview_URL { get; set; }

        public string Permanent_URL { get; set; }

        public string File_Type { get; set; }

        public string Associated_By { get; set; }

        public string id { get; set; }

    }

    public class Parent_Id
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
