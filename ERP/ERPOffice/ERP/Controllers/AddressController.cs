///-----------------------------------------------------------------
///   Namespace:      ERP.Controllers
///   Class:          AddressController
///   Description:    Common Address Details Of ERP System 
///   Author:         S.Sathiyan                   Date: 02/05/2016
///   Notes:          you Can View the UK Address Details Using PAF Code Services 
///   Revision History: Last Update ON 22/07/2016
///   Name:   s.sathiyan        Date: 10/06/2016      
///-----------------------------------------------------------------

using ERP.Address.BL;
using ERP.Address.ViewModels;
using ERP.Admin.Models;
using ERP.Admin.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class AddressController : Controller
    {
        // GET: Address

        public AddressBL addressBL = new AddressBL();

        /// <summary>
        /// to render address user control,
        /// get postcode address file
        /// </summary>
        /// <param name="postcode"></param>
        /// <param name="countryid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PAF(string postcode, int countryid)
        {
            //if (Request.IsAjaxRequest() == true) { }

            AddressViewModel addressViewModel = new AddressViewModel();
            if (postcode != "")
            {
                if (countryid == 1)
                {
                    string dataKey = "I_22FA798668834EA4A266379226DD9A";
                    string username = "user1";
                    string searchtype = "UK";

                    uk.co.simplylookupadmin.www.WebService getPostCode = new uk.co.simplylookupadmin.www.WebService();
                    uk.co.simplylookupadmin.www.PL_AddressRecord returnadd = new uk.co.simplylookupadmin.www.PL_AddressRecord();
                    returnadd = getPostCode.SearchForThoroughfareAddress(dataKey, username, searchtype, postcode);
                    if (returnadd.AddressRecordGotWithoutError == true)
                    {
                        addressViewModel.StreetName = returnadd.Line1;
                        addressViewModel.Locality = returnadd.Line2;
                        addressViewModel.Town = returnadd.Town;
                        addressViewModel.County = returnadd.CountyState;
                        addressViewModel.Postcode = returnadd.PostZipCode;
                        addressViewModel.CountryID = countryid;
                    }
                }
                else
                {

                    addressViewModel.Postcode = postcode;
                    addressViewModel.CountryID = countryid;
                    return Json(new
                    {
                        success = false,
                        errorMsg = "This Country Doesn't Have A PAF Address...!!"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorMsg = "Please Enter your PostCode....!!!"
                });
            }
            AddressViewModel addressModel = new AddressViewModel();
            addressModel = addressViewModel;
            //ViewBag.CountryList = addressBL.GetCountryList().Select(x => new SelectListItem() { Value = x.CountryID.ToString(), Text = x.CountryName, Selected = x.IsSelected });
            //addressViewModel.CountryID = addressBL.GetCountryList().Where(x => x.IsSelected == true).Select(x => x.CountryID).SingleOrDefault();
            if(addressModel.County!="" || addressModel.BuildingName!=null|| addressModel.StreetName!="" || addressModel.Locality!="" || addressModel.Town!="")
            {
                return Json(addressModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorMsg = "This PAF Code Is Not In Our Service...!!!"
                });
            }
        }
    }
}
