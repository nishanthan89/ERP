using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.ViewModels
{
    public class LogoutViewModel
    {
        public long LoginLogoutID { get; set; }
        public string UserID { get; set; }
        public DateTime Login { get; set; }
        public int? DeviceID { get; set; }
    }
}
