//--------------------------------------------------------------------------------
// <copyright file="EmployeeBL.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains all the functions related to Employee 
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using ERP.DA;
using ERP.Resource.Models;
using ERP.Utility.Models;
using ERP.Resource.ViewModels;
using System.IO;
using ERP.Address.BL;
using ERP.Address.ViewModels;
using EntityFramework.Extensions;

namespace ERP.Resource.BL
{
    public class EmployeeBL
    {
        private ERPEntities db = new ERPEntities();         //Gets models from the Database
        private AddressBL addressBL = new AddressBL();      //Gets all the methods from "AddressBL"

        #region Employee

        /// <summary>
        /// Gets Employee List for Index Page
        /// </summary>
        /// <param name="employeeSearchBO"></param>
        /// <returns>empList</returns>
        public IEnumerable<EmployeeViewBO> GetEmployeeList(EmployeeSearchBO employeeSearchBO)
        {
            IEnumerable<EmployeeViewBO> empList = (from emp in db.Resource_Employee
                                                   select new EmployeeViewBO()
                                                   {
                                                       AddressID = emp.AdderessID,
                                                       Town = emp.Common_Address.Town,
                                                       //BranchID = (int)emp.Resource_BranchDetails.Where(x => x.ResourceID == emp.EmployeeID).Select(c => c.BranchID).FirstOrDefault(),      //Gets "BranchID"
                                                       //BranchName = emp.Resource_BranchDetails.Where(x => x.ResourceID == emp.EmployeeID).Select(c => c.Branch_Details.BranchName).FirstOrDefault(),    //Gets "BranchName"
                                                       BList = emp.Resource_BranchDetails.Where(e => e.ResourceID == emp.EmployeeID).Select(b => b.Branch_Details.BranchName).ToList(),
                                                       //ShiftPatternID = (int)emp.Resource_ShiftSchedule.Where(x => x.EmployeeID == emp.EmployeeID).Select(c => c.ShiftPatternID).FirstOrDefault(),      //Gets "ShiftPatternID"
                                                       //ShiftPatternName = emp.Resource_ShiftSchedule.Where(x => x.EmployeeID == emp.EmployeeID).Select(c => c.Resource_ShiftPattern.Description).FirstOrDefault(),    //Gets "ShiftPatternName"
                                                       //ShiftPatternList = emp.Resource_ShiftSchedule.Where(x => x.EmployeeID == emp.EmployeeID).Select(c => c.Resource_ShiftPattern.Description).Distinct().ToList(),   //Gets Shift Pattern with the Distinction
                                                       DateOfJoin = emp.DateOfJoin,
                                                       DateOfLeave = emp.DateOfLeave,
                                                       DoB = emp.DoB,
                                                       Email = emp.Email,
                                                       EmployeeID = emp.EmployeeID,
                                                       EmployeeTypeID = emp.EmployeeTypeID,
                                                       EmployeeTypeName = emp.Common_EmployeeType.EmployeeTypeName,
                                                       EthnicGroupID = emp.EthnicGroupID,
                                                       EthnicGroupName = emp.Common_EthnicGroup.EthnicGroupName,
                                                       FirstName = emp.FirstName,
                                                       ImmigrationStatusID = emp.ImmigrationStatusID,
                                                       ImmigrationStatusName = emp.Common_ImmigrationStatus.ImmigrationName,
                                                       TimeSheetFrequencyID = emp.TimeSheetFrequencyID,
                                                       TimeSheetFrequencyName = emp.Common_TimeSheetFrequency.TimeSheetFrequencyName,
                                                       KIN_MobileNo = emp.KIN_MobileNo,
                                                       KIN_TelephoneNo = emp.KIN_TelephoneNo,
                                                       LastName = emp.LastName,
                                                       MaritalStatusID = emp.MaritalStatusID,
                                                       MaritalStatusName = emp.Common_MaritalStatus.MaritalStatusName,
                                                       MiddleName = emp.MiddleName,
                                                       MobileNo = emp.MobileNo,
                                                       NationalityID = emp.NationalityID,
                                                       NextOfKIN_Name = emp.NextOfKIN_Name,
                                                       NINo = emp.NINo,
                                                       PhotoFileType = emp.PhotoFileType,
                                                       ProfilePhoto = emp.ProfilePhoto,
                                                       Sex = emp.Sex,
                                                       TelephoneNo = emp.TelephoneNo,
                                                       TitleID = emp.TitleID,
                                                       TitleName = emp.Common_Title.TitleName
                                                   });
            if (employeeSearchBO.SearchBy == "BranchName")     //Search Employee by "BranchName"
            {
                var resourceBranchList = db.Resource_BranchDetails.Where(c => c.BranchID == employeeSearchBO.BranchID).Select(e => e.Branch_Details.BranchName).ToList();    //Gets Resource_Branch List for particular "BranchID" and gets the "BranchName" for that "BranchID" from "Resource_BranchDetails" table
                if (resourceBranchList.Count() > 0) //When the "resourceBranchList" has atleast one record
                {
                    foreach (var branchName in resourceBranchList)
                    {
                        if (/*employeeSearchBO.BranchID != 0 && */!string.IsNullOrEmpty(employeeSearchBO.Text))     //When the Text field has values
                        {
                            employeeSearchBO.Text = employeeSearchBO.Text.ToLower();    //Sets the text field value in lower case
                            empList = empList.Where(employee => employee.BList.Contains(branchName) && (employee.FirstName.ToLower().Contains(employeeSearchBO.Text) || employee.LastName.ToLower().Contains(employeeSearchBO.Text)));     //Gets Employee records for the selected "BranchName" with "First Name" or "Last Name"
                        }
                        else   //When the Text field is empty
                        {
                            empList = empList.Where(employee => employee.BList.Contains(branchName));
                            //empList = empList.Where(employee => employee.BList.Contains(branchName)).ToList();
                        }
                    }
                }
                else    //When the "resourceBranchList" has no any record
                {
                    return empList.ToList().OrderBy(e => e.FirstName); //Gets Employee List
                }
            }
            else if (!string.IsNullOrEmpty(employeeSearchBO.SearchBy) && !string.IsNullOrEmpty(employeeSearchBO.Text))
            {
                employeeSearchBO.Text = employeeSearchBO.Text.ToLower();    //Sets the text field value in lower case
                if (employeeSearchBO.SearchBy == "FName")       //Search Employee by "FirstName"
                {
                    empList = empList.Where(employee => employee.FirstName.ToLower().Contains(employeeSearchBO.Text));
                }
                else if (employeeSearchBO.SearchBy == "LName")      //Search Employee by "LastName"
                {
                    empList = empList.Where(employee => employee.LastName.ToLower().Contains(employeeSearchBO.Text));
                }
                else if (employeeSearchBO.SearchBy == "EmpType")    //Search Employee by "EmployeeType"
                {
                    empList = empList.Where(employee => employee.EmployeeTypeName.ToLower().Contains(employeeSearchBO.Text));
                }
                else if (employeeSearchBO.SearchBy == "TimeSheetFrequency")     //Search Employee by "TimeSheetFrequency"
                {
                    empList = empList.Where(employee => employee.TimeSheetFrequencyName.ToLower().Contains(employeeSearchBO.Text));
                }
                else
                {
                    return empList.ToList().OrderBy(e => e.FirstName);    //Gets Employee List
                }
            }
            else
            {
                return empList.ToList().OrderBy(e => e.FirstName);    //Gets Employee List
            }
            return empList.ToList().OrderBy(e => e.FirstName);    //Gets Employee List
            //return empList.ToList().OrderByDescending(e => e.EmployeeID);
        }

        /// <summary>
        /// Gets Employee List for Infinite Scroll
        /// </summary>
        /// <returns>empList</returns>
        public IEnumerable<EmployeeViewBO> GetEmployeeLists()
        {
            IEnumerable<EmployeeViewBO> empList = (from emp in db.Resource_Employee
                                                   select new EmployeeViewBO()
                                                   {
                                                       AddressID = emp.AdderessID,
                                                       Town = emp.Common_Address.Town,
                                                       //BranchID = (int)emp.Resource_BranchDetails.Where(x => x.ResourceID == emp.EmployeeID).Select(c => c.BranchID).FirstOrDefault(),  //Gets "BranchID"
                                                       //BranchName = emp.Resource_BranchDetails.Where(x => x.ResourceID == emp.EmployeeID).Select(c => c.Branch_Details.BranchName).FirstOrDefault(),    //Gets "BranchName"
                                                       BList = emp.Resource_BranchDetails.Where(e => e.ResourceID == emp.EmployeeID).Select(b => b.Branch_Details.BranchName).ToList(),
                                                       DateOfJoin = emp.DateOfJoin,
                                                       DateOfLeave = emp.DateOfLeave,
                                                       DoB = emp.DoB,
                                                       Email = emp.Email,
                                                       EmployeeID = emp.EmployeeID,
                                                       EmployeeTypeID = emp.EmployeeTypeID,
                                                       EmployeeTypeName = emp.Common_EmployeeType.EmployeeTypeName,
                                                       EthnicGroupID = emp.EthnicGroupID,
                                                       EthnicGroupName = emp.Common_EthnicGroup.EthnicGroupName,
                                                       FirstName = emp.FirstName,
                                                       ImmigrationStatusID = emp.ImmigrationStatusID,
                                                       ImmigrationStatusName = emp.Common_ImmigrationStatus.ImmigrationName,
                                                       TimeSheetFrequencyID = emp.TimeSheetFrequencyID,
                                                       TimeSheetFrequencyName = emp.Common_TimeSheetFrequency.TimeSheetFrequencyName,
                                                       KIN_MobileNo = emp.KIN_MobileNo,
                                                       KIN_TelephoneNo = emp.KIN_TelephoneNo,
                                                       LastName = emp.LastName,
                                                       MaritalStatusID = emp.MaritalStatusID,
                                                       MaritalStatusName = emp.Common_MaritalStatus.MaritalStatusName,
                                                       MiddleName = emp.MiddleName,
                                                       MobileNo = emp.MobileNo,
                                                       NationalityID = emp.NationalityID,
                                                       NextOfKIN_Name = emp.NextOfKIN_Name,
                                                       NINo = emp.NINo,
                                                       PhotoFileType = emp.PhotoFileType,
                                                       ProfilePhoto = emp.ProfilePhoto,
                                                       Sex = emp.Sex,
                                                       TelephoneNo = emp.TelephoneNo,
                                                       TitleID = emp.TitleID,
                                                       TitleName = emp.Common_Title.TitleName
                                                   });
            return empList.ToList().OrderBy(e => e.FirstName);
        }

