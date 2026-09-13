using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoProjects
{
    public class CreateTaskResponse
    {
        public CreateTaskResponse()
        {
            tasks = new List<TaskDetail>();
        }

        public List<TaskDetail> tasks { get; set; }

    }
}
