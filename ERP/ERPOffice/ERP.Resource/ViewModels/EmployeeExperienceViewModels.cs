//--------------------------------------------------------------------------------
// <copyright file="EmployeeExperienceViewModels.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Experience
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.Resource.ViewModels
{
    public class EmployeeExperienceViewModels : IValidatableObject
    {
        public int ExperienceID { get; set; }
        public int ResourceID { get; set; }

        [Required, Display(Name = "Designation")]
        public int DesignationID { get; set; }

        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }     //For display the DesignationName for a DesignationID

        [Required, Display(Name = "Company")]
        //[MaxLength(70, ErrorMessage = "Company Name Cannot Be Longer Than 70 Characters")]
        public int CompanyID { get; set; }

        public string Company { get; set; }    //Displays the Company Name

        [MaxLength(70, ErrorMessage = "Description Cannot Be Longer Than 70 Characters")]
        public string Description { get; set; }

        [Display(Name = "External Designation")]
        [MaxLength(70, ErrorMessage = "External Designation Cannot Be Longer Than 70 Characters")]
        public string ExternalDesignation { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Duration { get; set; }        //For display the Experience duration as a format
        public string Start { get; set; }           //For display
        public string End { get; set; }             //For display

        public int Month { get; set; }              //For display
        public int Year { get; set; }               //For display

        [Required]
        public int? StartYear { get; set; }
        [Required]
        public int? StartMonth { get; set; }
        [Required]
        public int? EndYear { get; set; }
        [Required]
        public int? EndMonth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  //"Validate" method which is inherits from "IValidatableObject" class
        {
            if (StartYear > EndYear)   //Model state validation for EndYear, It should be greater than the StartYear
            {
                yield return new ValidationResult("'End Year' Must Be Greater Than The 'Start Year'");    //Returns the error message
            }
            else if (StartYear == EndYear)  //When the StartYear is equal to EndMonth
            {
                if (StartMonth >= EndMonth) //Model state validation for EndMonth, It should be greater than the StartMonth
                {
                    yield return new ValidationResult("'End Month' Must Be Greater Than The 'Start Month'");    //Returns the error message

                }
            }
        }
    }
}