        /// <summary>
        /// Gets Branch List from "Branch_Details" Table
        /// </summary>
        /// <returns></returns>
        public List<Branch_Details> GetBranch()
        {
            var branchList = (from branch in db.Branch_Details
                              select branch).ToList();
            return branchList;
        }

        /// <summary>
        /// Gets Shift Pattern List from "Resource_ShiftPattern" Table
        /// </summary>
        /// <returns></returns>
        public List<Resource_ShiftPattern> GetShiftPattern()
        {
            var shiftPatternList = (from shiftPattern in db.Resource_ShiftPattern
                                    select shiftPattern).ToList();
            return shiftPatternList;
        }

        /// <summary>
        /// Gets Title List from "Common_Title" Table
        /// </summary>
        /// <returns></returns>
        public List<Common_Title> GetTitle()
        {
            var titleList = (from title in db.Common_Title
                             select title).ToList();
            return titleList;
        }

        /// <summary>
        /// Gets Marital Status List from "Common_MaritalStatus" Table
        /// </summary>
        /// <returns>maritalStatusList</returns>
        public List<Common_MaritalStatus> GetMaritalStatusList()
        {
            var maritalStatusList = (from maritalStatus in db.Common_MaritalStatus
                                     select maritalStatus).ToList();
            return maritalStatusList;
        }

        /// <summary>
        /// Gets Nationality List from "Common_Nationality" Table
        /// </summary>
        /// <returns>nationalityList</returns>
        public List<Common_Nationality> GetNationalityList()
        {
            var nationalityList = (from nationality in db.Common_Nationality
                                   select nationality).ToList();
            return nationalityList;
        }

        /// <summary>
        /// Gets Employee Type List from "Common_EmployeeType" Table
        /// </summary>
        /// <returns>empTypeList</returns>
        public List<Common_EmployeeType> GetEmployeeTypeList()
        {
            var empTypeList = (from empType in db.Common_EmployeeType
                               select empType).ToList();
            return empTypeList;
        }

        /// <summary>
        /// Gets Ethnic Group List from "Common_EthnicGroup" Table
        /// </summary>
        /// <returns>ethnicGroupList</returns>
        public List<Common_EthnicGroup> GetEthnicGroupList()
        {
            var ethnicGroupList = (from ethnicGroup in db.Common_EthnicGroup
                                   select ethnicGroup).ToList();
            return ethnicGroupList;
        }

        /// <summary>
        /// Gets Immigration Status List from "Common_ImmigrationStatus" table
        /// </summary>
        /// <returns>immigrationStatusList</returns>
        public List<Common_ImmigrationStatus> GetImmigrationStatusList()
        {
            var immigrationStatusList = (from immigrationStatus in db.Common_ImmigrationStatus
                                         select immigrationStatus).ToList();
            return immigrationStatusList;
        }

        /// <summary>
        /// Gets TimeFrequency List from "Common_TimeSheetFrequency" table
        /// </summary>
        /// <returns>timeSheetFrequencyList</returns>
        public List<Common_TimeSheetFrequency> GetTimeSheetFrequencyList()
        {
            var timeSheetFrequencyList = (from timeSheetFrequency in db.Common_TimeSheetFrequency
                                          select timeSheetFrequency).ToList();
            return timeSheetFrequencyList;
        }

        /// <summary>
        /// Converts images into bytes
        /// </summary>
        /// <param name="UploadFile"></param>
        /// <returns>imageBytes</returns>
        public byte[] ConvertToBytes(HttpPostedFileBase UploadFile)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(UploadFile.InputStream);
            imageBytes = reader.ReadBytes((int)UploadFile.ContentLength);
            return imageBytes;
        }

