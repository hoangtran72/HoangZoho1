using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.Custom
{

    public class QuerySolicitorsDto
    {

        public QuerySolicitorsDto()
        {
            solicitors = new List<SolicitorDto>();
        }

        public List<SolicitorDto> solicitors { get; set; }

    }

    public class SolicitorDto
    {

        public string FullName { get; set; }

        public string LawFirmName { get; set; }

        public string Title { get; set; }

        public string OrattoLawyerShortlist { get; set; }

        public string MatterExpertise { get; set; }

        public string SubCategories { get; set; }

        public string id { get; set; }

        public bool? Select { get; set; }

    }

}
