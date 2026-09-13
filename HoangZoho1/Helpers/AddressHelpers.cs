using System.Collections.Generic;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using HoangZoho1.Models.Common;
using System.Linq;

namespace HoangZoho1.Helpers
{

    public class AddressHelpers
    {

        public static readonly Dictionary<string, string> USStateMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Alabama", "AL" },
                { "Alaska", "AK" },
                { "Arizona", "AZ" },
                { "Arkansas", "AR" },
                { "California", "CA" },
                { "Colorado", "CO" },
                { "Connecticut", "CT" },
                { "Delaware", "DE" },
                { "Florida", "FL" },
                { "Georgia", "GA" },
                { "Hawaii", "HI" },
                { "Idaho", "ID" },
                { "Illinois", "IL" },
                { "Indiana", "IN" },
                { "Iowa", "IA" },
                { "Kansas", "KS" },
                { "Kentucky", "KY" },
                { "Louisiana", "LA" },
                { "Maine", "ME" },
                { "Maryland", "MD" },
                { "Massachusetts", "MA" },
                { "Michigan", "MI" },
                { "Minnesota", "MN" },
                { "Mississippi", "MS" },
                { "Missouri", "MO" },
                { "Montana", "MT" },
                { "Nebraska", "NE" },
                { "Nevada", "NV" },
                { "New Hampshire", "NH" },
                { "New Jersey", "NJ" },
                { "New Mexico", "NM" },
                { "New York", "NY" },
                { "North Carolina", "NC" },
                { "North Dakota", "ND" },
                { "Ohio", "OH" },
                { "Oklahoma", "OK" },
                { "Oregon", "OR" },
                { "Pennsylvania", "PA" },
                { "Rhode Island", "RI" },
                { "South Carolina", "SC" },
                { "South Dakota", "SD" },
                { "Tennessee", "TN" },
                { "Texas", "TX" },
                { "Utah", "UT" },
                { "Vermont", "VT" },
                { "Virginia", "VA" },
                { "Washington", "WA" },
                { "West Virginia", "WV" },
                { "Wisconsin", "WI" },
                { "Wyoming", "WY" }
            };

        public static string GetUSStateCode(string stateName)
        {
            if (string.IsNullOrWhiteSpace(stateName))
                return null;

            stateName = RemoveDiacritics(stateName).Trim();

            return USStateMap.TryGetValue(stateName, out var code) ? code : null;
        }

        public static readonly Dictionary<string, string> CanadaProvinceMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Alberta", "AB" },
                { "British Columbia", "BC" },
                { "Manitoba", "MB" },
                { "New Brunswick", "NB" },
                { "Newfoundland and Labrador", "NL" },
                { "Northwest Territories", "NT" },
                { "Nova Scotia", "NS" },
                { "Nunavut", "NU" },
                { "Ontario", "ON" },
                { "Prince Edward Island", "PE" },
                { "Quebec", "QC" },
                { "Saskatchewan", "SK" },
                { "Yukon", "YT" }
            };

        public static string GetCanadaProvinceCode(string provinceName)
        {
            if (string.IsNullOrWhiteSpace(provinceName))
                return null;

            provinceName = RemoveDiacritics(provinceName).Trim();

            return CanadaProvinceMap.TryGetValue(provinceName, out var code) ? code : null;
        }

        public static ExtractAddressResponse ExtractAustralianAddress(string input)
        {

            var extractAddressResponse = new ExtractAddressResponse();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null; 
            }

            input = input.Replace(",", "").Trim();

            // Regex to extract [everything] [STATE] [POSTCODE]
            var match = Regex.Match(input, @"^(.*)\s+([A-Za-z]{2,3})\s+(\d{4})$", RegexOptions.IgnoreCase);
            if (!match.Success) return null;

            string streetAndSuburb = match.Groups[1].Value.Trim();
            string state = match.Groups[2].Value.Trim().ToUpper();
            string postcode = match.Groups[3].Value.Trim();

            extractAddressResponse.Street = streetAndSuburb;
            extractAddressResponse.State = state;
            extractAddressResponse.Postcode = postcode;

            return extractAddressResponse;

        }

        public static ParseCreatorAddress ParseCreatorAddress(string fullAddress)
        {
            if (string.IsNullOrWhiteSpace(fullAddress))
                return new ParseCreatorAddress();

            // Split by comma
            var parts = fullAddress.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var result = new ParseCreatorAddress();

            if (parts.Length > 0)
                result.AddressLine1 = parts[0];

            // Try to detect postcode (numeric, 4 digits in AU)
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                string partStr = parts[i];
                partStr = partStr.Trim();

                if (Regex.IsMatch(partStr, @"^\d{4}$"))
                {
                    result.Postcode = parts[i];
                    // The part before postcode is likely state
                    if (i - 1 >= 1)
                        result.State = parts[i - 1];
                    // The part before state is likely suburb
                    if (i - 2 >= 1)
                        result.Suburb = parts[i - 2];
                    return result;
                }
            }

            // If no postcode found, try to assign state (assuming AU states abbreviations)
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(parts[i], @"^(NSW|VIC|QLD|WA|SA|TAS|ACT|NT)$", RegexOptions.IgnoreCase))
                {
                    result.State = parts[i].ToUpper();
                    if (i - 1 >= 1)
                        result.Suburb = parts[i - 1];
                    return result;
                }
            }

            // Fallback: if only 2-3 parts
            if (parts.Length >= 2)
                result.Suburb = parts[1];
            if (parts.Length >= 3 && string.IsNullOrEmpty(result.State))
                result.State = parts[2];

            return result;
        }

        public static string RemoveDiacritics(string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }

}
