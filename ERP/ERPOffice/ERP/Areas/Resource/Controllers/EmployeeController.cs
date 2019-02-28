//--------------------------------------------------------------------------------
// <copyright file="EmployeeController.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      Provides Create, Edit and Delete functionalities to Employee
//      It allows to add "Payment" , "Working Pattern" , "Experience" and "Skills" for Employee
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Resource.BL;
using ERP.Address.BL;
using ERP.Address.ViewModels;
using ERP.Resource.Models;
using ERP.Utility.Models;
using ERP.Resource.ViewModels;
using System.IO;
using ERP.DA;
using System.Web.Helpers;
using ERP.MvcSecurity;

namespace ERP.Areas.Resource.Controllers
{
    [Authorize]
    [AuthorizeUserAttribute(Permission = 7)]    //Provide Permission to access the Employee functions
    public class EmployeeController : Controller
    {
        private EmployeeBL bl = new EmployeeBL();   //Gets all methods from EmployeeBL for Employee related functions
        private AddressBL addressBl = new AddressBL();  //Gets all methods from AddressBL

        #region Employee

        /// <summary>
        /// Sets ViewBags values
        /// </summary>
        private void SetViewBag()
        {
            //Employee related dropdowns       
            ViewBag.Branch = (bl.GetBranch().Select(x => new SelectListItem()
            {
                Value = x.BranchID.ToString(),
                Text = x.BranchName
            }).OrderBy(branch => branch.Text));    //Gets Branch List in the alphabet order

            ViewBag.Title = (bl.GetTitle().Select(x => new SelectListItem()
            {
                Value = x.TitleID.ToString(),
                Text = x.TitleName
            }).OrderBy(title => title.Text));     //Gets Title List in the alphabet order

            ViewBag.MaritalStatus = (bl.GetMaritalStatusList().Select(x => new SelectListItem()
            {
                Value = x.MaritalStatusID.ToString(),
                Text = x.MaritalStatusName
            }).OrderBy(mStatus => mStatus.Text));     //Gets Marital Status List in the alphabet order

            ViewBag.NationalityStatus = (bl.GetNationalityList().Select(x => new SelectListItem()
            {
                Value = x.NationalityID.ToString(),
                Text = x.NationalityName
            }).OrderBy(nStatus => nStatus.Text));     //Gets Nationality Status List in the alphabet order

            ViewBag.EmployeeType = (bl.GetEmployeeTypeList().Select(x => new SelectListItem()
            {
                Value = x.EmployeeTypeID.ToString(),
                Text = x.EmployeeTypeName
            }).OrderBy(empType => empType.Text));     //Gets Employee Type List in the alphabet order

            ViewBag.EthnicGroup = (bl.GetEthnicGroupList().Select(x => new SelectListItem()
            {
                Value = x.EthnicGroupID.ToString(),
                Text = x.EthnicGroupName
            }).OrderBy(eGroup => eGroup.Text));     //Gets Ethnic Group List in the alphabet order

            ViewBag.ImmigrationStatus = (bl.GetImmigrationStatusList().Select(x => new SelectListItem()
            {
                Value = x.ImmigrationStatusID.ToString(),
                Text = x.ImmigrationName
            }).OrderBy(immiStatus => immiStatus.Text));     //Gets Immigration Status List in the alphabet order

            ViewBag.TimeSheetFrequency = (bl.GetTimeSheetFrequencyList().Select(x => new SelectListItem()
            {
                Value = x.TimeSheetFrequencyID.ToString(),
                Text = x.TimeSheetFrequencyName
            }).OrderBy(timeSheet => timeSheet.Text));     //Gets Timesheet Frequency List in the alphabet order

            //Country related dropdowns
            ViewBag.CountryList = addressBl.GetCountryList().Select(x => new SelectListItem()
            {
                Value = x.CountryID.ToString(),
                Text = x.CountryName,
                Selected = x.IsSelected
            });      //Gets Country List

            //Payment related dropdowns
            ViewBag.PaymentType = (bl.GetPaymentType().Select(x => new SelectListItem()
            {
                Value = x.PaymentTypeID.ToString(),
                Text = x.PaymentName
            }).OrderBy(payType => payType.Text));     //Gets PaymentType List in the alphabet order

            ViewBag.ListOfBranch = new MultiSelectList(bl.GetBranch(), "BranchID", "BranchName");   //Gets Branch List for multi Branch selection
        }