        /// <summary>
        /// Saves Employee Detail in "Resource_Employee" Table
        /// </summary>
        /// <param name="employeeBO"></param>
        /// <param name="emsg"></param>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        public bool SaveEmployee(EmployeeViewBO employeeBO, out string emsg, HttpPostedFileBase UploadFile)
        {
            emsg = "";
            try
            {
                var existingEmployee = (from emp in db.Resource_Employee
                                        where emp.NINo == employeeBO.NINo
                                        select emp).SingleOrDefault();  //Checks Existing Employee

                if (existingEmployee == null)   //When the Employee is not exist
                {
                    if (employeeBO.AddressViewModel.Postcode != null)
                    {
                        employeeBO.AddressID = (int)addressBL.CheckAddress(employeeBO.AddressViewModel);    //Assign a AddressID for Employee
                        employeeBO.AddressViewModel.AddressID = employeeBO.AddressID;
                        if (employeeBO.AddressViewModel.AddressID == 0)
                        {
                            emsg = "AddressID Is Not Saved!.....";
                            return false;
                        }
                    }
                    //Saves Employee Record
                    var employee = new Resource_Employee();
                    employee.AdderessID = (int)employeeBO.AddressID;
                    employee.DateOfJoin = employeeBO.DateOfJoin;
                    employee.DateOfLeave = employeeBO.DateOfLeave;
                    if (employeeBO.DateOfJoin != null && employeeBO.DateOfLeave != null)    //Validates "DateOfJoin" and "DateOfLeave" Fields
                    {
                        if (employeeBO.DateOfLeave <= employeeBO.DateOfJoin)
                        {
                            emsg = "DateOfLeft Should Be Greater Than DateOfJoin....";
                            return false;
                        }
                    }
                    employee.DoB = employeeBO.DoB;
                    if (employeeBO.DoB != null && employee.DateOfJoin != null) //Validates DateOfJoin
                    {
                        if (employee.DateOfJoin.Value.Year < employee.DoB.Value.Year + 15)
                        {
                            emsg = "DateOfJoin Should Be Greater Than BirthDate Year + 15 Years...Please Confirm Your BirthDate...";
                            return false;
                        }
                    }
                    employee.Email = employeeBO.Email;
                    employee.EmployeeTypeID = employeeBO.EmployeeTypeID;
                    employee.EthnicGroupID = employeeBO.EthnicGroupID;
                    if (employeeBO.FirstName != null && employeeBO.FirstName.Length > 50) //Validates FirstName Length
                    {
                        emsg = "FirstName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    else if (employeeBO.MiddleName != null && employeeBO.MiddleName.Length > 50) //Validates MiddleName Length
                    {
                        emsg = "MiddleName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    else if (employeeBO.LastName != null && employeeBO.LastName.Length > 50)   //Validates LastName Length
                    {
                        emsg = "LastName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    employee.FirstName = employeeBO.FirstName;
                    employee.ImmigrationStatusID = employeeBO.ImmigrationStatusID;
                    employee.TimeSheetFrequencyID = employeeBO.TimeSheetFrequencyID;
                    employee.KIN_MobileNo = employeeBO.KIN_MobileNo;
                    employee.KIN_TelephoneNo = employeeBO.KIN_TelephoneNo;
                    employee.LastName = employeeBO.LastName;
                    employee.MaritalStatusID = employeeBO.MaritalStatusID;
                    employee.MiddleName = employeeBO.MiddleName;
                    employee.MobileNo = employeeBO.MobileNo;
                    employee.NationalityID = employeeBO.NationalityID;
                    employee.NextOfKIN_Name = employeeBO.NextOfKIN_Name;
                    employee.NINo = employeeBO.NINo;
                    if (UploadFile == null)     //When the profile is not uploaded
                    {
                        employee.PhotoFileType = null;
                        employee.ProfilePhoto = null;
                    }
                    else   //When the profile is uploaded
                    {
                        if (UploadFile.ContentType == "image/jpeg" || UploadFile.ContentType == "image/png" || UploadFile.ContentType == "image/gif")
                        {
                            employee.PhotoFileType = UploadFile.ContentType;
                            employee.ProfilePhoto = ConvertToBytes(UploadFile);
                        }
                        else
                        {
                            emsg = "ImageType Should Be 'jpeg' Or 'png' Or 'gif' ";
                            return false;
                        }
                    }
                    employee.Sex = employeeBO.Sex;
                    employee.TelephoneNo = employeeBO.TelephoneNo;
                    employee.TitleID = employeeBO.TitleID;

                    var empPayment = new Resource_EmployeePayment();
                    db.Resource_Employee.Add(employee); //Adds Employee record to the "Resource_Employee" table
                    db.SaveChanges();

                    var empRecord = (from e in db.Resource_Employee
                                     select e).ToList().LastOrDefault();    //Gets last stored EmployeeID to create Payment record for a particular Employee

                    if (employeeBO.BranchList.Count() > 0)      //When the Branch List is not empty
                    {
                        foreach (var B_ID in employeeBO.BranchList)     //Gets selected Branch as a list
                        {
                            //Saves Branch Details to "Resource_BranchDetails" table
                            var branch = new Resource_BranchDetails();
                            branch.BranchID = B_ID;
                            branch.ResourceID = empRecord.EmployeeID;
                            db.Resource_BranchDetails.Add(branch);      //Adds Branch record to the "Resource_BranchDetails" table
                            db.SaveChanges();
                        }
                    }

                    //Saves EmployeePayment for a particular Employee
                    empPayment.EmployeeID = empRecord.EmployeeID;
                    empPayment.EndDate = employeeBO.EmployeePaymentBO.EndDate;
                    empPayment.PaymentAmount = (double)employeeBO.EmployeePaymentBO.PaymentAmount;
                    empPayment.PaymentTypeID = employeeBO.EmployeePaymentBO.PaymentTypeID;
                    //empPayment.StartDate = employeeBO.EmployeePaymentBO.StartDate;
                    empPayment.StartDate = (DateTime)employeeBO.DateOfJoin;

                    db.Resource_EmployeePayment.Add(empPayment);    // Adds EmployeePayment record to the "Resource_EmployeePayment" table
                    db.SaveChanges();

                    //Saves Employee_WorkingPattern details
                    var workingPatternDetails = new Resouce_WorkingPatternDetails();
                    workingPatternDetails.ResourceID = empRecord.EmployeeID;
                    workingPatternDetails.NoOfWorkingHours = (double)employeeBO.WorkingPatternDetailsBO.NoOfWorkingHours;
                    workingPatternDetails.ChangeDate = employeeBO.WorkingPatternDetailsBO.ChangeDate;
                    workingPatternDetails.Sunday = employeeBO.WorkingPatternDetailsBO.Sunday;
                    workingPatternDetails.Monday = employeeBO.WorkingPatternDetailsBO.Monday;
                    workingPatternDetails.Tuesday = employeeBO.WorkingPatternDetailsBO.Tuesday;
                    workingPatternDetails.Wednesday = employeeBO.WorkingPatternDetailsBO.Wednesday;
                    workingPatternDetails.Thursday = employeeBO.WorkingPatternDetailsBO.Thursday;
                    workingPatternDetails.Friday = employeeBO.WorkingPatternDetailsBO.Friday;
                    workingPatternDetails.Saturday = employeeBO.WorkingPatternDetailsBO.Saturday;

                    db.Resouce_WorkingPatternDetails.Add(workingPatternDetails);    // Adds Employee_WorkingPattern Details to the "Resouce_WorkingPatternDetails" table
                    db.SaveChanges();

                    if (UploadFile == null)     //When the Profile is not uploaded
                    {
                        emsg = employeeBO.FirstName + " Is Successfully Created With Out Upload Your Profile Image...";
                        return true;
                    }
                    else  //When the Profile is uploaded
                    {
                        emsg = employeeBO.FirstName + " Is Successfully Created";
                        return true;
                    }
                }
                else  //When the entered "NIC Number" is already exist
                {
                    emsg = "This NIC Number May Used By Another Resource!..Please Provide A Correct NIC Number...";
                    return false;
                }
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Deletes Employee record
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool DeleteEmployee(int id, out string emsg)
        {
            emsg = "";
            var empRecord = (from emp in db.Resource_Employee
                             where emp.EmployeeID == id
                             select emp).SingleOrDefault();      //Gets selected Employee Detail

            if (empRecord != null)
            {
                if (empRecord.Resource_Holiday.Count() > 0)   //When the Holiday is allocated for a slected Employee
                {
                    emsg = empRecord.FirstName + "! May Have A Holiday Allocated To It So It Cannot Be Deleted";
                    return false;
                }

                //if (empRecord.Resource_HolidayStatus.Count() > 0)   //When the Holiday Status is allocated for a slected Employee
                //{
                //    emsg = "HolidayStatus May Changed By " + empRecord.FirstName + "! So It Cannot Be Deleted";
                //    return false;
                //}

                if (empRecord.Resource_ShiftSchedule.Count() > 0)   //When the Shift is scheduled for a slected Employee
                {
                    emsg = empRecord.FirstName + "! May Have A Shift Schedule Allocated To It So It Cannot Be Deleted";
                    return false;
                }

                if (empRecord.Resource_TimeSheet.Count() > 0) //When the Time Sheet is generated for a slected Employee
                {
                    emsg = empRecord.FirstName + " May Have A TimeSheet So It Cannot Be Deleted";
                    return false;
                }

                var empWorkingPatternRecord = (from empWorkingPattern in db.Resouce_WorkingPatternDetails
                                               where empWorkingPattern.ResourceID == id
                                               select empWorkingPattern).ToList();    //Gets Working Pattern Detail for selected Employee

                var empBranchRecord = (from empBranch in db.Resource_BranchDetails
                                       where empBranch.ResourceID == id
                                       select empBranch).ToList();    //Gets Branch Detail for selected Employee 

                var empPaymentRecord = (from empPayment in db.Resource_EmployeePayment
                                        where empPayment.EmployeeID == id
                                        select empPayment).ToList();    //Gets Payment Detail for selected Employee 

                var empExperienceRecord = (from empExperience in db.Resource_ExperienceDetails
                                           where empExperience.ResourceID == id
                                           select empExperience).ToList();    //Gets Experience Detail for selected Employee 

                var empSkillRecord = (from empSkill in db.Resource_SkillDetails
                                      where empSkill.ResourceID == id
                                      select empSkill).ToList();    //Gets Skills Detail for selected Employee

                if (empBranchRecord.Count != 0)
                {
                    empBranchRecord.ForEach(branch => { db.Resource_BranchDetails.Remove(branch); });   //Removes all the Branch records for a selected Employee
                    db.SaveChanges();
                }
                if (empWorkingPatternRecord.Count != 0)
                {
                    empWorkingPatternRecord.ForEach(workingPattern => { db.Resouce_WorkingPatternDetails.Remove(workingPattern); });  //Removes all the Working patterns for a selected Employee
                    db.SaveChanges();
                }
                if (empPaymentRecord.Count != 0)
                {
                    empPaymentRecord.ForEach(payment => { db.Resource_EmployeePayment.Remove(payment); });  // Removes all the Payment records for a selected Employee 
                    db.SaveChanges();
                }
                if (empExperienceRecord.Count != 0)
                {
                    empExperienceRecord.ForEach(experience => { db.Resource_ExperienceDetails.Remove(experience); }); // Removes all the Experiences for a selected Employee 
                    db.SaveChanges();
                }
                if (empSkillRecord.Count != 0)
                {
                    empSkillRecord.ForEach(skills => { db.Resource_SkillDetails.Remove(skills); }); // Removes all the Skills for a selected Employee 
                    db.SaveChanges();
                }

                db.Resource_Employee.Remove(empRecord);     //Removes Employee record from "Resource_Employee" table
                db.SaveChanges();
                emsg = "Selected Resource Is Successfully Removed";
                return true;
            }
            else  //Employee record is not found
            {
                emsg = "Unable To Find This Resource, Please Try Again!";
                return false;
            }
        }


        /// <summary>
        /// Removes Profile Image
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool RemoveProfile(int id, out string emsg)
        {
            emsg = "";
            var existingEmployee = (from employee in db.Resource_Employee
                                    where employee.EmployeeID == id
                                    select employee).SingleOrDefault(); //Select a particular Employee record

            if (existingEmployee != null)   //When the selected employee is available
            {
                if (existingEmployee.PhotoFileType == null && existingEmployee.ProfilePhoto == null)    //When the profile is not uploaded
                {
                    emsg = existingEmployee.FirstName + "'s Profile May Be Deleted By Some One... You Have To Upload Profile First..";
                    return false;
                }
                else //When the profile is uploaded
                {
                    existingEmployee.PhotoFileType = null;
                    existingEmployee.ProfilePhoto = null;
                    db.SaveChanges();
                    emsg = existingEmployee.FirstName + "'s Profile Is Successfully Removed";
                    return true;
                }
            }
            else  //Employee record is not found
            {
                emsg = "Unable To Find This Resource!... Please Try Again!...";
                return false;
            }
        }

        /// <summary>
        /// Gets Details of selected Employee for edit
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <param name="emsg"></param>
        /// <returns>employeerecord</returns>
        public EmployeeViewBO EditEmployee(int id, out string emsg)
        {
            emsg = "";

            var branchID = (from branch in db.Resource_BranchDetails
                            where branch.ResourceID == id
                            select (int)branch.BranchID).ToList();      //Gets BranchList for a particular Employee

            var workingPattern_EmployeeDetails = (from workingPattern in db.Resouce_WorkingPatternDetails
                                                  where workingPattern.ResourceID == id
                                                  select workingPattern).FirstOrDefault();      //Gets BranchList for a particular Employee

            //Gets all the details for a selected Employee
            var employeerecord = (from emp in db.Resource_Employee
                                  where emp.EmployeeID == id
                                  select new EmployeeViewBO()
                                  {
                                      AddressID = (long)emp.AdderessID,
                                      //BranchID = branchID,
                                      BranchList = branchID,    //Gets the list of Branch for a particular Employee
                                      DateOfJoin = emp.DateOfJoin,
                                      DateOfLeave = emp.DateOfLeave,
                                      DoB = emp.DoB,
                                      Email = emp.Email,
                                      EmployeeID = emp.EmployeeID,
                                      EmployeeTypeID = emp.EmployeeTypeID,
                                      EthnicGroupID = emp.EthnicGroupID,
                                      FirstName = emp.FirstName,
                                      ImmigrationStatusID = emp.ImmigrationStatusID,
                                      TimeSheetFrequencyID = emp.TimeSheetFrequencyID,
                                      KIN_MobileNo = emp.KIN_MobileNo,
                                      KIN_TelephoneNo = emp.KIN_TelephoneNo,
                                      LastName = emp.LastName,
                                      MaritalStatusID = emp.MaritalStatusID,
                                      MiddleName = emp.MiddleName,
                                      MobileNo = emp.MobileNo,
                                      NationalityID = emp.NationalityID,
                                      NextOfKIN_Name = emp.NextOfKIN_Name,
                                      NINo = emp.NINo,
                                      PhotoFileType = emp.PhotoFileType,
                                      ProfilePhoto = emp.ProfilePhoto,
                                      Sex = emp.Sex,
                                      TelephoneNo = emp.TelephoneNo,
                                      TitleID = emp.TitleID,
                                      AddressViewModel = new AddressViewModel()
                                      {
                                          AddressID = emp.AdderessID,
                                          BuildingName = emp.Common_Address.AddressLine1,
                                          StreetName = emp.Common_Address.AddressLine2,
                                          Locality = emp.Common_Address.AddressLine3,
                                          Town = emp.Common_Address.Town,
                                          County = emp.Common_Address.County,
                                          Postcode = emp.Common_Address.PostCode,
                                          CountryID = emp.Common_Address.CountryID.Value
                                      } //Gets PAF details from AddressViewModel                                      
                                  }).FirstOrDefault();
            if (employeerecord == null)     //Employee record is not found
            {
                emsg = "Resource May Be Deleted By Some One Else... Please Try Again!...";
                return null;
            }
            return employeerecord;
        }

        /// <summary>
        /// Updates Employee Detail
        /// </summary>
        /// <param name="employeeBO"></param>
        /// <param name="emsg"></param>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        public bool UpdateEmployee(EmployeeViewBO employeeBO, out string emsg, HttpPostedFileBase UploadFile)
        {
            emsg = "";
            try
            {
                var isExist = (from emp in db.Resource_Employee
                               where emp.NINo.Equals(employeeBO.NINo) && emp.EmployeeID != employeeBO.EmployeeID
                               select emp).DefaultIfEmpty().Count(x => x != null) > 0;    //Checks Existing Employee

                var isExistBranchName = (from branch in db.Resource_BranchDetails
                                         where branch.ResourceID == employeeBO.EmployeeID
                                         select branch).DefaultIfEmpty().Count(x => x != null) > 0; //Gets record for Existing BranchName for a particular Employee

                if (isExistBranchName)      //A particular Employee has Branch record
                {
                    if (employeeBO.BranchList.Count() > 0)      //When the Branch List is not empty
                    {
                        var empBranchRecord = (from empBranch in db.Resource_BranchDetails
                                               where empBranch.ResourceID == employeeBO.EmployeeID
                                               select empBranch).ToList();    //Gets Branch List for selected Employee 

                        if (empBranchRecord != null)    //When the selected Employee has atleast one Branch
                        {
                            empBranchRecord.ForEach(branch => { db.Resource_BranchDetails.Remove(branch); });    //Removes List of Branch Details from "Resource_BranchDetails" table for a particular Employee
                            db.SaveChanges();
                        }

                        foreach (var B_ID in employeeBO.BranchList)     //Gets selected Branch as a list and saves Branch Details to "Resource_BranchDetails" table 
                        {
                            var branch = new Resource_BranchDetails();
                            branch.BranchID = B_ID;
                            branch.ResourceID = employeeBO.EmployeeID;
                            db.Resource_BranchDetails.Add(branch);
                            db.SaveChanges();
                        }
                    }
                }
                if (!isExistBranchName)         //When the Branch are not allocate
                {
                    foreach (var BranchID in employeeBO.BranchList)     //Gets selected Branch as a list and saves Branch Details to "Resource_BranchDetails" table 
                    {
                        var branch = new Resource_BranchDetails();
                        branch.BranchID = BranchID;
                        branch.ResourceID = employeeBO.EmployeeID;
                        db.Resource_BranchDetails.Add(branch);
                        db.SaveChanges();
                    }
                }
                if (isExist)  //Entered "NIC Number" is already exist
                {
                    emsg = "This NIC Number May Used By Another Resource!..Please Provide A Correct NIC Number...";
                    return false;
                }

                Resource_Employee updateEmployee = db.Resource_Employee.SingleOrDefault(employee => employee.EmployeeID == employeeBO.EmployeeID);  //Gets Employee Details for a particular Employee
                if (updateEmployee != null)     //When the selected employee is available
                {
                    //Updates detail for selected Employee in "Resource_Employee" table
                    updateEmployee.AdderessID = (int)addressBL.CheckAddress(employeeBO.AddressViewModel);  //Assigns an "AddressID" for a particular Employee
                    if (employeeBO.AddressViewModel.AddressID == 0)     //When the AddressID is Zero
                    {
                        emsg = "AddressID Is Not Saved!.... ";
                        return false;
                    }
                    else if (employeeBO.AddressViewModel.CountryID != 1)     //When the selected Country is not "United Kingdom"
                    {
                        emsg = "PAF Is Only Available For 'United Kingdom', So Please Select A Country As 'United Kingdom' !... ";
                        return false;
                    }
                    updateEmployee.DateOfJoin = employeeBO.DateOfJoin;
                    updateEmployee.DateOfLeave = employeeBO.DateOfLeave;
                    if (employeeBO.DateOfJoin != null && employeeBO.DateOfLeave != null)  //Validates "DateOfJoin" and "DateOfLeave" Fields
                    {
                        if (employeeBO.DateOfLeave <= employeeBO.DateOfJoin)
                        {
                            emsg = "DateOfLeft Should Be Greater Than DateOfJoin....";
                            return false;
                        }
                    }
                    updateEmployee.DoB = employeeBO.DoB;
                    if (updateEmployee.DoB != null && updateEmployee.DateOfJoin != null) //Validates "DateOfBirth"
                    {
                        if (updateEmployee.DateOfJoin.Value.Year < updateEmployee.DoB.Value.Year + 15)
                        {
                            emsg = "DateOfJoin Should Be Greater Than BirthDate Year + 15 Years...Please Confirm Your BirthDate...";
                            return false;
                        }
                    }
                    updateEmployee.Email = employeeBO.Email;
                    updateEmployee.EmployeeID = employeeBO.EmployeeID;
                    updateEmployee.EmployeeTypeID = employeeBO.EmployeeTypeID;
                    updateEmployee.EthnicGroupID = employeeBO.EthnicGroupID;
                    if (employeeBO.FirstName != null && employeeBO.FirstName.Length > 50)  //Validates FirstName
                    {
                        emsg = "FirstName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    else if (employeeBO.MiddleName != null && employeeBO.MiddleName.Length > 50)    //Validates MiddleName
                    {
                        emsg = "MiddleName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    else if (employeeBO.LastName != null && employeeBO.LastName.Length > 50)  //Validates LastName
                    {
                        emsg = "LastName Length Should Be Less Than Or Equal To 50 Characters ";
                        return false;
                    }
                    updateEmployee.FirstName = employeeBO.FirstName;
                    updateEmployee.ImmigrationStatusID = employeeBO.ImmigrationStatusID;
                    updateEmployee.TimeSheetFrequencyID = employeeBO.TimeSheetFrequencyID;
                    updateEmployee.KIN_MobileNo = employeeBO.KIN_MobileNo;
                    updateEmployee.KIN_TelephoneNo = employeeBO.KIN_TelephoneNo;
                    updateEmployee.LastName = employeeBO.LastName;
                    updateEmployee.MaritalStatusID = employeeBO.MaritalStatusID;
                    updateEmployee.MiddleName = employeeBO.MiddleName;
                    updateEmployee.MobileNo = employeeBO.MobileNo;
                    updateEmployee.NationalityID = employeeBO.NationalityID;
                    updateEmployee.NextOfKIN_Name = employeeBO.NextOfKIN_Name;
                    updateEmployee.NINo = employeeBO.NINo;

                    if (UploadFile == null)     //When the image is not browsed for upload
                    {
                        updateEmployee.PhotoFileType = (from emp in db.Resource_Employee
                                                        where emp.EmployeeID == employeeBO.EmployeeID
                                                        select emp.PhotoFileType).SingleOrDefault();    //Select Photo File Type
                        updateEmployee.ProfilePhoto = (from emp in db.Resource_Employee
                                                       where emp.EmployeeID == employeeBO.EmployeeID
                                                       select emp.ProfilePhoto).SingleOrDefault();      //Select Profile Photo
                    }
                    else   //When the image is browsed for upload
                    {

                        if (UploadFile.ContentType == "image/jpeg" || UploadFile.ContentType == "image/png" || UploadFile.ContentType == "image/gif")   //Uploaded image should be "gif","jpeg" and "png"
                        {
                            //if(GetPhotoFileTypeVal(employeeBO) == null && GetImageFromDataBase(employeeBO.EmployeeID, out emsg) == null)
                            //{
                            //    updateEmployee.PhotoFileType = UploadFile.ContentType;
                            //    updateEmployee.ProfilePhoto = ConvertToBytes(UploadFile);
                            //}
                            updateEmployee.PhotoFileType = UploadFile.ContentType;
                            updateEmployee.ProfilePhoto = ConvertToBytes(UploadFile);
                        }
                        else
                        {
                            emsg = "ImageType Should Be 'jpeg' Or 'png' Or 'gif' ";
                            return false;
                        }
                    }
                    updateEmployee.Sex = employeeBO.Sex;
                    updateEmployee.TelephoneNo = employeeBO.TelephoneNo;
                    updateEmployee.TitleID = employeeBO.TitleID;
                    db.SaveChanges();   //Updates Employee Details for a particular Employee

                    emsg = "Personal Details Are Successfully Updated For " + employeeBO.FirstName;
                }
                else    //Selected Employee record is not exist
                {
                    emsg = "Resource Is Not Exist!.. It May Be Deleted By Some One Else... Please Cancel The Update!...";
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets Images from Database
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int id, out string emsg)
        {
            emsg = "";
            var q = from temp in db.Resource_Employee where temp.EmployeeID == id select temp.ProfilePhoto; //Gets the ProfilePhoto as binarycode
            byte[] image = q.First();
            if (image == null)
            {
                emsg = "Profile Is Not Uploaded For This Resource...";
            }
            return image;
        }

        /// <summary>
        /// Checks entered NIC Number is already Exist or Not
        /// </summary>
        /// <param name="nic:NICNumber"></param>
        /// <param name="empID"></param>
        /// <returns>emp</returns>
        public bool CheckNICNo(string nic, string empID)
        {
            var emp = db.Resource_Employee.Where(x => x.NINo.ToLower() == nic && x.EmployeeID.ToString() != empID).FirstOrDefault() != null;    //Checks the entered "NIC Number" is exist or not
            return emp;
        }

        /// <summary>
        /// Checks Birth Year less than '2005'
        /// </summary>
        /// <param name="DoB:DateOfBirth"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        public bool CheckBirthYear(DateTime DoB)
        {
            if (DoB.Year >= (DateTime.Now.Year - 15))       //Validates "DateOfBirth" field
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get "PhotoFileType" value for a particular Employee
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string GetPhotoFileTypeVal(EmployeeViewBO employeeViewBO)
        {
            employeeViewBO.PhotoFileType = (from emp in db.Resource_Employee
                                            where emp.EmployeeID == employeeViewBO.EmployeeID
                                            select emp.PhotoFileType).SingleOrDefault();    //Select Photo File Type

            return employeeViewBO.PhotoFileType;

        }

        #endregion Employee

        #region EmployeePayment

        /// <summary>
        /// Gets PaymentType List from "Common_PaymentType" Table
        /// </summary>
        /// <returns>paymentTypeList</returns>
        public IEnumerable<Common_PaymentType> GetPaymentType()
        {
            var paymentTypeList = (from paymentType in db.Common_PaymentType
                                   select paymentType).ToList();
            return paymentTypeList;
        }

        /// <summary>
        /// Gets PaymentList for a particular Employee
        /// </summary>
        /// <param name="employeeViewBO"></param>
        /// <returns>empPaymentList</returns>
        public IEnumerable<EmployeePaymentViewModels> GetEmployeePaymentList(EmployeeViewBO employeeViewBO)
        {
            IEnumerable<EmployeePaymentViewModels> empPaymentList = (from emp in db.Resource_EmployeePayment/*.OrderByDescending(empPayment => empPayment.EmployeePayID)*/
                                                                     where emp.EmployeeID == employeeViewBO.EmployeeID
                                                                     select new EmployeePaymentViewModels
                                                                     {
                                                                         EmployeeID = (int)emp.EmployeeID,
                                                                         EmployeeName = emp.Resource_Employee.FirstName + "  " + emp.Resource_Employee.LastName,
                                                                         EmployeePayID = emp.EmployeePayID,
                                                                         EndDate = (DateTime)emp.EndDate,
                                                                         PaymentAmount = emp.PaymentAmount,
                                                                         PaymentTypeID = emp.PaymentTypeID,
                                                                         PaymentTypeName = emp.Common_PaymentType.PaymentName,
                                                                         StartDate = emp.StartDate,
                                                                         TimesheetAuthorizedDateList = emp.Resource_Employee.Resource_TimeSheet.Where(e => e.ResourceID == emp.EmployeeID && e.TimeSheetAuthorizationDate >= emp.StartDate).Select(authorizedDate => authorizedDate.TimeSheetAuthorizationDate).ToList()
                                                                     });
            return empPaymentList.OrderByDescending(a => a.StartDate).ToList();     //returns "Payment List"
        }

        /// <summary>
        /// Saves Payment for a particular Employee in "Resource_EmployeePayment" Table
        /// </summary>
        /// <param name="employeePaymentBO"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool SaveEmpPayment(EmployeePaymentBO employeePaymentBO, out string emsg)
        {
            emsg = "";
            try
            {
                //var paymentList = (from employee in db.Resource_EmployeePayment
                //                   where employee.EmployeeID == employeePaymentBO.EmployeeID
                //                   select employee).ToList().LastOrDefault();  //Gets last record from the Payment List for a specified Employee

                var empDateOfJoin = (from emp in db.Resource_Employee
                                     where emp.EmployeeID == employeePaymentBO.EmployeeID
                                     select emp.DateOfJoin).FirstOrDefault();  //Gets DateOfJoin for a specific Employee

                var empPayList = (from employee in db.Resource_EmployeePayment
                                  where employee.EmployeeID == employeePaymentBO.EmployeeID
                                  select employee).ToList();    //Gets the Payment List for a specified Employee

                if (empPayList.Count() == 0 || employeePaymentBO.StartDate > empPayList.LastOrDefault().StartDate) //Checks the StartDate for new Payment creation by checking with the last Payment StartDate
                {
                    //Saves Payment Record for particular Employee
                    var savePayment = new Resource_EmployeePayment();
                    savePayment.EmployeeID = employeePaymentBO.EmployeeID;
                    savePayment.EndDate = employeePaymentBO.EndDate;
                    savePayment.PaymentAmount = (double)employeePaymentBO.PaymentAmount;
                    savePayment.PaymentTypeID = employeePaymentBO.PaymentTypeID;

                    if (empDateOfJoin != null)      //Validates "Payment StartDate"
                    {
                        if (employeePaymentBO.StartDate != empDateOfJoin && empPayList.Count() == 0)
                        {
                            emsg = "The Payment Start Date Should Be The Same As The Date Of Joined On " + " ' " + empDateOfJoin.Value.Date.Day.ToString() + "-" + empDateOfJoin.Value.Date.ToString("MMM") + "-" + empDateOfJoin.Value.Date.Year.ToString() + " '";
                            return false;
                        }
                    }
                    savePayment.StartDate = employeePaymentBO.StartDate;
                    db.Resource_EmployeePayment.Add(savePayment);   //Adds Employee Payment to "Resource_EmployeePayment" table
                    db.SaveChanges();

                    if (empPayList.Count() != 0)    //When Employee has atleast one Payment
                    {
                        empPayList.LastOrDefault().EndDate = employeePaymentBO.StartDate;  //Updates the "EndDate" for the last Payment
                        db.SaveChanges();
                    }
                }
                else if (empPayList.Count() != 0 && employeePaymentBO.StartDate <= empPayList.LastOrDefault().StartDate)    //Checks with the last Payment StartDate
                {
                    emsg = "Payment StartDate Should Be Greater Than Last Payment StartDate " + empPayList.LastOrDefault().StartDate.Date.Day.ToString() + "-" + empPayList.LastOrDefault().StartDate.Date.ToString("MMM") + "-" + empPayList.LastOrDefault().StartDate.Date.Year.ToString();
                    return false;
                }
                emsg = "Payment Is Successfully Added";
                return true;
            }
            catch (Exception ex)
            {
                emsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Edits EmployeePayment
        /// </summary>
        /// <param name="id:EmployeePaymentID"></param>
        /// <param name="emsg"></param>
        /// <returns>employeePaymentRecord</returns>
        public EmployeePaymentBO EditEmployeePayment(int id, out string emsg)
        {
            emsg = "";
            //Gets the Payment record for a  particular Payment
            var employeePaymentRecord = (from emp in db.Resource_EmployeePayment
                                         where emp.EmployeePayID == id
                                         select new EmployeePaymentBO()
                                         {
                                             EmployeeID = (int)emp.EmployeeID,
                                             EmployeePayID = emp.EmployeePayID,
                                             EndDate = (DateTime)emp.EndDate,
                                             PaymentAmount = emp.PaymentAmount,
                                             PaymentTypeID = emp.PaymentTypeID,
                                             StartDate = emp.StartDate,
                                         }).FirstOrDefault();   //Gets all the details for a particular Employee Payment record
            if (employeePaymentRecord == null)  //Employee Payment is not exist
            {
                emsg = "Selected Record May Be Deleted By Some One Else... Please Try Again!";
                return null;
            }
            return employeePaymentRecord;
        }

        /// <summary>
        /// Updates EmployeePayment in "Resource_EmployeePayment" Table
        /// </summary>
        /// <param name="employeePaymentBO"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool UpdatePayment(EmployeePaymentBO employeePaymentBO, out string emsg)
        {
            emsg = "";
            EmployeeViewBO employeeViewBO = new EmployeeViewBO();
            var empPayment = db.Resource_EmployeePayment.SingleOrDefault(payment => payment.EmployeePayID == employeePaymentBO.EmployeePayID);  //Gets details for a particular EmployeePaymentID

            var lastBeforePayment = (from employee in db.Resource_EmployeePayment
                                     where employee.EmployeeID == employeePaymentBO.EmployeeID
                                     select employee).ToList().OrderByDescending(empPay => empPay.EmployeePayID).Skip(1).FirstOrDefault();  //Gets last before record from the Payment List for a specified Employee            

            try
            {
                if (empPayment != null) //When the Employee has atleast one Payment
                {
                    if (lastBeforePayment != null)  //Employee has more than one Payment record
                    {
                        if (employeePaymentBO.StartDate <= lastBeforePayment.StartDate)     //Checks the StartDate is greater than the last before Payment StartDate
                        {
                            emsg = "Payment StartDate Should Be Greater Than Last Payment StartDate " + lastBeforePayment.StartDate.Date.Day.ToString() + "-" + lastBeforePayment.StartDate.Date.ToString("MMM") + "-" + lastBeforePayment.StartDate.Date.Year.ToString();
                            return false;
                        }
                        lastBeforePayment.EndDate = employeePaymentBO.StartDate;  //Updates the "EndDate" for the last before Payment
                        db.SaveChanges();
                    }
                    //Updates EmployeePayment for a particular Employee
                    empPayment.EmployeeID = employeePaymentBO.EmployeeID;
                    empPayment.EndDate = employeePaymentBO.EndDate;
                    empPayment.PaymentAmount = (double)employeePaymentBO.PaymentAmount;
                    empPayment.PaymentTypeID = employeePaymentBO.PaymentTypeID;

                    var empDateOfJoin = (from emp in db.Resource_Employee
                                         where emp.EmployeeID == empPayment.EmployeeID
                                         select emp.DateOfJoin).FirstOrDefault();  //Gets DateOfJoin for a specific Employee

                    if (empDateOfJoin != null)  //When the Employee's "DateOfJoin" field has a value
                    {
                        if (employeePaymentBO.StartDate != empDateOfJoin && lastBeforePayment == null)  //For the first EmployeePayment creation
                        {
                            emsg = "The Payment Start Date Should Be The Same As The Date Of Joined On " + " ' " + empDateOfJoin.Value.Date.Day.ToString() + "-" + empDateOfJoin.Value.Date.ToString("MMM") + "-" + empDateOfJoin.Value.Date.Year.ToString() + " '";
                            return false;
                        }
                        else if (employeePaymentBO.StartDate <= empDateOfJoin && lastBeforePayment != null)  //For the every last Payment creation
                        {
                            emsg = "The Payment Start Date Should Be The Same As The Date Of Joined On " + " ' " + empDateOfJoin.Value.Date.Day.ToString() + "-" + empDateOfJoin.Value.Date.ToString("MMM") + "-" + empDateOfJoin.Value.Date.Year.ToString() + " '";
                            return false;
                        }
                    }
                    empPayment.StartDate = employeePaymentBO.StartDate;
                    db.SaveChanges();   //Updates a particular Payment record
                    emsg = "Payment Is Successfully Updated!...";
                    return true;
                }
                else    //Selected Employee Payment record is not exist
                {
                    emsg = "Sorry!...No Records Found For This ID..Please Cancel The Update..";
                    return false;
                }
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Deletes EmployeePayment from "Resource_EmployeePayment" Table
        /// </summary>
        /// <param name="id:EmployeePaymentID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool DeleteEmployeePayment(int id, out string emsg)
        {
            emsg = "";
            var record = (from emp in db.Resource_EmployeePayment
                          where emp.EmployeePayID == id
                          select emp).SingleOrDefault();    //Select a EmployeePayment record for a particular Employee

            if (record != null)     //Employee Payment is exist
            {
                var lastBeforePayment = (from employee in db.Resource_EmployeePayment
                                         where employee.EmployeeID == record.EmployeeID
                                         select employee).ToList().OrderByDescending(empPay => empPay.EmployeePayID).Skip(1).FirstOrDefault();  //Gets last before record from the Payment List for a specified Employee
                if (lastBeforePayment != null)  //Selected Employee has more then one Payment record
                {
                    lastBeforePayment.EndDate = null;  //Deletes the "EndDate" for the last before payment
                    db.SaveChanges();
                }
                db.Resource_EmployeePayment.Remove(record); //Delete particular Employee Payment record
                db.SaveChanges();
            }
            else   //Seleted Employee Payment record is not exist
            {
                emsg = "Unable To Find This Resource Payment Record!...Please Try Again..";
                return false;
            }
            emsg = "Selected Payment Is Successfully Removed";
            return true;
        }

        #endregion EmployeePayment

        #region EmployeeWorkingPattern

        /// <summary>
        /// Gets a list of Working Pattern for a particular Employee
        /// </summary>
        /// <param name="employeeViewBO"></param>
        /// <returns></returns>
        public IEnumerable<Employee_WorkingPatternDetailsBO> GetEmployeeWorkingPatternList(EmployeeViewBO employeeViewBO)
        {
            IEnumerable<Employee_WorkingPatternDetailsBO> empWorkingPatternList = (from emp in db.Resouce_WorkingPatternDetails/*.OrderByDescending(empWorkingPattern => empWorkingPattern.WorkingPatternID)*/
                                                                                   where emp.ResourceID == employeeViewBO.EmployeeID
                                                                                   select new Employee_WorkingPatternDetailsBO
                                                                                   {
                                                                                       EmployeeID = (int)emp.ResourceID,
                                                                                       WorkingPatternID = emp.WorkingPatternID,
                                                                                       ChangeDate = (DateTime)emp.ChangeDate,
                                                                                       Sunday = emp.Sunday,
                                                                                       Monday = emp.Monday,
                                                                                       Tuesday = emp.Tuesday,
                                                                                       Wednesday = emp.Wednesday,
                                                                                       Thursday = emp.Thursday,
                                                                                       Friday = emp.Friday,
                                                                                       Saturday = emp.Saturday,
                                                                                       NoOfWorkingHours = emp.NoOfWorkingHours,
                                                                                   });
            return empWorkingPatternList.OrderByDescending(a => a.WorkingPatternID).ToList();     //returns "Working Pattern List"
        }

        /// <summary>
        /// Saves Employee Working Pattern
        /// </summary>
        /// <param name="employeeWorkingPatternBO"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool SaveEmpWorkingPattern(Employee_WorkingPatternDetailsBO employeeWorkingPatternBO, out string emsg)
        {
            emsg = "";
            try
            {
                var empDateOfJoin = (from emp in db.Resource_Employee
                                     where emp.EmployeeID == employeeWorkingPatternBO.EmployeeID
                                     select emp.DateOfJoin).FirstOrDefault();  //Gets DateOfJoin for a specific Employee

                //Saves Working Pattern Record for particular Employee
                var saveWorkingPattern = new Resouce_WorkingPatternDetails();
                saveWorkingPattern.ResourceID = employeeWorkingPatternBO.EmployeeID;
                saveWorkingPattern.Monday = employeeWorkingPatternBO.Monday;
                saveWorkingPattern.Tuesday = employeeWorkingPatternBO.Tuesday;
                saveWorkingPattern.Wednesday = employeeWorkingPatternBO.Wednesday;
                saveWorkingPattern.Thursday = employeeWorkingPatternBO.Thursday;
                saveWorkingPattern.Friday = employeeWorkingPatternBO.Friday;
                saveWorkingPattern.Saturday = employeeWorkingPatternBO.Saturday;
                saveWorkingPattern.Sunday = employeeWorkingPatternBO.Sunday;
                saveWorkingPattern.NoOfWorkingHours = (double)employeeWorkingPatternBO.NoOfWorkingHours;
                saveWorkingPattern.ChangeDate = employeeWorkingPatternBO.ChangeDate;

                if (empDateOfJoin != null && employeeWorkingPatternBO.ChangeDate != null)      //Validates "Working Pattern ChangeDate"
                {
                    if (employeeWorkingPatternBO.ChangeDate < empDateOfJoin)
                    {
                        emsg = "The Working Pattern ChangeDate Should Be Greater Than The Date Of Joined On " + " ' " + empDateOfJoin.Value.Date.Day + "-" + empDateOfJoin.Value.Date.ToString("MMM") + "-" + empDateOfJoin.Value.Year + " '";
                        return false;
                    }
                }
                if (saveWorkingPattern.Monday == false && saveWorkingPattern.Tuesday == false && saveWorkingPattern.Wednesday == false && saveWorkingPattern.Thursday == false && saveWorkingPattern.Friday == false && saveWorkingPattern.Saturday == false && saveWorkingPattern.Sunday == false)     //When the all working patterns are false
                {
                    emsg = "Please Select Atleast A Working Pattern...";
                    return false;
                }
                if (saveWorkingPattern.NoOfWorkingHours < 1 || saveWorkingPattern.NoOfWorkingHours > 12)    //When the "NoOfWorkingHours" is less than 1 or greater than 12
                {
                    emsg = "The Working Hours Must Be Between 1 And 12.";
                    return false;
                }

                db.Resouce_WorkingPatternDetails.Add(saveWorkingPattern);   //Adds Employee Working Pattern to "Resouce_WorkingPatternDetails" table
                db.SaveChanges();

                emsg = "Working Pattern Is Successfully Added";
                return true;
            }
            catch (Exception ex)
            {
                emsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets all the Working Pattern Details for a particular WorkingPatternID for a particular Employee
        /// </summary>
        /// <param name="id:WorkingPatternID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public Employee_WorkingPatternDetailsBO EditEmployeeWorkingPattern(int id, out string emsg)
        {
            emsg = "";
            //Gets the Working Pattern record for a  particular Payment
            var employeeWorkingPatternRecord = (from emp in db.Resouce_WorkingPatternDetails
                                                where emp.WorkingPatternID == id
                                                select new Employee_WorkingPatternDetailsBO()
                                                {
                                                    EmployeeID = emp.ResourceID,
                                                    WorkingPatternID = emp.WorkingPatternID,
                                                    ChangeDate = emp.ChangeDate,
                                                    Friday = emp.Friday,
                                                    Monday = emp.Monday,
                                                    NoOfWorkingHours = emp.NoOfWorkingHours,
                                                    Saturday = emp.Saturday,
                                                    Sunday = emp.Sunday,
                                                    Thursday = emp.Thursday,
                                                    Tuesday = emp.Tuesday,
                                                    Wednesday = emp.Wednesday
                                                }).FirstOrDefault();   //Gets all the details for a particular Employee WorkingPattern record
            if (employeeWorkingPatternRecord == null)  //Employee WorkingPattern is not exist
            {
                emsg = "Selected Record May Be Deleted By Some One Else... Please Try Again!";
                return null;
            }
            return employeeWorkingPatternRecord;
        }

        /// <summary>
        /// Updates Employee Working Pattern
        /// </summary>
        /// <param name="empWorkingPatternBO"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool UpdateEmpWorkingPattern(Employee_WorkingPatternDetailsBO empWorkingPatternBO, out string emsg)
        {
            emsg = "";
            var employeeViewBO = new EmployeeViewBO();
            var empWorkingPattern = db.Resouce_WorkingPatternDetails.SingleOrDefault(pattern => pattern.WorkingPatternID == empWorkingPatternBO.WorkingPatternID);  //Gets details for a particular WorkingPatternID for a particular Employee
            try
            {
                if (empWorkingPattern != null) //When the Employee has atleast one WorkingPattern
                {
                    //Updates EmployeeWorking Pattern for a particular Employee
                    empWorkingPattern.ResourceID = empWorkingPatternBO.EmployeeID;
                    empWorkingPattern.Monday = empWorkingPatternBO.Monday;
                    empWorkingPattern.Tuesday = empWorkingPatternBO.Tuesday;
                    empWorkingPattern.Wednesday = empWorkingPatternBO.Wednesday;
                    empWorkingPattern.Thursday = empWorkingPatternBO.Thursday;
                    empWorkingPattern.Friday = empWorkingPatternBO.Friday;
                    empWorkingPattern.Saturday = empWorkingPatternBO.Saturday;
                    empWorkingPattern.Sunday = empWorkingPatternBO.Sunday;
                    empWorkingPattern.NoOfWorkingHours = (double)empWorkingPatternBO.NoOfWorkingHours;

                    if (empWorkingPattern.Monday == false && empWorkingPattern.Tuesday == false && empWorkingPattern.Wednesday == false && empWorkingPattern.Thursday == false && empWorkingPattern.Friday == false && empWorkingPattern.Saturday == false && empWorkingPattern.Sunday == false)    //When the all working patterns are false
                    {
                        emsg = "Please Select Atleast A Working Pattern...";
                        return false;
                    }
                    if (empWorkingPattern.NoOfWorkingHours < 1 || empWorkingPattern.NoOfWorkingHours > 12)  //When the "NoOfWorkingHours" is less than 1 or greater than 12
                    {
                        emsg = "The Working Hours Must Be Between 1 And 12.";
                        return false;
                    }
                    var empDateOfJoin = (from emp in db.Resource_Employee
                                         where emp.EmployeeID == empWorkingPattern.ResourceID
                                         select emp.DateOfJoin).FirstOrDefault();  //Gets DateOfJoin for a specific Employee

                    if (empDateOfJoin != null && empWorkingPatternBO.ChangeDate != null)  //When the Employee's "DateOfJoin" field has a value
                    {
                        if (empWorkingPatternBO.ChangeDate < empDateOfJoin)  //Validates the 'ChangeDate'
                        {
                            emsg = "The Working Pattern ChangeDate Should Be Greater Than The Date Of Joined On " + " ' " + empDateOfJoin.Value.Date.Day + "-" + empDateOfJoin.Value.Date.ToString("MMM") + "-" + empDateOfJoin.Value.Year + " '";
                            return false;
                        }
                    }

                    empWorkingPattern.ChangeDate = empWorkingPatternBO.ChangeDate;
                    db.SaveChanges();   //Updates a particular Working Pattern record
                    emsg = "Working Pattern Is Successfully Updated!...";
                    return true;
                }
                else    //Selected Employee WorkingPattern is not exist
                {
                    emsg = "Sorry!...No Records Found For This ID..Please Cancel The Update..";
                    return false;
                }
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Deletes a particular Working Pattern for a particular Employee
        /// </summary>
        /// <param name="id:WorkingPatternID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool DeleteEmployeeWorkingPattern(int id, out string emsg)
        {
            emsg = "";
            var record = (from emp in db.Resouce_WorkingPatternDetails
                          where emp.WorkingPatternID == id
                          select emp).SingleOrDefault();    //Select a EmployeeWorkingPattern record for a particular Employee

            if (record != null)     //When the Employee Working Pattern is exist
            {
                db.Resouce_WorkingPatternDetails.Remove(record); //Deletes particular Employee Working Pattern
                db.SaveChanges();
            }
            else   //When the seleted Employee WorkingPattern is not exist
            {
                emsg = "Unable To Find This Resource Working pattern Record!...Please Try Again..";
                return false;
            }
            emsg = "Selected Working Pattern Is Successfully Removed";
            return true;
        }
        #endregion EmployeeWorkingPattern

        #region EmployeeExperience

        /// <summary>
        /// Gets Designation List from "Common_Designation" table
        /// </summary>
        /// <returns></returns>
        public List<Common_Designation> GetDesignation()
        {
            var designationList = (from designation in db.Common_Designation
                                   select designation).ToList();
            return designationList;
        }

        /// <summary>
        /// Gets CompanyName List from "Common_Company" table
        /// </summary>
        /// <returns></returns>
        public List<Common_Company> GetCompany()
        {
            var companyList = (from company in db.Common_Company
                               select company).ToList();
            return companyList;
        }

        /// <summary>
        /// Gets Experience List for a particular Employee
        /// </summary>
        /// <param name="empExperienceViewModel"></param>
        /// <returns></returns>
        public IEnumerable<EmployeeExperienceViewModels> GetExperienceList(EmployeeExperienceViewModels empExperienceViewModel)
        {
            var experienceList = (from experience in db.Resource_ExperienceDetails
                                  where experience.ResourceID == empExperienceViewModel.ResourceID
                                  select new EmployeeExperienceViewModels()
                                  {
                                      CompanyID = experience.CompanyID,
                                      Company = experience.Common_Company.CompanyName,
                                      Description = experience.Description,
                                      ExperienceID = experience.ExperienceID,
                                      ExternalDesignation = experience.ExternalDesignation,
                                      DesignationID = experience.DesignationID,
                                      DesignationName = experience.Common_Designation.DesignationName,
                                      Duration = experience.StartDate.Month + "/" + experience.StartDate.Year + " - " + experience.EndDate.Value.Month + "/" + experience.EndDate.Value.Year
                                  });
            return experienceList.ToList(); //returns experience list for a particular Employee
        }

        /// <summary>
        /// Saves Experience for a particular Employee
        /// </summary>
        /// <param name="empExperience"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool SaveExperience(EmployeeExperienceViewModels empExperience, out string emsg)
        {
            emsg = "";
            try
            {
                var existingExperience = (from experience in db.Resource_ExperienceDetails
                                          where experience.CompanyID == empExperience.CompanyID
                                          && experience.DesignationID == empExperience.DesignationID
                                          && experience.ResourceID == empExperience.ResourceID
                                          select experience).ToList();  //Check the existing Experience

                if (existingExperience.Count() >= 1)    // When the existing Experience count is greater then one
                {
                    emsg = "Experience Is Already Exist For The Same Designation And Company!";
                    return false;
                }

                empExperience.StartDate = new DateTime((int)empExperience.StartYear, (int)empExperience.StartMonth, 1);     //Defaultly put 1 for StartDate 
                empExperience.EndDate = new DateTime((int)empExperience.EndYear, (int)empExperience.EndMonth, 1);       //Defaultly put 1 for EndDate

                var employeeExperience = new Resource_ExperienceDetails();
                employeeExperience.ResourceID = empExperience.ResourceID;
                employeeExperience.ExternalDesignation = empExperience.ExternalDesignation;
                employeeExperience.DesignationID = empExperience.DesignationID;
                employeeExperience.StartDate = empExperience.StartDate;
                employeeExperience.EndDate = empExperience.EndDate;
                employeeExperience.Description = empExperience.Description;
                employeeExperience.CompanyID = empExperience.CompanyID;

                db.Resource_ExperienceDetails.Add(employeeExperience);  //Adds Employee experience
                db.SaveChanges();                   //Saves Employee experience                    
                emsg = "Experience Is Successfully Added";
                return true;
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets all details for a selected Experience record
        /// </summary>
        /// <param name="id:ExperienceID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public EmployeeExperienceViewModels EditExperience(int id, out string emsg)
        {
            emsg = "";
            var employeeExperienceRecord = (from empExperience in db.Resource_ExperienceDetails
                                            where empExperience.ExperienceID == id
                                            select new EmployeeExperienceViewModels()
                                            {
                                                ResourceID = empExperience.ResourceID,
                                                EndMonth = empExperience.EndDate.Value.Month,
                                                EndYear = empExperience.EndDate.Value.Year,
                                                StartMonth = empExperience.StartDate.Month,
                                                StartYear = empExperience.StartDate.Year,
                                                ExperienceID = empExperience.ExperienceID,
                                                DesignationID = empExperience.DesignationID,
                                                Description = empExperience.Description,
                                                CompanyID = empExperience.CompanyID,
                                                ExternalDesignation = empExperience.ExternalDesignation
                                            }).FirstOrDefault();   //Gets all the details for a particular Employee Payment record
            if (employeeExperienceRecord == null)  //Employee Payment is not exist
            {
                emsg = "Selected Record May Be Deleted By Some One Else... Please Try Again!";
                return null;
            }
            return employeeExperienceRecord;
        }

        /// <summary>
        /// Updates the Experience record for a particular Employee
        /// </summary>
        /// <param name="empExperience"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool UpdateExperience(EmployeeExperienceViewModels empExperience, out string emsg)
        {
            emsg = "";
            try
            {
                empExperience.StartDate = new DateTime((int)empExperience.StartYear, (int)empExperience.StartMonth, 1);     //Defaultly put 1 for StartDate 
                empExperience.EndDate = new DateTime((int)empExperience.EndYear, (int)empExperience.EndMonth, 1);       //Defaultly put 1 for EndDate

                var isExistingExperience = (from experience in db.Resource_ExperienceDetails
                                            where experience.DesignationID.Equals(empExperience.DesignationID) && experience.CompanyID.Equals(empExperience.CompanyID) && experience.ResourceID == empExperience.ResourceID && experience.ExperienceID != empExperience.ExperienceID
                                            select experience).Count(x => x != null) > 0;

                if (isExistingExperience)
                {
                    emsg = "Experience Is Already Exist For The Same Designation And Company!";
                    return false;
                }

                Resource_ExperienceDetails updateEmpExperience = db.Resource_ExperienceDetails.SingleOrDefault(experience => experience.ResourceID == empExperience.ResourceID && experience.ExperienceID == empExperience.ExperienceID);  //Gets Experience Details for a particular Employee
                if (updateEmpExperience != null)     //When the selected employee is available
                {
                    //Updates experience detail for selected Employee in "Resource_ExperienceDetails" table
                    updateEmpExperience.ResourceID = empExperience.ResourceID;
                    updateEmpExperience.ExternalDesignation = empExperience.ExternalDesignation;
                    updateEmpExperience.DesignationID = empExperience.DesignationID;
                    updateEmpExperience.StartDate = empExperience.StartDate;
                    updateEmpExperience.EndDate = empExperience.EndDate;
                    updateEmpExperience.Description = empExperience.Description;
                    updateEmpExperience.CompanyID = empExperience.CompanyID;

                    db.SaveChanges();                   //Updates Employee experience
                    emsg = "Experience Is Successfully Updated";
                    return true;
                }
                emsg = "Unable To Find The Experience Record, So Please Cancel The Update!";
                return false;
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete the Experience for a particular Employee
        /// </summary>
        /// <param name="id:ExperienceID"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool DeleteExperience(int id, out string emsg)
        {
            emsg = "";
            var experience = db.Resource_ExperienceDetails.SingleOrDefault(exp => exp.ExperienceID == id);
            try
            {
                if (experience != null)
                {
                    db.Resource_ExperienceDetails.Remove(experience);
                    db.SaveChanges();
                    emsg = "Selected Experience Is Successfully Removed!";
                    return true;
                }
                emsg = "Experience May Removed By Some One!";
                return false;
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        #endregion EmployeeExperience

        #region EmployeeSkills       

        /// <summary>
        /// Gets "Skill CategoryName","SkillName","CompanyName","SkillLevel" and the "ProjectStartDate" as a list
        /// </summary>
        /// <param name="id:EmployeeID"></param>
        /// <returns></returns>
        public List<SkillCategory> GetSkillCategoryList(int id)
        {
            var employeeSkillViewBO = new EmployeeSkillViewBO();
            employeeSkillViewBO.SkillCategoryList = new List<SkillCategory>();  //Gets the Catergory and the Skills as a list

            var resourceSkillGroup = (from empSkill in db.Resource_SkillDetails
                                      where empSkill.ResourceID == id
                                      select new SkillCategory()
                                      {
                                          CategoryID = (int)empSkill.SkillCategoryID,
                                          SkillLevel = (int)empSkill.SkillLevel,
                                          SkillID = (int)empSkill.SkillID,
                                          CompanyID = (int)empSkill.CompanyID,
                                          ProjectStartDate = empSkill.ProjectStartDate,
                                          EmpID = (int)empSkill.ResourceID
                                      });   //Gets the values from "Resource_SkillDetails" table for a particular Employee

            var skillGroup = db.Common_SkillGroup; //Gets all the SkillGroup records from "Common_SkillGroup" table

            foreach (var item in skillGroup)    //Gets SkillGroup record one by one
            {
                var skillCategory = new SkillCategory();  //Gets the "SkillCategory" Models values
                skillCategory.CategoryID = (int)item.SkillCategoryID;
                skillCategory.Category = item.Common_SkillCategory.CategoryName;
                skillCategory.SkillID = (int)item.SkillID;
                skillCategory.skillName = item.Common_Skill.SkillName;
                skillCategory.IsSelectedCategory = resourceSkillGroup.FirstOrDefault(skill => skill.SkillID == item.SkillID && skill.CategoryID == item.SkillCategoryID) != null ? true : false; //Once the record is available for particular Employee then sets the checkbox as "true" otherwise it is "false"

                var empSkill = db.Resource_SkillDetails.Where(skill => skill.SkillID == item.SkillID && skill.SkillCategoryID == item.SkillCategoryID && skill.ResourceID == id).FirstOrDefault();  //Gets the SkillList for a particular Employee

                if (skillCategory.IsSelectedCategory == true)    //When the Category is selected gets the "CompanyID","SkillLevel" and "ProjectStartDate"
                {
                    skillCategory.CompanyID = (int)db.Resource_SkillDetails.Where(skill => skill.SkillID == item.SkillID && skill.SkillCategoryID == item.SkillCategoryID && skill.ResourceID == id).Select(a => a.CompanyID).FirstOrDefault();
                    skillCategory.SkillLevel = (int)db.Resource_SkillDetails.Where(skill => skill.SkillID == item.SkillID && skill.SkillCategoryID == item.SkillCategoryID && skill.ResourceID == id).Select(a => a.SkillLevel).FirstOrDefault();
                    if (empSkill.ProjectStartDate != null)     //When the "ProjectStartDate" has a value
                    {
                        skillCategory.ProjectStartDate = empSkill.ProjectStartDate;
                    }
                }
                employeeSkillViewBO.SkillCategoryList.Add(skillCategory);   //Sets the values for "SkillCategory" Models once the Skill is updated
            }
            return employeeSkillViewBO.SkillCategoryList;       //Returns the overall SkillCategory list
        }

        /// <summary>
        /// Saves and Updates the Resource Skill Details to "Resource_SkillDetails" table
        /// </summary>
        /// <param name="employeeSkillViewBO"></param>
        /// <param name="emsg"></param>
        /// <returns></returns>
        public bool SaveSkills(EmployeeSkillViewBO employeeSkillViewBO, out string emsg)
        {
            emsg = "";
            try
            {
                var isExistEmployeeSkills = (from empSkillDetails in db.Resource_SkillDetails
                                             where empSkillDetails.ResourceID == employeeSkillViewBO.EmployeeID
                                             select empSkillDetails).ToList();      //Gets the all Resource Skill Details for a particular Employee from "Resource_SkillDetails" table
                if (isExistEmployeeSkills.Count() > 0)
                {
                    var employeeSkillDetails = db.Resource_SkillDetails.Where(empSkill => empSkill.ResourceID == employeeSkillViewBO.EmployeeID).Delete();  //Gets all the Resource Skill Details as a list for a particular Employee and deletes them
                    //db.SaveChanges();
                }

                var SelectedCategoryCount = employeeSkillViewBO.SkillCategoryList.Where(category => category.IsSelectedCategory == true).Count();    //Gets Counts for ticked Category
                if (SelectedCategoryCount == 0)     //When the any SkillCategory is not ticked once the user is click on "Save" button
                {
                    emsg = "You Are Not Add Any Skill For This Resource...Please Add Atleast A Skill...";
                    return false;
                }

                if (employeeSkillViewBO.SkillCategoryList.Count() > 0)      //When the SkillCategory List is not empty
                {
                    foreach (var category in employeeSkillViewBO.SkillCategoryList)     //Gets selected Category as a list
                    {
                        if (category.IsSelectedCategory == false)
                        {
                            if (category.CompanyID != 0 && category.SkillLevel != 0)    //When the CompanyID and SkillLevel are selected without tick the SkillCategory
                            {
                                emsg = "You Are Added A 'Company' And A 'SkillLevel' But You Are Forgot To Tick A 'SkillCategory'...So Please Tick '" + category.skillName + "' Skill";
                                return false;
                            }
                            else if (category.CompanyID != 0)           //When the CompanyID is selected without tick the SkillCategory
                            {
                                emsg = "You Are Added A 'Company' But You Are Forgot To Tick A 'SkillCategory'...So Please Tick '" + category.skillName + "' Skill";
                                return false;
                            }
                            else if (category.SkillLevel != 0)      //When the SkillLevel is selected without tick the SkillCategory
                            {
                                emsg = "You Are Added A 'SkillLevel' But You Are Forgot To Tick A 'SkillCategory'...So Please Tick '" + category.skillName + "' Skill";
                                return false;
                            }
                        }
                        else if (category.IsSelectedCategory == true)
                        {
                            //Saves Skills Details to "Resource_SkillDetails" table
                            var skill = new Resource_SkillDetails();
                            skill.ResourceID = employeeSkillViewBO.EmployeeID;
                            skill.CompanyID = category.CompanyID;
                            skill.ProjectStartDate = category.ProjectStartDate;
                            skill.SkillID = (int)category.SkillID;
                            skill.SkillCategoryID = category.CategoryID;
                            skill.SkillLevel = category.SkillLevel;
                            if (skill.CompanyID == 0 && skill.SkillLevel == 0)  //When the SkillCategory is ticked without select the CompanyID and SkillLevel 
                            {
                                emsg = "You Are Ticked '" + category.Category + "' SkillCategory With '" + category.skillName + "' Skill...But You Are Not Provided A 'Company' And As Well As A 'SkillLevel' For This Skill...So Please Give Them";
                                return false;
                            }
                            else if (skill.CompanyID == 0)      //When the SkillCategory is ticked without select the CompanyID
                            {
                                emsg = "You Are Ticked '" + category.Category + "' SkillCategory With '" + category.skillName + "' Skill...But You Are Not Provided A 'Company' For This Skill...So Please Give A Company";
                                return false;
                            }
                            else if (skill.SkillLevel == 0)     //When the SkillCategory is ticked without select the SkillLevel 
                            {
                                emsg = "You Are Ticked '" + category.Category + "' SkillCategory With '" + category.skillName + "' Skill...But You Are Not Provided A 'SkillLevel' For This Skill...So Please Give A SkillLevel";
                                return false;
                            }
                            db.Resource_SkillDetails.Add(skill);      //Adds Skill records to the "Resource_SkillDetails" table
                            db.SaveChanges();
                        }
                    }
                }
                emsg = "Skills Are Successfully Added";
                return true;
            }
            catch (Exception exc)
            {
                emsg = exc.Message;
                return false;
            }
        }

        #endregion EmployeeSkills

    }
}
