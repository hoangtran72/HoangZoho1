using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Models.PinjarraBakery.ZohoProjects
{
    public class TaskCustomFields
    {

        // UDF_TEXT1
        public string EmailContent { get; set; }

        // UDF_CHAR1
        public string EmailSubject { get; set; }

        public string CreateRequestString()
        {
            int countComma = 0;
            string result = "{";
            if (!string.IsNullOrWhiteSpace(EmailContent))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_TEXT1\":\"" + EmailContent + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(EmailSubject))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR1\":\"" + EmailSubject + "\"";
                countComma++;
            }
            result += "}";
            return result;
        }

    }
}
