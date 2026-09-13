using HoangZoho1.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HoangZoho1.Helpers
{
    public class StringHelpers
    {

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static decimal ParseCurrency(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            // Remove all characters except digits, decimal point, and minus sign
            var cleaned = new string(input.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            if (decimal.TryParse(cleaned, out var result))
                return result;

            return 0;
        }

        public static string ExtractZohoProjectId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Regex: first long sequence of digits (16–20 digits) after "#"
            var match = Regex.Match(url, @"#(?:[^/]*/)*?(\d{16,20})");

            return match.Success ? match.Groups[1].Value : null;
        }

        public static string ExtractAccountNumber(string input)
        {

            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Regex to capture digits after XX-
            var match = Regex.Match(input, @"^[A-Z]{2}-(\d+)");
            return match.Success ? match.Groups[1].Value : string.Empty;

        }

        public static string ExtractMeetingLink(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Pattern for Google Meet, Microsoft Teams, and Zoom
            string pattern = @"https:\/\/(?:
                meet\.google\.com\/[^\s<>]+ |
                teams\.microsoft\.com\/l\/meetup-join\/[^\s<>]+ |
                (?:[\w\-]+\.)?zoom\.us\/j\/\d+(\?pwd=[^\s<>]+)?     # Zoom join link
            )";

            // Remove extra whitespace from regex
            pattern = Regex.Replace(pattern, @"\s+", "");

            Match match = Regex.Match(input, pattern);

            return match.Success ? match.Value : null;
        }

        public static string ExtractProjectId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Match the 16+ digit number right after #.../ or /.../
            var match = Regex.Match(url, @"(?:#|/)project(?:detail|s)?/(\d{15,})", RegexOptions.IgnoreCase);

            // If above doesn't match, try the variant where it's right after #dashboard/ or #zp/projects/
            if (!match.Success)
            {
                match = Regex.Match(url, @"(?:#(?:dashboard|zp/projects))/(\d{15,})", RegexOptions.IgnoreCase);
            }

            return match.Success ? match.Groups[1].Value : null;
        }

        public static string CleanSingleQuote(string text)
        {
            if (text.Contains("'"))
            {
                return text.Replace("'", "''");
            }
            return text;
        }

        public static string ExtractInvoiceNumber(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\d+");
            return matches.Count > 0 ? matches[matches.Count - 1].Value : string.Empty;
        }

        public static string CleanPhoneNumber(string mobile)
        {

            mobile = mobile.Replace(" ", "").Replace("-", "")
                .Replace("(", "").Replace(")", "");

            return mobile;

        }

        public static string ConvertNumberToFormat(decimal number, string currency)
        {

            string result = number.ToString("#,##0.##");

            if (!string.IsNullOrEmpty(currency))
            {
                result = currency + " " + result;
            }

            return result;

        }

        public static string ToSha256String(string value)
        {
            SHA256 digest = SHA256.Create();
            byte[] digestBytes = digest.ComputeHash(Encoding.UTF8.GetBytes(value));
            // Convert the byte array into an unhyphenated hexadecimal string.
            return BitConverter.ToString(digestBytes).Replace("-", string.Empty);
        }

        public static string NormalizeAndHash(string value, bool trimIntermediateSpaces = false)
        {
            SHA256 digest = SHA256.Create();
            string normalized;
            if (trimIntermediateSpaces)
            {
                normalized = value.Replace(" ", "").ToLower();
            }
            else
            {
                normalized = ToNormalizedValue(value);
            }
            return ToSha256String(normalized);
        }

        public static string ToNormalizedValue(string value)
        {
            return value.Trim().ToLower();
        }

        public static string ConvertSecondsToText(int totalSeconds)
        {

            int months = totalSeconds / CommonConstants.SecondsInMonth;
            totalSeconds %= CommonConstants.SecondsInMonth;

            int days = totalSeconds / CommonConstants.SecondsInDay;
            totalSeconds %= CommonConstants.SecondsInDay;

            int hours = totalSeconds / CommonConstants.SecondsInHour;
            totalSeconds %= CommonConstants.SecondsInHour;

            int minutes = totalSeconds / CommonConstants.SecondsInMinute;
            int seconds = totalSeconds % CommonConstants.SecondsInMinute;

            var parts = new List<string>();

            if (months > 0)
            {
                parts.Add($"{months} month{(months > 1 ? "s" : "")}");
            }
            if (days > 0) {
                parts.Add($"{days} day{(days > 1 ? "s" : "")}");
            }
            if (hours > 0)
            {
                parts.Add($"{hours} hour{(hours > 1 ? "s" : "")}");
            }
            if (minutes > 0)
            {
                parts.Add($"{minutes} minute{(minutes > 1 ? "s" : "")}");
            }
            if (seconds > 0 || parts.Count == 0) // Always show seconds, especially if it's the only unit left
            {
                parts.Add($"{seconds} second{(seconds != 1 ? "s" : "")}");
            }

            return string.Join(", ", parts);

        }

        public static string ExtractJsonData(string jsonString)
        {

            var readableText = new StringBuilder();

            try
            {

                var json = JObject.Parse(jsonString);

                foreach (var property in json.Properties())
                {
                    string key = ToTitleCase(property.Name.Replace("_", " "));
                    string value = property.Value.Type == JTokenType.String ? property.Value.ToString() : property.Value.ToString();

                    readableText.AppendLine($"- {key}: {value}");

                }

                return readableText.ToString();

            }
            catch (Exception ex)
            {
                return $"{ex.Message} - {ex.StackTrace}";
            }
        }

        public static string CleanPhpText(string serializedText)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                StringBuilder currentItem = new StringBuilder();
                bool insideString = false;
                foreach (char c in serializedText)
                {
                    if (c == '"')
                    {
                        insideString = !insideString;
                        if (!insideString)
                        {
                            // Closing quote indicates we have a complete string
                            if (builder.Length > 0)
                            {
                                builder.Append(", ");
                            }
                            builder.Append(currentItem.ToString());
                            currentItem.Clear();
                        }
                    }
                    else if (insideString)
                    {
                        currentItem.Append(c);
                    }
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

            return builder.ToString();
        }

        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string withSpaces = input.Replace("_", " ");

            return CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(withSpaces);
        }

        /// <summary>
        /// Calculates similarity between two dimensions using normalized Euclidean distance.
        /// </summary>
        /// <param name="height">Height of the dimension.</param>
        /// <param name="width">Width of the dimension.</param>
        /// <param name="depth">Depth of the dimension.</param>
        /// <param name="targetHeight">Target height.</param>
        /// <param name="targetWidth">Target width.</param>
        /// <param name="targetDepth">Target depth.</param>
        /// <returns>A value from 0 (furthest) to 1 (nearest).</returns>
        public static double CalculateDimensionSimilarity(
            double targetWidth, double targetDepth, double targetHeight,
            double itemWidth, double itemDepth, double itemHeight)
        {
            double widthMatch = GetDimensionScore(targetWidth, itemWidth);
            double depthMatch = GetDimensionScore(targetDepth, itemDepth);
            double heightMatch = GetDimensionScore(targetHeight, itemHeight);

            double averageMatch = (widthMatch + depthMatch + heightMatch) / 3.0;
            return averageMatch;
        }

        private static double GetDimensionScore(double target, double actual)
        {
            double diff = Math.Abs(target - actual);
            double score = (1 - (diff / target)) * 100;
            return Math.Max(0, Math.Min(100, score)); // clamp between 0 and 100
        }

        public static (int Width, int Depth, int Height) ExtractDimensions(string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return (0, 0, 0);
            }    

            // Normalize input: remove extra spaces and lowercase
            input = Regex.Replace(input, @"\s+", " ").Trim();


            // Match pattern: number followed by letter in parentheses, e.g., 914(W)
            var pattern = @"(\d+)\s*\((W|w|D|d|H|h)\)";
            var matches = Regex.Matches(input, pattern);

            int width = 0, depth = 0, height = 0;

            foreach (Match match in matches)
            {
                int value = int.Parse(match.Groups[1].Value);
                string dimension = match.Groups[2].Value;

                switch (dimension)
                {
                    case "W":
                        width = value;
                        break;
                    case "w":
                        width = value;
                        break;
                    case "D":
                        depth = value;
                        break;
                    case "d":
                        depth = value;
                        break;
                    case "H":
                        height = value;
                        break;
                    case "h":
                        height = value;
                        break;
                }
            }

            return (width, depth, height);
        }

        public static (string Carrier, string ShippingType) ParseShippingMethod(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ("", "");

            // Split on '.'
            var parts = input.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return ("", "");

            // Capitalize first letter, lowercase rest
            string carrier = char.ToUpper(parts[0][0]) + parts[0][1..].ToLower();
            string shippingType = char.ToUpper(parts[1][0]) + parts[1][1..].ToLower();

            return (carrier, shippingType);
        }

        /// <summary>
        /// Checks whether a name looks human (non-empty, alphabetical, reasonable length).
        /// </summary>
        private static bool IsValidHumanName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (name.Length < 2 || name.Length > 40) return false;

            // Must contain at least one letter and only letters, hyphen, or space
            return Regex.IsMatch(name, @"^[A-Za-zÀ-ÿ'\- ]+$");
        }

        /// <summary>
        /// Extracts a first and last name from an email (e.g. john.doe@gmail.com -> John, Doe).
        /// </summary>
        private static (string firstName, string lastName) ExtractFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return ("", "");

            var username = email.Split('@')[0];
            username = username.Replace(".", " ").Replace("_", " ").Replace("-", " ");
            username = Regex.Replace(username, @"\d+", ""); // remove numbers
            username = Regex.Replace(username, @"\s{2,}", " ").Trim();

            var parts = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return ("", "");

            string first = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[0].ToLower());
            string last = parts.Length > 1
                ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[^1].ToLower())
                : first; // duplicate if only one word

            return (first, last);
        }

        /// <summary>
        /// Returns a "cleaned" first/last name pair based on provided data and email.
        /// </summary>
        public static (string FirstName, string LastName) GetBetterName(string firstName, string lastName, string email)
        {
            bool validFirst = IsValidHumanName(firstName);
            bool validLast = IsValidHumanName(lastName);

            if (validFirst && validLast)
                return (Capitalize(firstName), Capitalize(lastName));

            var extracted = ExtractFromEmail(email);

            // Replace only invalid or missing values
            string finalFirst = validFirst ? Capitalize(firstName) : extracted.firstName;
            string finalLast = validLast ? Capitalize(lastName) : extracted.lastName;

            // Ensure both are non-empty
            if (string.IsNullOrEmpty(finalFirst) && !string.IsNullOrEmpty(finalLast))
                finalFirst = finalLast;
            if (string.IsNullOrEmpty(finalLast) && !string.IsNullOrEmpty(finalFirst))
                finalLast = finalFirst;

            return (finalFirst, finalLast);
        }

        private static string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower().Trim());
        }

        #region Postal Code

        public static string FormatCanadianPostalCode(string postalCode)
        {
            // 1. Remove all spaces and non-alphanumeric characters.
            string cleanPostalCode = Regex.Replace(postalCode, "[^a-zA-Z0-9]", "");

            // 2. Convert to uppercase.
            string upperCasePostalCode = cleanPostalCode.ToUpper();

            // 3. Check for the correct length and format (A1A1A1).
            string pattern = "^[A-Z]\\d[A-Z]\\d[A-Z]\\d$";
            if (!Regex.IsMatch(upperCasePostalCode, pattern))
            {
                return "Invalid postal code format. Should be A1A1A1 (without spaces).";
            }

            // 4. Insert space after the third character to get A1A 1A1
            string formattedPostalCode = upperCasePostalCode.Insert(3, " ");
            return formattedPostalCode;
        }

        public static (string PhoneNumber, string Extension) 
            ExtractPhoneAndExtension(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
                return (null, null);

            // Regex pattern:
            // - Capture phone number (with optional spaces or dashes)
            // - Optionally capture extension (e.g., ext, ext., extension, etc.)
            string pattern = @"(?<phone>\d{3}[-\s]?\d{3}[-\s]?\d{4})\s*(?:ext\.?|extension)?\s*(?<ext>\d+)?";

            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string phone = match.Groups["phone"].Value;
                string ext = match.Groups["ext"].Success ? match.Groups["ext"].Value : null;
                return (phone, ext);
            }

            return (null, null);

        }

        #endregion

        #region Jaro–Winkler distance

        /* The Winkler modification will not be applied unless the 
         * percent match was at or above the mWeightThreshold percent 
         * without the modification. 
         * Winkler's paper used a default value of 0.7
         */
        private static readonly double mWeightThreshold = 0.7;

        /* Size of the prefix to be concidered by the Winkler modification. 
         * Winkler's paper used a default value of 4
         */
        private static readonly int mNumChars = 4;

        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (perfect match) to 1 (no match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double distance(string aString1, string aString2)
        {
            return 1.0 - Proximity(aString1, aString2);
        }

        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (no match) to 1 (perfect match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <returns></returns>
        public static double Proximity(string aString1, string aString2)
        {
            // 1. Handle nulls and empty strings
            if (aString1 == null || aString2 == null) return 0.0;

            int lLen1 = aString1.Length;
            int lLen2 = aString2.Length;

            if (lLen1 == 0) return lLen2 == 0 ? 1.0 : 0.0;

            // 2. Define standard Jaro-Winkler constants
            const double mWeightThreshold = 0.7;
            const int mNumChars = 4; // Max prefix length

            // 3. Match Range
            int lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

            bool[] lMatched1 = new bool[lLen1];
            bool[] lMatched2 = new bool[lLen2];

            int lNumCommon = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                int lStart = Math.Max(0, i - lSearchRange);
                int lEnd = Math.Min(i + lSearchRange + 1, lLen2);

                for (int j = lStart; j < lEnd; ++j)
                {
                    if (lMatched2[j]) continue;
                    if (aString1[i] != aString2[j]) continue;

                    lMatched1[i] = true;
                    lMatched2[j] = true;
                    ++lNumCommon;
                    break;
                }
            }

            if (lNumCommon == 0) return 0.0;

            // 4. Transposition Calculation
            int lNumHalfTransposed = 0;
            int k = 0;
            for (int i = 0; i < lLen1; ++i)
            {
                if (!lMatched1[i]) continue;
                while (!lMatched2[k]) ++k;

                if (aString1[i] != aString2[k])
                    ++lNumHalfTransposed;
                ++k;
            }

            double lNumCommonD = lNumCommon;
            double lNumTransposed = lNumHalfTransposed / 2.0;

            // 5. Jaro Score
            double lWeight = (lNumCommonD / lLen1
                             + lNumCommonD / lLen2
                             + (lNumCommonD - lNumTransposed) / lNumCommonD) / 3.0;

            // 6. Winkler Adjustment (Prefix Scale)
            if (lWeight <= mWeightThreshold) return lWeight;

            int lMaxPrefix = Math.Min(mNumChars, Math.Min(lLen1, lLen2));
            int lPrefixLength = 0;

            while (lPrefixLength < lMaxPrefix && aString1[lPrefixLength] == aString2[lPrefixLength])
                ++lPrefixLength;

            return lWeight + 0.1 * lPrefixLength * (1.0 - lWeight);
        }

        #endregion

        #region HTML

        public static bool IsValidUrl(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return true;
            }

            return false;

        }

        public static bool IsValidURL(string URL)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(URL);
        }

        public static async Task<string> GetPageTitleAsync(string url)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Fetch the HTML content
                    var html = await client.GetStringAsync(url);

                    // Extract the <title> content using regex
                    var match = Regex.Match(html, @"<title>\s*(.+?)\s*</title>", RegexOptions.IgnoreCase);

                    if (match.Success)
                        return match.Groups[1].Value.Trim();

                    return "(No title found)";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        public static string CleanUrl(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string url = input.Trim();

            // Replace escaped forward slashes (\/) with /
            url = url.Replace("\\/", "/");

            // Replace escaped backslashes (\\) with \
            url = url.Replace(@"\\", @"\");

            // Remove wrapping quotes if present
            if ((url.StartsWith("\"") && url.EndsWith("\"")) ||
                (url.StartsWith("'") && url.EndsWith("'")))
            {
                url = url.Substring(1, url.Length - 2);
            }

            // Fix accidental double "https://" or "http://"
            url = url.Replace("https:////", "https://")
                     .Replace("http:////", "http://");

            return url;

        }

        #endregion

    }
}
