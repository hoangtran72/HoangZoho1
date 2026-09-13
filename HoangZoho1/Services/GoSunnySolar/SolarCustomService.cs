using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{
    public class SolarCustomService : ISolarCustomService
    {
        private readonly ISolarGhlService _ghlService;
        private readonly ISolarCrmService _crmService;

        public SolarCustomService(ISolarGhlService ghlService, ISolarCrmService crmService)
        {
            _ghlService = ghlService;
            _crmService = crmService;
        }

        public async Task<ApiResultDto<string>> UpsertLeadsInCrmFromGhl(string contactId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                // Step 1: Get contact details from GHL
                var getContactResult = await _ghlService.GetContactById(contactId);
                if (getContactResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }
                var contactDetails = getContactResult.Data.contact;
                string firstName = contactDetails.firstName;
                string lastName = contactDetails.lastName;
                string email = contactDetails.email;
                string source = contactDetails.source;

                if (string.IsNullOrEmpty(email))
                {
                    return apiResult;
                }

                if (string.IsNullOrEmpty(source))
                {
                    source = "Go High Level";
                }
                else
                {
                    source = source.ToUpper();
                }

                // Step 2: Search CRM Leads using Email
                var searchLeadsResult = await _crmService.SearchLeadsByEmail(email);
                if (searchLeadsResult.Code == ResultCode.BadRequest)
                {
                    return apiResult;
                }
                else if (searchLeadsResult.Code == ResultCode.OK)
                {
                    // Step 2.1: Lead already exists, NEED TO UPDATE LEAD
                    var searchLeads = searchLeadsResult.Data.data;
                    foreach (var lead in searchLeads)
                    {
                        string leadId = lead.id;
                        var updateData = new UpsertRequest<LeadForUpdation>();

                        var leadForUpdation = new LeadForUpdation()
                        {
                            GHL_Link = $"{GoSunnySolarConstants.GhlPrefixContactUrl}/{contactId}" 
                        };
                        updateData.data.Add(leadForUpdation);
                        updateData.trigger.Add(CommonConstants.ZohoWorkflow);
                        var updateLeadResult = await _crmService.UpdateLead(leadId, updateData);
                    }
                }
                else if (searchLeadsResult.Code == ResultCode.NoContent)
                {
                    // Step 2.2: Lead doesn't exist, NEED TO CREATE LEAD
                    var insertData = new UpsertRequest<LeadForCreation>();

                    var leadForCreation = new LeadForCreation()
                    {
                        First_Name = firstName,
                        Last_Name = lastName,
                        GHL_Link = $"{GoSunnySolarConstants.GhlPrefixContactUrl}/{contactId}",
                        Lead_Source = source
                    };
                    insertData.data.Add(leadForCreation);
                    insertData.trigger.Add(CommonConstants.ZohoWorkflow);
                    var insertLeadResult = await _crmService.CreateLead(insertData);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

    }
}
