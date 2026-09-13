using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Custom
{

    public class GetRatesFromWidgetRequest
    {

        public string SalesOrderId { get; set; }

        public string PackageId { get; set; }

        public List<RateBox> Boxes { get; set; }

        public bool? ResidentialAddress { get; set; }

        public string ExpectedShipDate { get; set; }

        public string ReadyAtHour { get; set; }

        public string ReadyAtMinute { get; set; }

        public string ReadyUntilHour { get; set; }

        public string ReadyUntilMinute { get; set; }

    }

    public class RateBox
    {

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

    }

}
