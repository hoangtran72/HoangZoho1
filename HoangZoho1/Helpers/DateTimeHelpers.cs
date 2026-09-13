using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoangZoho1.Helpers
{
    public class DateTimeHelpers
    {

        public static string ConvertFormDateToCrmDate(string formDate)
        {
            var dateTime = Convert.ToDateTime(formDate);
            return dateTime.ToString("yyyy-MM-dd");
        }

        

        public static string ConvertSecondsToDisplayTime(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = (totalSeconds % 60);

            if (hours > 0)
            {
                return string.Format("{0}h{1}m{2}s", hours, minutes, seconds);
            }
            else if (minutes > 0)
                return string.Format("{0}m{1}s", minutes, seconds);
            else
                return string.Format("{0}s", seconds);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTime;
        }

        public static DateTime ConvertDateTimeToTimeZone(DateTime dateTime, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

        }

        public static string UnixTimeStampToDateTime(ConvertTimeStampRequest request)
        {

            double unixTimeStamp = request.TimeStamp.Value;
            string format = request.ReturnedFormat;
            double type = request.Type.Value;
            string finalDateTimeStr = "";

            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            if (type == 1)
            {
                dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            }
            else
            {
                dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToUniversalTime();
            }

            string timeZone = request.TimeZone;
            TimeZoneInfo tz = null;
            if (!string.IsNullOrEmpty(timeZone))
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                finalDateTimeStr = TimeZoneInfo.ConvertTimeFromUtc
                    (dateTime, tz).ToString(format);
            }
            else
            {
                finalDateTimeStr = dateTime.ToString(format);
            }

            return finalDateTimeStr;
        }

        public static Dictionary<string, string> LoadFromJson(string filePath)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(filePath));
            var zones = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var mapZones = doc
                .RootElement
                .GetProperty("supplemental")
                .GetProperty("windowsZones")
                .GetProperty("mapTimezones")
                .EnumerateArray();

            foreach (var mapZoneElement in mapZones)
            {
                var mz = mapZoneElement.GetProperty("mapZone");
                var windowsId = mz.GetProperty("_other").GetString();
                var territory = mz.GetProperty("_territory").GetString();

                // Only use global mappings
                //if (territory != "001") continue;

                var types = mz.GetProperty("_type").GetString();
                var ianaZones = types.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var ianaZone in ianaZones)
                {
                    if (!zones.ContainsKey(ianaZone))
                        zones[ianaZone] = windowsId;
                }
            }

            return zones;
        }

        public static TimeZoneInfo ConvertToTimeZoneInfo(string ianaZone, Dictionary<string, string> map)
        {
            if (!map.TryGetValue(ianaZone, out var windows))
                throw new TimeZoneNotFoundException($"Unknown IANA time zone: {ianaZone}");
            return TimeZoneInfo.FindSystemTimeZoneById(windows);
        }

        public static TimeZoneInfo OlsonTimeZoneToTimeZoneInfo(string olsonTimeZoneId)
        {
            var olsonWindowsTimes = new Dictionary<string, string>()
            {
                { "Africa/Abidjan", "GMT Standard Time" },
                { "Africa/Accra", "GMT Standard Time" },
                { "Africa/Addis_Ababa", "E. Africa Standard Time" },
                { "Africa/Algiers", "W. Central Africa Standard Time" },
                { "Africa/Asmara", "E. Africa Standard Time" },
                { "Africa/Bamako", "GMT Standard Time" },
                { "Africa/Bangui", "W. Central Africa Standard Time" },
                { "Africa/Banjul", "GMT Standard Time" },
                { "Africa/Bissau", "GMT Standard Time" },
                { "Africa/Blantyre", "South Africa Standard Time" },
                { "Africa/Brazzaville", "W. Central Africa Standard Time" },
                { "Africa/Bujumbura", "South Africa Standard Time" },
                { "Africa/Cairo", "Egypt Standard Time" },
                { "Africa/Casablanca", "Morocco Standard Time" },
                { "Africa/Ceuta", "W. Central Africa Standard Time" },
                { "Africa/Conakry", "GMT Standard Time" },
                { "Africa/Dakar", "GMT Standard Time" },
                { "Africa/Dar_es_Salaam", "E. Africa Standard Time" },
                { "Africa/Djibouti", "E. Africa Standard Time" },
                { "Africa/Douala", "W. Central Africa Standard Time" },
                { "Africa/El_Aaiun", "GMT Standard Time" },
                { "Africa/Freetown", "GMT Standard Time" },
                { "Africa/Gaborone", "South Africa Standard Time" },
                { "Africa/Harare", "South Africa Standard Time" },
                { "Africa/Johannesburg", "South Africa Standard Time" },
                { "Africa/Juba", "E. Africa Standard Time" },
                { "Africa/Kampala", "E. Africa Standard Time" },
                { "Africa/Khartoum", "E. Africa Standard Time" },
                { "Africa/Kigali", "South Africa Standard Time" },
                { "Africa/Kinshasa", "W. Central Africa Standard Time" },
                { "Africa/Lagos", "W. Central Africa Standard Time" },
                { "Africa/Libreville", "W. Central Africa Standard Time" },
                { "Africa/Lome", "GMT Standard Time" },
                { "Africa/Luanda", "W. Central Africa Standard Time" },
                { "Africa/Lubumbashi", "South Africa Standard Time" },
                { "Africa/Lusaka", "South Africa Standard Time" },
                { "Africa/Malabo", "W. Central Africa Standard Time" },
                { "Africa/Maputo", "South Africa Standard Time" },
                { "Africa/Maseru", "South Africa Standard Time" },
                { "Africa/Mbabane", "South Africa Standard Time" },
                { "Africa/Mogadishu", "E. Africa Standard Time" },
                { "Africa/Monrovia", "GMT Standard Time" },
                { "Africa/Nairobi", "E. Africa Standard Time" },
                { "Africa/Ndjamena", "W. Central Africa Standard Time" },
                { "Africa/Niamey", "W. Central Africa Standard Time" },
                { "Africa/Nouakchott", "GMT Standard Time" },
                { "Africa/Ouagadougou", "GMT Standard Time" },
                { "Africa/Porto-Novo", "W. Central Africa Standard Time" },
                { "Africa/Sao_Tome", "GMT Standard Time" },
                { "Africa/Tripoli", "Middle East Standard Time" },
                { "Africa/Tunis", "W. Central Africa Standard Time" },
                { "Africa/Windhoek", "Namibia Standard Time" },
                { "America/Adak", "Hawaiian Standard Time" },
                { "America/Anchorage", "Alaskan Standard Time" },
                { "America/Anguilla", "Pacific SA Standard Time" },
                { "America/Antigua", "Atlantic Standard Time" },
                { "America/Araguaina", "E. South America Standard Time" },
                { "America/Argentina/Buenos_Aires", "Argentina Standard Time" },
                { "America/Argentina/Catamarca", "Argentina Standard Time" },
                { "America/Argentina/Cordoba", "Argentina Standard Time" },
                { "America/Argentina/Jujuy", "Argentina Standard Time" },
                { "America/Argentina/La_Rioja", "Argentina Standard Time" },
                { "America/Argentina/Mendoza", "Argentina Standard Time" },
                { "America/Argentina/Rio_Gallegos", "Argentina Standard Time" },
                { "America/Argentina/Salta", "Argentina Standard Time" },
                { "America/Argentina/San_Juan", "Argentina Standard Time" },
                { "America/Argentina/San_Luis", "Argentina Standard Time" },
                { "America/Argentina/Tucuman", "Argentina Standard Time" },
                { "America/Argentina/Ushuaia", "Argentina Standard Time" },
                { "America/Aruba", "Atlantic Standard Time" },
                { "America/Asuncion", "Paraguay Standard Time" },
                { "America/Atikokan", "Eastern Standard Time" },
                { "America/Bahia", "E. South America Standard Time" },
                { "America/Barbados", "Pacific SA Standard Time" },
                { "America/Belem", "E. South America Standard Time" },
                { "America/Belize", "Central America Standard Time" },
                { "America/Blanc-Sablon", "Atlantic Standard Time" },
                { "America/Boa_Vista", "Atlantic Standard Time" },
                { "America/Bogota", "SA Pacific Standard Time" },
                { "America/Boise", "Mountain Standard Time" },
                { "America/Cambridge_Bay", "Mountain Standard Time" },
                { "America/Campo_Grande", "Atlantic Standard Time" },
                { "America/Cancun", "Central Standard Time (Mexico)" },
                { "America/Caracas", "Venezuela Standard Time" },
                { "America/Cayenne", "SA Eastern Standard Time" },
                { "America/Cayman", "Eastern Standard Time" },
                { "America/Chicago", "Central Standard Time" },
                { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
                { "America/Costa_Rica", "Central America Standard Time" },
                { "America/Cuiaba", "Central Brazilian Standard Time" },
                { "America/Curacao", "Central Brazilian Standard Time" },
                { "America/Danmarkshavn", "GMT Standard Time" },
                { "America/Dawson", "Pacific Standard Time" },
                { "America/Dawson_Creek", "Mountain Standard Time" },
                { "America/Denver", "Mountain Standard Time" },
                { "America/Detroit", "Eastern Standard Time" },
                { "America/Dominica", "Pacific SA Standard Time" },
                { "America/Edmonton", "Mountain Standard Time" },
                { "America/Eirunepe", "Atlantic Standard Time" },
                { "America/El_Salvador", "Central America Standard Time" },
                { "America/Fortaleza", "SA Eastern Standard Time" },
                { "America/Glace_Bay", "Atlantic Standard Time" },
                { "America/Godthab", "Greenland Standard Time" },
                { "America/Goose_Bay", "Atlantic Standard Time" },
                { "America/Grand_Turk", "Eastern Standard Time" },
                { "America/Grenada", "Atlantic Standard Time" },
                { "America/Guadeloupe", "Atlantic Standard Time" },
                { "America/Guatemala", "Central America Standard Time" },
                { "America/Guayaquil", "Eastern Standard Time" },
                { "America/Guyana", "Atlantic Standard Time" },
                { "America/Halifax", "Atlantic Standard Time" },
                { "America/Havana", "SA Pacific Standard Time" },
                { "America/Hermosillo", "Mountain Standard Time" },
                { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
                { "America/Indiana/Knox", "Central Standard Time" },
                { "America/Indiana/Marengo", "US Eastern Standard Time" },
                { "America/Indiana/Petersburg", "US Eastern Standard Time" },
                { "America/Indiana/Tell_City", "Central Standard Time" },
                { "America/Indiana/Vevay", "US Eastern Standard Time" },
                { "America/Indiana/Vincennes", "US Eastern Standard Time" },
                { "America/Indiana/Winamac", "US Eastern Standard Time" },
                { "America/Inuvik", "Mountain Standard Time" },
                { "America/Iqaluit", "Eastern Standard Time" },
                { "America/Jamaica", "Eastern Standard Time" },
                { "America/Juneau", "Alaskan Standard Time" },
                { "America/Kentucky/Louisville", "Eastern Standard Time" },
                { "America/Kentucky/Monticello", "Eastern Standard Time" },
                { "America/La_Paz", "SA Western Standard Time" },
                { "America/Lima", "SA Western Standard Time" },
                { "America/Los_Angeles", "Pacific Standard Time" },
                { "America/Maceio", "E. South America Standard Time" },
                { "America/Managua", "Central Standard Time" },
                { "America/Manaus", "SA Western Standard Time" },
                { "America/Marigot", "Atlantic Standard Time" },
                { "America/Martinique", "Atlantic Standard Time" },
                { "America/Matamoros", "Central Standard Time" },
                { "America/Mazatlan", "Mountain Standard Time" },
                { "America/Menominee", "Central Standard Time" },
                { "America/Merida", "Central Standard Time" },
                { "America/Mexico_City", "Central Standard Time (Mexico)" },
                { "America/Miquelon", "E. South America Standard Time" },
                { "America/Moncton", "Atlantic Standard Time" },
                { "America/Monterrey", "Central Standard Time (Mexico)" },
                { "America/Montevideo", "Montevideo Standard Time" },
                { "America/Montreal", "Eastern Standard Time" },
                { "America/Montserrat", "Atlantic Standard Time" },
                { "America/Nassau", "Eastern Standard Time" },
                { "America/New_York", "Eastern Standard Time" },
                { "America/Nome", "Alaskan Standard Time" },
                { "America/Noronha", "Mid-Atlantic Standard Time" },
                { "America/North_Dakota/Center", "Central Standard Time" },
                { "America/North_Dakota/New_Salem", "Central Standard Time" },
                { "America/Ojinaga", "US Mountain Standard Time" },
                { "America/Panama", "SA Pacific Standard Time" },
                { "America/Pangnirtung", "Eastern Standard Time" },
                { "America/Paramaribo", "SA Eastern Standard Time" },
                { "America/Phoenix", "US Mountain Standard Time" },
                { "America/Port_of_Spain", "Atlantic Standard Time" },
                { "America/Port-au-Prince", "Eastern Standard Time" },
                { "America/Porto_Velho", "Central Brazilian Standard Time" },
                { "America/Puerto_Rico", "Central Brazilian Standard Time" },
                { "America/Rainy_River", "Central Standard Time" },
                { "America/Rankin_Inlet", "Central Standard Time" },
                { "America/Recife", "SA Eastern Standard Time" },
                { "America/Regina", "Central Standard Time" },
                { "America/Resolute", "Eastern Standard Time" },
                { "America/Rio_Branco", "Central Brazilian Standard Time" },
                { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
                { "America/Santarem", "E. South America Standard Time" },
                { "America/Santiago", "Pacific SA Standard Time" },
                { "America/Santo_Domingo", "Atlantic Standard Time" },
                { "America/Sao_Paulo", "E. South America Standard Time" },
                { "America/Scoresbysund", "Azores Standard Time" },
                { "America/Shiprock", "Mountain Standard Time" },
                { "America/St_Barthelemy", "Atlantic Standard Time" },
                { "America/St_Johns", "Newfoundland Standard Time" },
                { "America/St_Kitts", "Atlantic Standard Time" },
                { "America/St_Lucia", "Atlantic Standard Time" },
                { "America/St_Thomas", "Atlantic Standard Time" },
                { "America/St_Vincent", "Atlantic Standard Time" },
                { "America/Swift_Current", "Central Standard Time" },
                { "America/Tegucigalpa", "Central Standard Time" },
                { "America/Thule", "Atlantic Standard Time" },
                { "America/Thunder_Bay", "Eastern Standard Time" },
                { "America/Tijuana", "Pacific Standard Time (Mexico)" },
                { "America/Toronto", "Eastern Standard Time" },
                { "America/Tortola", "Atlantic Standard Time" },
                { "America/Vancouver", "Pacific Standard Time" },
                { "America/Whitehorse", "Pacific Standard Time" },
                { "America/Winnipeg", "Central Standard Time" },
                { "America/Yakutat", "Alaskan Standard Time" },
                { "America/Yellowknife", "Mountain Standard Time" },
                { "Antarctica/Casey", "W. Australia Standard Time" },
                { "Antarctica/Davis", "SE Asia Standard Time" },
                { "Antarctica/Macquarie", "AUS Eastern Standard Time" },
                { "Antarctica/Mawson", "Central Asia Standard Time" },
                { "Antarctica/Palmer", "Atlantic Standard Time" },
                { "Antarctica/Rothera", "Argentina Standard Time" },
                { "Antarctica/Syowa", "Kaliningrad Standard Time" },
                { "Antarctica/Vostok", "GMT Standard Time" },
                { "Arctic/Longyearbyen", "W. Europe Standard Time" },
                { "Asia/Aden", "Arabian Standard Time" },
                { "Asia/Almaty", "Central Asia Standard Time" },
                { "Asia/Amman", "Jordan Standard Time" },
                { "Asia/Aqtau", "West Asia Standard Time" },
                { "Asia/Aqtobe", "West Asia Standard Time" },
                { "Asia/Ashgabat", "West Asia Standard Time" },
                { "Asia/Baghdad", "Arabic Standard Time" },
                { "Asia/Bahrain", "Arabic Standard Time" },
                { "Asia/Baku", "Caucasus Standard Time" },
                { "Asia/Bangkok", "SE Asia Standard Time" },
                { "Asia/Beirut", "Middle East Standard Time" },
                { "Asia/Bishkek", "Central Asia Standard Time" },
                { "Asia/Brunei", "Singapore Standard Time" },
                { "Asia/Choibalsan", "Ulaanbaatar Standard Time" },
                { "Asia/Chongqing", "China Standard Time" },
                { "Asia/Colombo", "Sri Lanka Standard Time" },
                { "Asia/Damascus", "Syria Standard Time" },
                { "Asia/Dhaka", "Bangladesh Standard Time" },
                { "Asia/Dili", "Tokyo Standard Time" },
                { "Asia/Dubai", "Arabian Standard Time" },
                { "Asia/Dushanbe", "West Asia Standard Time" },
                { "Asia/Gaza", "West Bank Standard Time" },
                { "Asia/Harbin", "China Standard Time" },
                { "Asia/Ho_Chi_Minh", "SE Asia Standard Time" },
                { "Asia/Hong_Kong", "China Standard Time" },
                { "Asia/Hovd", "N. Central Asia Standard Time" },
                { "Asia/Jakarta", "SE Asia Standard Time" },
                { "Asia/Irkutsk", "North Asia East Standard Time" },
                { "Asia/Jerusalem", "Israel Standard Time" },
                { "Asia/Kabul", "Afghanistan Standard Time" },
                { "Asia/Kamchatka", "Kamchatka Standard Time" },
                { "Asia/Karachi", "Pakistan Standard Time" },
                { "Asia/Kathmandu", "Nepal Standard Time" },
                { "Asia/Kolkata", "India Standard Time" },
                { "Asia/Krasnoyarsk", "North Asia Standard Time" },
                { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
                { "Asia/Kuwait", "Arab Standard Time" },
                { "Asia/Magadan", "Magadan Standard Time" },
                { "Asia/Manila", "Singapore Standard Time" },
                { "Asia/Makassar", "Singapore Standard Time" },
                { "Asia/Muscat", "Arabian Standard Time" },
                { "Asia/Nicosia", "Middle East Standard Time" },
                { "Asia/Novokuznetsk", "N. Central Asia Standard Time" },
                { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
                { "Asia/Omsk", "N. Central Asia Standard Time" },
                { "Asia/Oral", "West Asia Standard Time" },
                { "Asia/Phnom_Penh", "SE Asia Standard Time" },
                { "Asia/Pontianak", "SE Asia Standard Time" },
                { "Asia/Qatar", "Arab Standard Time" },
                { "Asia/Qyzylorda", "Central Asia Standard Time" },
                { "Asia/Rangoon", "Myanmar Standard Time" },
                { "Asia/Riyadh", "Arab Standard Time" },
                { "Asia/Samarkand", "West Asia Standard Time" },
                { "Asia/Seoul", "Korea Standard Time" },
                { "Asia/Shanghai", "China Standard Time" },
                { "Asia/Singapore", "Singapore Standard Time" },
                { "Asia/Taipei", "Taipei Standard Time" },
                { "Asia/Tashkent", "West Asia Standard Time" },
                { "Asia/Tbilisi", "Georgian Standard Time" },
                { "Asia/Tehran", "Iran Standard Time" },
                { "Asia/Thimphu", "Central Asia Standard Time" },
                { "Asia/Tokyo", "Tokyo Standard Time" },
                { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
                { "Asia/Vientiane", "SE Asia Standard Time" },
                { "Asia/Vladivostok", "Vladivostok Standard Time" },
                { "Asia/Yakutsk", "Yakutsk Standard Time" },
                { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
                { "Asia/Yerevan", "Caucasus Standard Time" },
                { "Atlantic/Azores", "Azores Standard Time" },
                { "Atlantic/Bermuda", "Atlantic Standard Time" },
                { "Atlantic/Canary", "GMT Standard Time" },
                { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
                { "Atlantic/Faroe", "GMT Standard Time" },
                { "Atlantic/Madeira", "GMT Standard Time" },
                { "Atlantic/Reykjavik", "Greenwich Standard Time" },
                { "Atlantic/South_Georgia", "Mid-Atlantic Standard Time" },
                { "Atlantic/St_Helena", "Greenwich Standard Time" },
                { "Atlantic/Stanley", "Argentina Standard Time" },
                { "Australia/Adelaide", "Cen. Australia Standard Time" },
                { "Australia/Brisbane", "E. Australia Standard Time" },
                { "Australia/Broken_Hill", "AUS Central Standard Time" },
                { "Australia/Darwin", "AUS Central Standard Time" },
                { "Australia/Eucla", "Aus Central W. Standard Time" },
                { "Australia/Hobart", "Tasmania Standard Time" },
                { "Australia/Lindeman", "AUS Eastern Standard Time" },
                { "Australia/Lord_Howe", "Lord Howe Standard Time" },
                { "Australia/Melbourne", "AUS Eastern Standard Time" },
                { "Australia/Perth", "W. Australia Standard Time" },
                { "Australia/Sydney", "AUS Eastern Standard Time" },
                { "Australia/Queensland", "AUS Eastern Standard Time" },
                { "Etc/GMT", "UTC" },
                { "Etc/GMT+11", "UTC-11" },
                { "Etc/GMT+12", "Dateline Standard Time" },
                { "Etc/GMT+2", "UTC-02" },
                { "Etc/GMT-12", "UTC+12" },
                { "Europe/Amsterdam", "W. Europe Standard Time" },
                { "Europe/Andorra", "W. Europe Standard Time" },
                { "Europe/Athens", "GTB Standard Time" },
                { "Europe/Belgrade", "Central Europe Standard Time" },
                { "Europe/Berlin", "W. Europe Standard Time" },
                { "Europe/Bratislava", "Central Europe Standard Time" },
                { "Europe/Brussels", "Romance Standard Time" },
                { "Europe/Bucharest", "GTB Standard Time" },
                { "Europe/Budapest", "Central Europe Standard Time" },
                { "Europe/Chisinau", "E. Europe Standard Time" },
                { "Europe/Copenhagen", "Romance Standard Time" },
                { "Europe/Dublin", "GMT Standard Time" },
                { "Europe/Gibraltar", "W. Europe Standard Time" },
                { "Europe/Guernsey", "W. Europe Standard Time" },
                { "Europe/Helsinki", "FLE Standard Time" },
                { "Europe/Isle_of_Man", "GMT Standard Time" },
                { "Europe/Istanbul", "GTB Standard Time" },
                { "Europe/Jersey", "GMT Standard Time" },
                { "Europe/Kaliningrad", "Kaliningrad Standard Time" },
                { "Europe/Kiev", "FLE Standard Time" },
                { "Europe/Lisbon", "GMT Standard Time" },
                { "Europe/Ljubljana", "Central Europe Standard Time" },
                { "Europe/London", "GMT Standard Time" },
                { "Europe/Luxembourg", "W. Europe Standard Time" },
                { "Europe/Madrid", "E. Europe Standard Time" },
                { "Europe/Malta", "W. Europe Standard Time" },
                { "Europe/Mariehamn", "E. Europe Standard Time" },
                { "Europe/Minsk", "Kaliningrad Standard Time" },
                { "Europe/Monaco", "W. Europe Standard Time" },
                { "Europe/Moscow", "Russian Standard Time" },
                { "Europe/Oslo", "W. Europe Standard Time" },
                { "Europe/Paris", "Romance Standard Time" },
                { "Europe/Podgorica", "W. Europe Standard Time" },
                { "Europe/Prague", "W. Europe Standard Time" },
                { "Europe/Riga", "FLE Standard Time" },
                { "Europe/Rome", "W. Europe Standard Time" },
                { "Europe/Samara", "Russian Standard Time" },
                { "Europe/San_Marino", "W. Europe Standard Time" },
                { "Europe/Sarajevo", "Central Europe Standard Time" },
                { "Europe/Simferopol", "E. Europe Standard Time" },
                { "Europe/Skopje", "Central Europe Standard Time" },
                { "Europe/Sofia", "FLE Standard Time" },
                { "Europe/Stockholm", "W. Europe Standard Time" },
                { "Europe/Tallinn", "FLE Standard Time" },
                { "Europe/Tirane", "W. Europe Standard Time" },
                { "Europe/Uzhgorod", "E. Europe Standard Time" },
                { "Europe/Vaduz", "W. Europe Standard Time" },
                { "Europe/Vatican", "W. Europe Standard Time" },
                { "Europe/Vienna", "W. Europe Standard Time" },
                { "Europe/Vilnius", "FLE Standard Time" },
                { "Europe/Volgograd", "Russian Standard Time" },
                { "Europe/Warsaw", "Central Europe Standard Time" },
                { "Europe/Zagreb", "Central Europe Standard Time" },
                { "Europe/Zaporozhye", "E. Europe Standard Time" },
                { "Europe/Zurich", "W. Europe Standard Time" },
                { "GMT", "GMT Standard Time" },
                { "Indian/Antananarivo", "Arab Standard Time" },
                { "Indian/Chagos", "Central Asia Standard Time" },
                { "Indian/Christmas", "SE Asia Standard Time" },
                { "Indian/Cocos", "Myanmar Standard Time" },
                { "Indian/Comoro", "Arab Standard Time" },
                { "Indian/Kerguelen", "West Asia Standard Time" },
                { "Indian/Mahe", "Mauritius Standard Time" },
                { "Indian/Maldives", "West Asia Standard Time" },
                { "Indian/Mauritius", "Mauritius Standard Time" },
                { "Indian/Mayotte", "Arab Standard Time" },
                { "Indian/Reunion", "Mauritius Standard Time" },
                { "Pacific/Apia", "Samoa Standard Time" },
                { "Pacific/Auckland", "New Zealand Standard Time" },
                { "Pacific/Easter", "Central America Standard Time" },
                { "Pacific/Fakaofo", "Hawaiian Standard Time" },
                { "Pacific/Fiji", "Fiji Standard Time" },
                { "Pacific/Galapagos", "Central America Standard Time" },
                { "Pacific/Gambier", "Alaskan Standard Time" },
                { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
                { "Pacific/Guam", "West Pacific Standard Time" },
                { "Pacific/Honolulu", "Hawaiian Standard Time" },
                { "Pacific/Johnston", "Hawaiian Standard Time" },
                { "Pacific/Marquesas", "Hawaiian Standard Time" },
                { "Pacific/Midway", "Hawaiian Standard Time" },
                { "Pacific/Niue", "Samoa Standard Time" },
                { "Pacific/Norfolk", "Norfolk Standard Time" },
                { "Pacific/Pago_Pago", "Samoa Standard Time" },
                { "Pacific/Pitcairn", "Pacific Standard Time" },
                { "Pacific/Port_Moresby", "West Pacific Standard Time" },
                { "Pacific/Rarotonga", "Hawaiian Standard Time" },
                { "Pacific/Tahiti", "Hawaiian Standard Time" },
                { "Pacific/Tongatapu", "Tonga Standard Time" },
                { "UTC", "UTC" }
            };

            var windowsTimeZoneId = default(string);
            var windowsTimeZone = default(TimeZoneInfo);
            if (olsonWindowsTimes.TryGetValue(olsonTimeZoneId, out windowsTimeZoneId))
            {
                try { windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId); }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }
            return windowsTimeZone;
        }

        public static string GetCurrentTime(GetCurrentTimeRequest request)
        {
            string timeZone = request.TimeZone;
            string format = request.Format;

            DateTime dateTimeNow = DateTime.UtcNow;

            string defaultFormat = "yyyy-MM-dd HH:mm:ss";

            if (!string.IsNullOrEmpty(timeZone))
            {
                var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                dateTimeNow = TimeZoneInfo.ConvertTimeFromUtc(dateTimeNow, zone);
            }

            string dateTimeStr;
            if (!string.IsNullOrEmpty(format))
            {
                dateTimeStr = dateTimeNow.ToString(format);
            }
            else
            {
                dateTimeStr = dateTimeNow.ToString(defaultFormat);
            }

            return dateTimeStr;

        }

        public static long ZohoTimeToUnixEpoch(ConvertZohoTime2UnixEpochRequest request)
        {

            string zohoTime = request.ZohoTime;

            if (string.IsNullOrWhiteSpace(zohoTime))
                throw new ArgumentException("ZohoTime cannot be null or empty.", nameof(zohoTime));

            // Try ISO 8601 format first
            if (DateTimeOffset.TryParse(zohoTime, null, DateTimeStyles.RoundtripKind, out var dto))
                return dto.ToUnixTimeSeconds();

            // Try "dd-MMM-yyyy HH:mm:ss" (like 17-Oct-2025 14:13:30)
            string[] formats =
            {
            "dd-MMM-yyyy HH:mm:ss",
            "dd-MMM-yyyy H:mm:ss",
            "dd-MMM-yyyy hh:mm:ss tt", // just in case AM/PM
        };

            if (DateTime.TryParseExact(
                    zohoTime,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal,
                    out var dt))
            {
                // Assume local time if no offset given
                var localOffset = new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));
                return localOffset.ToUnixTimeSeconds();
            }

            throw new FormatException($"Invalid Zoho time format: {zohoTime}");
        
        }

        public static string AdjustZohoTime(AdjustZohoTimeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.ZohoTime))
                throw new ArgumentException("ZohoTime cannot be null or empty.", nameof(request.ZohoTime));
            if (string.IsNullOrWhiteSpace(request.TimeZoneId))
                throw new ArgumentException("TimeZoneId cannot be null or empty.", nameof(request.TimeZoneId));

            TimeZoneInfo tz;
            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
            }
            catch
            {
                throw new ArgumentException($"Invalid TimeZoneId: {request.TimeZoneId}");
            }

            DateTimeOffset dto;

            // Try ISO 8601 first
            if (DateTimeOffset.TryParse(request.ZohoTime, null, DateTimeStyles.RoundtripKind, out var parsedIso))
            {
                if (parsedIso.Offset == TimeSpan.Zero && !request.ZohoTime.EndsWith("Z"))
                {
                    // ISO without offset → treat as local in target timezone
                    TimeSpan offset = tz.GetUtcOffset(parsedIso.DateTime);
                    dto = new DateTimeOffset(parsedIso.DateTime, offset);
                }
                else
                {
                    // ISO with offset → convert to target timezone
                    dto = TimeZoneInfo.ConvertTime(parsedIso, tz);
                }
            }
            else
            {
                // Custom format dd-MMM-yyyy HH:mm:ss
                string[] formats = { "dd-MMM-yyyy HH:mm:ss", "dd-MMM-yyyy H:mm:ss", "dd-MMM-yyyy hh:mm:ss tt" };
                if (!DateTime.TryParseExact(request.ZohoTime, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedNaive))
                    throw new FormatException($"Invalid Zoho time format: {request.ZohoTime}");

                // Treat naive time as already in target timezone
                TimeSpan offset = tz.GetUtcOffset(parsedNaive);
                dto = new DateTimeOffset(parsedNaive, offset);
            }

            // Apply OffsetValue / OffsetType
            switch (request.OffsetType?.Trim().ToLowerInvariant())
            {
                case "hours": dto = dto.AddHours(request.OffsetValue); break;
                case "days": dto = dto.AddDays(request.OffsetValue); break;
                case "months": dto = dto.AddMonths(request.OffsetValue); break;
                case "years": dto = dto.AddYears(request.OffsetValue); break;
            }

            return dto.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
        }

        public static string GetMiddleDate(GetMiddleDateRequest request)
        {

            string startDateStr = request.StartDate;
            string endDateStr = request.EndDate;

            var startDate = DateTime.ParseExact(startDateStr,
                "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(endDateStr,
                "yyyy-MM-dd", CultureInfo.InvariantCulture);

            double dateDiff = (endDate - startDate).TotalDays;
            int halfDateDiff = (int)dateDiff / 2;

            var middleDate = startDate.AddDays(halfDateDiff);

            string format = request.Format;
            if (string.IsNullOrEmpty(format))
            {
                format = "yyyy-MM-dd";
            }

            return middleDate.ToString(format);

        }

        public static string ToUniversalIso8601(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");
        }

        public static string GetUtcOffset(string timeZoneId)
        {

            var offsetBuilder = new StringBuilder();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            
            // Get the Current Time
            var currentTime = DateTime.UtcNow;

            // Calculate the UTC offset for EST
            var offset = timeZone.GetUtcOffset(currentTime);

            // Display the offset in hours
            int offsetHours = offset.Hours;
            int tempOffsetHours = offsetHours;
            int offsetMinutes = offset.Minutes;
            int tempOffsetMinutes = offsetMinutes;

            if (tempOffsetHours < 0)
            {
                offsetBuilder.Append("-");
                tempOffsetHours = tempOffsetHours * -1;
            }
            else
            {
                offsetBuilder.Append("+");
            }
            if (tempOffsetHours < 10)
            {
                offsetBuilder.Append("0");
            }
            offsetBuilder.Append(tempOffsetHours);
            offsetBuilder.Append(":");
            if (tempOffsetMinutes < 0)
            {
                tempOffsetMinutes = tempOffsetMinutes * -1;
            }
            if (tempOffsetMinutes < 10)
            {
                offsetBuilder.Append("0");
            }
            offsetBuilder.Append(tempOffsetMinutes);

            return offsetBuilder.ToString();
        }

        public static string ConvertZohoTimeToText(ConvertZohoTime2Text request)
        {
            string zohoTime = request.ZohoTime;
            string outputFormat = request.OutputFormat;

            if (string.IsNullOrWhiteSpace(zohoTime))
                throw new ArgumentException("zohoTime cannot be empty");

            // Default AU business format
            if (string.IsNullOrWhiteSpace(outputFormat))
                outputFormat = "dd/MM/yyyy HH:mm";

            // Parse Zoho's ISO datetime (with offset)
            if (!DateTimeOffset.TryParse(zohoTime, out var dto))
                throw new FormatException("Invalid Zoho time format");

            // Convert to the actual local clock time (drop offset)
            DateTime local = dto.DateTime;

            // Return formatted AU time
            return local.ToString(outputFormat, CultureInfo.InvariantCulture);
        }

        public static GetFirstAndLastDayOfMonthResponse 
            GetFirstAndLastDayOfMonth(DateTime date)
        {

            var getFirstAndLastDayResponse = new GetFirstAndLastDayOfMonthResponse();

            DateTime firstDay = new DateTime(date.Year, date.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

            getFirstAndLastDayResponse.FirstDayOfMonth = firstDay.ToString("yyyy-MM-dd");
            getFirstAndLastDayResponse.LastDayOfMonth = lastDay.ToString("yyyy-MM-dd");

            return getFirstAndLastDayResponse;
        }

    }
}
