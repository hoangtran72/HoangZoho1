namespace HoangZoho1.Models.OneCorp.ZohoCRM
{
    public class QueryBookingsResponse
    {

        public BookingData[] data { get; set; }

        public Info info { get; set; }

    }

    public class BookingData
    {

        public string Booking_Id { get; set; }

        public string id { get; set; }

    }

}
