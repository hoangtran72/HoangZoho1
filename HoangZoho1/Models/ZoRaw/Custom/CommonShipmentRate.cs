using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Custom
{

    public class CommonShipmentRate
    {

        public CommonShipmentRate()
        {
            
            AddOns = new List<CommonAddOn>();

        }

        public string Provider { get; set; }

        public string ProviderShort { get; set; }

        public string ServiceId { get; set; }

        public string CarrierName { get; set; }

        public float? Base { get; set; }

        public float? Tax { get; set; }

        public float? AddOnRate { get; set; }

        public float? Total { get; set; }

        public string Currency { get; set; }

        public string DeliveryDays { get; set; }

        public List<CommonAddOn> AddOns { get; set; }

        public object GetRatesDetails { get; set; }

    }

    public class CommonAddOn
    {

        public string Name { get; set; }

        public float? Cost { get; set; }

        public string Currency { get; set; }

    }

}
