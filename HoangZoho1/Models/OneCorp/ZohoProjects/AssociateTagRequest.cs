using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoProjects
{
    public class AssociateTagRequest
    {

        public string project_id { get; set; }

        public string tag_id { get; set; }

        public string entity_id { get; set; }

        public string entityType { get; set; }

    }
}
