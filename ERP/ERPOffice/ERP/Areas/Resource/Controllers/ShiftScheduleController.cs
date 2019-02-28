//--------------------------------------------------------------------------------
// <copyright file="ShiftScheduleController.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

// <Author>
//      M Thinesh 
// </Author>
//--------------------------------------------------------------------------------
using ERP.MvcSecurity;
using ERP.Resource.BL;
using ERP.Resource.Models;
using ERP.Resource.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ERP.Areas.Resource.Controllers
{
    [Authorize]
    [AuthorizeUserAttribute(Permission = 9)]
    public class ShiftScheduleController : Controller
    {

        private ShiftScheduleBL shiftScheduleBL = new ShiftScheduleBL();
        // GET: Resource/ShiftSchedule
        [HttpGet]
        public ActionResult Index()
        {
            ShiftBO shiftBO = new ShiftBO();
            shiftBO.ShiftSearch = new ShiftSearchViewModel();
            shiftBO.ShiftSearch.StartDate = DateTime.Today;
            shiftBO.ShiftSearch.EndDate = DateTime.Today;
            shiftBO.ShiftScheduleList = shiftScheduleBL.GetShiftScheduleList(shiftBO.ShiftSearch);
            _ViewBagList();
            return View(shiftBO);
        }
        [HttpPost]
        public ActionResult Index(ShiftSearchViewModel shiftSearch, DateTime endDate, DateTime startDate, int empID, int branchID, int shiftPatternID)
        {
            ShiftBO shiftBO = new ShiftBO();
            string enddate = endDate.ToString(MvcApplication.DateFormat);
            shiftSearch.EndDate = Convert.ToDateTime(enddate);
            shiftSearch.StartDate = Convert.ToDateTime(startDate.ToString(MvcApplication.DateFormat));
            shiftSearch.EmployeeID = empID;
            shiftSearch.BranchID = branchID;
            shiftSearch.ShiftPatternNameID = shiftPatternID;
            shiftBO.ShiftSearch = shiftSearch;

            shiftBO.ShiftScheduleList = shiftScheduleBL.GetShiftScheduleList(shiftBO.ShiftSearch);
            _ViewBagList();
            return View(shiftBO);
        }
        /// <summary>
        /// ViewBag List
        /// </summary>
        public void _ViewBagList()
        {
            ViewBag.ShiftPatternList = (shiftScheduleBL.GetPatternList().Select(x => new SelectListItem()
            {
                Value = x.ShiftPatternID.ToString(),
                Text = x.Description,
                Selected = (bool)x.IsEnable
            }).OrderBy(i => i.Text).Where(c => c.Selected == true));
            ViewBag.EmployeeList = (shiftScheduleBL.GetEmployeeList().Select(x => new SelectListItem()
            {
                Value = x.EmployeeID.ToString(),
                Text = x.FirstName
            }).OrderBy(i => i.Text));
            ViewBag.BranchList = (shiftScheduleBL.GetBranchList().Select(x => new SelectListItem()
            {
                Value = x.BranchID.ToString(),
                Text = x.BranchName
            }).OrderBy(i => i.Text));
        }
        /// <summary>
        /// Add ShiftSchedule Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddShiftSchedule()
        {
            ShiftScheduleViewModel ShiftSchedule = new ShiftScheduleViewModel();
            // ShiftSchedule.ResourceList = shiftScheduleBL.GetResourceList();
            _ViewBagList();
            return PartialView("_AddShiftSchedule", ShiftSchedule);
        }
        /// <summary>
        /// Add ShiftSchedule Post
        /// </summary>
        /// <param name="shiftSchedule"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddShiftSchedule(ShiftScheduleViewModel shiftSchedule)
        {
            ShiftBO shiftBO = new ShiftBO();
            string msg = "";
            try
            {
                if (shiftSchedule.ShiftScheduleID != 0)
                {
                    ModelState.Remove("AssignType");
                }

                if (ModelState.IsValid)
                {

                    if (shiftSchedule.ShiftScheduleID != 0)
                    {
                        //var xx = ((shiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOffTime)) - (shiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOnTime))).TotalMinutes;
                        if ((shiftSchedule.ActualOnTime != null && shiftSchedule.ActualOffTime != null && ((shiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOffTime)) - (shiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOnTime))).TotalMinutes < 0)|| (shiftSchedule.ActualOnTime != null && shiftSchedule.ActualOffTime != null && ((shiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOffTime)) - (shiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOnTime))).TotalMinutes > 660) || (shiftSchedule.ActualOnDateTime< shiftSchedule.ShiftStartDate) || (shiftSchedule.ActualOnDateTime == null && shiftSchedule.ActualOnTime != null) || (shiftSchedule.ActualOnDateTime != null && shiftSchedule.ActualOnTime == null) || (shiftSchedule.ActualOffDateTime != null && shiftSchedule.ActualOffTime == null) || (shiftSchedule.ActualOffDateTime == null && shiftSchedule.ActualOffTime != null) || shiftSchedule.EditResourceID == 0)
                        {
                            if (shiftSchedule.EditResourceID == 0)
                            {
                                ModelState.AddModelError("ResourceList", "Please Select An Employee");
                            }
                            if (shiftSchedule.ActualOffDateTime != null && shiftSchedule.ActualOffTime == null)
                            {
                                ModelState.AddModelError("ActualOffTime", "Actual Off Time field is required.");
                            }
                            if (shiftSchedule.ActualOnDateTime != null && shiftSchedule.ActualOnTime == null)
                            {
                                ModelState.AddModelError("ActualOnTime", "Actual On Time field is required.");
                            }
                            if (shiftSchedule.ActualOffDateTime == null && shiftSchedule.ActualOffTime != null)
                            {
                                ModelState.AddModelError("ActualOffTime", "Actual Off Date Time field is required.");
                            }
                            if (shiftSchedule.ActualOnDateTime == null && shiftSchedule.ActualOnTime != null)
                            {
                                ModelState.AddModelError("ActualOnTime", "Actual On Date Time field is required.");
                            }
                            if ((shiftSchedule.ActualOnDateTime< shiftSchedule.ShiftStartDate) )
                            {
                                ModelState.AddModelError("ActualOnTime", "Actual Shift on date should be same as the shift date");
                            }
                            if (shiftSchedule.ActualOnTime != null && shiftSchedule.ActualOffTime != null && ((shiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOffTime)) - (shiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOnTime))).TotalMinutes < 0)
                            {
                                ModelState.AddModelError("ActualOffTime", "Actual Shift on time should be  earlier than the Actual Shift off time");
                            }
                            if (shiftSchedule.ActualOnDateTime!=null&&shiftSchedule.ActualOffDateTime<shiftSchedule.ActualOnDateTime)
                            {
                                ModelState.AddModelError("ActualOffTime", "Actual Shift off date should be  greater than the Actual Shift on date");
                            }
                            if(shiftSchedule.ActualOnTime!=null&&shiftSchedule.ActualOffTime!=null&&((shiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOffTime))-(shiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftSchedule.ActualOnTime))).TotalMinutes>660)
                            {
                                ModelState.AddModelError("ActualOffTime", "Shift duration should be less than Eleven Hours");
                            }
                            _ViewBagList();
                            return PartialView("_AddShiftSchedule", shiftSchedule);
                        }
                        shiftBO.ShiftSearch = new ShiftSearchViewModel();
                        shiftBO.ShiftSchedule = shiftSchedule;
                        shiftBO.ShiftSearch.BranchID = shiftSchedule.BranchID;
                        shiftBO.ShiftSearch.EmployeeID = shiftSchedule.EmployeeID;
                        shiftBO.ShiftSearch.EndDate = (DateTime)shiftSchedule.EndDate;
                        shiftBO.ShiftSearch.StartDate = (DateTime)shiftSchedule.StartDate;
                        shiftBO.ShiftSearch.ShiftPatternNameID = shiftSchedule.ShiftPatternNameID;
                        if (shiftScheduleBL.UpdateSchedule(shiftBO, out msg))
                        {
                            return Json(new
                            {
                                success = true,
                                successMsg = msg,
                                shiftsearch = shiftBO.ShiftSearch
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                successMsg = msg,
                                shiftsearch = shiftBO.ShiftSearch
                            });
                        }
                    }
                    if (shiftSchedule.ShiftScheduleID == 0)
                    {
                        shiftSchedule.ResourceList = shiftSchedule.ResourceList == null ? new List<Person>() : shiftSchedule.ResourceList;
                        TimeSpan dayDiff = (shiftSchedule.ShiftEndDate - shiftSchedule.ShiftStartDate);
                        int count = 0;
                        for (var i = 0; i <= dayDiff.Days; i++)
                        {
                            var testDate = shiftSchedule.ShiftStartDate.AddDays(i);
                            if ((testDate.DayOfWeek == DayOfWeek.Friday && shiftSchedule.WeeklyFriday == true) || (testDate.DayOfWeek == DayOfWeek.Monday && shiftSchedule.WeeklyMonday == true) || (testDate.DayOfWeek == DayOfWeek.Saturday && shiftSchedule.WeeklySaturday == true) || (testDate.DayOfWeek == DayOfWeek.Sunday && shiftSchedule.WeeklySunday == true) || (testDate.DayOfWeek == DayOfWeek.Thursday && shiftSchedule.WeeklyThursday == true) || (testDate.DayOfWeek == DayOfWeek.Tuesday && shiftSchedule.WeeklyTuesday == true) || (testDate.DayOfWeek == DayOfWeek.Wednesday && shiftSchedule.WeeklyWednesday == true))
                            {
                                count = count + 1;
                            }
                        }
                        var resList = shiftSchedule.ResourceList.Where(x => x.ResourceCheck == true).ToList();
                        int cout = 0;

                        List<ShiftOverride> listOver = new List<ShiftOverride>();
                        // ShiftOverride shiftOver = new ShiftOverride();
                        var dayDi = (shiftSchedule.ShiftEndDate - shiftSchedule.ShiftStartDate).Days + 1;
                        var startDat = shiftSchedule.ShiftStartDate;
                        if (resList.Count != 0)
                        {
                            for (int y = 0; y < dayDi; y++)
                            {
                                ShiftOverride shiftOver = new ShiftOverride();
                                shiftOver.ListName = new List<string>();
                                //for (int i = 0; i < resList.Count; i++)
                                resList.ForEach(r =>
                                {

                                    var shiftPattern = shiftScheduleBL.GetResourceShiftPattern(startDat, r.ResourceID);
                                    var patternDetail = shiftScheduleBL.GetShiftPatternID(shiftSchedule.ShiftPatternID);
                                    if (shiftPattern.Count != 0)
                                    {
                                        //for (int x = 0; x < shiftPattern.Count; x++)
                                        shiftPattern.ForEach(u =>
                                       {
                                           var checkPattern = shiftScheduleBL.GetShiftPatternID((int)u);
                                           if ((checkPattern.ShiftStartTime < patternDetail.ShiftStartTime && (patternDetail.ShiftStartTime + patternDetail.Duration) < (checkPattern.ShiftStartTime + checkPattern.Duration)) || ((patternDetail.ShiftStartTime + patternDetail.Duration) > checkPattern.ShiftStartTime && (checkPattern.ShiftStartTime + checkPattern.Duration) > patternDetail.ShiftStartTime))
                                           {

                                               shiftOver.ShiftStartDate = startDat.Date.ToString(MvcApplication.DateFormat);
                                               shiftOver.ListName.Add(r.Resource);
                                               cout = cout + 1;
                                           }
                                       });

                                    }

                                });
                                if (shiftOver.ListName.Count != 0)
                                {
                                    listOver.Add(shiftOver);
                                }
                                startDat = startDat.AddDays(1);
                            }
                        }

                        if (cout > 0 || shiftSchedule.ShiftEndDate < shiftSchedule.ShiftStartDate || shiftSchedule.ResourceList.Where(x => x.ResourceCheck == true).Count() == 0 || (shiftSchedule.WeeklySunday == false && shiftSchedule.WeeklyMonday == false && shiftSchedule.WeeklyTuesday == false && shiftSchedule.WeeklyWednesday == false && shiftSchedule.WeeklyThursday == false && shiftSchedule.WeeklyFriday == false && shiftSchedule.WeeklySaturday == false && shiftSchedule.AssignType != "NoofDays") || (count == 0 && shiftSchedule.AssignType != "NoofDays") || shiftSchedule.NoofDays < 0)
                        {
                            if (cout > 0)
                            {
                                StringBuilder builder = new StringBuilder();
                                // for (int p = 0; p < listOver.Count; p++)
                                listOver.ForEach(m =>
                                {
                                    var xx = m.ShiftStartDate;
                                    string outputName = string.Join(",", m.ListName.ToArray());
                                    builder.Append(outputName).Append(" on ").Append(xx).Append(" , ");
                                });
                                ModelState.AddModelError("ResourceList", "You Cannot Schedule this Shift Pattern to  " + "'" + builder + "'");
                            }
                            if (count == 0 && (shiftSchedule.WeeklyFriday == true || shiftSchedule.WeeklyMonday == true || shiftSchedule.WeeklySaturday == true || shiftSchedule.WeeklySunday == true || shiftSchedule.WeeklyThursday == true || shiftSchedule.WeeklyTuesday == true || shiftSchedule.WeeklyWednesday == true))
                            {
                                ModelState.AddModelError("ShiftStartDate", "Not suitable to create shift Schedule within Your Required Days !");
                            }
                            if (shiftSchedule.ShiftEndDate < shiftSchedule.ShiftStartDate)
                            {
                                ModelState.AddModelError("ShiftEndDate", "Invalid Date Range");
                            }
                            if (shiftSchedule.NoofDays < 0)
                            {
                                ModelState.AddModelError("NoofDays", "NoofDays cannot be Zero or less than Zero!");
                            }
                            if (shiftSchedule.ResourceList.Where(x => x.ResourceCheck == true).Count() == 0)
                            {
                                ModelState.AddModelError("ResourceList", "Please Select An Employee");
                            }
                            if (shiftSchedule.WeeklySunday == false && shiftSchedule.WeeklyMonday == false && shiftSchedule.WeeklyTuesday == false && shiftSchedule.WeeklyWednesday == false && shiftSchedule.WeeklyThursday == false && shiftSchedule.WeeklyFriday == false && shiftSchedule.WeeklySaturday == false && shiftSchedule.AssignType != "NoofDays")
                            {
                                ModelState.AddModelError("AssignType", "Please Select At least One Week Day");
                            }

                            _ViewBagList();
                            return PartialView("_AddShiftSchedule", shiftSchedule);
                        }
                        if (shiftScheduleBL.AssignShiftSchedule(shiftSchedule, out msg))
                        {
                            return Json(new
                            {
                                succeed = true,
                                successMsg = msg
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                succeed = false,
                                successMsg = msg
                            });
                        }
                    }
                }
                _ViewBagList();
                return PartialView("_AddShiftSchedule", shiftSchedule);
            }
            catch
            {
                TempData["Error"] = msg;
                _ViewBagList();
                return PartialView("_AddShiftSchedule", shiftSchedule);
            }

        }
        /// <summary>
        /// Shift Pattern
        /// </summary>
        /// <param name="shiftPattrenId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShiftPattern(int shiftPattrenId)
        {
            var shif = shiftScheduleBL.GetShiftPattern(shiftPattrenId);
            if (shif != null)
            {
                string shiftStartTime = TimeSpan.FromSeconds((double)shif.ShiftStartTime).ToString().Remove(0, 3);
                string shiftEndTime = TimeSpan.FromSeconds(((double)shif.ShiftStartTime + (double)shif.Duration) > 1440 ? ((double)shif.ShiftStartTime + (double)shif.Duration) - 1440 : ((double)shif.ShiftStartTime + (double)shif.Duration)).ToString().Remove(0, 3);
                return Json(new
                {
                    shiftStartTime,
                    shiftEndTime
                }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        /// <summary>
        /// Search ShiftSchedule
        /// </summary>
        /// <param name="shiftSearch"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchShiftSchedule(ShiftSearchViewModel shiftSearch)
        {

            ShiftBO shiftBO = new ShiftBO();
            shiftBO.ShiftSearch = shiftSearch;
            if (shiftSearch.SelectView == "Weekly")
            {
                shiftBO.ShiftSchedule = shiftScheduleBL.GetWeeklyShiftList(shiftSearch);
                return PartialView("_WeeklyShiftView", shiftBO);
            }
            shiftBO.ShiftScheduleList = shiftScheduleBL.GetShiftScheduleList(shiftSearch);
            shiftBO.ShiftSearch.CountData = shiftBO.ShiftScheduleList.Count;
            return PartialView("_ShiftSearchView", shiftBO);
        }
        /// <summary>
        /// Delete Schedule
        /// </summary>
        /// <param name="shiftScheduleID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="EndDate"></param>
        /// <param name="StartDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSchedule(int shiftScheduleID, int EmployeeID, int BranchID, int shiftPatternID, DateTime EndDate, DateTime StartDate)
        {
            string msg;
            ShiftBO shiftBO = new ShiftBO();
            shiftBO.ShiftSearch = new ShiftSearchViewModel();
            shiftBO.ShiftSearch.EmployeeID = EmployeeID;
            shiftBO.ShiftSearch.BranchID = BranchID;
            shiftBO.ShiftSearch.StartDate = StartDate;
            shiftBO.ShiftSearch.EndDate = EndDate;
            shiftBO.ShiftSearch.ShiftPatternNameID = shiftPatternID;
            if (shiftScheduleID != 0)
            {
                shiftBO.ShiftSearch.ShiftScheduleID = shiftScheduleID;
                if (shiftScheduleBL.DeleteShiftSchedule(shiftBO.ShiftSearch, out msg))
                {
                    //return PartialView("_ShiftSearchView", shiftBO);
                    return Json(new
                    {
                        success = true,
                       // blockNo= blockNo.ToString(),
                        msg = msg
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        msg = msg
                    });
                }
            }
            return Json(new
            {
                success = false,
                errorMsg = "Something Wrong"
            });
        }
        /// <summary>
        /// Edit Schedule
        /// </summary>
        /// <param name="shiftScheduleID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="BranchID"></param>
        /// <param name="EndDate"></param>
        /// <param name="StartDate"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSchedule(int shiftScheduleID, int EmployeeID, int BranchID, DateTime EndDate, DateTime StartDate, int shiftPatternNameID)
        {
            string msg;
            ShiftScheduleViewModel shiftSchedule = new ShiftScheduleViewModel();
            shiftSchedule.EmployeeID = EmployeeID;
            shiftSchedule.BranchNameID = BranchID;
            shiftSchedule.StartDate = StartDate;
            shiftSchedule.EndDate = EndDate;
            shiftSchedule.ShiftPatternNameID = shiftPatternNameID;
            if (shiftScheduleID != 0)
            {
                shiftSchedule.ShiftScheduleID = shiftScheduleID;
                var shiftModel = shiftScheduleBL.GetEditSchedule(shiftSchedule, out msg);
                if (shiftModel != null)
                {
                    shiftSchedule = shiftModel;
                    _ViewBagList();
                    //return PartialView("_AddShiftSchedule", shiftSchedule);
                    return Json(new
                    {
                        success = true,
                        partialview = RenderPartialViewToString("_AddShiftSchedule", shiftSchedule),
                        msg = msg
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        msg = msg
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                success = false,
                msg = "Something Wrong"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Infinite Scroll
        /// </summary>
        /// <param name="BlockNumber"></param>
        /// <param name="shiftBO"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="EmpID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public ActionResult InfiniteScroll(int BlockNumber, ShiftBO shiftBO, DateTime? StartDate, DateTime? EndDate, int? EmpID, int? BranchID, int? shiftPatternID)
        {
            if (EndDate != null)
            {
                shiftBO.ShiftSearch.EndDate = (DateTime)EndDate;
            }
            if (EmpID != null)
            {
                shiftBO.ShiftSearch.EmployeeID = (int)EmpID;
            }
            if (BranchID != null)
            {
                shiftBO.ShiftSearch.BranchID = (int)BranchID;
            }
            if (StartDate != null)
            {
                shiftBO.ShiftSearch.StartDate = (DateTime)StartDate;
            }
            if (shiftPatternID != null)
            {
                shiftBO.ShiftSearch.ShiftPatternNameID = shiftPatternID;
            }

            int BlockSize = 20;
            List<ShiftScheduleViewModel> shiftSchedule = shiftScheduleBL.InfinateShiftScheduleList(BlockNumber, BlockSize, shiftBO.ShiftSearch);
            shiftBO.ShiftScheduleList = shiftSchedule;
            shiftBO.ShiftSearch.BlockNumber = BlockNumber;
            JsonModel jsonModel = new JsonModel();

            if (shiftBO.ShiftScheduleList != null && shiftBO.ShiftSearch.BlockNumber > 1)
            {
                jsonModel.NoMoreData = shiftBO.ShiftScheduleList.Count < BlockSize;
                if (jsonModel.NoMoreData == false)
                {
                    jsonModel.DataCounts = shiftBO.ShiftScheduleList.Count * shiftBO.ShiftSearch.BlockNumber;
                }
                else
                {
                    jsonModel.DataCounts = shiftBO.ShiftScheduleList.Count == 0 ? 0 : (BlockSize * (shiftBO.ShiftSearch.BlockNumber - 1)) + shiftBO.ShiftScheduleList.Count;
                }
                jsonModel.HTMLString = RenderPartialViewToString("InfiniteScroll", shiftBO);
            }

            return Json(jsonModel);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        /// <summary>
        /// Get Branch Resource List
        /// </summary>
        /// <param name="branchNameID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetResourceList(int branchNameID)
        {
            if (branchNameID != 0)
            {
                ShiftScheduleViewModel shiftSchedule = new ShiftScheduleViewModel();
                shiftSchedule.ResourceList = shiftScheduleBL.GetBranchEmployeeList(branchNameID);
                return PartialView("_ResourceCheckList", shiftSchedule);
            }
            return null;
        }
        /// <summary>
        /// Import ExcelFile To Database
        /// </summary>
        /// <param name="FileUpload"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportExcelFileToDatabase(HttpPostedFileBase FileUpload)
        {
            string msg = "";
            //check we have a file
            if (FileUpload != null && FileUpload.ContentLength > 0)
            {
                //Try and upload
                try
                {
                   
                    //open file
                    if (Request.Files.Count == 1)
                    {
                        //get file
                        var postedFile = Request.Files[0];
                        if (FileUpload.FileName.EndsWith(".csv"))
                        {
                            List<ResourceShift> listOfCsv = new List<ResourceShift>();
                            //read data from input stream
                            using (var csvReader = new System.IO.StreamReader(postedFile.InputStream))
                            {
                                string inputLine = "";

                                //read each line
                                while ((inputLine = csvReader.ReadLine()) != null)
                                {
                                    //get lines values
                                    string[] values = inputLine.Split(new char[] { ',' });
                                    if (values[0] != "EmployeeID"&& values[0] != "")
                                    {
                                        //do something with each line and split value
                                        ResourceShift resourceShift = new ResourceShift();
                                        resourceShift.EmployeeID = int.Parse(values[0]);
                                        resourceShift.ShiftPatternID = int.Parse(values[1]);
                                        resourceShift.ShiftDate = DateTime.Parse(values[2]);
                                        resourceShift.ExpectedOnTime = DateTime.Parse(values[3]);
                                        resourceShift.ExpectedOffTime = DateTime.Parse(values[4]);
                                        //if (values[5] != "")
                                        //{
                                        //    resourceShift.ActualOnTime = DateTime.Parse(values[5]);
                                        //}
                                        //if (values[6] != "")
                                        //{
                                        //    resourceShift.ActualOffTime = DateTime.Parse(values[6]);
                                        //}
                                        resourceShift.BranchID = int.Parse(values[5]);
                                        listOfCsv.Add(resourceShift);

                                    }

                                }

                                csvReader.Close();
                            }
                            if (listOfCsv.Count != 0)
                            {
                                foreach (var resourceShift in listOfCsv)
                                {
                                    shiftScheduleBL.SaveCSVFileData(resourceShift, out msg);
                                    TempData["Success"] = msg;
                                    //return RedirectToAction("Index");
                                }
                            }
                            else
                            {
                                TempData["Error"] = "There are no data in csv file";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "please select the csv file format Excel";
                        }
                    }

                    

                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    // return RedirectToAction("Index");
                }
            }
            else {
                TempData["Error"] = "Please Choose the csv file";
            }
            return RedirectToAction("Index");
        }
       
        /// <summary>
        /// Download Template
        /// </summary>
        /// <returns></returns>
       /// [HttpPost]
        public void DownloadTemplate()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("EmployeeID,ShiftPatternID,ShiftDate,ExpectedOnTime,ExpectedOffTime,BranchID");
            string facsCsv = sb.ToString();
            // Return the file content with response body. 
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=BulkUploadTemplate.csv");
            Response.Write(facsCsv);
            Response.End();
           // return RedirectToAction("Index");
        }
        /// <summary>
        /// Create Pdf
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public FileResult CreatePdf(DateTime date)
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("WeeklyPDF" +"_"+ date.ToString(MvcApplication.DateFormat) + "" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 8 columns
            PdfPTable tableLayout = new PdfPTable(8);
            doc.SetMargins(0f, 0f, 30f, 0f);
            //Create PDF Table

            //file will created in this path
            //string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(AddContentToPDF(tableLayout,date));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }
        /// <summary>
        /// Add Content To PDF
        /// </summary>
        /// <param name="tableLayout"></param>
        /// <param name="date"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected PdfPTable AddContentToPDF(PdfPTable tableLayout,DateTime date)
        {

            float[] headers = { 50, 50, 50, 50, 50,50,50,50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 80;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            ShiftSearchViewModel shiftSearch = new ShiftSearchViewModel();
            shiftSearch.StartDate = date;
            shiftSearch.SelectView = "Weekly";
            var employees = shiftScheduleBL.GetWeeklyShiftList(shiftSearch);

            //Paragraph header = new Paragraph("Template Name:" + "Weekly" + " " + DateTime.UtcNow, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
            //header.Alignment = Element.ALIGN_BOTTOM;
           // tableLayout.AddCell(new Paragraph(DateTime.UtcNow.ToShortDateString(), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)));

            // tableLayout.HeaderHeight = 400;
            tableLayout.AddCell(new PdfPCell(new Phrase("Weekly Report"+"-"+date.ToString(MvcApplication.DateFormat), new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            //PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
            string imageURL = HttpContext.Server.MapPath("~/Content/dist/img/avatar5.png");
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(14f, 12f);
            //Give space before image
            jpg.SpacingBefore = 10f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_LEFT;

            //tableLayout.AddCell(jpg);
            tableLayout.AddCell(new PdfPCell(new Phrase()) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
            
            ////Add header
            AddCellToHeader(tableLayout, "Resource");
            AddCellToHeader(tableLayout, date.DayOfWeek.ToString().Substring(0, 3)+" "+date.ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(1).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(1).ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(2).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(2).ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(3).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(3).ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(4).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(4).ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(5).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(5).ToString(MvcApplication.DateFormat));
            AddCellToHeader(tableLayout, date.AddDays(6).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(6).ToString(MvcApplication.DateFormat));

            ///Daily Total Shift
            int count1 = 0;
            employees.ResourceNameList.ForEach(x =>
            {
                count1 = count1 + x.ShiftStartDateList.Where(p => p == date).ToList().Count;
            });
            int count2 = 0;
            DateTime date2 = date.AddDays(1);
            employees.ResourceNameList.ForEach(x =>
            {
                count2 = count2 + x.ShiftStartDateList.Where(p => p == date2).ToList().Count;
            });
            int count3 = 0;
            DateTime date3 = date.AddDays(2);
            employees.ResourceNameList.ForEach(x =>
            {
                count3 = count3 + x.ShiftStartDateList.Where(p => p == date3).ToList().Count;
            });
            int count4 = 0;
            DateTime date4 = date.AddDays(3);
            employees.ResourceNameList.ForEach(x =>
            {
                count4 = count4 + x.ShiftStartDateList.Where(p => p == date4).ToList().Count;
            });
            int count5 = 0;
            DateTime date5 = date.AddDays(4);
            employees.ResourceNameList.ForEach(x =>
            {
                count5 = count5 + x.ShiftStartDateList.Where(p => p == date5).ToList().Count;
            });
            int count6 = 0;
            DateTime date6 = date.AddDays(5);
            employees.ResourceNameList.ForEach(x =>
            {
                count6 = count6 + x.ShiftStartDateList.Where(p => p == date6).ToList().Count;
            });
            int count7 = 0;
            DateTime date7 = date.AddDays(6);
            employees.ResourceNameList.ForEach(x =>
            {
                count7 = count7 + x.ShiftStartDateList.Where(p => p == date7).ToList().Count;
            });
            ////Add body
            foreach (var emp in employees.ResourceNameList)
            {

                AddCellToBody(tableLayout, emp.ResourceWeekly.ToString());
                List<string> strBuilder = new List<string>();
                List<string> strBuilder1 = new List<string>();
                List<string> strBuilder2 = new List<string>();
                List<string> strBuilder3 = new List<string>();
                List<string> strBuilder4 = new List<string>();
                List<string> strBuilder5 = new List<string>();
                List<string> strBuilder6 = new List<string>();
                for (int i = 0; i < emp.ShiftStartDateList.Count; i++)
                {
                    if (date == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                   else if (date.AddDays(1) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder1.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                   else if (date.AddDays(2) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder2.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                  else  if (date.AddDays(3) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder3.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                   else if (date.AddDays(4) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder4.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                   else if (date.AddDays(5) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder5.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                   else if (date.AddDays(6) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder6.Add(emp.ShiftPatternList.ElementAt(i));
                    }

                }
                if (strBuilder.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder));
                }
                else
                {
                 
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder));
                }
                if (strBuilder1.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder1));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder1));
                }
                if (strBuilder2.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder2));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder2));
                }
                if (strBuilder3.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder3));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder3));
                }
                if (strBuilder4.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder4));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder4));
                }
                if (strBuilder5.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder5));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder5));
                }
                if (strBuilder6.Count == 0)
                {
                    AddCellToBody(tableLayout, string.Join("\n", strBuilder6));
                }
                else
                {
                    AddCellToBodyGreen(tableLayout, string.Join("\n", strBuilder6));
                }

            }
            AddCellToBody(tableLayout, "Daily Shift Total");
            if (count1 == 0)
            {
                AddCellToBodyRed(tableLayout, count1.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count1.ToString());
            }
            if (count2 == 0)
            {
                AddCellToBodyRed(tableLayout, count2.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count2.ToString());
            }
            if (count3 == 0)
            {
                AddCellToBodyRed(tableLayout, count3.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count3.ToString());
            }
            if (count4 == 0)
            {
                AddCellToBodyRed(tableLayout, count4.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count4.ToString());
            }
            if (count5 == 0)
            {
                AddCellToBodyRed(tableLayout, count5.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count5.ToString());
            }
            if (count6 == 0)
            {
                AddCellToBodyRed(tableLayout, count6.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count6.ToString());
            }
            if (count7 == 0)
            {
                AddCellToBodyRed(tableLayout, count7.ToString());
            }
            else
            {
                AddCellToBodyLightGreen(tableLayout, count7.ToString());
            }
            return tableLayout;
        }
        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(207,229, 219) });
        }
        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }
        // Method to add single cell as Red Colour to the body
        private static void AddCellToBodyRed(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 99, 71) });
        }
        // Method to add single cell as Green Colour to the body
        private static void AddCellToBodyGreen(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(152, 251, 152) });
        }
        // Method to add single cell as Light Green Colour to the body
        private static void AddCellToBodyLightGreen(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(0, 255, 0) });
        }
        /// <summary>
        /// Export To Excel
        /// </summary>
        /// <param name="date"></param>
        public void ExportToExcel(DateTime date)
        {
            string strExcelFileName = string.Format("WeeklyExcel" + "_" + date.ToString(MvcApplication.DateFormat) + "" + ".xls");
            var products = new System.Data.DataTable("excel");
            products.Columns.Add("Resource", typeof(string));
            products.Columns.Add(date.DayOfWeek.ToString().Substring(0, 3) + " " + date.ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(1).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(1).ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(2).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(2).ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(3).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(3).ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(4).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(4).ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(5).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(5).ToString(MvcApplication.DateFormat), typeof(string));
            products.Columns.Add(date.AddDays(6).DayOfWeek.ToString().Substring(0, 3) + " " + date.AddDays(6).ToString(MvcApplication.DateFormat), typeof(string));

            ShiftSearchViewModel shiftSearch = new ShiftSearchViewModel();
            shiftSearch.StartDate = date;
            shiftSearch.SelectView = "Weekly";
            var employees = shiftScheduleBL.GetWeeklyShiftList(shiftSearch);
            ///Daily Total Shift
            int count1 = 0;
            employees.ResourceNameList.ForEach(x =>
            {
                count1 = count1 + x.ShiftStartDateList.Where(p => p == date).ToList().Count;
            });
            int count2 = 0;
            DateTime date2 = date.AddDays(1);
            employees.ResourceNameList.ForEach(x =>
            {
                count2 = count2 + x.ShiftStartDateList.Where(p => p == date2).ToList().Count;
            });
            int count3 = 0;
            DateTime date3 = date.AddDays(2);
            employees.ResourceNameList.ForEach(x =>
            {
                count3 = count3 + x.ShiftStartDateList.Where(p => p == date3).ToList().Count;
            });
            int count4 = 0;
            DateTime date4 = date.AddDays(3);
            employees.ResourceNameList.ForEach(x =>
            {
                count4 = count4 + x.ShiftStartDateList.Where(p => p == date4).ToList().Count;
            });
            int count5 = 0;
            DateTime date5 = date.AddDays(4);
            employees.ResourceNameList.ForEach(x =>
            {
                count5 = count5 + x.ShiftStartDateList.Where(p => p == date5).ToList().Count;
            });
            int count6 = 0;
            DateTime date6 = date.AddDays(5);
            employees.ResourceNameList.ForEach(x =>
            {
                count6 = count6 + x.ShiftStartDateList.Where(p => p == date6).ToList().Count;
            });
            int count7 = 0;
            DateTime date7 = date.AddDays(6);
            employees.ResourceNameList.ForEach(x =>
            {
                count7 = count7 + x.ShiftStartDateList.Where(p => p == date7).ToList().Count;
            });
            foreach (var emp in employees.ResourceNameList)
            {
                List<string> strBuilder = new List<string>();
                List<string> strBuilder1 = new List<string>();
                List<string> strBuilder2 = new List<string>();
                List<string> strBuilder3 = new List<string>();
                List<string> strBuilder4 = new List<string>();
                List<string> strBuilder5 = new List<string>();
                List<string> strBuilder6 = new List<string>();
                for (int i = 0; i < emp.ShiftStartDateList.Count; i++)
                {
                    if (date == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(1) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder1.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(2) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder2.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(3) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder3.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(4) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder4.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(5) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder5.Add(emp.ShiftPatternList.ElementAt(i));
                    }
                    else if (date.AddDays(6) == emp.ShiftStartDateList.ElementAt(i))
                    {
                        strBuilder6.Add(emp.ShiftPatternList.ElementAt(i));
                    }

                }
                products.Rows.Add(emp.ResourceWeekly, string.Join(",", strBuilder), string.Join(",", strBuilder1), string.Join(",", strBuilder2), string.Join(",", strBuilder3), string.Join(",", strBuilder4), string.Join(",", strBuilder5), string.Join(",", strBuilder6));
                
            }
           
            products.Rows.Add("Daily Shift Total", count1.ToString(), count2.ToString(), count3.ToString(), count4.ToString(), count5.ToString(),count6.ToString(), count7.ToString());
            
           
            //PdfPTable tableLayout = new PdfPTable(8);
            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();
           
            foreach (GridViewRow row in grid.Rows)
            {
                for (int i = 1; i < 8; i++)
                {
                    int result = 0;
                    if (int.TryParse(row.Cells[i].Text, out result))
                    {
                        if (result > 0)
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.Chartreuse;
                        }
                        else
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.Tomato;
                        }
                    }
                    else
                    {
                        if (row.Cells[i].Text!= "&nbsp;")
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename="+strExcelFileName);
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

           // return View();
        }
        [HttpPost]
        public void ExportToExcelDaily(ShiftSearchViewModel shiftSearch, string counts)
        {
            string strExcelFileName = string.Format("DailyViewExcel" + "_" + counts + "" + ".xls");
            var products = new System.Data.DataTable("excel");
            products.Columns.Add("Resource", typeof(string));
            products.Columns.Add("Branch", typeof(string));
            products.Columns.Add("Shift Date", typeof(string));
            products.Columns.Add("Shift Pattern", typeof(string));
            products.Columns.Add("Expected On Time", typeof(string));
            products.Columns.Add("Actual On Time", typeof(string));
            products.Columns.Add("Expected Off Time", typeof(string));
            products.Columns.Add("Actual Off Time", typeof(string));


            if (!string.IsNullOrEmpty(counts))
            {
                shiftSearch.CountData = int.Parse(counts);
            }
            //shiftSearch.SelectView = "Weekly";
            var employees = shiftScheduleBL.GetShiftScheduleList(shiftSearch);
            for (int i = 0; i < employees.Count; i++)
            {
                products.Rows.Add(employees[i].Employee, employees[i].BranchName, employees[i].ShiftStartDate, employees[i].ShiftPattern, employees[i].ExpectedOnDateTime, employees[i].ActualOnDateTime, employees[i].ExpectedOffDateTime, employees[i].ActualOffDateTime);
            }
            

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();
            
                for (int i = 0; i < 8; i++)
                {
                    grid.HeaderRow.Cells[i].BackColor= System.Drawing.Color.DarkSeaGreen;
                    //row.Cells[i].BackColor= System.Drawing.Color.DarkSeaGreen;
                }
            
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + strExcelFileName);
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    
}