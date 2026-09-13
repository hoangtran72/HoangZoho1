using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoProjects
{
    
    public class AddCommentResponse
    {
        public Comment[] comments { get; set; }
    }

    public class Comment
    {

        public string content { get; set; }

        public long id { get; set; }

        public long created_time_long { get; set; }

        public string added_by { get; set; }

        public string added_person { get; set; }

        public string created_time_format { get; set; }

        public string created_time { get; set; }

    }

}
