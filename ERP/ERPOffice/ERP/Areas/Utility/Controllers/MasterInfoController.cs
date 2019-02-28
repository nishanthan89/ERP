//--------------------------------------------------------------------------------
// <copyright file="MasterInfoController.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>
// <Author> H.Keerthika, T.Genga, S.Sathiyan </Author>
// <Description> To Manage Master Info Detail</Description>    
//--------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.MvcSecurity;
using ERP.Utility.BL;
using ERP.Utility.Models;
using ERP.Utility.ViewModels;

namespace ERP.Areas.Utility.Controllers
{
	[Authorize]
	[AuthorizeUserAttribute(Permission = 16)]
	public class MasterInfoController : Controller
	{
		private MasterInfoBL mBL = new MasterInfoBL();//private instance of  Masterinfo class

		#region Title        

		/// <summary>
		/// Returns Title list and title creation form
		/// </summary>
		/// <returns></returns>
		public ActionResult Title()
		{
			TitleView titleView = new TitleView();
			titleView.Title = new TitleBO();
			titleView.TitleList = mBL.GetTitleList();
			return View(titleView);
		}

		/// <summary>
		/// Returns title creation view as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateTitle()
		{
			//need to pass model otherwise create title second time without refresh, model state will fail.
			return PartialView("_CreateTitle", new TitleBO());
		}

		/// <summary>
		/// save and update Title
		/// </summary>
		/// <param name="titleBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateTitle(TitleBO titleBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveTitle(titleBO, out eMessage) == true)//Successful Save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//Fail to Save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateTitle", titleBO);//model state is not valid
		}

		/// <summary>
		/// Returns the title list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetTitleList()
		{
			return PartialView("_TitleList", mBL.GetTitleList());
		}

