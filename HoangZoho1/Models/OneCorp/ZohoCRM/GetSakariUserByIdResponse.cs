using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetSakariUserByIdResponse
    {

        public SakariUser[] data { get; set; }

    }

    public class SakariUser
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public DateTime? Modified_Time { get; set; }

        public Sakari_Groups[] Sakari_Groups { get; set; }

        public DateTime? Created_Time { get; set; }

        public Created_By Created_By { get; set; }

    }

    public class Sakari_Groups
    {

        public string Group_Id { get; set; }

        public DateTime Created_Time { get; set; }

        public Parent_Id Parent_Id { get; set; }

        public string Group_Name { get; set; }

        public string Phone_Number { get; set; }

        public string id { get; set; }

        public string Group_Tags { get; set; }

    }

}