        /// <summary>
        /// Displays an Index page 
        /// </summary>
        /// <param name="searchBO"></param>
        /// <returns></returns>
        public ActionResult Index(EmployeeSearchBO searchBO)
        {
            SetViewBag();   //ViewBag sets only for Branch's dropdown
            searchBO.EmployeeList = bl.GetEmployeeList(searchBO);   //Gets List of all Employees
            return View(searchBO);
        }

        /// <summary>
        /// Search Employees according to the Search Criteria
        /// </summary>
        /// <param name="searchBO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchEmployee(EmployeeSearchBO searchBO)
        {
            var searchList = bl.GetEmployeeList(searchBO);  //Gets EmployeeList according to the search
            return PartialView("_EmployeeList", searchList);    //Partially returns the possible EmployeeList
        }

        /// <summary>
        /// GET: /Employee/CreateEmployee
        /// Gets EmployeeCreation Form 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            SetViewBag();   //ViewBag values set for all dropdowns in the Employee creation
            var employee = new EmployeeViewBO();
            employee.WorkingPatternDetailsBO = new Employee_WorkingPatternDetailsBO();
            employee.AddressViewModel = new AddressViewModel();
            employee.AddressViewModel.CountryID = addressBl.GetDefaultCountry();    //Sets the default country for Country's dropdown
            return View("CreateEditEmployee", employee);
        }

        /// <summary>
        /// POST: /Employee/CreateEmployee
        /// Posts Employee Detail
        /// </summary>
        /// <param name="employeeBO"></param>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeViewBO employeeBO, HttpPostedFileBase UploadFile)
        {
            string error = "";
            SetViewBag();   // ViewBag values set for all dropdowns in the Employee creation
            if (ModelState.IsValid)
            {
                if (employeeBO.EmployeeID == 0)     //Creates a new Employee
                {
                    if (bl.SaveEmployee(employeeBO, out error, UploadFile) == true)     //Successfully saved
                    {
                        TempData["Success"] = error;
                        return RedirectToAction("Index");   //Returns to the Index page
                    }
                    else  //Failed to save Employee
                    {
                        TempData["Error"] = error;
                    }
                }
                else     //Updates a particular Employee
                {
                    if (bl.UpdateEmployee(employeeBO, out error, UploadFile) == true)       //Successfully updated
                    {
                        TempData["Success"] = error;
                        //return RedirectToAction("Index");   //Returns to the Index page
                    }
                    else  //Failed to Employee update 
                    {
                        TempData["Error"] = error;
                    }
                    employeeBO.ResourceSubMenuBO = new ResourceSubMenuBO();     //Sets the values for "ResourceSubMenuBO" models
                    employeeBO.ResourceSubMenuBO.FName = employeeBO.FirstName;
                    employeeBO.ResourceSubMenuBO.LName = employeeBO.LastName;
                    employeeBO.ResourceSubMenuBO.EmployeeID = employeeBO.EmployeeID;

                    employeeBO.EmployeePaymentList = bl.GetEmployeePaymentList(employeeBO);   //Gets the EmployeePayment list for a particular Employee
                    employeeBO.EmployeeWorkingPatternList = bl.GetEmployeeWorkingPatternList(employeeBO);   //Gets the EmployeeWorkingPattern list for a particular Employee
                    employeeBO.PhotoFileType = bl.GetPhotoFileTypeVal(employeeBO);
                    employeeBO.ProfilePhoto = bl.GetImageFromDataBase(employeeBO.EmployeeID, out error);
                    if (employeeBO.ProfilePhoto == null)    //When the profile is not uploaded
                    {
                        return View("CreateEditEmployee", employeeBO);
                    }
                    employeeBO.PhotoString = "data: image / png; base64," + Convert.ToBase64String(employeeBO.ProfilePhoto);    //Profile is uploaded then Binary Image converts to string for viewing purpose
                }
            }

            return View("CreateEditEmployee", employeeBO);
        }