		/// <summary>
		/// Delete the title
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteTitle(int id)
		{
			string msg;
			if (mBL.DeleteTitle(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected data in edit view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditTitle(int id)
		{
			return PartialView("_CreateTitle", mBL.GetTitle(id));
		}

		#endregion

		#region Employee Type		
		/// <summary>
		/// set the view bag value for  reportingTo
		/// method overloading
		/// to use different view bag items in edit and create mode
		/// in edit, block the employee type from the view bag select list item 
		/// </summary>
		/// <param name="employeeTypeId"></param>
		public void GetReportingPerson(int? employeeTypeId)
		{
			if (employeeTypeId != null)//set  value for view bag to Edit function
			{
				ViewBag.Reporting = mBL.GetReportingTo((int)employeeTypeId).Select(e => new SelectListItem()
				{
					Value = e.EmployeeTypeID.ToString(),
					Text = e.EmployeeTypeName
				});
			}
			else//set value for view bag to save function
			{
				ViewBag.Reporting = mBL.GetReportingTo().Select(e => new SelectListItem()
				{
					Value = e.EmployeeTypeID.ToString(),
					Text = e.EmployeeTypeName
				});
			}
		}

		/// <summary>
		/// Returns Employee type List and Create form
		/// </summary>
		/// <returns></returns>
		public ActionResult EmployeeType()
		{
			GetReportingPerson(null);
			EmployeeTypeView empTypeView = new EmployeeTypeView();
			empTypeView.EmployeeType = new EmployeeTypeBO();
			empTypeView.EmployeeTypeList = mBL.GetTypeList();
			return View(empTypeView);
		}

		/// <summary>
		/// Returns Create form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateEmployeeType()
		{
			GetReportingPerson(null);
			return PartialView("_CreateEmployeeType", new EmployeeTypeBO());
		}

		/// <summary>
		/// save and update Employee Type
		/// </summary>
		/// <param name="eTypeBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateEmployeeType(EmployeeTypeBO eTypeBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveEmpType(eTypeBO, out eMessage) == true)//successful save/Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save/Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			//model state is not valid
			GetReportingPerson(null);
			return PartialView("_CreateEmployeeType", eTypeBO);
		}

		/// <summary>
		///Returns the employeeType List as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetTypeList()
		{
			return PartialView("_EmpTypeList", mBL.GetTypeList());
		}

		/// <summary>
		/// Return the selected employee type data
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult EditEmployeeType(int id)
		{
			GetReportingPerson(id);
			return PartialView("_CreateEmployeeType", mBL.GetEmpType(id));
		}

		/// <summary>
		/// Delete the Employee type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteEmployeeType(int id)
		{
			string msg;
			if (mBL.DeleteType(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Ethnic Type
		/// <summary>
		/// Return the Ethnic type list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult EthnicType()
		{
			EthnicTypeView ethnicTypeView = new EthnicTypeView();
			ethnicTypeView.EthnicType = new EthnicTypeBO();
			ethnicTypeView.EthnicTypeList = mBL.GetEthnicList();
			return View(ethnicTypeView);
		}

		/// <summary>
		///Return the create form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateEthnicType()
		{
			return PartialView("_CreateEditEthnicType", new EthnicTypeBO());
		}

		/// <summary>
		/// Save and update Ethnic Type
		/// </summary>
		/// <param name="ethnicTypeBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateEthnicType(EthnicTypeBO ethnicTypeBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveEthnicType(ethnicTypeBO, out eMessage) == true)//Successful save/Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else    //Fail to save/Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateEditEthnicType", ethnicTypeBO);//Model state not valid
		}

		/// <summary>
		/// Return the ethnic type list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetEthnicList()
		{
			return PartialView("_EthnicTypeList", mBL.GetEthnicList());
		}

		/// <summary>
		/// Delete ethnic type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteEthnicType(int id)
		{
			string msg;
			if (mBL.DeleteEthnicType(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected ethnic type detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditEthnicType(int id)
		{
			return PartialView("_CreateEditEthnicType", mBL.GetEditEthnic(id));
		}

		#endregion

		#region Marital Status		
		/// <summary>
		/// Returns the marital status list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult MaritalStatus()
		{
			MaritalTypeView maritalStatus = new MaritalTypeView();
			maritalStatus.MaritalStatus = new MaritalStatusBO();
			maritalStatus.MaritalStatusList = mBL.GetMaritalList();
			return View(maritalStatus);
		}

		/// <summary>
		/// Returns the create form of Marital status as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateMaritalStatus()
		{
			return PartialView("_CreateEditMaritalStatus", new MaritalStatusBO());
		}

		/// <summary>
		/// Save and Update of Marital Status
		/// </summary>
		/// <param name="maritalStausBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateMaritalStatus(MaritalStatusBO maritalStausBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveMaritalStatus(maritalStausBO, out eMessage) == true)//successfully save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateEditMaritalStatus", maritalStausBO);// model state not valid
		}

		/// <summary>
		/// Returns the marital status List
		/// </summary>
		/// <returns></returns>
		public ActionResult GetMaritalStatus()
		{
			return PartialView("_MaritalStatusList", mBL.GetMaritalList());
		}

		/// <summary>
		/// Delete Marital status
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteMaritalStatus(int id)
		{
			string msg;
			if (mBL.DeleteMarital(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected Marital status
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditMaritalStatus(int id)
		{
			return PartialView("_CreateEditMaritalStatus", mBL.GetEditMarital(id));
		}

		#endregion

		#region Immigration Status
		/// <summary>
		/// Returns the Immigration type List and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult ImmigrationType()
		{
			ImmigrationTypeView immigrationView = new ImmigrationTypeView();
			immigrationView.ImmigrationType = new ImmigrationTypeBO();
			immigrationView.ImmigrationTypeList = mBL.GetImmigrationList();
			return View(immigrationView);
		}

		/// <summary>
		/// Returns the create form
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateImmigrationType()
		{
			return PartialView("_CreateEditImmigrationType", new ImmigrationTypeBO());
		}

		/// <summary>
		/// Save and update Immigration Type
		/// </summary>
		/// <param name="immigrationTypeBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateImmigrationType(ImmigrationTypeBO immigrationTypeBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveImmigrationType(immigrationTypeBO, out eMessage) == true)//successful save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save or update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateEditImmigrationType", immigrationTypeBO);//Model state is not valid
		}

		/// <summary>
		/// Returns List as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetImmigrationList()
		{
			return PartialView("_ImmigrationTypeList", mBL.GetImmigrationList());
		}

		/// <summary>
		/// Delete ImmigrationType
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteImmigrationType(int id)
		{
			string msg;
			if (mBL.DeleteImmigration(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected Immigration type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditImmigrationType(int id)
		{
			return PartialView("_CreateEditImmigrationType", mBL.GetEditImmigration(id));
		}
		#endregion

		#region Payment Frequency
		/// <summary>
		/// Returns the Payment type list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult PaymentType()
		{
			PaymentTypeView paymentTypeView = new PaymentTypeView();
			paymentTypeView.PaymentType = new PaymentTypeBO();
			paymentTypeView.PaymentTypeList = mBL.GetPaymentList();
			return View("paymentType", paymentTypeView);
		}

		/// <summary>
		/// Returns the create form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreatePaymentType()
		{
			return PartialView("_CreateEditPaymentType", new PaymentTypeBO());
		}

		/// <summary>
		/// Save and update Payment Type
		/// </summary>
		/// <param name="paymentTypeBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreatePaymentType(PaymentTypeBO paymentTypeBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SavePaymentFrequency(paymentTypeBO, out eMessage) == true)//successful save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateEditPaymentType", paymentTypeBO);//model state not valid
		}

		/// <summary>
		/// Returns the Payment list partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetPaymentList()
		{
			return PartialView("_PaymentTypeList", mBL.GetPaymentList());
		}

		/// <summary>
		/// Delete payment
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeletePaymentType(int id)
		{
			string msg;
			if (mBL.DeletePayment(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected payment type detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditPaymentType(int id)
		{
			return PartialView("_CreateEditPaymentType", mBL.GetEditPayment(id));
		}
		#endregion

		#region Shift Pattern

		/// <summary>
		/// Gets Shift pattern List and Create View for Shift Pattern
		/// </summary>
		/// <returns></returns>
		public ActionResult ShiftPattern()
		{
			ShiftPatternView shift = new ShiftPatternView();
			shift.ShiftPattern = new ShiftPatternBO();
			shift.ShiftPatternList = mBL.GetShiftPatternList();
			return View(shift);
		}

		/// <summary>
		/// Remote Validation to checks Shift Pattern
		/// </summary>
		/// <param name="Description"></param>
		/// <param name="shiftPatternID"></param>
		/// <returns></returns>
		[HttpGet]
		public JsonResult CheckShiftPattern(string description, int shiftPatternID)
		{
			bool isValid = mBL.CheckShiftPatternNameExist(description, shiftPatternID);
			return Json(!isValid, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Creates and Edits Shift Pattern
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateShiftPattern()
		{
			return PartialView("_CreateEditShiftPattern", new ShiftPatternBO());
		}

		/// <summary>
		/// Save and Update the shift pattern
		/// </summary>
		/// <param name="shiftPatternViewBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateShiftPattern(ShiftPatternBO shiftPatternViewBO)
		{
			string error;
			if (ModelState.IsValid)
			{
				if (mBL.SaveShiftPattern(shiftPatternViewBO, out error) == true)//Successful Save or Update based on ID
				{
					return Json(new
					{
						success = true,
						errorMsg = error
					});
				}
				else//Fail to save or Update
				{
					ModelState.AddModelError(string.Empty, error);
				}
			}
			return PartialView("_CreateEditShiftPattern", shiftPatternViewBO);//Model state not valid
		}

		/// <summary>
		/// Gets Shift Pattern List
		/// </summary>
		/// <returns></returns>
		public ActionResult GetShiftPatternList()
		{
			return PartialView("_ShiftPatternList", mBL.GetShiftPatternList());
		}

		/// <summary>
		/// Deletes Shift Pattern
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteShiftPattern(int id)
		{
			string msg;
			if (mBL.DeleteShiftPattern(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Edits Shift Pattern
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditShiftPattern(int id)
		{
			string error = "";
			return PartialView("_CreateEditShiftPattern", mBL.EditShiftPattern(id, out error));
		}

		#endregion ShiftPattern

		#region Nationality
		/// <summary>
		/// Returns the Nationality list and Create form
		/// </summary>
		/// <returns></returns>
		public ActionResult Nationality()
		{
			NationalityView nationalityView = new NationalityView();
			nationalityView.Nationality = new NationalityBO();
			nationalityView.NationalityList = mBL.GetNationalities();
			return View(nationalityView);
		}

		/// <summary>
		/// Returns the Create form partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateNationality()
		{
			return PartialView("_CreateNationality", new NationalityBO());
		}
		/// <summary>
		/// Save and Update Nationality
		/// </summary>
		/// <param name="nationalityBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateNationality(NationalityBO nationalityBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveNationality(nationalityBO, out eMessage) == true)//Successful save or Update
				{
					return Json(new { success = true, errorMsg = eMessage });
				}
				else//Fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateNationality", nationalityBO);//Model state not valid
		}

		/// <summary>
		/// Returns the Nationality list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetNationalityList()
		{
			return PartialView("_NationalityList", mBL.GetNationalities());
		}

		/// <summary>
		/// Returns the selected Nationality detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditNationality(int id)
		{
			return PartialView("_CreateNationality", mBL.GetNationality(id));
		}

		/// <summary>
		/// Delete nationality
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteNationality(int id)
		{
			string msg;
			if (mBL.DeleteNationality(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Timesheet Frequency        		
		/// <summary>
		/// Get the day of week list to populate in drop-down list
		/// </summary>
		public void getDayofWeek()
		{
			ViewBag.day = mBL.GetDayofweek().Select(d => new SelectListItem()
			{
				Value = d.DayName,
				Text = d.DayName
			});
		}

		/// <summary>
		/// Returns the Time sheet frequency List and Create form
		/// </summary>
		/// <returns></returns>
		public ActionResult TSFrequency()
		{
			TSFrequencyView frequencyView = new TSFrequencyView();
			frequencyView.Frequency = new TSFrequencyBO();
			frequencyView.TSFrequencyList = mBL.GetFrequencyList();
			return View(frequencyView);
		}

		/// <summary>
		/// Update Time sheet frequency
		/// </summary>
		/// <param name="frequencyBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateTSFrequency(TSFrequencyBO frequencyBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.UpdateTSFrequency(frequencyBO, out eMessage) == true)//successful update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//Fail to update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateTSFrequency", frequencyBO);//Model state not valid
		}

		/// <summary>
		/// Returns the list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetTSFrequencyList()
		{
			return PartialView("_TSFrequencyList", mBL.GetFrequencyList());
		}

		/// <summary>
		/// Returns the selected Time sheet frequency detail to edit
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditTSFrequency(int id)
		{
			TSFrequencyView frequencyView = new TSFrequencyView();
			frequencyView.Frequency = mBL.GetTSFrequency(id);
			if (frequencyView.Frequency.TimeSheetFrequencyName.ToLower() == "weekly")
			{
				getDayofWeek();
			}

			return PartialView("_CreateTSFrequency", frequencyView.Frequency);
		}

		#endregion

		#region Currency        
		/// <summary>
		/// Returns the currency list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult Currency()
		{
			CurrencyView currencyView = new CurrencyView();
			currencyView.Currency = new CurrencyBO();
			currencyView.currencyList = mBL.GetCurrencyList();
			return View(currencyView);
		}

		/// <summary>
		/// Returns the create form partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateCurrency()
		{
			return PartialView("_CreateCurrency", new CurrencyBO());
		}

		/// <summary>
		/// save and update Currency
		/// </summary>
		/// <param name="currencyBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateCurrency(CurrencyBO currencyBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveCurrency(currencyBO, out eMessage))//successful save
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateCurrency", currencyBO);//model state failed
		}

		/// <summary>
		/// Returns the List as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCurrencyList()
		{
			return PartialView("_CurrencyList", mBL.GetCurrencyList());
		}

		/// <summary>
		/// Delete Currency
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteCurrency(int id)
		{
			string msg;
			if (mBL.DeleteCurrency(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns selected Currency detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditCurrency(int id)
		{
			return PartialView("_CreateCurrency", mBL.GetCurrency(id));
		}

		#endregion

		#region Country        
		/// <summary>
		/// Returns the Country list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult Country()
		{
			CountryView countryView = new CountryView();
			countryView.Country = new CountryBO();
			countryView.CountryList = mBL.GetCountryList();
			return View(countryView);
		}

		/// <summary>
		/// Returns the create form partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateCountry()
		{
			return PartialView("_CreateCountry", new CountryBO());
		}

		/// <summary>
		/// Save and Update actions
		/// </summary>
		/// <param name="countryBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateCountry(CountryBO countryBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveCountry(countryBO, out eMessage) == true)   //Successful save/Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//Fail to save/Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateCountry", countryBO);//Model state not valid
		}

		/// <summary>
		/// Returns the Country list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCountryList()
		{
			return PartialView("_CountryList", mBL.GetCountryList());
		}

		/// <summary>
		/// Delete Country
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteCountry(int id)
		{
			string msg;
			if (mBL.DeleteCountry(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Returns the selected country detail to edit
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditCountry(int id)
		{
			return PartialView("_CreateCountry", mBL.GetCountry(id));
		}

		/// <summary>
		/// Remote validation to check whether the entered country name is already exist.
		/// </summary>
		/// <param name="countryName,countryId"></param>
		/// <returns></returns>
		public JsonResult CheckCountryNameExist(string countryName, int countryId)
		{
			bool isCountryExist = mBL.CheckCountryName(countryName, countryId);
			return Json(!isCountryExist, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Remote validation to check whether the entered country Code is already exist.
		/// </summary>
		/// <param name="countryCode,countryId"></param>
		/// <returns></returns>
		public JsonResult CheckCountryCodeExist(string countryCode, int countryId)
		{
			bool isCountryCodeExist = mBL.CheckCountryCode(countryCode, countryId);
			return Json(!isCountryCodeExist, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Resource Holiday Type        
		/// <summary>
		/// Returns the resource Holiday type list and create form
		/// </summary>
		/// <returns></returns>
		public ActionResult ResourceHolidayType()
		{
			ResourceHolidayTypeView rholidayTypeView = new ResourceHolidayTypeView();
			rholidayTypeView.RHolidayTypeBO = new ResourceHolidayTypeBO();
			rholidayTypeView.RHolidayTypeList = mBL.GetHolidayTypeList();
			return View(rholidayTypeView);
		}

		/// <summary>
		/// Returns the Create form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateResourceHolidayType()
		{
			return PartialView("_CreateResourceHolidayType", new ResourceHolidayTypeBO());
		}

		/// <summary>
		/// Save and Update
		/// </summary>
		/// <param name="holidayTypeBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateResourceHolidayType(ResourceHolidayTypeBO holidayTypeBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveHolidayType(holidayTypeBO, out eMessage) == true)
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateResourceHolidayType", holidayTypeBO);//Model state is not valid
		}

		/// <summary>
		/// Returns the Holiday type list
		/// </summary>
		/// <returns></returns>         
		public ActionResult GetResourceHolidayTypeList()
		{
			return PartialView("_ResourceHolidayTypeList", mBL.GetHolidayTypeList());
		}

		/// <summary>
		/// Delete the Resource holiday type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteResourceHolidayType(int id)
		{
			string msg;
			if (mBL.DeleteResourceHolidayType(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Return the selected data in edit view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditResourceHolidayType(int id)
		{
			return PartialView("_CreateResourceHolidayType", mBL.GetResourceHolidayType(id));
		}

		/// <summary>
		/// Remotely validate whether the Holiday Type name is already exist or not.
		/// </summary>
		/// <param name="holidayTypeName"></param>
		/// <param name="holidayTypeID"></param>
		/// <returns></returns>
		public JsonResult CheckHolidayTypeExist(string holidayTypeName, int holidayTypeID)
		{
			bool isHolidayTypeExist = mBL.CheckHolidayType(holidayTypeName, holidayTypeID);
			return Json(!isHolidayTypeExist, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Remotely validate whether the abbreviation is already exist.
		/// </summary>
		/// <param name="abbriviatedHolidayType"></param>
		/// <param name="holidayTypeID"></param>
		/// <returns></returns>
		public JsonResult CheckAbbreviationExist(string abbriviatedHolidayType, int holidayTypeID)
		{
			bool isAbbreviationExist = mBL.CheckAbbreviation(abbriviatedHolidayType, holidayTypeID);
			return Json(!isAbbreviationExist, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region SkillCategory

		/// <summary>
		/// Returns the view with Skill Category list and Skill Category creation form
		/// </summary>
		/// <returns></returns>
		public ActionResult SkillCategory()
		{
			ViewBag.CurrentTab = "SkillCategory";
			SkillCategoryView categoryView = new SkillCategoryView();
			categoryView.SkillCategory = new SkillCategoryBO();
			categoryView.SkillCategoryList = mBL.GetSkillCategoryList();
			return View(categoryView);
		}

		/// <summary>
		/// Returns the Skill Category creation form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateSkillCategory()
		{
			return PartialView("_CreateSkillCategory", new SkillCategoryBO());
		}

		/// <summary>
		/// Accept skillCategoryBO model with user entered data and do save/Update and return the Json result 
		/// </summary>
		/// <param name="skillCategoryBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateSkillCategory(SkillCategoryBO skillCategoryBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveSkillCategory(skillCategoryBO, out eMessage) == true)//successful Save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to Save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateSkillCategory", skillCategoryBO);//model state is not valid
		}

		/// <summary>
		/// Return the Skill Category list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetSkillCategoryList()
		{
			return PartialView("_SkillCategoryList", mBL.GetSkillCategoryList());
		}

		/// <summary>
		///Delete the Skill Category
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteSkillCategory(int id)
		{
			string msg;
			if (mBL.DeleteSkillCategory(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Get the selected SKill Category detail and return as a partial view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditSkillCategory(int id)
		{
			return PartialView("_CreateSkillCategory", mBL.GetSkillCategory(id));
		}

		public ActionResult ShowSkills(int? id)
		{
			return PartialView("_ShowSkills", mBL.GetSkill(id));
		}

		#endregion

		#region SkillLevelScheme
		/// <summary>
		/// Returns the view with Skill Level list and Skill Level creation form
		/// </summary>
		/// <returns></returns>
		public ActionResult SkillLevel()
		{
			ViewBag.CurrentTab = "SkillLevel";
			SkillLevelView skillLevelView = new SkillLevelView();
			skillLevelView.SkillLevel = new SkillLevelBO();
			skillLevelView.SkillLevelList = mBL.GetSkillLevelList();
			return View(skillLevelView);
		}

		/// <summary>
		/// Returns the Skill Level creation form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateSkillLevel()
		{
			return PartialView("_CreateSkillLevel", new SkillLevelBO());
		}

		/// <summary>
		/// Accept skillLevelBO model with user entered data and do save/Update and return the Json result 
		/// </summary>
		/// <param name="skillLevelBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateSkillLevel(SkillLevelBO skillLevelBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveSkillLevel(skillLevelBO, out eMessage) == true)//successful Save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to Save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateSkillLevel", skillLevelBO);//model state is not valid
		}

		/// <summary>
		/// Return the Skill Level list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetSkillLevelList()
		{
			return PartialView("_SkillLevelList", mBL.GetSkillLevelList());
		}

		/// <summary>
		///Delete the Skill Level
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteSkillLevel(int id)
		{
			string msg;
			if (mBL.DeleteSkillLevel(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Get the selected SKill Level detail and return as a partial view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditSkillLevel(int id)
		{
			return PartialView("_CreateSkillLevel", mBL.GetSkillLevel(id));
		}
		#endregion

		#region Skill

		private void SetViewBag()
		{
			//For Skill Level          
			ViewBag.SkillLevel = (mBL.GetSkillLevelList().Select(s => new SelectListItem()
			{
				Value = s.SkillLevelID.ToString(),
				Text = s.SkillLevelName
			}));

		}
		/// <summary>
		/// Returns the view with Skill list and Skill creation form
		/// </summary>
		/// <returns></returns>
		public ActionResult Skill()
		{
			SetViewBag();
			ViewBag.CurrentTab = "Skill";
			SkillView skill = new SkillView();
			skill.SkillList = mBL.GetSkillList();			
			return View(skill);
		}

		/// <summary>
		/// Returns the Skill creation form as a partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateSkill()
		{
			SetViewBag();
			SkillView skill = new SkillView();
			skill.SkillCategoryGroup = mBL.GetSkillCatList();
			skill.Skill = new SkillBO();			
			return PartialView("_CreateSkill", skill);
		}

		/// <summary>
		/// Accept skillcreationBO model with user entered data and do save/Update and return the Json result 
		/// </summary>
		/// <param name="skillcreationBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateSkill(SkillView skillcreationBO)
		{
			SetViewBag();
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveSkill(skillcreationBO, out eMessage) == true)//successful Save or Update
				{
					return Json(new
					{
						success = true,
						errorMsg = eMessage
					});
				}
				else//fail to save
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateSkill", skillcreationBO);//model state is not valid
		}

		/// <summary>
		/// Return the Skill list as a partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetSkillList()
		{
			return PartialView("_SkillList", mBL.GetSkillList());
		}

		/// <summary>
		///Delete the Skill 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteSkill(int id)
		{
			string msg;
			if (mBL.DeleteSkill(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Get the selected SKill detail and return as a partial view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditSkill(int id)
		{
			SetViewBag();
			//SkillView skillView = mBL.GetSkill(id);
			//return PartialView("_CreateSkill", skillView);
			//var x = mBL.GetSkill(id);
			return PartialView("_CreateSkill", mBL.GetSkill(id));
		}

		/// <summary>
		/// Return the Skill Categories and Skill Levels for that Skill as a partial view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ShowSkillCategoryLevel(int? id)
		{
			return PartialView("_ShowSkillCategoryLevel", mBL.GetSkillCategoryLevel(id));
		}

		/// <summary>
		/// return the list of employees who have that skill, as a partial view
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ShowEmployees(int id)
		{
			return PartialView("_ShowEmployees", mBL.GetEmployees(id));
		}
		#endregion
		#region Designation
		/// <summary>
		/// Return the Designation list and Create form
		/// </summary>
		/// <returns></returns>
		public ActionResult Designation()
		{
			DesignationView designationView = new DesignationView();
			designationView.Designation = new DesignationBO();//to pass in create form
			designationView.DesignationList = mBL.GetDesignationList();
			return View(designationView);
		}

		/// <summary>
		/// Return the Create form partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateDesignation()
		{
			return PartialView("_CreateDesignation", new DesignationBO());
		}
		/// <summary>
		/// Save and Update Designation
		/// </summary>
		/// <param name="DesignationBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateDesignation(DesignationBO designationBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveDesignation(designationBO, out eMessage) == true)//Successful save or Update
				{
					return Json(new { success = true, errorMsg = eMessage });
				}
				else//Fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateDesignation", designationBO);//Model state not valid
		}

		/// <summary>
		/// Return the Designation list as partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetDesignationList()
		{
			return PartialView("_DesignationList", mBL.GetDesignationList());
		}

		/// <summary>
		/// Return the selected Designation detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditDesignation(int id)
		{
			return PartialView("_CreateDesignation", mBL.GetDesignation(id));
		}

		/// <summary>
		/// Delete Designation
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteDesignation(int id)
		{
			string msg;
			if (mBL.DeleteDesignation(id, out msg) == true)//Successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Company
		/// <summary>
		/// Return the Company list and Create form
		/// </summary>
		/// <returns></returns>
		public ActionResult Company()
		{
			CompanyView companyView = new CompanyView();
			companyView.Company = new CompanyBO();
			companyView.CompanyList = mBL.GetCompanyList();
			return View(companyView);
		}

		/// <summary>
		/// Return the Create form partial view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateCompany()
		{
			return PartialView("_CreateCompany", new CompanyBO());
		}
		/// <summary>
		/// Save and Update Company
		/// </summary>
		/// <param name="CompanyBO"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateCompany(CompanyBO companyBO)
		{
			string eMessage;
			if (ModelState.IsValid)
			{
				if (mBL.SaveCompany(companyBO, out eMessage) == true)//Successful save or Update
				{
					return Json(new { success = true, errorMsg = eMessage });
				}
				else//Fail to save or Update
				{
					ModelState.AddModelError(string.Empty, eMessage);
				}
			}
			return PartialView("_CreateCompany", companyBO);//Model state not valid
		}

		/// <summary>
		/// Load the Company list partial view
		/// </summary>
		/// <returns></returns>
		public ActionResult GetCompanyList()
		{
			return PartialView("_CompanyList", mBL.GetCompanyList());
		}

		/// <summary>
		/// return the selected Company detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult EditCompany(int id)
		{
			return PartialView("_CreateCompany", mBL.GetCompany(id));
		}

		/// <summary>
		/// Delete Company
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteCompany(int id)
		{
			string msg;
			if (mBL.DeleteCompany(id, out msg) == true)//successfully Deleted
			{
				return Json(new { success = true, errorMsg = msg }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, errorMsg = msg }, JsonRequestBehavior.AllowGet);
		}
		#endregion
	}
}