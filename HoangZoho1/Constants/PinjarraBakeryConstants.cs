using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class PinjarraBakeryConstants
    {

        public const string PinjarraBakery = "PinjarraBakery";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("PINJARRA_BAKERY_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("PINJARRA_BAKERY_ZOHO_CLIENT_SECRET");

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("PINJARRA_BAKERY_ZOHO_CRM_REFRESH_TOKEN");

        public static string ZohoProjects_RefreshToken => EnvironmentConstants.Get("PINJARRA_BAKERY_ZOHO_PROJECTS_REFRESH_TOKEN");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com/crm/v3";

        public const string PrefixContactEndpoint = "https://crm.zoho.com/crm/org774688909/tab/Contacts";

        public const string PrefixR04Endpoint = "https://crm.zoho.com/crm/org774688909/tab/CustomModule1";

        #endregion

        #region Zoho Projects

        public const string ProjectEndpoint = "https://projectsapi.zoho.com/restapi/portal/pinjarrabakeryfranchising";

        #region Task Descriptions

        public const string Step1_DistributionDescription = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>Preferably in person but can be via email.<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div>The applicant receives the <b>Franchise Information Pack </b>which provides them with enough information to whet their appetite and encourage them to attend a face-to-face meeting with the franchisor or a Studio Tour. <br /></div><div>This includes your <b>Confidentiality Agreement </b>provided by the lawyer (if not already dealt with), <b>Promotional material</b>, <b>Expression of Interest Form (EOI)</b> for them to complete, plus the <b>ACCC Information Statement</b>. Explain that you will also include a copy of the <b>Journey to Become a franchisee </b>which outlines the full process that they might undertake, so that they can see the logical steps ahead. Explain that to them.<br /></div><div><b><br /></b></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><span style=\"color: rgb(0, 76, 153)\"><br /></span></div><div><span style=\"font-size: 16px\"><span style=\"color: rgb(0, 0, 0)\">Provide the ‘Information Pack’ via email so that it is recorded:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://drive.google.com/file/d/16PIQ9ismSqja0MvytQrJXdA-raoP-r3o/view?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">ACCC Information Statement form R-03A</a></span><br /></li><li><span><a href=\"https://docs.google.com/document/d/1kaeuWfTw_CmmMDJqqKWJZOwStq8qAjry5uFCFln0g8Y/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Expression of Interest form (EOI) R – 04</a></span><br /></li><li><span><a href=\"https://drive.google.com/file/d/1SGuqTIYjQWjYmMPJJHMGqXA7x3KPhEBr/view?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Journey to Become a Franchisee form R – 01</a></span><br /></li><li>Confidentiality Deed<br /></li><li>Promotional material<br /></li></ul><div><br /></div>";
        
        public const string Step2_FollowUp = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Settings:</span></span><br /></div><div>Telephone conversation<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><ul dir=\"ltr\"><li><span>Ideally the <b>EOI - R 04</b>, is on hand to prompt questions or deal with anything that looks out of order to continue. <br /></span></li><li><span>If suitable based on what is known, then this conversation communicates to the applicant that they are important, <b><u>pre-qualifies them financially</u></b>, based on the information in the EOI.<br /></span></li><li><span>Invite them for a<b> trial session</b> at a store if required, or a <b>Store tour</b>, or a <b>meeting</b>.</span><br /></li></ul><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><div>Prior to the First Informal Meeting:<br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1yrkZSjfrowetNjwIfYW9Vjd_GnACKKpbrGhtWfoufuY/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Follow Up Telephone Call Script - Prior to First Informal Meeting R-07</a></span><br /></li></ul><div><br /></div>";
        
        public const string Step2_FirstInformalMeeting = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>Informal meeting at the franchisor office<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Franchisor:</b> <br /></div><ul dir=\"ltr\"><li><span>This is often the franchisor’s first opportunity to properly present the business to the applicant<br /></span></li><li><span>Create an excellent first impression<br /></span></li><li><span>Initial analysis of the ‘cultural compatibility’ of the applicant<br /></span></li><li><span><u>If deemed suitable to take the enquiry further</u>:<br /></span></li><ul><li><span>The Applicant is informed that once they have signed the Confidentiality Agreement, they can proceed further</span><br /></li></ul></ul><div><b>Review together in detail:</b><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1kaeuWfTw_CmmMDJqqKWJZOwStq8qAjry5uFCFln0g8Y/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Expression of Interest form R – O4</a><br /></span></li></ul><div><b>Applicant:</b><br /></div><ul dir=\"ltr\"><li><span>The Applicant is advised that they will need to undertake an online Psychometric Evaluation during the application process. Timing to be advised.<br /></span></li><li><span>The Applicant is to arrange for a National Police Clearance<br /></span><a target=\"_blank\" rel=\"noopener noreferrer\" href=\"https://www.afp.gov.au/what-we-do/services/criminal-records/national-police-checks\">https://www.afp.gov.au/what-we-do/services/criminal-records/national-police-checks</a><br /></li></ul><div><b>If deemed UNSUITABLE the applicant is advised that “We will get back to you etc”</b><br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1IbVInPPZr3a-cnPn7UXkS5V0izlhhpeOF6VwgP3USc8/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Interviewer Notes - First Informal Meeting – R 07A </a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1dQ9HoCecV8Rujk82ZvFIvyNHvVQDffAIVAByU5hlZfY/edit?usp=sharing\" target=\"_blank\">Agenda and Questionnaire - R 07B</a></span><br /></li><li><span>First Informal Meeting - Handouts</span><br /></li><li>Information Pack if not already provided <br /></li><li><span>Confidentiality Agreement – if not dealt with</span><br /></li><li><a href=\"https://drive.google.com/file/d/1SGuqTIYjQWjYmMPJJHMGqXA7x3KPhEBr/view?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Journey to Become a Franchisee form R – 01 </a><br /></li><li><span><a href=\"https://docs.google.com/document/d/1kaeuWfTw_CmmMDJqqKWJZOwStq8qAjry5uFCFln0g8Y/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Franchisee Expression of Interest Form – R 04</a></span><br /></li><li><span><a href=\"https://drive.google.com/file/d/16PIQ9ismSqja0MvytQrJXdA-raoP-r3o/view?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">ACCC Information Statement for Prospective Franchisees – R 03A<br /></a></span></li><li><span><a href=\"https://docs.google.com/document/d/1f92k5CAqhy87SS_BFR6oxm0Swd7CHnt_XkS-Wz__5wM/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Recruitment Process Checklist – R 21</a><br /></span></li></ul><div><br /></div>";
        
        public const string Step3_InitialAssessment = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Settings:</span><br /></span></div><div>After the meeting, franchisor staff complete this exercise<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><b><br /></b></div><div><b>Franchisor:</b><b> <br /></b></div><ul dir=\"ltr\"><li><span>Complete a preliminary assessment of the Applicant based on the last meeting and/or Studio Tour and discuss the findings from the first informal interview to decide whether or not to continue. Analyse and assess their financial standing and affordability/borrowing capacity.<br /></span></li><li><span>Complete the <b>Applicant Scorecard – R 11</b> where appropriate<br /></span></li></ul><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">​</span></span><span style=\"font-size: 16px\"><span style=\"color: rgb(0, 0, 0)\"><a href=\"https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Applicant Scorecard – R 11</a></span></span><br /></span></li></ul>";
        
        public const string Step3_IfSuitable = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>Informal meeting at the franchisor office<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Applicant:</b> <br /></div><ul dir=\"ltr\"><li><span>Returns completed and signed <b>Expression of Interest Form</b> if not already received<br /></span></li><li><span>Provides the <b>National Police Clearance Report</b><br /></span></li></ul><div><b>Franchisor:</b><br /></div><ul dir=\"ltr\"><li><span>Re-tests assumptions made at the initial meeting regarding the “cultural and skills compatibility” as well as financial capacity of the potential franchisee<br /></span></li></ul><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1y3LmzlE936KTVWlTXzBOlnv6WNWN5li7Uw0YrLX-Rv4/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Interviewer Notes - Second Informal Meeting - R 08</a><br /></span></li></ul>";
        
        public const string Step4_Assessment = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>Franchisor staff complete this exercise<br /></div><div><b><br /></b></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Franchisor:</b> <br /></div><ul dir=\"ltr\"><li><span>Thoroughly analyse the completed Expression of Interest Form<br /></span></li><li><span>Conducts reference checks<br /></span></li><li><span>Reviews National Police Clearance report<br /></span></li><li><span>Analyses the applicant’s preliminary financial situation for affordability and loan capacity<br /></span></li><li><span>Analyses the applicant’s skills and experience<br /></span></li><li><span>Start to complete the Applicant Scorecard<br /></span></li></ul><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><span style=\"font-size: 16px\"><span style=\"color: rgb(0, 0, 0)\"><a href=\"https://docs.google.com/document/d/1i9UgJ19vAv2wEYQqTe9jKXFgzSJZ-pozo5_DU7vctE4/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Application form - Support Office Assessment – R 05A</a></span></span></span><br /></li><li><span><span style=\"font-size: 16px\"><span style=\"color: rgb(0, 0, 0)\"><a href=\"https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Applicant Scorecard - R 11​</a></span></span><br /></span></li></ul><div><br /></div>";
        
        public const string Step5_IfSuitable = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>Formal meeting in the franchisor office<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Franchisor:</b> <br /></div><ul dir=\"ltr\"><li><span>Discuss the responses in the Expression of Interest Form with the applicant in detail of not already completed.<br /></span></li><li><span>Clear up any vagaries<br /></span></li><li><span>Discuss any concerns<br /></span></li><li><span>Advise them that you would like them to go further with their enquiry. Make a fuss about them here, talk positively if you feel that way!<br /></span></li><li><span>Take detailed notes during the meeting and not afterwards<br /></span></li><li><span>Arrange for the Psychometric Evaluation to take place after this meeting<br /></span></li></ul><div><br /></div><div>Support Materials:<br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/136qkc_degQKbBGYPLa_NN2QZwCDRbIjcwz5vZtWTXoI/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Interviewer Notes - First Formal Meeting R 09</a><br /></span></li></ul>";

        public const string Step5_Suitable2Proceed = "";

        public const string Step6_FollowUp = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:&nbsp;</span></span><br /></div><div>Telephone calls at least weekly until next step<br /></div><div><br /></div><div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Objectives/Outcome:</span></span><br /></div><ul dir=\"ltr\"><li><span>They are working their way through the Business Pack and it is quite daunting for some people, especially if they have not had business experience. <br /></span></li><li><span>Ask how they are going, do they have any questions, is it all making sense? Offer support to guide them with this process. Would they prefer to meet or Zoom with you?<br /></span></li><li><span>Gently remind applicant of the timeline to get their formal Application Form in.<span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\"><br /></span></span></span></li></ul><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\"><br /></span></span></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1V0IGCjqcCXCJDRcuZUhFNL2bXl4oSrFvkZPQuM9epzo/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Follow Up Telephone Call Script – Post First Formal Meeting – R 10 </a><br /></span></li></ul><div><i>Use this record of conversations after each catch up/phone call or Zoom</i><br /></div>";

        public const string Step6_ReceiveApplicationForm = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Settings:</span></span><br /></div><div>Application form via email preferred ahead of the next meeting<br /></div><div><br /></div><div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Objectives/Outcome:</span></span><br /></div><ul dir=\"ltr\"><li><span>Applicant sends completed Application for to franchisor. <br /></span></li><li><span>Assessment of Application<br /></span></li><li><span>Assessment of capacity to borrow the necessary funds<br /></span></li><li><span>Further completion of Applicant Scorecard</span><br /></li></ul><div><br /></div><div><span><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1SlJ9VzPj86fVNuoZiFJ5DWvN1uJIxb9M1Bikl2F-YWk/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\"><span>​</span>Reference Check Questionnaire – Personal R 13 </a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1wCdOBVM9ySQLjuD_qz8MQDyKBApkr-j5J9Kpi8kYFLw/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Reference Check Questionnaire - Financial R 12</a><br /></span></li><li><span>Review Psychometric Report – DO NOT DISCUSS WITH THE APPLICANT<br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1i9UgJ19vAv2wEYQqTe9jKXFgzSJZ-pozo5_DU7vctE4/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Application form - Support Office Assessment – R 05A</a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Applicant Scorecard - R 11</a><br /></span></li></ul>>";

        public const string Step7_Not2Proceed = "<div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Setting:</span></span><br /></div><div>By registered post or face to face<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Applicant:</b> <br /></div><ul dir=\"ltr\"><li><span>Return all documents and material received from the franchisor.<br /></span></li><li><span>Provide the franchisor with receipt of returned deposit moneys.</span><br /></li></ul><div><span><b>Franchisor:</b><br /></span></div><ul dir=\"ltr\"><li><span>Find out the real reason they are withdrawing if you can<br /></span></li><li><span>Withhold Document Security Deposit monies until all material returned.<br /></span></li><li><span>Document the receipt of all documents provided to the applicant.<br /></span></li><li><span>Then return all Deposit moneys received up until this point.</span><br /></li></ul><div><span><i>No further communication is maintained following this outcome</i><br /></span></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1nqXevepwdebkP2cymFpWKIYy3f-aNr1SlMNQGJSxNrk/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Document Return Checklist – R 23</a><br /></span></li></ul>";

        public const string Step7_Proceed = "<div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Setting:</span></span><br /></div><div>Formal meeting at the [GROUP NAME] HQ or Applicant’s home / office (if practicable) to assess domestic situation, if considered appropriate?<br /></div><div><br /></div><div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Objectives/Outcome:</span></span><br /></div><div><b>Franchisor:</b> <br /></div><ul dir=\"ltr\"><li><span>Hold discussions on ‘Significant Capital Expenditure’ as required under the Code<br /></span></li><li><span>Answer any outstanding questions (Not Legal Advice)<br /></span></li><li><span>Request second Document Security Deposit of $7,000<br /></span></li><li><span>Start to talk about Locations/Timeframes<br /></span></li><li><span>Inform the Applicant that they will need to set up their company/Trust quite soon, which will become the franchisee<br /></span></li><li><span>Provide the Applicant with a Deposit Receipt once received<br /></span></li><li><span>Revisit Applicant Scorecard to make preliminary decision as to acceptance as a franchisee. DO NOT ADVISE THEM AT THIS POINT<br /></span></li><li><span>Complete Recruitment Process Checklist<br /></span></li></ul><div><b>Applicant:</b><br /></div><ul dir=\"ltr\"><li><span>May provide the franchisor with a finance approval letter if available?<br /></span></li><li><span>Pays a further $7,000 deposit<br /></span></li><li><span>Sign ‘Advice to Proceed with Document Preparation’<br /></span></li></ul><div><i>No further communication is maintained following this outcome</i><br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1X7GXmnIM82spjSijNO_UWHYzGdKbimxrQRStqK63Pfc/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Interviewer Notes - Second Formal Meeting R – 09A </a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/13hJMG75x3wpdb2WUYyL0bYFKTlB8_LjMvr6lEV9bW9s\" target=\"_blank\" rel=\"noopener noreferrer\">Advice to Proceed with Document Preparation R – 14A</a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Applicant Scorecard - R 11</a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1f92k5CAqhy87SS_BFR6oxm0Swd7CHnt_XkS-Wz__5wM/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Recruitment Process Checklist R - 21</a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/12V7bj6uKSckVvgZEUytsb4oF7MFs19m7H7G4Pv9yECQ/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Deposit Receipt R 18</a><br /></span></li></ul>";

        public const string Step9_ProvideFinalSetOfDocuments = "<div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Setting:&nbsp;</span></span><br /></div><div>Provided by the Lawyer to the Franchisor or, directly to the Applicant<br /></div><div><br /></div><div><span style=\"font-size: 18.6667px\"><span style=\"color: rgb(255, 127, 0)\">Objectives/Outcome:</span></span><br /></div><div><b>Lawyer:&nbsp;</b><span>Sends the Franchisor or the Applicant the following documents:<br /></span></div><ul dir=\"ltr\"><li><span>Franchise Agreement (Complete and including all details and any Special Conditions)<br /></span></li><li><span>Disclosure Document<br /></span></li><li><span>A copy of the Franchising Code of Conduct<br /></span></li><li><span>Minimum 14 Day waiting period commences prior to execution of the Franchise Agreement<br /></span></li><li><span>Advisor Certificates in blank format for completion by their advisors<br /></span></li></ul><div><b>The Franchise Agreement cannot be signed by the franchisee until the ‘14-day’ waiting period has expired following the issue of these documents to the franchisee.</b><br /></div><div><br /></div><div><b>Franchisor:</b><br /></div><ul dir=\"ltr\"><li><span>Written receipt by the franchisee of the Disclosure Document (contained on last page of Disclosure Document) under Clause 10(2)<br /></span></li><li><span>Monitors 14-day waiting period and follows up franchisee to set date for execution of Franchise Agreement and Settlement.<br /></span></li><li><span>Waits to receive completed Advisor certificates – send to lawyer for review before execution of FA<br /></span></li><li><span>Prepare Receipt for Balance of Monies at Settlement - R 19<br /></span></li><li><span><b>Review of Recruitment Compliance Checklist</b><br /></span></li><li><span><b>Completion of Recruitment Process Checklist</b><br /></span></li></ul><div><br /></div><div><span style=\"color: rgb(255, 127, 0)\"><span style=\"font-size: 18.6667px\">Support Materials:</span></span><br /></div><ul dir=\"ltr\"><li><span><a href=\"https://docs.google.com/document/d/1O-bekXryUU5XZJFaGaQvrX2zOjwIDY9CqnCit1OdAYY/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Meeting Notes Settlement R - 15 </a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1zH-cSBiAG8ruSMUziBWdFlhuOaPSnhNZKeNYO4JYgLM/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Recruitment Compliance Checklist – R 20</a><br /></span></li><li><span><a href=\"https://docs.google.com/document/d/1f92k5CAqhy87SS_BFR6oxm0Swd7CHnt_XkS-Wz__5wM/edit?usp=sharing\" target=\"_blank\" rel=\"noopener noreferrer\">Recruitment Process Checklist – R 21</a><br /></span></li></ul><div><br /></div>";

        #endregion

        #endregion

        #region Zoho Forms

        public const string PrefixCrmR04Url = "https://crm.zoho.com/crm/org774688909/tab/CustomModule1";

        public const string FormR05AURL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R05AFranchiseeApplicationFormSupportOfficeAssessme";

        public const string FormR07URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R07FollowUpTelephoneCallScriptPriortoFirstInformal";

        public const string FormR07AURL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R07AInterviewerNotesFIRSTINFORMALMeeting";

        public const string FormR08URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R08InterviewerNotesSECONDINFORMALMeeting";

        public const string FormR09URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R09InterviewerNotesFIRSTFORMALMeeting";

        public const string FormR09AURL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R09AInterviewerNotesSECONDFORMALMeeting";

        public const string FormR10URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R10FollowUpNotesPOSTFORMALMeetings";

        public const string FormR11URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R11ApplicantScorecard";

        public const string FormR12URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R12ReferenceCheckQuestionnaireFinancial";

        public const string FormR13URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R13ReferenceCheckQuestionnairePersonal";

        public const string FormR14AURL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R14AAdvicetoProceedwithDocumentPreparation";

        public const string FormR15URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R15MeetingNotesSettlement";

        public const string FormR20URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R20ACCCRecruitmentComplianceChecklist";

        public const string FormR21URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R21RecruitmentProcessChecklist";

        public const string FormR23URL = "https://forms.zoho.com/pinjarrabakeryfranchising/form/R23DocumentReturnChecklist";

        #endregion

        #region Twilio

        public static string AccountSID => EnvironmentConstants.Get("PINJARRA_BAKERY_ACCOUNT_SID");

        public static string AuthToken => EnvironmentConstants.Get("PINJARRA_BAKERY_AUTH_TOKEN");

        public const string From = "+61482070216";

        public const string BookingUrl = "https://bit.ly/3wlh3D9";

        public const string BookingSmsBody = "Hi {FirstName},\n\n" +
            "Thanks for registering your interest to join the Pinjarra Bakery Family.\n\n" +
            "Please click the link to book your phone interview.\n" +
            "{BookingUrl}\n\n" +
            "We look forward to hearing from you soon.\n\n" +
            "Kind regards,\n" +
            "Pinjarra Bakery";

        #endregion

        #region Send Recruitment SMS

        public const string SendBookingSMS_200 = "Send Recruitment SMS SUCCESSFULLY!";

        public const string SendBookingSMS_400 = "Send Recruitment SMS FAILED!";

        #endregion

        #region PB Email Template

        public const string PinjarraEmailTemplate = "<!doctype html><html> <head> <meta name='viewport' content='width=device-width, initial-scale=1.0'/> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/> <title>Simple Transactional Email</title> <style>/* ------------------------------------- GLOBAL RESETS ------------------------------------- */ /*All the styling goes here*/ img{border: none; -ms-interpolation-mode: bicubic; max-width: 100%;}body{background-color: #f6f6f6; font-family: Arial, Helvetica, sans-serif; -webkit-font-smoothing: antialiased; font-size: 14px; line-height: 1.4; margin: 0; padding: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}table{border-collapse: separate; width: 100%;}table td{font-family: sans-serif; font-size: 14px; vertical-align: top;}/* ------------------------------------- BODY & CONTAINER ------------------------------------- */ .body{background-color: #f6f6f6; width: 100%;}/* Set a max-width, and make it display as block so it will automatically stretch to that width, but will also shrink down on a phone or something */ .container{display: block; margin: 0 auto !important; /* makes it centered */ max-width: 580px; padding: 10px; width: 580px;}/* This should also be a block element, so that it will fill 100% of the .container */ .content{box-sizing: border-box; display: block; margin: 0 auto; max-width: 580px; padding: 10px;}/* ------------------------------------- HEADER, FOOTER, MAIN ------------------------------------- */ .main{background: #ffffff; border-radius: 3px; width: 100%;}.wrapper{box-sizing: border-box; padding: 20px;}.content-block{padding-bottom: 10px; padding-top: 10px;}.footer{clear: both; margin-top: 10px; text-align: center; width: 100%;}.footer td, .footer p, .footer span, .footer a{color: #999999; font-size: 12px; text-align: center;}/* ------------------------------------- TYPOGRAPHY ------------------------------------- */ h1, h2, h3, h4{color: #000000; font-family: sans-serif; font-weight: 400; line-height: 1.4; margin: 0; margin-bottom: 30px;}h1{font-size: 35px; font-weight: 300; text-align: center; text-transform: capitalize;}p, ul, ol{font-family: sans-serif; font-size: 14px; font-weight: normal; margin: 0; margin-bottom: 15px;}p li, ul li, ol li{list-style-position: inside; margin-left: 5px;}a{color: #3498db; text-decoration: underline;}/* ------------------------------------- BUTTONS ------------------------------------- */ .btn{box-sizing: border-box; width: 100%;}.btn > tbody > tr > td{padding-bottom: 15px;}.btn table{width: auto;}.btn table td{background-color: #ffffff; border-radius: 5px; text-align: center;}.btn a{background-color: #ffffff; border: solid 1px #3498db; border-radius: 5px; box-sizing: border-box; color: #3498db; cursor: pointer; display: inline-block; font-size: 14px; font-weight: bold; margin: 0; padding: 12px 25px; text-decoration: none; text-transform: capitalize;}.btn-primary table td{background-color: #3498db;}.btn-primary a{background-color: #3498db; border-color: #3498db; color: #ffffff;}/* ------------------------------------- OTHER STYLES THAT MIGHT BE USEFUL ------------------------------------- */ .last{margin-bottom: 0;}.first{margin-top: 0;}.align-center{text-align: center;}.align-right{text-align: right;}.align-left{text-align: left;}.clear{clear: both;}.mt0{margin-top: 0;}.mb0{margin-bottom: 0;}.preheader{color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0;}.powered-by a{text-decoration: none;}hr{border: 0; border-bottom: 1px solid #f6f6f6; margin: 20px 0;}/* ------------------------------------- RESPONSIVE AND MOBILE FRIENDLY STYLES ------------------------------------- */ @media only screen and (max-width: 620px){table.body h1{font-size: 28px !important; margin-bottom: 10px !important;}table.body p, table.body ul, table.body ol, table.body td, table.body span, table.body a{font-size: 16px !important;}table.body .wrapper, table.body .article{padding: 10px !important;}table.body .content{padding: 0 !important;}table.body .container{padding: 0 !important; width: 100% !important;}table.body .main{border-left-width: 0 !important; border-radius: 0 !important; border-right-width: 0 !important;}table.body .btn table{width: 100% !important;}table.body .btn a{width: 100% !important;}table.body .img-responsive{height: auto !important; max-width: 100% !important; width: auto !important;}}/* ------------------------------------- PRESERVE THESE STYLES IN THE HEAD ------------------------------------- */ @media all{.ExternalClass{width: 100%;}.ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div{line-height: 100%;}.apple-link a{color: inherit !important; font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; text-decoration: none !important;}#MessageViewBody a{color: inherit; text-decoration: none; font-size: inherit; font-family: inherit; font-weight: inherit; line-height: inherit;}.btn-primary table td:hover{background-color: #34495e !important;}.btn-primary a:hover{background-color: #34495e !important; border-color: #34495e !important;}}</style> </head> <body> <table role='presentation' border='0' cellpadding='0' cellspacing='0' class='body'> <tr> <td>&nbsp;</td><td class='container'> <div class='content'> <table role='presentation' class='main'> <tr> <td class='wrapper'> <table role='presentation' border='0' cellpadding='0' cellspacing='0'> <tr> <td>{EmailContent}</td></tr></table> </td></tr></table> </div></td><td>&nbsp;</td></tr></table> </body></html>";

        public const string RefusalEmailSubject = "Your Franchise Application to Pinjarra Bakery";

        public const string RefusalEmailTemplate = "Dear {FirstName},\\n\\nThank you for your interest in the Pinjarra Bakery franchise. The number of responses we have received has been overwhelming. Unfortunately, and at this stage we do not have a suitable franchise opportunity for you.\\n\\nHowever, with your agreement, we would like to retain your personal details on file, since new opportunities are constantly emerging, and we may be able to revisit your application at a later date.\\n\\nYours sincerely,\\nDaniel Pantaleo\\nHead of Franchising\\nPinjarra Bakery Franchising Pty Ltd";

        public const string BookingEmailSubject = "We would like to know more about you!";

        public const string BookingEmailTemplate = "Dear {FirstName},\\n\\nThank you for your interest in the Pinjarra Bakery franchise.\\n\\nWe have reviewed your application and we think you are a suitable candidate to become a Pinjarra Bakery franchisee. Therefore, we would love to know more about you.\\n\\nCould you please go to: <a href='https://bit.ly/3wlh3D9' target='_blank'>Pinjarra Bakery Booking Link</a> and schedule a meeting with us?\\n\\nWe look forward to hearing from you.\\n\\nYours sincerely,\\nDaniel Pantaleo\\nHead of Franchising\\nPinjarra Bakery Franchising Pty Ltd";

        public const string R04NotiEmailSubject = "New R04 - Franchisee Expression Of Interest Form Submission - {FullName}";

        public const string R04NotiEmailTemplate = "Dear Mr. Daniel,<br/><br/>A new franchisee has just submitted the form <span style='background-color:rgb(255,255,153)'>R 04 Franchisee Expression of Interest</span>.<br/>Below is the main information:<br/><ul><li>Full Name: {FullName}</li><li>Email: <a href='mailto:{Email}' target='_blank'>{Email}</a></li><li>Mobile: {Mobile}</li><li>Contact URL: <a href='{ContactURL}' target='_blank'>Click here</a></li><li>R04 - Franchisee Expression Of Interest Form URL: <a href='{R04URL}' target='_blank'>Click here</a></li></ul>Thanks & Regards,<br/>PB Franchising";

        #endregion

        #region PB Franchisee Email

        public const string DanEmail = "daniel@pinjarrabakery.com.au";

        public const string Franchisee_Username = "franchising@pinjarrabakery.com.au";

        public const string PBFranchisingName = "PB Franchising";

        public static string Franchisee_Password => EnvironmentConstants.Get("PINJARRA_BAKERY_FRANCHISEE_PASSWORD");

        #endregion

        #region Custom Functions

        // Initialize Recruitment Tasks (IRT)

        public const string IRT_200 = "Initialize Tasks SUCCESSFULLY!";

        public const string IRT_400 = "Initialize Tasks FAILED!";

        public const string IRT_GetProjectById_400 = "[IRT-P01] Get project details FAILED!";

        public const string IRT_ContactUrlEmpty = "[IRT-C01] Contact URL is EMPTY!";

        public const string IRT_GetAllTasks_400 = "[IRT-R04] Get all tasks FAILED!";

        // Send Email (SE)

        public const string SE_200 = "Send Email SUCCESSFULLY!";

        public const string SE_400 = "Send Email FAILED!";

        public const string SE_GetProjectById_400 = "[IRT-P01] Get project details FAILED!";

        public const string SE_GetTaskById_400 = "[IRT-T01] Get task details FAILED!";

        public const string SE_EmailContentEmpty_400 = "[IRT-T02] Email Content in Task is EMPTY!";

        // R04 Notification (R04)

        public const string R04_200 = "Send R04 Notification Email SUCCESSFULLY!";

        public const string R04_400 = "Send R04 Notification Email FAILED!";

        // Handle Bakehouse Product Log from ZohoForms

        public const string BPL_200 = "Handle Bakehouse Product Log SUCCESSFULLY!";

        public const string BPL_400 = "Handle Bakehouse Product Log FAILED!";

        public const string BPL_SearchProductLog_400 = "[BPL-L01] Search Product Log FAILED!";

        public const string BPL_CreateProductLog_400 = "[BPL-L02] Create Product Log FAILED!";

        public const string BPL_UpdateProductLog_400 = "[BPL-L03] Update Product Log FAILED!";

        #endregion

    }
}
