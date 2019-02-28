///-----------------------------------------------------------------
///   Namespace:      ERP.Areas.Admin.Controllers
///   Class:          BranchInfoController
///   Description:    Branch Details Of ERP Companies 
///   Author:         S.Sathiyan                   Date: 02/06/2016
///   Notes:          save multiple Branch Details in One Company 
///   Revision History: Last Update ON 21/06/2016
///   Name:   s.sathiyan        Date: 21/06/2016     
///-------------------------------------------------------
///
using ERP.Address.BL;
using ERP.Admin.BL;
using ERP.Admin.Models;
using ERP.Admin.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Areas.Admin.Controllers
{
    public class BranchInfoController : Controller
    {
        BranchInfoBL branchInfoBL = new BranchInfoBL();
        AddressBL addressBL = new AddressBL();
        // GET: Admin/BranchInfo

        //DropDown List
        public void ViewBagList()
        {
     
            ViewBag.BranchStatus = (branchInfoBL.BranchStatusView().Select(x=> new SelectListItem() { Value=x.BranchStatusID.ToString(), Text=x.BranchStatus }));
            ViewBag.BranchType = (branchInfoBL.BranchTypeView().Select(x=>new SelectListItem() {Value =x.BranchTypeID.ToString(), Text=x.BranchType }));
            ViewBag.CountryList = addressBL.GetCountryList().Select(x => new SelectListItem() { Value = x.CountryID.ToString(), Text = x.CountryName, Selected = x.IsSelected });
         
        }

        /// <summary>
        ///View the Total Branch Details 
        /// </summary>
        /// <returns>
        /// Branch List
        /// </returns>
        /// 
        public ActionResult Index()

         {
            
            BranchInfoBO branchInfoBo = new BranchInfoBO();
            BranchInfoViewModel branchInfoView = new BranchInfoViewModel();
            branchInfoView.branchInfoViewModel = branchInfoBL.GetBranchList();
            branchInfoView.branchInfoBO = new BranchInfoBO();

            return View(branchInfoView);
        }

        /// <summary>
        ///  for Get the List using Ajax (Inner Update List)
        /// </summary>
        /// <returns></returns>
        public ActionResult ListBranch()
        {
         
            BranchInfoBO branchInfoBo = new BranchInfoBO();
            BranchInfoViewModel branchInfoView = new BranchInfoViewModel();
            branchInfoView.branchInfoViewModel = branchInfoBL.GetBranchList();
            return PartialView("_BranchListView", branchInfoView.branchInfoViewModel);
        }

        /// <summary>
        ///  for Get the List using Ajax (Inner Update List)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateBranch()
        {
            ViewBagList();
            BranchInfoViewModel branchInfoView = new BranchInfoViewModel();
            branchInfoView.branchInfoBO = new BranchInfoBO();
            branchInfoView.branchInfoBO.Address = new Address.ViewModels.AddressViewModel();
            branchInfoView.branchInfoBO.Address.CountryID = addressBL.GetDefaultCountry();
            return PartialView("_CreateEditBranch",branchInfoView.branchInfoBO);

        }
        /// <summary>
        ///  Create & Update using Branch ID Value 
        /// </summary>
        /// <param name="branchInfoBO"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CreateBranch(BranchInfoBO branchInfoBO)
        {

            string eMessage;
            if (ModelState.IsValid)
            {
                if (branchInfoBO.BranchID == 0)
                {
                    if (branchInfoBL.SaveBranchDetails(branchInfoBO, out eMessage) == true)
                    {

                        return Json(new
                        {
                            success = true,

                            errorMsg = eMessage
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            errorMsg = eMessage,
                            //AjaxReturn = PartialView("_CreateEditBranch", branchInfoBO).RenderTostring()
                        });
                        //ModelState.AddModelError("BranchCode", eMessage);
                    }
                }
            
            
            else
            {
                if (branchInfoBL.UpdateBranchInfo(branchInfoBO, out eMessage) == true)
                    {
                        return Json(new { success = true, errorMsg = eMessage });
                    }
                    else
                        {
                        return Json(new
                        {
                            success = false,

                            errorMsg = eMessage,
                           
                        });

                    }
                }
            }
            ViewBagList();
            return PartialView("_CreateEditBranch",branchInfoBO);
        }

        /// <summary>
        /// delete branch details from DB  
        //Post method
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            string msg;
            if (branchInfoBL.ChkEditBranch(Id))
            {
                if (branchInfoBL.DeleteBranchdetails(Id, out msg))
                {
                    return Json(new { success = true, errorMsg = msg });
                }
                else
                {
                    return Json(new { success = false, errorMsg = msg });

                }
            }
            else
            {

                return Json(new { success = false, errorMsg = "This Branch Details Is Already Deleted....!!!" });
            }

        }

        /// <summary>
        ///  Edit Branch details & view to  _CreateEditBranch partial view 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditBranch(int Id)
        {
            if (branchInfoBL.ChkEditBranch(Id)==true)
            {
                BranchInfoBO branchInfoBO = new BranchInfoBO();
                branchInfoBO = branchInfoBL.GetEditBranch(Id);
                ViewBagList();
                return Json(new { success = true, partialview = RenderViewToString("_CreateEditBranch", branchInfoBO), message = "" });
               // return PartialView("_CreateEditBranch", branchInfoBO);
            }

            else
            {
                return Json(new
                {
                    success = false,

                    errorMsg = "This Branch Details Is Already Deleted....!! ",
                },JsonRequestBehavior.AllowGet);
            }

           
        }

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


        public JsonResult BranchNameChk(string BranchName,string BranchId)
        {

            bool isExist = branchInfoBL.BranchRemoteNameChk(BranchName, BranchId);

            return Json(!isExist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BranchCodeChk(string BranchCode,string branchId)
        {

            bool isExist = branchInfoBL.BranchRemoteCodeChk(BranchCode, branchId);
            return Json(!isExist,JsonRequestBehavior.AllowGet);
        }
    }
}