        /// <summary>
        /// POST: /Employee/DeleteEmployee
        /// Deletes Employee Record
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            string error = "";
            if (bl.DeleteEmployee(id, out error) == true)    //Successfully deletes Employee record
            {
                return Json(new
                {
                    success = true,
                    errorMsg = error
                }, JsonRequestBehavior.AllowGet);
            }
            else      //Failed to delete Employee record
            {
                return Json(new
                {
                    success = false,
                    errorMsg = error
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET: /Employee/EditEmployee
        /// Gets Employee Details for edit
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            string error = "";
            ViewBag.CurrentTab = "Employee";    //Sets the "Resource Details" tab as active
            SetViewBag();    // ViewBag values set for all dropdowns in the Employee update
            EmployeeViewBO employeeBO = bl.EditEmployee(id, out error);     //Gets all fields values for a selected Employee
            if (employeeBO != null)     //When the selected Employee is exist
            {
                employeeBO.ResourceSubMenuBO = new ResourceSubMenuBO();     //Sets the values for "ResourceSubMenuBO" models
                employeeBO.ResourceSubMenuBO.FName = employeeBO.FirstName;
                employeeBO.ResourceSubMenuBO.LName = employeeBO.LastName;
                employeeBO.ResourceSubMenuBO.EmployeeID = employeeBO.EmployeeID;

                employeeBO.EmployeePaymentBO = new EmployeePaymentBO();     //Gets the Employee Payment models values
                employeeBO.EmployeePaymentList = bl.GetEmployeePaymentList(employeeBO);   //Gets the Payment List for a particular Employee

                employeeBO.WorkingPatternDetailsBO = new Employee_WorkingPatternDetailsBO();     //Gets the Employee WorkingPattern models values
                employeeBO.EmployeeWorkingPatternList = bl.GetEmployeeWorkingPatternList(employeeBO);   //Gets the WorkingPattern List for a particular Employee

                if (employeeBO.ProfilePhoto == null)    //When the profile is not uploaded
                {
                    return View("CreateEditEmployee", employeeBO);
                }
                employeeBO.PhotoString = "data: image / png; base64," + Convert.ToBase64String(employeeBO.ProfilePhoto);    //Profile is uploaded then Binary Image converts to string for viewing purpose
            }
            else   //When the selected Employee is not exist
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");    //Returns to the Index page
            }
            return View("CreateEditEmployee", employeeBO);
        }

        /// <summary>
        /// Removes Profile Image
        /// </summary>
        /// <param name="employeeBO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveProfile(int id)
        {
            string error = "";
            //SetViewBag();    // ViewBag values set for all dropdowns in the Employee update
            EmployeeViewBO employeeBO = bl.EditEmployee(id, out error);     //Gets all details for a particular Employee to edit
            //employeeBO.EmployeePaymentList = bl.GetEmployeePaymentList(employeeBO);   //Gets payment list for a particular Employee
            if (bl.RemoveProfile(id, out error) == true)    //Successfully removed Profile 
            {
                return Json(new
                {
                    success = true,
                    errorMsg = error
                }, JsonRequestBehavior.AllowGet);
            }
            else  //Failed to remove the Profile
            {
                return Json(new
                {
                    success = false,
                    errorMsg = error
                }, JsonRequestBehavior.AllowGet);
            }
            //return View("CreateEditEmployee", employeeBO);
        }

        /// <summary>
        /// Remote Validation for Existing NIC Number
        /// </summary>
        /// <param name="NINo:NICNumber"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsNICExist(string NINo, string EmployeeID)
        {
            bool isExist = bl.CheckNICNo(NINo, EmployeeID); //Checks the NIC Number is exist only for one time
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Remote Validation For BirthYear
        /// </summary>
        /// <param name="DoB:BirthDate"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult BirthDateCheck(DateTime DoB)
        {
            bool isExist = bl.CheckBirthYear(DoB);  //Checks whether the "Birth Year" is less than 15 years from this year
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets a collection of EmployeeList to show for each scroll
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetData(int pageIndex, int pageSize)
        {
            //System.Threading.Thread.Sleep(5000);
            //pageSize = 20;
            //pageIndex = 0;
            var empList = bl.GetEmployeeLists()
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize);   //Takes a part of the EmployeeList for one scroll
            //pageIndex++;
            return Json(empList.ToList(), JsonRequestBehavior.AllowGet);
        }


        #endregion Employee

        #region EmployeePayment

        /// <summary>
        /// GET: /Employee/CreatePayment
        /// Gets CreatePayment form for a Employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreatePayment()
        {
            var empView = new EmployeeViewBO();
            empView.EmployeePaymentBO = new EmployeePaymentBO();
            empView.EmployeePaymentList = bl.GetEmployeePaymentList(empView);     //Gets List of Payment for a particular Employee
            SetViewBag();   //ViewBag values set for all dropdowns in the Employee creation or update
            return PartialView("_CreateEditPayment", empView.EmployeePaymentBO);    //Partially returns "_CreateEditPayment" 
        }

        /// <summary>
        /// POST: /Employee/CreatePayment
        /// Posts EmployeePayment record
        /// </summary>
        /// <param name="empView"></param>
        /// <param name="PaymentAmount"></param>
        /// <param name="PaymentTypeID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EmpPayId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePayment(EmployeePaymentBO empView, float paymentAmount, int paymentTypeID, DateTime startDate, int empPayId/*, DateTime EndDate*/)
        {
            string error;
            empView.EmployeePayID = empPayId;
            empView.EmployeeID = empView.EmployeeID;
            empView.PaymentAmount = paymentAmount;
            empView.PaymentTypeID = paymentTypeID;
            empView.StartDate = startDate;
            //empView.EndDate = EndDate;
            SetViewBag();   //ViewBag values set for all dropdowns in the Employee creation or update
            //if (ModelState.IsValid)
            //{            
            if (empView.EmployeePayID == 0)     //Creates a new Payment
            {
                if (bl.SaveEmpPayment(empView, out error) == true)  //Successfully creates Payment
                {
                    return Json(new
                    {
                        success = true,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
                else  //Failed to create Payment
                {
                    return Json(new
                    {
                        success = false,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else  //Updates a particular Employee Payment
            {
                if (bl.UpdatePayment(empView, out error) == true) //Successfully updated the Payment
                {
                    return Json(new
                    {
                        success = true,
                        errorMsg = error
                    });
                }
                else   //Failed to update the Payment
                {
                    return Json(new
                    {
                        success = false,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            //}
            //return View("CreateEditEmployee", empView);
        }

        /// <summary>
        /// GET: /Employee/EditPayment
        /// Gets Detail for edit EmployeePayment
        /// </summary>
        /// <param name="id:EmployeePaymentID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPayment(int id)
        {
            string error;
            var empView = new EmployeeViewBO();
            empView.EmployeePaymentBO = bl.EditEmployeePayment(id, out error);    //Gets details for Edit Payment
            empView.EmployeePaymentList = bl.GetEmployeePaymentList(empView);     //Gets PaymentList for a particular Employee
            SetViewBag();  //ViewBag values set for all dropdowns in the Employee creation or update
            return PartialView("_CreateEditPayment", empView.EmployeePaymentBO);    //Partially returns "_CreateEditPayment"
        }

        /// <summary>
        /// POST: /Employee/DeleteEmployeePayment
        /// Deletes Employee Payment
        /// </summary>
        /// <param name="id:EmployeePaymentID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEmployeePayment(int id)
        {
            string errorMsg = "";
            if (bl.DeleteEmployeePayment(id, out errorMsg) == true)   //Successfully deletes the Employee Payment
            {
                return Json(new
                {
                    success = true,
                    errorMsg = errorMsg
                }, JsonRequestBehavior.AllowGet);
            }
            else   //Failed to delete the Employee Payment
            {
                return Json(new
                {
                    success = false,
                    errorMsg = errorMsg
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets Employee Payment List for a particular Employee
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListPayment()
        {
            var employeePaymentViewBO = new EmployeeViewBO();
            employeePaymentViewBO.EmployeePaymentList = bl.GetEmployeePaymentList(employeePaymentViewBO);     //Gets Payment List for a particular Employee
            return PartialView("_EmployeePaymentList", employeePaymentViewBO.EmployeePaymentList);      //Partially returns "_EmployeePaymentList"
        }

        #endregion EmployeePayment

        #region EmployeeWorkingPattern

        /// <summary>
        /// Creates WorkingPattern's "Get" Action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateWorkingPattern()
        {
            var empView = new EmployeeViewBO();
            empView.WorkingPatternDetailsBO = new Employee_WorkingPatternDetailsBO();
            empView.EmployeeWorkingPatternList = bl.GetEmployeeWorkingPatternList(empView);     //Gets List of Working Pattern for a particular Employee
            return PartialView("_CreateEditWorkingPattern", empView.WorkingPatternDetailsBO);    //Partially returns "_CreateEditWorkingPattern"
        }

        /// <summary>
        /// Saves and Updates the Employee Working Pattern Details
        /// </summary>
        /// <param name="empView"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateWorkingPattern(Employee_WorkingPatternDetailsBO empView, DateTime changeday, int workingPatternID, float noOfHours, bool mon /*= false*/, bool tues /*= true || false*/, bool wed /*= false*/, bool thur/* = false*/, bool fri /*= false*/, bool sat/* = false*/, bool sun /*= false*/)
        {
            string error;
            empView.WorkingPatternID = workingPatternID;
            empView.EmployeeID = empView.EmployeeID;
            empView.Monday = mon;
            empView.Tuesday = tues;
            empView.Wednesday = wed;
            empView.Thursday = thur;
            empView.Friday = fri;
            empView.Saturday = sat;
            empView.Sunday = sun;
            empView.ChangeDate = changeday;
            empView.NoOfWorkingHours = noOfHours;
            SetViewBag();   //ViewBag values set for all dropdowns in the Employee creation or update
            //if (ModelState.IsValid)
            //{            
            if (empView.WorkingPatternID == 0)     //Creates a new Working Pattern
            {
                if (bl.SaveEmpWorkingPattern(empView, out error) == true)  //Successfully creates Working Pattern
                {
                    return Json(new
                    {
                        success = true,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
                else  //Failed to create Working Pattern
                {
                    return Json(new
                    {
                        success = false,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else  //Updates a particular Working Pattern
            {
                if (bl.UpdateEmpWorkingPattern(empView, out error) == true) //Successfully updates the Working Pattern
                {
                    return Json(new
                    {
                        success = true,
                        errorMsg = error
                    });
                }
                else   //Failed to update the Working Pattern
                {
                    return Json(new
                    {
                        success = false,
                        errorMsg = error
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            //}
            //return View("CreateEditEmployee", empView);
        }

        /// <summary>
        /// Edits Working Pattern Form
        /// </summary>
        /// <param name="id:WorkingPatternID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditWorkingPattern(int id)
        {
            string error;
            var empView = new EmployeeViewBO();
            empView.WorkingPatternDetailsBO = bl.EditEmployeeWorkingPattern(id, out error);    //Gets details for edit the Working Pattern
            empView.EmployeeWorkingPatternList = bl.GetEmployeeWorkingPatternList(empView);     //Gets Working Pattern List for a particular Employee
            SetViewBag();  //ViewBag values set for all dropdowns in the Employee creation or update
            return PartialView("_CreateEditWorkingPattern", empView.WorkingPatternDetailsBO);    //Partially returns "_CreateEditWorkingPattern"
        }

        /// <summary>
        /// Deletes Employee Working Pattern
        /// </summary>
        /// <param name="id:WorkingPatternID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEmployeeWorkingPatterns(int id)
        {
            string errorMsg = "";
            if (bl.DeleteEmployeeWorkingPattern(id, out errorMsg) == true)   //Successfully deletes the Working Pattern
            {
                return Json(new
                {
                    success = true,
                    errorMsg = errorMsg
                }, JsonRequestBehavior.AllowGet);
            }
            else   //Failed to delete the Working Pattern
            {
                return Json(new
                {
                    success = false,
                    errorMsg = errorMsg
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion EmployeeWorkingPattern

        #region EmployeeExperience

        /// <summary>
        /// Sets ViewBags values for Experience
        /// </summary>
        private void SetExperienceViewBag()
        {
            //Employee Experience related dropdowns           
            ViewBag.Designations = (bl.GetDesignation().Select(x => new SelectListItem()
            {
                Value = x.DesignationID.ToString(),
                Text = x.DesignationName
            }).OrderBy(desig => desig.Text));    //Gets Designation List in the alphabet order

            ViewBag.CompanyName = (bl.GetCompany().Select(x => new SelectListItem()
            {
                Value = x.CompanyID.ToString(),
                Text = x.CompanyName
            }).OrderBy(company => company.Text));    //Gets CompanyName List in the alphabet order
        }

        /// <summary>
        /// Displays an Index page for "Resource Experience"
        /// </summary>
        /// <param name="id: EmployeeID"></param>
        /// <returns></returns>
        public ActionResult ExperienceIndex(int id)
        {
            string error;
            ViewBag.CurrentTab = "Experience";  //Sets the "Experience" tab as active
            SetExperienceViewBag();
            EmployeeViewBO employeeBO = bl.EditEmployee(id, out error); //Gets the EmployeeID for update the Experience for a particular Employee

            var employeeExperienceViewBO = new EmployeeExperienceViewBO();
            employeeExperienceViewBO.EmployeeID = employeeBO.EmployeeID;        //Gets EmployeeID

            employeeExperienceViewBO.EmployeeExperienceViewModels = new EmployeeExperienceViewModels();
            employeeExperienceViewBO.EmployeeExperienceViewModels.ResourceID = employeeBO.EmployeeID;    //Sets the EmployeeID to update the "Experience"
            employeeExperienceViewBO.EmployeeExpereienceList = bl.GetExperienceList(employeeExperienceViewBO.EmployeeExperienceViewModels);   //Gets List of all Experience for a particular Employee

            employeeExperienceViewBO.ResourceSubMenuBO = new ResourceSubMenuBO();     //Sets the values for "ResourceSubMenuBO" models
            employeeExperienceViewBO.ResourceSubMenuBO.FName = employeeBO.FirstName;
            employeeExperienceViewBO.ResourceSubMenuBO.LName = employeeBO.LastName;
            employeeExperienceViewBO.ResourceSubMenuBO.EmployeeID = employeeBO.EmployeeID;

            return View(employeeExperienceViewBO);
        }

        /// <summary>
        /// Gets Create form for "Experience"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateExperience()
        {
            SetExperienceViewBag();     //ViewBag values set for all dropdowns in the Employee Experience
            var empExperienceViewBO = new EmployeeExperienceViewBO();
            empExperienceViewBO.EmployeeExperienceViewModels = new EmployeeExperienceViewModels();
            return PartialView("_CreateEditExperience", empExperienceViewBO.EmployeeExperienceViewModels);
        }

        /// <summary>
        /// Post Action for Experience Save function
        /// </summary>
        /// <param name="emplExperienceViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateExperience(EmployeeExperienceViewModels empExperienceViewModel)
        {
            string error;
            SetExperienceViewBag();
            if (ModelState.IsValid)     //When the ModelState is true
            {
                if (empExperienceViewModel.ExperienceID == 0)     //Creates a new Experience
                {
                    if (bl.SaveExperience(empExperienceViewModel, out error) == true)     //Successfully Saves experience
                    {
                        return Json(new
                        {
                            success = true,
                            errorMsg = error
                        });
                    }
                    else  //Failed to save experience
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else     //Updates a particular Employee experience record
                {
                    if (bl.UpdateExperience(empExperienceViewModel, out error) == true)     //Successfully updates the experience
                    {
                        return Json(new
                        {
                            success = true,
                            errorMsg = error
                        });
                    }
                    else  //Failed to update experience
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            return PartialView("_CreateEditExperience", empExperienceViewModel);    //Partiallly returns "_CreateEditExperience"
        }

        /// <summary>
        /// Gets Experience List for a particular Employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExperienceList(ResourceSubMenuBO e)
        {
            var employeeExperience = new EmployeeExperienceViewBO();
            employeeExperience.EmployeeExperienceViewModels = new EmployeeExperienceViewModels();
            employeeExperience.EmployeeExpereienceList = bl.GetExperienceList(employeeExperience.EmployeeExperienceViewModels); //Gets List of Experience for a particular Employee
            return PartialView("_ExperienceList", employeeExperience.EmployeeExpereienceList);
        }

        /// <summary>
        /// Gets all details for a particular Experience for edit
        /// </summary>
        /// <param name="id:ExperienceID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditExperience(int id)
        {
            string error;
            SetExperienceViewBag();     //ViewBag values set for all dropdowns in the Experience
            var empExperienceViewBO = new EmployeeExperienceViewBO();
            empExperienceViewBO.EmployeeExperienceViewModels = bl.EditExperience(id, out error);    //Gets details for edit Experience
            if (empExperienceViewBO.EmployeeExperienceViewModels == null)       //When the selected Experience is not exist
            {
                //return Json(new
                //{
                //    success = false,
                //    errorMsg = error
                //});
                TempData["Error"] = error;
                return RedirectToAction("ExperienceIndex", empExperienceViewBO);    //Returns the "ExperienceIndex" 
            }
            return PartialView("_CreateEditExperience", empExperienceViewBO.EmployeeExperienceViewModels);  //Partially returns the "_CreateEditExperience"
        }

        /// <summary>
        /// Deletes Experience
        /// </summary>
        /// <param name="id:ExperienceID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteExperience(int id)
        {
            string error;
            if (bl.DeleteExperience(id, out error) == true)     //Successfully deletes the Experience
            {
                return Json(new
                {
                    success = true,
                    errorMsg = error
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new     //Failed to delete the Experience
            {
                success = false,
                errorMsg = error
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion EmployeeExperience

        #region EmployeeSkills

        private void SetSkillsViewBag()
        {
            //Employee Skill related dropdown           
            ViewBag.CompanyName = (bl.GetCompany().Select(x => new SelectListItem()
            {
                Value = x.CompanyID.ToString(),
                Text = x.CompanyName
            }).OrderBy(company => company.Text));    //Gets CompanyName List in the alphabet order
        }

        /// <summary>
        /// Index Page for Skills
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SkillsIndex(int id)
        {
            string error;
            ViewBag.CurrentTab = "Skill";  //Sets the "Skills" tab as active
            SetSkillsViewBag();
            EmployeeViewBO employeeBO = bl.EditEmployee(id, out error); //Gets the EmployeeID for update the Skills

            var employeeSkillViewBO = new EmployeeSkillViewBO();
            employeeSkillViewBO.EmployeeID = employeeBO.EmployeeID;        //Gets EmployeeID

            employeeSkillViewBO.ResourceSubMenuBO = new ResourceSubMenuBO();     //Sets the values for "ResourceSubMenuBO" models
            employeeSkillViewBO.ResourceSubMenuBO.FName = employeeBO.FirstName;
            employeeSkillViewBO.ResourceSubMenuBO.LName = employeeBO.LastName;
            employeeSkillViewBO.ResourceSubMenuBO.EmployeeID = employeeBO.EmployeeID;

            employeeSkillViewBO.SkillCategoryList = bl.GetSkillCategoryList(id);    //Gets the list of Skills for a particular Employee

            return View(employeeSkillViewBO);
        }

        /// <summary>
        /// Saves and updates Employee skills
        /// </summary>
        /// <param name="employeeSkillViewBO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEditSkills(EmployeeSkillViewBO employeeSkillViewBO)
        {
            string error;
            SetSkillsViewBag();
            //if (ModelState.IsValid)     //When the ModelState is true
            //{
            if (bl.SaveSkills(employeeSkillViewBO, out error))
            {
                return Json(new
                {
                    success = true,
                    errorMsg = error
                });     //Successfully saves the skill
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorMsg = error
                });     //Failed to save the skill
                //ModelState.AddModelError(string.Empty, error);
            }
            //}
            //return RedirectToAction("SkillsIndex", employeeSkillViewBO);    //Returns "SkillsIndex" view
        }

        #endregion EmployeeSkills

    }
}