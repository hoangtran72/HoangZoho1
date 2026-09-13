using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.LeadDistributionSystem
{

    public class TaskEntity
    {

        public string ZohoCRMId { get; set; }

        public string Subject { get; set; }

        public string Status { get; set; }

        public DateTime? ClosedTime { get; set; }

        public DateTime? DueDate { get; set; }

        public string ContactId { get; set; }

        public string ContactName { get; set; }

        public string Module { get; set; }

        public string WhatId { get; set; }

        public string WhatName { get; set; }

        public string Priority { get; set; }

        public string Description { get; set; }

        public string OwnerId { get; set; }

        public string Owner { get; set; }

        public string CreatedById { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string ModifiedById { get; set; }

        public DateTime? ModifiedTime { get; set; }

    }

}
