using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.BL
{
    public class GlobalApplicationVariables
    {
        private ERPEntities db = new ERPEntities();


        public string GetCommonDate() {

            var commonDate = db.Common_HostSettings.Where(x => x.HostKey.Equals("DateFormat")).Select(x => x.HostValue).FirstOrDefault();
            return commonDate;

        }

        public string GetCommonTime()
        {

            var commonTime = db.Common_HostSettings.Where(x => x.HostKey.Equals("TimeFormat")).Select(x => x.HostValue).FirstOrDefault();
            return commonTime;

        }

        public string GetCommonTimeZone()
        {
            var CommonTimeZone = db.Common_HostSettings.Where(x => x.HostKey.Equals("TimeZone")).Select(x => x.HostValue).FirstOrDefault();
            return CommonTimeZone;
        }

        public string GetCurrency()
        {
            var Currency = db.Common_HostSettings.Where(x => x.HostKey.Equals("Currency")).Select(x => x.HostValue).FirstOrDefault();
            return Currency;
        }
    }
}
