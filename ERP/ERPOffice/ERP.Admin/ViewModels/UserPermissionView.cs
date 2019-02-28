using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERP.Admin.ViewModels
{
     public class UserPermissionView
    {
        
        public IEnumerable<UserViewModel> userViewModel { get; set; }
        public  List<UserRoleViewModel> userRoleViewModelList { get; set; }
        public UserRoleViewModel userRoleViewModel { get; set; }

    }
}
