using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    public class LatestNews
    {

        public LatestNews()
        {

            ImageUrls = new List<string>();

        }

        public string NewsTitle { get; set; }

        public string NewsHeadline { get; set; }

        public string NewsContent { get; set; }

        public string HeadlineImageUrl { get; set; }

        public List<string> ImageUrls { get; set; }

    }

}
