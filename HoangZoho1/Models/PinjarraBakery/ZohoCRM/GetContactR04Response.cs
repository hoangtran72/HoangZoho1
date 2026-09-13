using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoCRM
{
 
    public class GetContactR04Response
    {
        public R04Data[] data { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public int per_page { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public bool more_records { get; set; }
    }

    public class R04Data
    {
        public Owner Owner { get; set; }
        public string How_many_years_you_expect_to_stay_in_this_business { get; set; }
        public string Do_you_commit_yourself_to_the_business_full_time { get; set; }
        public object field_states { get; set; }
        public string Are_you_hungry_for_financial_success { get; set; }
        public string Are_you_a_problem_solver { get; set; }
        public string Your_tolerance_to_disciplined_systems_and_procedur { get; set; }
        public string state { get; set; }
        public bool process_flow { get; set; }
        public string Currency { get; set; }
        public object Employer_Address_Line_2 { get; set; }
        public string Analytical_skills { get; set; }
        public string id { get; set; }
        public string Please_rate_your_leadership_skills { get; set; }
        public string Community_involvement_and_support { get; set; }
        public Approval approval { get; set; }
        public object Employer_State_Region_Province { get; set; }
        public DateTime Created_Time { get; set; }
        public object Address_Line_2 { get; set; }
        public string Are_you_a_declared_bankrupt { get; set; }
        public string Employer_Street_Address { get; set; }
        public string Your_ability_to_work_well_in_a_team { get; set; }
        public object Employer_Postal_Zip_Code { get; set; }
        public string Your_sense_of_humour_rating { get; set; }
        public string Your_family_support_of_moving_into_own_business { get; set; }
        public string Country { get; set; }
        public Created_By Created_By { get; set; }
        public string Own_a_franchise_business_before_Where_and_When { get; set; }
        public string Networking_at_events { get; set; }
        public string Street_Address { get; set; }
        public string Rate_your_safe_driving_record { get; set; }
        public Review_Process review_process { get; set; }
        public string How_often_will_you_take_holidays_and_what_period { get; set; }
        public string What_is_your_attention_to_detail_like { get; set; }
        public string General_Fitness_rating { get; set; }
        public string Computer_literacy_word_processing_excel_etc { get; set; }
        public string How_long_you_expect_to_business_well_established { get; set; }
        public string Customer_Service_Focus { get; set; }
        public string Timeframe_in_mind { get; set; }
        public string In_1st_year_or_so_to_get_the_business_established { get; set; }
        public string Understand_to_follow_PB_BS { get; set; }
        public string Salesmanship_skills { get; set; }
        public string Communication_skills { get; set; }
        public string Administrative_systems { get; set; }
        public string Public_Relations_PR { get; set; }
        public bool? orchestration { get; set; }
        public string Postal_Zip_Code { get; set; }
        public object Employer_Country { get; set; }
        public string Any_legal_action_current_or_pending_against { get; set; }
        public string Staff_management { get; set; }
        public object If_yes_will_he_she_be_in_the_business { get; set; }
        public object If_not_please_explain_and_how_many_hours_per_week { get; set; }
        public object State_Region_Province { get; set; }
        public string Do_you_have_ready_access_to_funds { get; set; }
        public object[] Tag { get; set; }
        public string Negotiation_techniques { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string currency_symbol { get; set; }
        public string Your_assessment_of_your_own_people_skills { get; set; }
        public string How_entrepreneurial_are_you { get; set; }
        public string Understanding_financial_reports { get; set; }
        public string Name { get; set; }
        public DateTime Last_Activity_Time { get; set; }
        public object Unsubscribed_Mode { get; set; }
        public int? Exchange_Rate { get; set; }
        public string Current_Employer { get; set; }
        public string Advertising { get; set; }
        public bool? approved { get; set; }
        public string Suburd_Location_in_mind { get; set; }
        public string Your_level_of_self_confidence { get; set; }
        public string Public_speaking { get; set; }
        public string Do_you_have_financial_partner_If_yes_their_name { get; set; }
        public int? Weekly_Income_Needs { get; set; }
        public string Experience_in_this_industry { get; set; }
        public bool? editable { get; set; }
        public string Duration { get; set; }
        public string City { get; set; }
        public string How_strong_is_your_competitive_streak { get; set; }
        public object If_yes_provide_details_no_1 { get; set; }
        public object If_yes_provide_details_no_2 { get; set; }
        public object Home_Phone { get; set; }
        public string How_many_people_full_time_in_franchise_business { get; set; }
        public string Past_Experiences_to_succeed_as_a_PB_Franchisee { get; set; }
        public string Level_of_family_involvement_in_the_business { get; set; }
        public string English_comprehension_and_writing_skills { get; set; }
        public string Have_you_had_preliminary_talks_with_your_Bank { get; set; }
        public string When_you_decide_to_get_into_your_own_business { get; set; }
        public string Occupation_Position { get; set; }
        public object Employer_City { get; set; }
        public string Mobile_Phone { get; set; }
        public string Been_in_business_before_Where_and_When { get; set; }
        public string Sign_Date { get; set; }
        public string First_Name { get; set; }
        public string General_Business_experience { get; set; }
        public string Do_you_relish_a_good_challenge { get; set; }
        public Modified_By Modified_By { get; set; }
        public object review { get; set; }
        public int? How_much_do_you_need_in_the_first_year { get; set; }
        public string What_impact_the_business_will_have_on_your_family { get; set; }
        public string Organisational_ability { get; set; }
        public string General_Health_rating { get; set; }
        public DateTime Modified_Time { get; set; }
        public string Date_of_Birth { get; set; }
        public string Bookkeeping { get; set; }
        public string Rate_your_expectations_of_financial_success_in_our { get; set; }
        public object Unsubscribed_Time { get; set; }
        public string Who_do_you_presently_bank_with { get; set; }
        public string How_much_of_a_priority_is_being_prompt_for_meeting { get; set; }
        public string How_many_hours_would_you_work_in_a_typical_week { get; set; }
        public string Listening_skills { get; set; }
        public string Understand_not_follow_PB_BS_will_reduce_success { get; set; }
        public string Dealing_with_objections { get; set; }
        public Contact Contact { get; set; }
        public int? How_much_would_you_need_to_borrow { get; set; }
        public string Working_unsupervised { get; set; }
        public string Role_of_PB_Franchisee { get; set; }
        public string How_assertive_are_you { get; set; }
        public bool? in_merge { get; set; }
        public string any_legal_action_taken_against_you_in_the_past { get; set; }
        public string Current_Salary_Benefits { get; set; }
        public string approval_state { get; set; }
        public string Your_income_expectations_including_your_own_wage { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Approval
    {
        public bool _delegate { get; set; }
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Created_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Review_Process
    {
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Modified_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Contact
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
