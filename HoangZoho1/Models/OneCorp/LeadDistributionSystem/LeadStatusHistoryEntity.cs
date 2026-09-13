using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.LeadDistributionSystem
{

    public class LeadStatusHistoryEntity
    {

        public string CrmLeadId { get; set; }

        public string LeadOwnerId { get; set; }

        public string LeadOwnerName { get; set; }

        public string LeadStatus { get; set; }

        public int LivingTime { get; set; }

        public string FriendlyLivingTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string CreatedById { get; set; }

        public string ModifiedById { get; set; }

    }

}
