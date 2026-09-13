using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class EmailConstants
    {

        #region Email hoagsun@gmail.com

        public const string Gmail_SmtpServer = "smtp.gmail.com";

        public const int SmtpPort = 587;

        public static string MyEmail_Username => EnvironmentConstants.Get("EMAIL_MY_EMAIL_USERNAME");

        public static string MyEmail_Password => EnvironmentConstants.Get("EMAIL_MY_EMAIL_PASSWORD");

        #endregion

    }
}
