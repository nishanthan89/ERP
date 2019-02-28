//--------------------------------------------------------------------------------
// <copyright file="HolidayController.cs" company="BDL">
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Areas.Resource.Controllers
{
    [Authorize]
    [AuthorizeUserAttribute(Permission = 12)]
    public class HolidayController : Controller
    {
        private HolidayBL holidayBL = new HolidayBL();
        // GET: Resource/Holiday
        [HttpGet]
        public ActionResult Index()
        {

            HolidayBO holidayBO = new HolidayBO();
            holidayBO.searchHoliday = new SearchHolidayViewModel();
            holidayBO.searchHoliday.Month = DateTime.Now.Month;
            holidayBO.searchHoliday.Year = DateTime.Now.Year;
            _CalenderDetails(holidayBO);

            _ViewBagListDetails();
            return View(holidayBO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SearchHolidayViewModel searchHoliday)
        {

            HolidayBO holidayBO = new HolidayBO();
            holidayBO.searchHoliday = searchHoliday;
            _CalenderDetails(holidayBO);

            _ViewBagListDetails();
            return View(holidayBO);
        }
        /// <summary>
        /// _ViewBagList Details
        /// </summary>
        public void _ViewBagListDetails()
        {
            ViewBag.ResourceList = (holidayBL.GetResourceList().Select(x => new SelectListItem()
            {
                Value = x.EmployeeID.ToString(),
                Text = x.FirstName
            }).OrderBy(i=>i.Text));
            ViewBag.HolidayTypeList = (holidayBL.GetHolidayTypeList().Select(x => new SelectListItem()
            {
                Value = x.HolidayTypeID.ToString(),
                Text = x.HolidayTypeName
            }).OrderBy(i=>i.Text));
            ViewBag.HolidayStatusTypeList = (holidayBL.GetHolidayStatusList().Select(x => new SelectListItem()
            {
                Value = x.HolidayStatusTypeID.ToString(),
                Text = x.HolidayStatusType
            }).OrderBy(i=>i.Text));
        }
        public void _CalenderDetails(HolidayBO holidayBO)
        {
            holidayBO.createHolidayList = holidayBL.GetHolidayList(holidayBO.searchHoliday);
            if (holidayBO.createHolidayList != null)
            {
                var clientList = new List<object>();
                holidayBO.createHolidayList.ForEach(e =>
                {
                    clientList.Add(
                        new
                        {
                            //color="yellow",
                            title = e.Resource + '-' + e.HolidayType + '-' + e.Status,
                            description = e.HolidayType,
                            start = e.HolidayDate.AddDays(1),
                            //end = e.HolidayDate,
                            allDay = false,
                            // textColor="black",
                            backgroundColor = e.Status == "Authorized" ? "green" : e.HoliColor,
                            borderColor = e.HoliColor
                        });
                });
                holidayBO.EventArray = clientList.ToArray();
            }
        }
        /// <summary>
        /// Add Holiday
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddHoliday(int month,int year,int? searchResourceID)
        {
            CreateHolidayViewModel createHoliday = new CreateHolidayViewModel();
            createHoliday.SearchHoliday = new SearchHolidayViewModel();
            createHoliday.SearchHoliday.Month =month;
            createHoliday.SearchHoliday.Year = year;
            createHoliday.SearchHoliday.SearchResourceID = searchResourceID==null?0:(int)searchResourceID;
            _ViewBagListDetails();
            return PartialView("_CreateHoliday", createHoliday);
        }
        /// <summary>
        /// Search Holiday
        /// </summary>
        /// <param name="searchHoliday"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchHoliday(SearchHolidayViewModel searchHoliday)
        {
            
            HolidayBO holidayBO = new HolidayBO();
            holidayBO.searchHoliday = searchHoliday;
            _CalenderDetails(holidayBO);
            if (searchHoliday.Month==null)
            {
                searchHoliday.Month = DateTime.Now.Month;
            }
            if (searchHoliday.Year == null)
            {
                searchHoliday.Year = DateTime.Now.Year;
            }
            return PartialView("_HolidayDetail", holidayBO);
        }
        /// <summary>
        /// Create Holiday
        /// </summary>
        /// <param name="createHoliday"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateHoliday(CreateHolidayViewModel createHoliday)
        {
            string msg;
            try
            {
                if(createHoliday.HolidayID != 0)
                {
                    ModelState.Remove("AssignType");
                }
                if (ModelState.IsValid)
                {
                    if (createHoliday.HolidayID != 0)
                    {
                        SearchHolidayViewModel searchHoliday = new SearchHolidayViewModel();
                        searchHoliday.Year = createHoliday.SearchHoliday.Year;
                        searchHoliday.Month = createHoliday.SearchHoliday.Month;
                        searchHoliday.SearchResourceID = createHoliday.SearchHoliday.SearchResourceID;
                        if (holidayBL.AddEditHoliday(createHoliday, out msg))
                        {
                            return Json(new { success = true, searchHoliday = searchHoliday, message = msg });
                        }
                        else
                        {
                            _ViewBagListDetails();
                            return Json(new { success = false, searchHoliday = searchHoliday, message = msg,partialview = RenderViewToString("_CreateHoliday", createHoliday) });
                        }
                    }
                    else {
                        var dayDi = (createHoliday.HolidayEndDate - createHoliday.HolidayStartDate).Days + 1;
                        //var startDat = createHoliday.HolidayStartDate;
                        int count = 0;
                        List<string> dateList = new List<string>();
                        if (createHoliday.ResourceID != 0)
                        {
                            for (int i = 0; i < dayDi; i++)
                            {
                                var testDate = createHoliday.HolidayStartDate.AddDays(i);
                                var takeHoli = holidayBL.GetDayHoliday(testDate, createHoliday.ResourceID);
                                if (((testDate.DayOfWeek == DayOfWeek.Friday && createHoliday.WeeklyFriday == true) || (testDate.DayOfWeek == DayOfWeek.Monday && createHoliday.WeeklyMonday == true) || (testDate.DayOfWeek == DayOfWeek.Saturday && createHoliday.WeeklySaturday == true) || (testDate.DayOfWeek == DayOfWeek.Sunday && createHoliday.WeeklySunday == true) || (testDate.DayOfWeek == DayOfWeek.Thursday && createHoliday.WeeklyThursday == true) || (testDate.DayOfWeek == DayOfWeek.Tuesday && createHoliday.WeeklyTuesday == true) || (testDate.DayOfWeek == DayOfWeek.Wednesday && createHoliday.WeeklyWednesday == true))&&createHoliday.AssignType!= "NoofDays")
                                {
                                    
                                    if (takeHoli == true)
                                    {
                                        dateList.Add(testDate.Date.ToString(MvcApplication.DateFormat));
                                    }
                                    count = count + 1;
                                }
                                if(createHoliday.AssignType== "NoofDays")
                                {
                                    if (takeHoli == true)
                                    {
                                        dateList.Add(testDate.Date.ToString(MvcApplication.DateFormat));
                                    }
                                }
                            }
                        }
                        if (createHoliday.HolidayStartDate < createHoliday.RequestedOn||createHoliday.HolidayStartDate > createHoliday.HolidayEndDate || (count == 0 && createHoliday.AssignType != "NoofDays") || dateList.Count > 0 || createHoliday.NoofDays < 0)
                        {
                            if (createHoliday.HolidayStartDate > createHoliday.HolidayEndDate)
                            {
                                ModelState.AddModelError("HolidayEndDate", "Invalid Date Range");
                            }
                            if (count == 0 && (createHoliday.WeeklyFriday == true || createHoliday.WeeklyMonday == true || createHoliday.WeeklySaturday == true || createHoliday.WeeklySunday == true || createHoliday.WeeklyThursday == true || createHoliday.WeeklyTuesday == true || createHoliday.WeeklyWednesday == true) && createHoliday.AssignType != "NoofDays")
                            {
                                ModelState.AddModelError("HolidayEndDate", "Not suitable to create shift Schedule within Your Required Days !");
                            }
                            if (dateList.Count > 0)
                            {
                                string outputName = string.Join(",", dateList.ToArray());
                                ModelState.AddModelError("HolidayEndDate", "You already Booked this Holiday on  " + "'" + outputName + "'");
                            }
                            if (createHoliday.NoofDays < 0)
                            {
                                ModelState.AddModelError("NoofDays", "NoofDays cannot be Zero or less than Zero!");
                            }
                            if (createHoliday.WeeklySunday == false && createHoliday.WeeklyMonday == false && createHoliday.WeeklyTuesday == false && createHoliday.WeeklyWednesday == false && createHoliday.WeeklyThursday == false && createHoliday.WeeklyFriday == false && createHoliday.WeeklySaturday == false && createHoliday.AssignType != "NoofDays")
                            {
                                ModelState.AddModelError("AssignType", "Please Select At least One Week Day");
                            }
                            if (createHoliday.HolidayStartDate < createHoliday.RequestedOn)
                            {
                                ModelState.AddModelError("RequestedOn", "Requested On date should be earlier than the holiday dates");
                            }
                            _ViewBagListDetails();
                            return PartialView("_CreateHoliday", createHoliday);
                        }
                        if (holidayBL.AssignHoliday(createHoliday, out msg))
                        {
                            return Json(new
                            {
                                succeed = true,
                                message = msg
                            });
                        }
                        else
                        {
                            _ViewBagListDetails();
                            return Json(new
                            {
                                succeed = false,
                                message = msg,
                                partialview = RenderViewToString("_CreateHoliday", createHoliday)
                            });
                        }
                    }
                }
                _ViewBagListDetails();
                return PartialView("_CreateHoliday", createHoliday);
            }
            catch (Exception ex)
            {
                _ViewBagListDetails();
                return Json(new
                {
                    success = false,
                    message = ex.Message,
                    partialview = RenderViewToString("_CreateHoliday", createHoliday)
                });
                
            }
            
        }
        /// <summary>
        /// Delete Holiday
        /// </summary>
        /// <param name="holidayID"></param>
        /// <param name="month"></param>
        /// <param name="employeeID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHoliday(int holidayID, int month, int employeeID, int year)
        {
            string msg;
            HolidayBO holidayBO = new HolidayBO();
            if (holidayID != 0)
            {

                holidayBO.searchHoliday = new SearchHolidayViewModel();
                holidayBO.searchHoliday.Month = month;
                holidayBO.searchHoliday.Year = year;
                holidayBO.searchHoliday.SearchResourceID = employeeID;
                holidayBO.searchHoliday.HolidayID = holidayID;
                if (holidayBL.RemoveHoliday(holidayBO.searchHoliday, out msg))
                {
                    _CalenderDetails(holidayBO);
                    return Json(new
                    {
                        success = true,
                        partialview = RenderViewToString("_HolidayDetail", holidayBO),
                        message = msg
                    },JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _CalenderDetails(holidayBO);
                    return Json(new
                    {
                        success = false,
                        partialview = RenderViewToString("_HolidayDetail", holidayBO),
                        message = msg
                    },JsonRequestBehavior.AllowGet);
                }
            }    
            return Json(new
            {
                success = false
            },JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Render the View ToString
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        /// <summary>
        /// Edit Holiday
        /// </summary>
        /// <param name="holidayID"></param>
        /// <param name="month"></param>
        /// <param name="employeeID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditHoliday(int holidayID, int month, int employeeID, int year)
        {
            string msg;
            HolidayBO holidayBO = new HolidayBO();
            if (holidayID != 0)
            {
                holidayBO.searchHoliday = new SearchHolidayViewModel();
                holidayBO.searchHoliday.Month = month;
                holidayBO.searchHoliday.Year = year;
                holidayBO.searchHoliday.SearchResourceID = employeeID;

                holidayBO.createHoliday = new CreateHolidayViewModel();
                holidayBO.createHoliday.HolidayID = holidayID;
                holidayBO.createHoliday.Month = month;
                holidayBO.createHoliday.Year = year;
                holidayBO.createHoliday.SearchResourceID = employeeID;
                holidayBO.createHoliday = holidayBL.EditHolidayDetail(holidayBO.createHoliday, out  msg);
                
                if (holidayBO.createHoliday != null)
                {
                    holidayBO.createHoliday.SearchHoliday = holidayBO.searchHoliday;
                    _ViewBagListDetails();
                    return Json(new
                    {
                        success = true,
                        partialview = RenderViewToString("_CreateHoliday", holidayBO.createHoliday),
                        message = msg
                    });
                }
                else
                {
                    
                    _CalenderDetails(holidayBO);
                    return Json(new
                    {
                        success = false,
                        partialview = RenderViewToString("_HolidayDetail", holidayBO),
                        message = msg
                    });
                }
                
            }
            //_ViewBagListDetails();
            return Json(new
            {
                error = true
            });
        }
        /// <summary>
        /// Details Holiday
        /// </summary>
        /// <param name="holidayID"></param>
        /// <param name="month"></param>
        /// <param name="employeeID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailsHoliday(int holidayID, int month, int employeeID, int year)
        {
            HolidayBO holidayBO = new HolidayBO();
            if ( holidayID != 0)
            {
                holidayBO.searchHoliday = new SearchHolidayViewModel();
                holidayBO.searchHoliday.Month = month;
                holidayBO.searchHoliday.Year = year;
                holidayBO.searchHoliday.SearchResourceID = employeeID;

                holidayBO.detailsHoliday = new DetailsHolidayViewModel();
                holidayBO.detailsHoliday.HolidayID = holidayID;
                holidayBO.detailsHolidayList = holidayBL.HolidayDetail(holidayBO.detailsHoliday);
                if (holidayBO.detailsHolidayList != null)
                {
                    _ViewBagListDetails();
                    return Json(new
                    {
                        success = true,
                        partialview = RenderViewToString("_HolidayStatusDetails", holidayBO)
                    });
                }
            }
            _ViewBagListDetails();
            return Json(new
            {
                error = true
            });
        }
        [HttpPost]
        public ActionResult RemainHoliday(int resourceID)
        {
            if (resourceID != 0)
            {
                int availHoli;
                int yearsWork = holidayBL.GetWorkingAge(resourceID);
                int takenHoli = holidayBL.GetTakenHoliday(resourceID);
                if (yearsWork == 0)
                {
                    availHoli = (27 - takenHoli);
                }
                else
                {
                    availHoli = (42 - takenHoli);
                }
                return Json("You have remaining " + availHoli + " days holiday",JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty,JsonRequestBehavior.AllowGet);
        }
        
    }
}