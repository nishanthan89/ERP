using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.Resource.Models
{
    public class EmployeeExpereienceBO
    {
        public int ExperienceID { get; set; }
        public int ResourceID { get; set; }
        public int DesignationID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public string ExternalDesignation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
