using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneBudget.ZohoCRM
{

    public class UpdateBlueprintRequest
    {

        public UpdateBlueprintRequest()
        {
            blueprint = new List<Blueprint>();
        }

        public List<Blueprint> blueprint { get; set; }

    }

    public class Blueprint
    {

        public string transition_id { get; set; }

        public object data { get; set; }

    }

}
