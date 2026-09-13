using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    public class QueryLastestNewsResponse
    {

        public LatestNewsData[] data { get; set; }

        public LatestNewsInfo info { get; set; }

    }

    public class LatestNewsData
    {

        public string id { get; set; }

        public string Name { get; set; }

        public string News_Headline { get; set; }

        public string News_Content { get; set; }

        public string News_Date { get; set; }

        public string Headline_Image_URL { get; set; }

        public string Content_Banner_Image_URL { get; set; }

        public string Image_URL_1 { get; set; }

        public string Image_URL_2 { get; set; }

        public string Image_URL_3 { get; set; }

        public string Image_URL_4 { get; set; }

        public string Image_URL_5 { get; set; }

        public string Image_URL_6 { get; set; }

        public string Image_URL_7 { get; set; }
        
        public string Image_URL_8 { get; set; }

        public string Image_URL_9 { get; set; }

        public string Image_URL_10 { get; set; }


    }

    public class LatestNewsInfo
    {
        public int count { get; set; }
        public bool more_records { get; set; }
    }

}
