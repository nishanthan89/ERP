using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class EmployeeSkillBO
    {
        public int ResourceSkillID { get; set; }
        public int ResourceID { get; set; }
        public int SkillCategoryID { get; set; }
        public int SkillID { get; set; }
        public int SkillLevel { get; set; }
        public int CompanyID { get; set; }
        public DateTime? ProjectStartDate { get; set; }
    }
}
