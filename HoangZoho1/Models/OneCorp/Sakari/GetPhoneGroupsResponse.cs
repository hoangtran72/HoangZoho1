using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class GetPhoneGroupsResponse
    {

        public bool? success { get; set; }

        public SakariPhoneGroup[] data { get; set; }

        public SakariPagination pagination { get; set; }

    }

    public class SakariPagination
    {

        public int? totalCount { get; set; }

        public int? limit { get; set; }

        public int? offset { get; set; }

    }

    public class SakariPhoneGroup
    {

        public string id { get; set; }

        public string name { get; set; }

        public string[] tags { get; set; }

        public bool? isDefault { get; set; }

        public bool? useSharedPool { get; set; }

        public User[] users { get; set; }

        public GroupPhoneNumber[] phoneNumbers { get; set; }

        public Sender[] senders { get; set; }

        public object[] notifications { get; set; }

        public SakariCreated created { get; set; }

        public SakariUpdated updated { get; set; }

    }

    public class SakariCreated
    {

        public DateTime? at { get; set; }
        
        public By by { get; set; }

    }

    public class SakariBy
    {

        public string id { get; set; }

        public string name { get; set; }

    }

    public class SakariUpdated
    {

        public DateTime? at { get; set; }

        public SakariBy by { get; set; }

    }


    public class User
    {

        public string id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }
        
        public string email { get; set; }
        
        public Mobile mobile { get; set; }
        
        public DateTime? tacAccepted { get; set; }
    
    }

    public class Mobile
    {
        
        public string number { get; set; }
        
        public string country { get; set; }
        
        public object verified { get; set; }
    
    }

    public class GroupPhoneNumber
    {

        public string id { get; set; }

        public string status { get; set; }

        public string number { get; set; }

        public string country { get; set; }

        public string[] supportedDestinations { get; set; }

        public string type { get; set; }

        public bool? verified { get; set; }

        public string provider { get; set; }

        public Group[] groups { get; set; }

        public Channels channels { get; set; }

    }

    public class Channels
    {

        public Sms sms { get; set; }

        public Voice voice { get; set; }

        public Whatsapp whatsapp { get; set; }

    }

    public class Sms
    {

        public bool? active { get; set; }

        public SupportedDestinations supportedDestinations { get; set; }

    }

    public class SupportedDestinations
    {

        public bool? AF { get; set; }

        public bool? AL { get; set; }

        public bool? DZ { get; set; }
        public bool? AS { get; set; }
        public bool? AD { get; set; }
        public bool AO { get; set; }
        public bool AI { get; set; }
        public bool AG { get; set; }
        public bool AR { get; set; }
        public bool AM { get; set; }
        public bool AW { get; set; }
        public bool AU { get; set; }
        public bool AT { get; set; }
        public bool AZ { get; set; }
        public bool BS { get; set; }
        public bool BH { get; set; }
        public bool BD { get; set; }
        public bool BB { get; set; }
        public bool BY { get; set; }
        public bool BE { get; set; }
        public bool BZ { get; set; }
        public bool BJ { get; set; }
        public bool BM { get; set; }
        public bool BT { get; set; }
        public bool BO { get; set; }
        public bool BA { get; set; }
        public bool BW { get; set; }
        public bool BR { get; set; }
        public bool VG { get; set; }
        public bool BN { get; set; }
        public bool BG { get; set; }
        public bool BF { get; set; }
        public bool BI { get; set; }
        public bool KH { get; set; }
        public bool CM { get; set; }
        public bool CA { get; set; }
        public bool CV { get; set; }
        public bool KY { get; set; }
        public bool CF { get; set; }
        public bool TD { get; set; }
        public bool CL { get; set; }
        public bool CN { get; set; }
        public bool CO { get; set; }
        public bool KM { get; set; }
        public bool CG { get; set; }
        public bool CD { get; set; }
        public bool CK { get; set; }
        public bool CR { get; set; }
        public bool HR { get; set; }
        public bool CU { get; set; }
        public bool CY { get; set; }
        public bool CZ { get; set; }
        public bool CI { get; set; }
        public bool DK { get; set; }
        public bool DJ { get; set; }
        public bool DM { get; set; }
        public bool DO { get; set; }
        public bool EC { get; set; }
        public bool EG { get; set; }
        public bool SV { get; set; }
        public bool GQ { get; set; }
        public bool ER { get; set; }
        public bool EE { get; set; }
        public bool ET { get; set; }
        public bool FK { get; set; }
        public bool FO { get; set; }
        public bool FJ { get; set; }
        public bool FI { get; set; }
        public bool FR { get; set; }
        public bool GF { get; set; }
        public bool PF { get; set; }
        public bool GA { get; set; }
        public bool GM { get; set; }
        public bool GE { get; set; }
        public bool DE { get; set; }
        public bool GH { get; set; }
        public bool GI { get; set; }
        public bool GR { get; set; }
        public bool GL { get; set; }
        public bool GD { get; set; }
        public bool GP { get; set; }
        public bool GU { get; set; }
        public bool GT { get; set; }
        public bool GG { get; set; }
        public bool GN { get; set; }
        public bool GW { get; set; }
        public bool GY { get; set; }
        public bool HT { get; set; }
        public bool HN { get; set; }
        public bool HK { get; set; }
        public bool HU { get; set; }
        public bool IS { get; set; }
        public bool IN { get; set; }
        public bool ID { get; set; }
        public bool IR { get; set; }
        public bool IQ { get; set; }
        public bool IE { get; set; }
        public bool IM { get; set; }
        public bool IL { get; set; }
        public bool IT { get; set; }
        public bool JM { get; set; }
        public bool JP { get; set; }
        public bool JE { get; set; }
        public bool JO { get; set; }
        public bool KZ { get; set; }
        public bool KE { get; set; }
        public bool KI { get; set; }
        public bool KW { get; set; }
        public bool KG { get; set; }
        public bool LA { get; set; }
        public bool LV { get; set; }
        public bool LB { get; set; }
        public bool LS { get; set; }
        public bool LR { get; set; }
        public bool LY { get; set; }
        public bool LI { get; set; }
        public bool LT { get; set; }
        public bool LU { get; set; }
        public bool MO { get; set; }
        public bool MK { get; set; }
        public bool MG { get; set; }
        public bool MW { get; set; }
        public bool MY { get; set; }
        public bool MV { get; set; }
        public bool ML { get; set; }
        public bool MT { get; set; }
        public bool MQ { get; set; }
        public bool MR { get; set; }
        public bool MU { get; set; }
        public bool MX { get; set; }
        public bool FM { get; set; }
        public bool MD { get; set; }
        public bool MC { get; set; }
        public bool MN { get; set; }
        public bool ME { get; set; }
        public bool MS { get; set; }
        public bool MA { get; set; }
        public bool MZ { get; set; }
        public bool MM { get; set; }
        public bool NA { get; set; }
        public bool NP { get; set; }
        public bool NL { get; set; }
        public bool NC { get; set; }
        public bool NZ { get; set; }
        public bool NI { get; set; }
        public bool NE { get; set; }
        public bool NG { get; set; }
        public bool NU { get; set; }
        public bool NF { get; set; }
        public bool NO { get; set; }
        public bool OM { get; set; }
        public bool PK { get; set; }
        public bool PW { get; set; }
        public bool PA { get; set; }
        public bool PG { get; set; }
        public bool PY { get; set; }
        public bool PE { get; set; }
        public bool PH { get; set; }
        public bool PL { get; set; }
        public bool PT { get; set; }
        public bool PR { get; set; }
        public bool QA { get; set; }
        public bool RO { get; set; }
        public bool RU { get; set; }
        public bool RW { get; set; }
        public bool RE { get; set; }
        public bool WS { get; set; }
        public bool SM { get; set; }
        public bool SA { get; set; }
        public bool SN { get; set; }
        public bool RS { get; set; }
        public bool SC { get; set; }
        public bool SL { get; set; }
        public bool SG { get; set; }
        public bool SK { get; set; }
        public bool SI { get; set; }
        public bool SB { get; set; }
        public bool SO { get; set; }
        public bool ZA { get; set; }
        public bool KR { get; set; }
        public bool SS { get; set; }
        public bool ES { get; set; }
        public bool LK { get; set; }
        public bool KN { get; set; }
        public bool LC { get; set; }
        public bool PM { get; set; }
        public bool VC { get; set; }
        public bool SD { get; set; }
        public bool SR { get; set; }
        public bool SZ { get; set; }
        public bool SE { get; set; }
        public bool CH { get; set; }
        public bool SY { get; set; }
        public bool ST { get; set; }
        public bool TW { get; set; }
        public bool TJ { get; set; }
        public bool TZ { get; set; }
        public bool TH { get; set; }
        public bool TL { get; set; }
        public bool TG { get; set; }
        public bool TO { get; set; }
        public bool TT { get; set; }
        public bool TN { get; set; }
        public bool TR { get; set; }
        public bool TM { get; set; }
        public bool TC { get; set; }
        public bool VI { get; set; }
        public bool UG { get; set; }
        public bool UA { get; set; }
        public bool AE { get; set; }
        public bool GB { get; set; }
        public bool US { get; set; }
        public bool UY { get; set; }
        public bool UZ { get; set; }
        public bool VU { get; set; }
        public bool VE { get; set; }
        public bool VN { get; set; }
        public bool YE { get; set; }
        public bool ZM { get; set; }
        public bool ZW { get; set; }
    }

    public class Voice
    {

        public bool? active { get; set; }

    }

    public class Whatsapp
    {

        public bool? active { get; set; }

    }

    public class Group
    {

        public string id { get; set; }

        public string name { get; set; }

    }

    public class Sender
    {

        public string id { get; set; }

        public string type { get; set; }

        public string subType { get; set; }

        public string provider { get; set; }

        public string identifier { get; set; }

        public Channels channels { get; set; }

        public Group[] groups { get; set; }

        public string status { get; set; }

        public SakariCreated created { get; set; }

        public SakariUpdated updated { get; set; }

    }

}
