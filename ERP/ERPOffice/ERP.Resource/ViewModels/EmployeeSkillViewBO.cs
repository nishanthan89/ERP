using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ERP.Utility.Models;
using ERP.Resource.Models;

namespace ERP.Resource.ViewModels
{
    public class EmployeeSkillViewBO
    {
        public int EmployeeID { get; set; }
        public ResourceSubMenuBO ResourceSubMenuBO { get; set; }    //Models for Employee tabs in edit mode
        public EmployeeSkillBO EmployeeSkillBO { get; set; }

        public IEnumerable<SkillBO> SkillList { get; set; }     //Gets Skill List

        public int SkillCategoryID { get; set; }

        [Display(Name = "Skill Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Skill")]
        public int SkillID { get; set; }

        [Display(Name = "Skill Level")]
        public int SkillLevel { get; set; }

        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Display(Name = "Project StartDate")]
        public DateTime? ProjectStartDate { get; set; }

        public bool SkillCategoy { get; set; }  //Sets the Skill Category's data type as bool for checkbox

        public List<SkillCategory> SkillCategoryList { get; set; }      //Gets list of Skill Category
    }

    /// <summary>
    /// Class for Skill Category check box
    /// </summary>
    public class SkillCategory
    {
        public int EmpID { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
        public bool IsSelectedCategory { get; set; }
        public int SkillID { get; set; }
        public int SkillGroupID { get; set; }
        public string skillName { get; set; }
        public int SkillLevel { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ProjectStartDate { get; set; }
    }
}
