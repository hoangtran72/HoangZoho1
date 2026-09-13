using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.Custom
{

    public class AssignSolicitorsToLeadRequest
    {

        public string LeadId { get; set; }

        public string SolicitorIds { get; set; }

    }
}
