using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetUsersResponse
    {

        public User[] users { get; set; }

        public Info info { get; set; }

    }

    public class User
    {

        public string country { get; set; }

        public Role role { get; set; }

        public string city { get; set; }

        public object Current_Shift { get; set; }

        public string language { get; set; }

        public string locale { get; set; }

        public bool? microsoft { get; set; }

        public bool? Isonline { get; set; }

        public Modified_By Modified_By { get; set; }

        public string street { get; set; }

        public string Currency { get; set; }

        public string alias { get; set; }

        public string id { get; set; }

        public string state { get; set; }

        public string fax { get; set; }

        public string country_locale { get; set; }

        public bool? sandboxDeveloper { get; set; }

        public string first_name { get; set; }

        public string email { get; set; }

        public object Reporting_To { get; set; }

        public string zip { get; set; }

        public object status_reason__s { get; set; }

        public DateTime? created_time { get; set; }

        public string website { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string time_format { get; set; }

        public int? offset { get; set; }

        public Profile profile { get; set; }

        public string mobile { get; set; }

        public string last_name { get; set; }

        public object Next_Shift { get; set; }

        public string time_zone { get; set; }

        public Created_By created_by { get; set; }

        public object Shift_Effective_From { get; set; }

        public string zuid { get; set; }

        public bool? confirm { get; set; }

        public string full_name { get; set; }

        public object[] territories { get; set; }

        public string phone { get; set; }

        public string dob { get; set; }

        public string date_format { get; set; }

        public string status { get; set; }

        public string decimal_separator { get; set; }

        public Customize_Info customize_info { get; set; }

        public object signature { get; set; }

        public string name_format { get; set; }

        public bool? personal_account { get; set; }

        public string default_tab_group { get; set; }

        public Theme theme { get; set; }

    }

    public class Role
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Profile
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Customize_Info
    {

        public bool? notes_desc { get; set; }

        public object show_right_panel { get; set; }

        public object bc_view { get; set; }

        public bool? show_home { get; set; }

        public bool? show_detail_view { get; set; }

        public object unpin_recent_item { get; set; }

    }

    public class Theme
    {

        public Normal_Tab normal_tab { get; set; }

        public Selected_Tab selected_tab { get; set; }

        public object new_background { get; set; }

        public string background { get; set; }

        public string screen { get; set; }

        public string type { get; set; }

    }

    public class Normal_Tab
    {

        public string font_color { get; set; }

        public string background { get; set; }

    }

    public class Selected_Tab
    {

        public string font_color { get; set; }

        public string background { get; set; }

    }

}
