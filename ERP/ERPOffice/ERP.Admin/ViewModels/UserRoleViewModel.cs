using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.ViewModels
{
   public class UserRoleViewModel
    {
        public string RoleID { get; set; }
        [Required(ErrorMessage = "Role Name is Required"),Display(Name ="Role Name")]
        public string RoleName { get; set; }
        public List<UserTagListModel> UserList { get; set; }
        public List<PermissionListView> PermissionList { get; set; }
        [Display(Name = "Users")]
        public List<string> SelectUsers { get; set; }
        [Display(Name = "Permissions")]
        public List<int> SelectPermissions { get; set; }
    }
}
