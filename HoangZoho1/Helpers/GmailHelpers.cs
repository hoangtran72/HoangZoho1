using HoangZoho1.Models.GoogleAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HoangZoho1.Helpers
{
    public class GmailHelpers
    {

        public static Regex EmailRegex = new Regex(@"([a-zA-Z0-9+._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+)", RegexOptions.IgnoreCase);

        // must be yyyy-MM-dd format
        public static (long, long) ConvertDateToTimestamp(string date = null)
        {
            var currentDate = DateTime.UtcNow;

            var startTime = new DateTime(currentDate.Year , currentDate.Month, currentDate.Day, 
                0, 0, 0, DateTimeKind.Utc);
            var endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 
                23, 59, 59, DateTimeKind.Utc);

            if (!string.IsNullOrEmpty(date))
            {
                var dateSplits = date.Split('-');
                startTime = new DateTime(Convert.ToInt32(dateSplits[0]), Convert.ToInt32(dateSplits[1]),
                Convert.ToInt32(dateSplits[2]), 0, 0, 0, DateTimeKind.Utc);
                endTime = new DateTime(Convert.ToInt32(dateSplits[0]), Convert.ToInt32(dateSplits[1]),
                Convert.ToInt32(dateSplits[2]), 23, 59, 59, DateTimeKind.Utc);
            }
            startTime = startTime.AddHours(-10);
            var startTimeStamp = ConvertDateTimeToTimestamp(startTime);
            endTime = endTime.AddHours(-10);
            var endTimeStamp = ConvertDateTimeToTimestamp(endTime);

            return (startTimeStamp, endTimeStamp);
        }

        public static (long, long) ConvertDateRangeTimestamp(string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return (0, 0);
            }

            var startSplits = startDate.Split('-');
            var startTime = new DateTime(Convert.ToInt32(startSplits[0]), Convert.ToInt32(startSplits[1]),
                Convert.ToInt32(startSplits[2]), 0, 0, 0, DateTimeKind.Utc);
            startTime = startTime.AddHours(-10);
            var startTimeStamp = ConvertDateTimeToTimestamp(startTime);

            var endSplits = startDate.Split('-');
            var endTime = new DateTime(Convert.ToInt32(endSplits[0]), Convert.ToInt32(endSplits[1]),
                Convert.ToInt32(endSplits[2]), 23, 59, 59, DateTimeKind.Utc);
            endTime = endTime.AddHours(-10);
            var endTimeStamp = ConvertDateTimeToTimestamp(endTime);

            return (startTimeStamp, endTimeStamp);
        }

        private static long ConvertDateTimeToTimestamp(DateTime value)
        {
            TimeSpan epoch = value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //return the total seconds (which is a UNIX timestamp)
            return (long) epoch.TotalSeconds;
        }

        public static string Base64Decode(string Base64Test)
        {

            //STEP-1: Replace all special Character of Base64Test
            string EncodTxt = Base64Test.Replace("-", "+");
            EncodTxt = EncodTxt.Replace("_", "/");
            EncodTxt = EncodTxt.Replace(" ", "+");
            EncodTxt = EncodTxt.Replace("=", "+");

            //STEP-2: Fixed invalid length of Base64Test
            if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('=', 4 - EncodTxt.Length % 4); }
            else if (EncodTxt.Length % 4 == 0)
            {
                EncodTxt = EncodTxt.Substring(0, EncodTxt.Length - 1);
                if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('+', 4 - EncodTxt.Length % 4); }
            }

            //STEP-3: Convert to Byte array
            byte[] ByteArray = Convert.FromBase64String(EncodTxt);

            //STEP-4: Encoding to UTF8 Format
            return Encoding.UTF8.GetString(ByteArray);
        }

        public static byte[] Base64ToByte(string Base64Test)
        {

            //STEP-1: Replace all special Character of Base64Test
            string EncodTxt = Base64Test.Replace("-", "+");
            EncodTxt = EncodTxt.Replace("_", "/");
            EncodTxt = EncodTxt.Replace(" ", "+");
            EncodTxt = EncodTxt.Replace("=", "+");

            //STEP-2: Fixed invalid length of Base64Test
            if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('=', 4 - EncodTxt.Length % 4); }
            else if (EncodTxt.Length % 4 == 0)
            {
                EncodTxt = EncodTxt.Substring(0, EncodTxt.Length - 1);
                if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('+', 4 - EncodTxt.Length % 4); }
            }

            //STEP-3: Convert to Byte array
            return Convert.FromBase64String(EncodTxt);
        }

        public static string MsgNestedParts(List<MessagePart> Parts)
        {
            string emailBody = string.Empty;
            if (Parts.Count() < 0)
            {
                return string.Empty;
            }
            else
            {
                List<MessagePart> plainTestMail = Parts.Where(x => x.mimeType == "text/plain").ToList(); 
                List<MessagePart> attachmentMail = Parts.Where(x => x.mimeType == "multipart/alternative").ToList();
                List<MessagePart> relatedMail = Parts.Where(x => x.mimeType == "multipart/related").ToList();
                if (plainTestMail.Count() > 0)
                {
                    foreach (MessagePart eachPart in plainTestMail)
                    {
                        if (eachPart.parts == null)
                        {
                            if (eachPart.body != null && eachPart.body.data != null)
                            {
                                emailBody += eachPart.body.data;
                                if (!string.IsNullOrEmpty(emailBody))
                                {
                                    return emailBody;
                                }
                            }
                        }
                        else
                        {
                            emailBody = MsgNestedParts(eachPart.parts);
                            if (!string.IsNullOrEmpty(emailBody))
                            {
                                return emailBody;
                            }
                        }
                    }
                }
                if (attachmentMail.Count() > 0)
                {
                    foreach (MessagePart eachPart in attachmentMail)
                    {
                        if (eachPart.parts == null)
                        {
                            if (eachPart.body != null && eachPart.body.data != null)
                            {
                                emailBody += eachPart.body.data;
                                if (!string.IsNullOrEmpty(emailBody))
                                {
                                    return emailBody;
                                }
                            }
                        }
                        else
                        {
                            emailBody = MsgNestedParts(eachPart.parts);
                            if (!string.IsNullOrEmpty(emailBody))
                            {
                                return emailBody;
                            }
                        }
                    }
                }
                if (relatedMail.Count() > 0)
                {
                    foreach (MessagePart eachPart in relatedMail)
                    {
                        if (eachPart.parts == null)
                        {
                            if (eachPart.body != null && eachPart.body.data != null)
                            {
                                emailBody = eachPart.body.data;
                            }
                        }
                        else
                        {
                            emailBody = MsgNestedParts(eachPart.parts);
                            if (!string.IsNullOrEmpty(emailBody))
                            {
                                return emailBody;
                            }
                        }
                    }
                }
                

                return emailBody;
            }
        }

        public static string GetEmailFromText(string inputText)
        {

            var matchedEmail = EmailRegex.Match(inputText);
            return matchedEmail.ToString();
        }
    }
}
