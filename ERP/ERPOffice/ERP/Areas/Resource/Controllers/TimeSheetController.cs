using ERP.MvcSecurity;
///-----------------------------------------------------------------
///   Namespace:      ERP.Areas.Resource
///   Class:          BranchInfoController
///   Description:    Branch Details Of ERP Companies 
///   Author:         S.Sathiyan                   Date: 02/06/2016
///   Notes:          save multiple Branch Details in One Company 
///   Revision History: Last Update ON 21/06/2016
///   Name:   s.sathiyan        Date: 21/06/2016     
///-------------------------------------------------------
using ERP.Resource.BL;
using ERP.Resource.Models;
using ERP.Resource.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Areas.Resource
{
    public class TimeSheetController : Controller
    {
        TimeSheetBL timeSheetBL = new TimeSheetBL();

        // GET: Resource/TimeSheet

        void viewBagList()
        {
            ViewBag.TimeSheetFrz = timeSheetBL.getTimeSheetFrz().Select(x => new SelectListItem() { Value = x.TimeSheetFrequencyID.ToString(), Text = x.TimeSheetFrequencyName });
            ViewBag.Resources = timeSheetBL.getResources().Select(x => new SelectListItem() { Value = x.EmployeeID.ToString(), Text = x.FirstName });
            ViewBag.Status = timeSheetBL.getStatus().Select(x => new SelectListItem() { Value = x.HolidayStatusTypeID.ToString(), Text = x.HolidayStatusType });
        }
        [HttpGet]
        [Authorize]
        [AuthorizeUser(Permission = 7)]
        public ActionResult TimeSheet(TimeSheetView timeSheetView)
        {
            // = new TimeSheetView();
            TempData["TimeSerachBO"] = timeSheetView.TsearchBBO;
            if (TempData["TimeSerachBO"] != null)
            {
                timeSheetView.TsearchBBO = TempData["TimeSerachBO"] as TimeSearchBO;
            }
            else
            {
                timeSheetView.TsearchBBO = new TimeSearchBO();
                timeSheetView.TsearchBBO.MonthID = DateTime.Now.Month;
                timeSheetView.TsearchBBO.Year = DateTime.Now.Year.ToString();
            }
            viewBagList();

            TimeSheetBO timeSheetBO = new TimeSheetBO();
            var timeSheetList = timeSheetBL.GetfilterTimeSheet(timeSheetView.TsearchBBO);
            timeSheetView.timeSheetBBO = new TimeSheetBO();
            timeSheetView.timeSheetView = timeSheetList;
            TempData["TimeSerachBO"] = timeSheetView.TsearchBBO;
            return View(timeSheetView);
        }

        [HttpPost]
        public ActionResult SearchTimeSheet(TimeSearchBO TsearchBO, string ButtonType)
        {
            TimeSheetView timeSheetView1 = new TimeSheetView();
            timeSheetView1.TsearchBBO = TsearchBO;
            timeSheetView1.timeSheetView = timeSheetBL.GetfilterTimeSheet(TsearchBO);

            TempData["TimeSerachBO"] = TsearchBO;
            if (ButtonType == "Search")
            {

                return PartialView("_TimeSheetView", timeSheetView1.timeSheetView);
            }

            else
            {
                viewBagList();
                timeSheetView1.graphID = 123;
                return PartialView("_GraphView", timeSheetView1.timeSheetView);
            }

        }
        [HttpPost]
        public ActionResult TimesheetAuthorized(int ResourcesID)
        {

            if (TempData["TimeSerachBO"] != null || Request.IsAjaxRequest())
            {
                TimeSheetView timeSheetView1 = new TimeSheetView();
                TimeSearchBO TsearchBO = TempData["TimeSerachBO"] as TimeSearchBO;

                timeSheetView1.timeSheetBBO = timeSheetBL.GetfilterTimeSheet(TsearchBO).Where(x => x.ResourcesID == ResourcesID).SingleOrDefault();
                timeSheetView1.timeSheetBBO.MonthID = TsearchBO.MonthID;
                timeSheetView1.timeSheetBBO.Year = TsearchBO.Year;

                return Json(new { success = true, partialview = RenderViewToString("_TimeSheetAuthorized", timeSheetView1.timeSheetBBO), message = "" });
                
            }

            return RedirectToAction("TimeSheet");
        }

  

      //partial View  by RenderViewToString 
        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }



        [HttpPost]
        public ActionResult TimeSheetAuthorizedSaved(string ButtonType, TimeSheetBO timeSheetBO)
        {
            
            //timeSheetBO.NoOFShift = Convert.ToInt16(TempData["TotalShift" + timeSheetBO.ResourcesID.ToString()]);
            //timeSheetBO.TolShiftHrs = Convert.ToInt64(TempData["TotalShiftHrs" + timeSheetBO.ResourcesID.ToString()]);
            //timeSheetBO.TolHly = Convert.ToDouble(TempData["TotalHly" + timeSheetBO.ResourcesID.ToString()]);
            //timeSheetBO.TolHlyHours = Convert.ToDouble(TempData["TotalHlytHours" + timeSheetBO.ResourcesID.ToString()]);
            timeSheetBO.Payment = Convert.ToInt64(TempData["Payment" + timeSheetBO.ResourcesID.ToString()]);

            DateTime StatusDate;
            string Msg, status  = "";
            if (timeSheetBL.TimeSheetAuthorizedFun(timeSheetBO, out Msg, ButtonType, out status,out StatusDate))
            {
                return Json(new { success = true, errorMsg = Msg, AuthoStatus = status , AuthoStatusDate= StatusDate });


            }
            else
            {
                return Json(new { success = false, errorMsg = Msg, AuthoStatus = status, AuthoStatusDate = StatusDate });

            }
        }




    }
}