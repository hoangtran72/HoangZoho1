using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetLeadByIdResponse
    {

        public LeadDetails[] data { get; set; }

    }

    public class LeadDetails
    {

        public string id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Phone_Other { get; set; }

        public string Age { get; set; }

        public string Marital_Status1 { get; set; }

        public string Number_of_Dependents { get; set; }

        public string Dependent_1_Age { get; set; }

        public string Wizard { get; set; }

        public string Lead_Status { get; set; }

        public string Lead_Quality { get; set; }

        public string Lead_Source { get; set; }

        public string Referal_Contact1 { get; set; }

        public string Campaigns_List { get; set; }

        public DateTime? Self_Booked_Date_Time { get; set; }

        public string Single_Line_143 { get; set; }

        public string Quiz_State { get; set; }

        public string Ad_Campaign_Source { get; set; }

        public string Quiz_Age_Group { get; set; }

        public string Quiz_Type { get; set; }

        public string Quiz_Marital_Status { get; set; }

        public string Quiz_Home_Owner { get; set; }

        public string Quiz_Income { get; set; }

        public string Quiz_Mortgage_Term { get; set; }

        public string Quiz_Super { get; set; }

        public string Quiz_Savings { get; set; }

        public string Quiz_How_Much_Equity { get; set; }

        public string Quiz_Home_Value { get; set; }

        public string Quiz_Result { get; set; }

        public DateTime? Quiz_Date_and_Time { get; set; }

        public string Appointment_Setter { get; set; }

        public string Appointment_Type { get; set; }

        public DateTime? Appointment_Date_Time { get; set; }

        public DateTime? Discovery_Call_Date_Time { get; set; }

        public string QC_Outcome { get; set; }

        public bool? QC_File_Check_Complete { get; set; }

        public DateTime? Strategy_Session_Booking_Made_Date_Time { get; set; }

        public decimal? PPR_Mortgage { get; set; }

        public DateTime? st_Appointment_Date_Time { get; set; }

        public string Sales_Person { get; set; }

        public string Borrowing_Capacity_IP { get; set; }

        public string Borrowing_Capacity_SMSF { get; set; }

        public string st_Appointment_Outcome { get; set; }

        public string Do_you_own_your_own_home { get; set; }

        public string PPR_Interest_Rate { get; set; }

        public decimal? PPR_Home_Value { get; set; }

        public string Reason_for_estimated_value { get; set; }

        public string Additional_Mortgage_Repayments { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip_Code { get; set; }

        public string PPR_Lender { get; set; }

        public decimal? Rent_or_Board_Paid { get; set; }

        public string Occupation_position { get; set; }

        public decimal? Gross_Salary_Per_Annum { get; set; }

        public string Employment_Status1 { get; set; }

        public string How_long_in_current_employment { get; set; }

        public string Shares { get; set; }

        public decimal? Savings { get; set; }

        public string Do_they_have_an_Investment_Property { get; set; }

        public string Additional_Comments_Investments { get; set; }

        public string Other_Assets { get; set; }

        public string Credit_Cards { get; set; }

        public string Car_Loans { get; set; }

        public string Personal_Loans1 { get; set; }

        public string Super_Fund_Name { get; set; }

        public string SMSF_Contributions { get; set; }

        public decimal? Super_Balance { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public string Mass_Update { get; set; }

        public string Source_Unique_Lead_ID { get; set; }

        public bool? The_Plains_Contact_Mapping { get; set; }

        public int? Days_Until_SS_Booked { get; set; }

        public int? Days_Until_SS_Completed { get; set; }

        public string Mortgage_Trigger { get; set; }

        public string PIG_Trigger { get; set; }

        public bool? Mortgage_Lead_Form { get; set; }

        public bool? Mortgage_Quiz { get; set; }

        public bool? Mortgage_Call_Booked { get; set; }

        public bool? PIG_Lead { get; set; }

        public bool? PIG_Quiz { get; set; }

        public bool? PIG_Call_Booked { get; set; }

        public string Lost_Reason { get; set; }

        public string utm_source { get; set; }

        public string utm_medium { get; set; }

        public string utm_campaign { get; set; }

        public string utm_content { get; set; }

        public string utm_term { get; set; }

        public DateTime? First_Visited_Time { get; set; }

        public string Referrer { get; set; }

        public DateTime? Last_Visited_Time { get; set; }

        public int? Number_Of_Chats { get; set; }

        public long? Visitor_Score { get; set; }

        public decimal? Average_Time_Spent_Minutes { get; set; }

        public string First_Visited_URL { get; set; }

        public int? Days_Visited { get; set; }

        public ModuleTag[] Tag { get; set; }

        public string LDS_Id { get; set; }

        public Owner Owner { get; set; }

        public Modified_By Modified_By { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Created_By Created_By { get; set; }

        public string Sakari_Id { get; set; }

    }

    public class ModuleTag
    {

        public string name { get; set; }

        public string id { get; set; }

        public string color_code { get; set; }

    }

    public class Created_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Modified_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

}
