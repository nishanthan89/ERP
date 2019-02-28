///-----------------------------------------------------------------
///   Namespace:      ERP.Areas.Admin.Controllers
///   Class:          CompanyInfoController
///   Description:    Company Details Of ERP System 
///   Author:         S.Sathiyan                   Date: 02/05/2016
///   Notes:           You can  Update  Only One Main Company for ERP System 
///   Revision History:  Last Update ON: 22/07/2016
///   Name:   s.sathiyan        Date: 10/06/2016      
///-----------------------------------------------------------------
using ERP.Address.BL;
using ERP.Admin.BL;
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
    /// <summary>
    ///   Updating Company Details 
    /// </summary>
    public class CompanyInfoController : Controller
    {
        private CompanyInfoBL cmyInfoBL = new CompanyInfoBL();
        private AddressBL addressBL = new AddressBL();

        /// <summary>
        ///  For DropDown List
        /// </summary>
        public void _ViewBagList()
        {
            ViewBag.TimeFormatList = (cmyInfoBL.CommonTime().Select(x => new SelectListItem() { Value = x.TimeID.ToString(), Text = x.TimeFormat.ToString() }));
            ViewBag.DateFormatList = (cmyInfoBL.CommonDate().Select(x => new SelectListItem() { Value = x.DateFormatID.ToString(), Text = x.DateFormat }));
            ViewBag.CurrencyList = (cmyInfoBL.CommonCurrency().Select(x => new SelectListItem() { Value = x.CurrencyID.ToString(), Text = x.Currency }));
            ViewBag.TimeZoneList = (cmyInfoBL.GetTimeZone().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.DisplayName }));
            ViewBag.CountryList = addressBL.GetCountryList().Select(x => new SelectListItem() { Value = x.CountryID.ToString(), Text = x.CountryName, Selected = x.IsSelected });
        }

        /// <summary>
        ///  GET: Admin/CompanyInfo
        /// </summary>
        /// <returns>
        /// Company Info Values 
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {

            _ViewBagList();

            ViewBag.TempData1 = TempData["Success"];
            ViewBag.TempData2 = TempData["Error"];
            var cmyInfo = cmyInfoBL.CmyInfoDets();
            return View(cmyInfo);
        }

        /// <summary>
        /// Update your Company Info 
        /// </summary>
        /// <param name="companyInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(CompanyInfoViewModel companyInfo, HttpPostedFileBase UploadFile)
        {
            string errorMsgs = "";
            if (ModelState.IsValid)
            {

                if (cmyInfoBL.UpdateCompanyInfo(companyInfo, out errorMsgs, UploadFile))
                {
                    TempData["Success"] = errorMsgs;
                    MvcApplication.Globalvariables();

                }

                else
                {

                    TempData["Error"] = errorMsgs;


                    // ModelState.AddModelError(string.Empty, errorMsgs);
                }


                //  _ViewBagList();
            }
            //  return Content("<script language='javascript' type='text/javascript'>swal('Thanks for Feedback!');</script>");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveLogo(int id)
        {


            string error = "";
            if (cmyInfoBL.DeleteImage(id, out error))
            {
                return Json(new

                { success = true, errorMsg = error });

            }

            else
            {
                return Json(new

                { success = false, errorMsg = error });
            }

        }

        /// <summary>
        /// Loading An Image From DB to View
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult LoadImage(string imageId)
        {
            try
            {

                return File(cmyInfoBL.GetImage(imageId), "image/png");
            }
            catch
            {
                return null; /*File(Server.MapPath("~/Content/dist/img/ERPLogo.png"), "image/png");*/
            }

        }

    }
}