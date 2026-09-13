namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class GetLocationByIdResponse
    {

        public int code { get; set; }

        public string message { get; set; }

        public Location location { get; set; }

    }

    public class Associated_Series
    {
        public string autonumbergenerationgroup_id { get; set; }
        public string autonumbergenerationgroup_name { get; set; }
        public bool is_default_series { get; set; }
    }

    public class Associated_Users
    {

        public string user_id { get; set; }
        
        public string user_name { get; set; }
    
    }
    public class Location
    {

        public string location_id { get; set; }
        
        public string location_name { get; set; }
        
        public string type { get; set; }
        
        public LocationAddress address { get; set; }
        
        public string phone { get; set; }
        
        public string website { get; set; }
        
        public string fax { get; set; }
        
        public string email { get; set; }
        
        public bool? is_location_active { get; set; }
        
        public bool is_primary_location { get; set; }
        
        public string contact_name { get; set; }
        
        public string autonumbergenerationgroup_id { get; set; }
        
        public string autonumbergenerationgroup_name { get; set; }
        
        public Associated_Series[] associated_series { get; set; }
        
        public string parent_location_id { get; set; }
        
        public string pricebook_id { get; set; }
        
        public string pricebook_name { get; set; }
        
        public string time_to_pack { get; set; }
        
        public string working_days { get; set; }
        
        public string holidays { get; set; }
        
        public Associated_Users[] associated_users { get; set; }
    
    }
    
    public class LocationAddress
    {
        public string attention { get; set; }
        public string street_address1 { get; set; }
        public string street_address2 { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string state_code { get; set; }
    }

}
