//--------------------------------------------------------------------------------
// <copyright file="CountryBL.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

// <Author>H.Keerthika, T.Genga, S.Sathiyan </Author>
// <Description> Business logic of Country </Description>    
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.DA;
using ERP.Utility.Models;
using ERP.Utility.ViewModels;
using EntityFramework.Extensions;//library the extends the functionality of Entity Framework to use like DELETE method.

namespace ERP.Utility.BL
{
	public class MasterInfoBL
	{
		private ERPEntities db = new ERPEntities();

		#region Country

		/// <summary>
		/// Get Country List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CountryBO> GetCountryList()
		{
			var countryList = (from c in db.Common_Country
							   select new CountryBO()
							   {
								   CountryID = c.CountryID,
								   CountryCode = c.CountryCode,
								   CountryName = c.CountryName,
								   IsSelected = c.IsSelected
							   }).OrderBy(c => c.CountryName).ToList();
			return countryList;
		}

		/// <summary>
		/// Save Country Details
		/// </summary>
		/// <param name="countryBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveCountry(CountryBO countryBO, out string eMessage)
		{
			var existingCountry = (from c in db.Common_Country
								   where c.CountryName.Equals(countryBO.CountryName) && c.CountryID != countryBO.CountryID
								   select c).SingleOrDefault();
			try
			{
				if (existingCountry == null)
				{
					var dbcountry = db.Common_Country.SingleOrDefault(c => c.CountryID == countryBO.CountryID);

					if (countryBO.CountryID == 0)
					{
						var country = new Common_Country();
						country.CountryName = countryBO.CountryName;
						country.CountryCode = countryBO.CountryCode;
						country.IsSelected = countryBO.IsSelected;
						db.Common_Country.Add(country);
					}
					else if (dbcountry != null)
					{
						dbcountry.CountryName = countryBO.CountryName;
						dbcountry.CountryCode = countryBO.CountryCode;
						dbcountry.IsSelected = countryBO.IsSelected;
					}
					else
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}

					//when saving country, with isSelected is true, then need to check and make isSelected as false to already selected country
					if (countryBO.IsSelected == true)
					{
						var selectedCountry = db.Common_Country.SingleOrDefault(c => c.IsSelected == true);
						if (selectedCountry != null)
						{
							selectedCountry.IsSelected = false;
						}
					}
					db.SaveChanges();
					eMessage = "Successfully Saved!!";
					return true;
				}

				eMessage = "Country Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}

		}

		/// <summary>
		/// Delete a selected Country record from the list
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteCountry(int id, out string msg)
		{
			msg = "";
			var item = db.Common_Country.SingleOrDefault(c => c.CountryID == id);
			try
			{
				if (item != null)
				{
					if (item.Common_Address.Count == 0)
					{
						db.Common_Country.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed";
						return true;
					}
					else
					{
						msg = "This Country Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}

				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the a selected country detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CountryBO GetCountry(int id)
		{
			var country = (from c in db.Common_Country
						   where c.CountryID == id
						   select new CountryBO()
						   {
							   CountryCode = c.CountryCode,
							   CountryName = c.CountryName,
							   IsSelected = c.IsSelected,
							   CountryID = c.CountryID
						   }).SingleOrDefault();
			if (country != null)
			{
				return country;
			}
			return new CountryBO();
		}

		/// <summary>
		///Method to check whether the entered country Name is already exist
		/// </summary>
		/// <param name="countryName"></param>
		/// <returns></returns>
		public bool CheckCountryName(string countryName, int countryId)
		{
			var existingName = db.Common_Country.SingleOrDefault(c => c.CountryName == countryName && c.CountryID != countryId);
			//country already exist with same name
			if (existingName != null)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// method to check whether the entered country Code is already exist
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public bool CheckCountryCode(string countryCode, int countryId)
		{
			var isCodeExist = db.Common_Country.Where(c => c.CountryCode == countryCode && c.CountryID != countryId)
				.SingleOrDefault() != null;
			return isCodeExist;
		}
		#endregion
		#region Currency
		/// <summary>
		/// Get the Currency List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CurrencyBO> GetCurrencyList()
		{
			var currencyList = (from c in db.Common_CurrencyFormat
								select new CurrencyBO
								{
									Currency = c.Currency,
									CurrencyID = c.CurrencyID
								}).OrderBy(cu => cu.Currency).ToList();
			return currencyList;
		}
		/// <summary>
		/// Save and Update Currency detail
		/// </summary>
		/// <param name="currencyBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveCurrency(CurrencyBO currencyBO, out string eMessage)
		{
			var existingCurrency = db.Common_CurrencyFormat.SingleOrDefault(c => c.Currency == currencyBO.Currency && c.CurrencyID != currencyBO.CurrencyID);
			var dbCurrency = db.Common_CurrencyFormat.SingleOrDefault(c => c.CurrencyID == currencyBO.CurrencyID);

			try
			{
				if (existingCurrency == null)
				{
					if (currencyBO.CurrencyID == 0)//save
					{
						var currency = new Common_CurrencyFormat();
						currency.Currency = currencyBO.Currency;
						db.Common_CurrencyFormat.Add(currency);
						eMessage = "Successfully Saved!!";
					}
					else if (dbCurrency != null)//Update
					{
						dbCurrency.Currency = currencyBO.Currency;
						eMessage = "Successfully Updated!!";
					}
					else
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				eMessage = "Currency Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete selected Currency
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteCurrency(int id, out string msg)
		{
			msg = "";
			var item = db.Common_CurrencyFormat.SingleOrDefault(c => c.CurrencyID == id);//select item to delete
			try
			{
				if (item != null)
				{
					var isCurrencyUsed = db.Common_HostSettings.Where(x => x.HostKey.Equals("Currency"))
						.Single().HostValue == item.Currency;//check whether the currency already in use
					if (isCurrencyUsed)//true
					{
						msg = "This Currency Is Already Used In Company Info So Cannot Be Deleted";
						return false;
					}
					db.Common_CurrencyFormat.Remove(item);
					db.SaveChanges();
					msg = "Successfully Removed!";
					return true;
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the selected currency detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CurrencyBO GetCurrency(int id)
		{
			var currency = (from c in db.Common_CurrencyFormat
							where c.CurrencyID == id
							select new CurrencyBO()
							{
								CurrencyID = c.CurrencyID,
								Currency = c.Currency
							}).SingleOrDefault();
			if (currency != null)
			{
				return currency;
			}
			return new CurrencyBO();
		}
		#endregion
		#region Employee Type
		/// <summary>
		/// Get the Employee type List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<EmployeeTypeBO> GetTypeList()
		{
			var typeList = (from t in db.Common_EmployeeType
							select new EmployeeTypeBO()
							{
								Description = t.Description,
								EmployeeTypeID = t.EmployeeTypeID,
								EmployeeTypeName = t.EmployeeTypeName,
								ReportingTo = t.ReportingTo,
								ReportingEmployee = (from r in db.Common_EmployeeType
													 where r.EmployeeTypeID == t.ReportingTo
													 select r.EmployeeTypeName).FirstOrDefault(),

							}).OrderBy(t => t.EmployeeTypeName).ToList();
			return typeList;
		}
		/// <summary>
		/// Save Employee type
		/// </summary>
		/// <param name="eTypeBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveEmpType(EmployeeTypeBO eTypeBO, out string eMessage)
		{
			eMessage = "";
			var existingType = db.Common_EmployeeType.SingleOrDefault(t => t.EmployeeTypeName == eTypeBO.EmployeeTypeName && t.EmployeeTypeID != eTypeBO.EmployeeTypeID);
			var type = db.Common_EmployeeType.SingleOrDefault(t => t.EmployeeTypeID == eTypeBO.EmployeeTypeID);
			try
			{
				if (existingType == null)
				{
					if (eTypeBO.EmployeeTypeID == 0)
					{
						var empType = new Common_EmployeeType();
						empType.EmployeeTypeName = eTypeBO.EmployeeTypeName;
						empType.ReportingTo = eTypeBO.ReportingTo;
						empType.Description = eTypeBO.Description;
						db.Common_EmployeeType.Add(empType);
						db.SaveChanges();
						eMessage = "Successfully Saved!";
						return true;
					}
					else if (type != null)
					{
						type.Description = eTypeBO.Description;
						type.ReportingTo = eTypeBO.ReportingTo;
						db.SaveChanges();
						eMessage = "Successfully updated!!";
						return true;
					}
					else
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
				}
				else
				{
					eMessage = "Resource Type Already Exist!";
					return false;
				}
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the Selected Employee type Detail from DB
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public EmployeeTypeBO GetEmpType(int id)
		{
			var empType = (from t in db.Common_EmployeeType
						   where t.EmployeeTypeID == id
						   select new EmployeeTypeBO()
						   {
							   EmployeeTypeID = t.EmployeeTypeID,
							   Description = t.Description,
							   EmployeeTypeName = t.EmployeeTypeName,
							   ReportingTo = t.ReportingTo
						   }).SingleOrDefault();

			if (empType == null)
			{
				return new EmployeeTypeBO();
			}
			return empType;
		}

		/// <summary>
		/// Delete Employee Type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteType(int id, out string msg)
		{
			msg = "";
			var item = db.Common_EmployeeType.SingleOrDefault(t => t.EmployeeTypeID == id);
			try
			{
				if (item != null)
				{
					var reportingTo = (from e in db.Common_EmployeeType where item.EmployeeTypeID == e.ReportingTo select e).ToList() ?? null;
					if (item.Resource_Employee.Count != 0)
					{
						msg = "This Resource Type Is Already In Use So Cannot Be Deleted.";
						return false;
					}
					else if (reportingTo != null && reportingTo.Count != 0)//check the selected employee type is used for another employee type's  reporting to
					{
						msg = "This Resource Type Is Already In Use! ";
						return false;
					}
					////////////////////////////////////////////////             
					db.Common_EmployeeType.Remove(item);
					db.SaveChanges();
					msg = "Successfully Removed!";
					return true;
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}

		}
		/// <summary>
		/// Get drop-down list values of reporting to
		/// Overloaded method
		/// drop-down list values for create functions
		/// </summary>
		/// <returns></returns>
		public IEnumerable<EmployeeTypeBO> GetReportingTo()
		{


			var reprtingPersons = (from r in db.Common_EmployeeType
								   select new EmployeeTypeBO()
								   {
									   EmployeeTypeID = r.EmployeeTypeID,
									   EmployeeTypeName = r.EmployeeTypeName
								   }).OrderBy(r => r.EmployeeTypeName).ToList();
			return reprtingPersons;

		}
		/// <summary>
		///  Get drop-down list values of reporting to
		///  Overloaded method
		///  drop-down list values for Edit function
		///  </summary>
		///  <param name="employeeTypeId"></param>
		/// <returns></returns>
		public IEnumerable<EmployeeTypeBO> GetReportingTo(int employeeTypeId)
		{
			IEnumerable<EmployeeTypeBO> reprtingPersons = null;

			if (employeeTypeId > 0)
			{
				reprtingPersons = (from r in db.Common_EmployeeType
								   where r.EmployeeTypeID != (int)employeeTypeId
								   select new EmployeeTypeBO()
								   {
									   EmployeeTypeID = r.EmployeeTypeID,
									   EmployeeTypeName = r.EmployeeTypeName
								   }).ToList();
				//return reprtingPersons;
			}
			return reprtingPersons;
		}
		#endregion
		#region Ethnic Type
		/// <summary>
		/// Get the Ethnic type List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<EthnicTypeBO> GetEthnicList()
		{
			var ethnicList = (from et in db.Common_EthnicGroup
							  select new EthnicTypeBO
							  {
								  EthGrpID = et.EthnicGroupID,
								  EthGrpName = et.EthnicGroupName

							  }).OrderBy(e => e.EthGrpName).ToList();
			return ethnicList;
		}

		/// <summary>
		/// Save the Ethnic Type
		/// </summary>
		/// <param name="ethnicTypeBO"></param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public bool SaveEthnicType(EthnicTypeBO ethnicTypeBO, out string errorMsg)
		{
			var existingEthnic = db.Common_EthnicGroup.SingleOrDefault(e => e.EthnicGroupName == ethnicTypeBO.EthGrpName && e.EthnicGroupID != ethnicTypeBO.EthGrpID);
			var dbEthnicGroup = db.Common_EthnicGroup.SingleOrDefault(e => e.EthnicGroupID == ethnicTypeBO.EthGrpID);
			try
			{
				if (existingEthnic == null)
				{
					if (ethnicTypeBO.EthGrpID == 0)//save
					{
						var newEthnic = new Common_EthnicGroup();
						newEthnic.EthnicGroupName = ethnicTypeBO.EthGrpName;
						db.Common_EthnicGroup.Add(newEthnic);
						errorMsg = "Successfully Saved!";
					}
					else if (dbEthnicGroup != null)//update
					{
						dbEthnicGroup.EthnicGroupName = ethnicTypeBO.EthGrpName;
						errorMsg = "Successfully Updated!";
					}
					else
					{
						errorMsg = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				else//ethnic already exist
				{
					errorMsg = "Ethnicity Is Already Exist!";
					return false;
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Ethnic Type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteEthnicType(int id, out string msg)
		{
			msg = "";
			var item = db.Common_EthnicGroup.SingleOrDefault(t => t.EthnicGroupID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_Employee.Count == 0)
					{
						db.Common_EthnicGroup.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Ethnic Group Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		/// <summary>
		/// Get the detail of selected Ethnic group to Edit
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public EthnicTypeBO GetEditEthnic(int id)
		{
			var editEthnic = (from et in db.Common_EthnicGroup.Where(x => x.EthnicGroupID.Equals(id))
							  select new EthnicTypeBO()
							  {
								  EthGrpID = et.EthnicGroupID,
								  EthGrpName = et.EthnicGroupName

							  }).SingleOrDefault();
			return editEthnic;
		}

		#endregion
		#region Immigration Status
		/// <summary>
		/// Get Immigration Type List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ImmigrationTypeBO> GetImmigrationList()
		{
			var immigration = (from et in db.Common_ImmigrationStatus
							   select new ImmigrationTypeBO
							   {
								   ImmigrationTypeId = et.ImmigrationStatusID,
								   ImmigrationStatus = et.ImmigrationName
							   }).OrderBy(i => i.ImmigrationStatus).ToList();
			return immigration;
		}
		/// <summary>
		/// Save and Update Immigration Type
		/// </summary>
		/// <param name="immigrationTypeBO"></param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public bool SaveImmigrationType(ImmigrationTypeBO immigrationTypeBO, out string errorMsg)
		{
			var exitingImmigration = db.Common_ImmigrationStatus.SingleOrDefault(x => x.ImmigrationName == immigrationTypeBO.ImmigrationStatus && x.ImmigrationStatusID != immigrationTypeBO.ImmigrationTypeId);
			var dbImmigration = db.Common_ImmigrationStatus.SingleOrDefault(i => i.ImmigrationStatusID == immigrationTypeBO.ImmigrationTypeId);
			try
			{
				if (exitingImmigration == null)//This Immigration Type not exist in database
				{
					if (immigrationTypeBO.ImmigrationTypeId == 0)//save
					{
						var immigrationcSave = new Common_ImmigrationStatus();
						immigrationcSave.ImmigrationName = immigrationTypeBO.ImmigrationStatus;
						db.Common_ImmigrationStatus.Add(immigrationcSave);
						errorMsg = "Successfully Saved!";
					}
					else if (dbImmigration != null)//update
					{
						dbImmigration.ImmigrationName = immigrationTypeBO.ImmigrationStatus;
						errorMsg = "Successfully Updated!";
					}
					else//if item is not available in db to update
					{
						errorMsg = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				else
				{
					errorMsg = "Immigration Status Already Exist!";
					return false;
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Immigration Type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteImmigration(int id, out string msg)
		{
			msg = "";
			var item = db.Common_ImmigrationStatus.SingleOrDefault(t => t.ImmigrationStatusID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_Employee.Count == 0)
					{
						db.Common_ImmigrationStatus.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else//immigration type assigned to employee
					{
						msg = "This Immigration Status Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		/// <summary>
		/// Get the detail of selected Immigration to edit
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ImmigrationTypeBO GetEditImmigration(int id)
		{
			var editImmigration = (from et in db.Common_ImmigrationStatus.Where(x => x.ImmigrationStatusID.Equals(id))
								   select new ImmigrationTypeBO()
								   {
									   ImmigrationTypeId = et.ImmigrationStatusID,
									   ImmigrationStatus = et.ImmigrationName

								   }).SingleOrDefault();
			return editImmigration;
		}

		#endregion
		#region Marital Status
		/// <summary>
		/// Get Marital status List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<MaritalStatusBO> GetMaritalList()
		{
			var maritalList = (from ms in db.Common_MaritalStatus
							   select new MaritalStatusBO
							   {
								   MaritalId = ms.MaritalStatusID,
								   MaritalStatus = ms.MaritalStatusName

							   }).OrderBy(m => m.MaritalStatus).ToList();
			return maritalList;
		}

		/// <summary>
		/// Save Marital status
		/// </summary>
		/// <param name="maritalTypeBO"></param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public bool SaveMaritalStatus(MaritalStatusBO maritalTypeBO, out string errorMsg)
		{
			var existingMaritalStatus = db.Common_MaritalStatus.SingleOrDefault(m => m.MaritalStatusName == maritalTypeBO.MaritalStatus && m.MaritalStatusID != maritalTypeBO.MaritalId);
			try
			{
				if (existingMaritalStatus == null)
				{
					var dbMaritalStatus = db.Common_MaritalStatus.SingleOrDefault(m => m.MaritalStatusID == maritalTypeBO.MaritalId);
					if (maritalTypeBO.MaritalId == 0)//save
					{
						var marital = new Common_MaritalStatus();
						marital.MaritalStatusName = maritalTypeBO.MaritalStatus;
						db.Common_MaritalStatus.Add(marital);
						db.SaveChanges();
						errorMsg = "Successfully Saved!";
						return true;
					}
					else if (dbMaritalStatus != null)//update
					{
						dbMaritalStatus.MaritalStatusName = maritalTypeBO.MaritalStatus; ;
						db.SaveChanges();
						errorMsg = "Successfully Updated!";
						return true;
					}
					else//if the selected marital status cannot be find in DB
					{
						errorMsg = "Unable To Find The Item To Update!";
						return false;
					}
				}
				else//already existing Marital status
				{
					errorMsg = "Marital Status Already Exist!";
					return false;
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Marital Status
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteMarital(int id, out string msg)
		{
			msg = "";
			var item = db.Common_MaritalStatus.SingleOrDefault(t => t.MaritalStatusID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_Employee.Count == 0)
					{
						db.Common_MaritalStatus.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Martial Status Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Get marital status detail From DB
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public MaritalStatusBO GetEditMarital(int id)
		{
			var editMarital = (from et in db.Common_MaritalStatus.Where(x => x.MaritalStatusID.Equals(id))
							   select new MaritalStatusBO()
							   {
								   MaritalId = et.MaritalStatusID,
								   MaritalStatus = et.MaritalStatusName

							   }).SingleOrDefault();
			return editMarital;
		}
		#endregion
		#region Nationality
		/// <summary>
		/// Get Nationality List from DB
		/// </summary>
		/// <returns></returns>
		public IEnumerable<NationalityBO> GetNationalities()
		{
			var natinalityList = (from t in db.Common_Nationality
								  select new NationalityBO()
								  {
									  NationalityName = t.NationalityName,
									  NationalityID = t.NationalityID
								  }).OrderBy(n => n.NationalityName).ToList();
			return natinalityList;
		}

		/// <summary>
		/// Save Nationality
		/// </summary>
		/// <param name="nationalityBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveNationality(NationalityBO nationalityBO, out string eMessage)
		{
			eMessage = "";
			var existingNationality = db.Common_Nationality.SingleOrDefault(n => n.NationalityName == nationalityBO.NationalityName && n.NationalityID != nationalityBO.NationalityID);
			var dbNationality = db.Common_Nationality.SingleOrDefault(n => n.NationalityID == nationalityBO.NationalityID);

			try
			{
				if (existingNationality == null)//entered nationality is not exist in DB
				{
					if (nationalityBO.NationalityID == 0)//save
					{
						var nationality = new Common_Nationality();
						nationality.NationalityName = nationalityBO.NationalityName;
						db.Common_Nationality.Add(nationality);
						eMessage = "Successfully Saved!";
					}
					else if (dbNationality != null)//Update
					{
						dbNationality.NationalityName = nationalityBO.NationalityName;
						eMessage = "Successfully Updated!";
					}
					else
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				eMessage = "Nationality Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the Detail of selected Nationality
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public NationalityBO GetNationality(int id)
		{
			var nationality = (from n in db.Common_Nationality
							   where n.NationalityID == id
							   select new NationalityBO()
							   {
								   NationalityID = n.NationalityID,
								   NationalityName = n.NationalityName
							   }).SingleOrDefault();

			if (nationality == null)
			{
				return new NationalityBO();
			}
			return nationality;
		}

		/// <summary>
		/// Delete Nationality
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteNationality(int id, out string msg)
		{
			msg = "";
			var nationality = db.Common_Nationality.SingleOrDefault(n => n.NationalityID == id);
			try
			{
				if (nationality != null)//check the nationality is exist before delete
				{
					if (nationality.Resource_Employee.Count == 0)
					{
						db.Common_Nationality.Remove(nationality);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Nationality Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Unable To Find The Nationality!!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		#endregion
		#region Payment Frequency
		/// <summary>
		/// Get Payment Frequency List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<PaymentTypeBO> GetPaymentList()
		{
			var paymentList = (from et in db.Common_PaymentType
							   select new PaymentTypeBO
							   {
								   PaymentID = et.PaymentTypeID,
								   PaymentName = et.PaymentName
							   }).OrderBy(p => p.PaymentName).ToList();
			return paymentList;
		}

		/// <summary>
		/// Save Payment Frequency
		/// </summary>
		/// <param name="paymentTypeBO"></param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public bool SavePaymentFrequency(PaymentTypeBO paymentTypeBO, out string errorMsg)
		{
			var existingPaymentType = db.Common_PaymentType.SingleOrDefault(p => p.PaymentName == paymentTypeBO.PaymentName && p.PaymentTypeID != paymentTypeBO.PaymentID);
			var dbPaymentType = db.Common_PaymentType.SingleOrDefault(p => p.PaymentTypeID == paymentTypeBO.PaymentID);

			try
			{
				if (existingPaymentType == null)
				{
					if (paymentTypeBO.PaymentID == 0)
					{
						var paymentSave = new Common_PaymentType();
						paymentSave.PaymentName = paymentTypeBO.PaymentName;
						db.Common_PaymentType.Add(paymentSave);
						errorMsg = "Successfully Saved!";
					}
					else if (dbPaymentType != null)
					{
						dbPaymentType.PaymentName = paymentTypeBO.PaymentName;
						errorMsg = "Successfully Updated!!";
					}
					else
					{
						errorMsg = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				else
				{
					errorMsg = "Payment Frequency Name Already Exist!";
					return false;
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		/// <summary>
		/// DeletePayment Type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeletePayment(int id, out string msg)
		{
			msg = "";
			var item = db.Common_PaymentType.SingleOrDefault(t => t.PaymentTypeID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_EmployeePayment.Count == 0)
					{
						db.Common_PaymentType.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Payment Frequency Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		/// <summary>
		/// Get selected Payment Type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PaymentTypeBO GetEditPayment(int id)
		{
			var editPayment = (from et in db.Common_PaymentType.Where(x => x.PaymentTypeID.Equals(id))
							   select new PaymentTypeBO()
							   {
								   PaymentID = et.PaymentTypeID,
								   PaymentName = et.PaymentName

							   }).SingleOrDefault();
			return editPayment;
		}
		#endregion

		#region Resource Holiday type
		/// <summary>
		/// Get Holiday type list
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ResourceHolidayTypeBO> GetHolidayTypeList()
		{
			var holidayTypeList = (from h in db.Resource_Holiday_Type
								   select new ResourceHolidayTypeBO()
								   {
									   AbbriviatedHolidayType = h.AbbriviatedHolidayType,
									   HolidayTypeID = h.HolidayTypeID,
									   HolidayTypeName = h.HolidayTypeName,
									   IconCode = h.IconCode
								   }).OrderBy(ht => ht.HolidayTypeName).ToList();
			return holidayTypeList;
		}

		/// <summary>
		/// Save Holiday type
		/// </summary>
		/// <param name="holidayTypeBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveHolidayType(ResourceHolidayTypeBO holidayTypeBO, out string eMessage)
		{
			eMessage = "";
			var isHolidayTypeExist = CheckHolidayType(holidayTypeBO.HolidayTypeName, holidayTypeBO.HolidayTypeID);//calling CheckHolidayType method
			var isAbbreviationExist = CheckAbbreviation(holidayTypeBO.AbbriviatedHolidayType, holidayTypeBO.HolidayTypeID);
			var dbHolidayType = db.Resource_Holiday_Type.SingleOrDefault(h => h.HolidayTypeID == holidayTypeBO.HolidayTypeID);

			try
			{
				if (!isHolidayTypeExist && !isAbbreviationExist)//if it is not already exist
				{
					if (holidayTypeBO.HolidayTypeID==0)//Save
					{
						var holidayType = new Resource_Holiday_Type();
						holidayType.AbbriviatedHolidayType = holidayTypeBO.AbbriviatedHolidayType;
						holidayType.HolidayTypeName = holidayTypeBO.HolidayTypeName;
						holidayType.IconCode = holidayTypeBO.IconCode;

						db.Resource_Holiday_Type.Add(holidayType);
						db.SaveChanges();
						eMessage = "Successfully Saved!";
						return true;
					}
					else 
					{
						if  (dbHolidayType != null)//Update
						{
							dbHolidayType.HolidayTypeName = holidayTypeBO.HolidayTypeName;
							dbHolidayType.IconCode = holidayTypeBO.IconCode;
							dbHolidayType.AbbriviatedHolidayType = holidayTypeBO.AbbriviatedHolidayType;
							db.SaveChanges();
							eMessage = "Successfully Updated!";
							return true;
						}
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
					
				}
				else if (isHolidayTypeExist)
				{
					eMessage = "Holiday Type Already Exist!";					
				}
				else /*if(isAbbreviationExist)*/
				{
					eMessage = "Abbreviated Holiday Type Already Exist!";				
				}			
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}
		///// <summary>
		///// Update Resource holiday type
		///// </summary>
		///// <param name="holidayTypeBO"></param>
		///// <param name="eMessage"></param>
		///// <returns></returns>
		//public bool UpdateResourceType(ResourceHolidayTypeBO holidayTypeBO, out string eMessage)
		//{
		//	eMessage = "";
		//	var holidayType = db.Resource_Holiday_Type.SingleOrDefault(h => h.HolidayTypeID == holidayTypeBO.HolidayTypeID);
		//	//var isExist = (from ht in db.Resource_Holiday_Type
		//	//			   where ht.HolidayTypeName.Equals(holidayTypeBO.HolidayTypeName) && ht.HolidayTypeID != holidayTypeBO.HolidayTypeID
		//	//			   select ht).SingleOrDefault();
		//	var isHolidayTypeExist = CheckHolidayType(holidayTypeBO.HolidayTypeName, holidayTypeBO.HolidayTypeID);//calling CheckHolidayType method
		//	var isAbbreviationExist = CheckAbbreviation(holidayTypeBO.AbbriviatedHolidayType, holidayTypeBO.HolidayTypeID);
		//	try
		//	{
		//		if (holidayType != null)
		//		{
		//			if (isHolidayTypeExist == true && isAbbreviationExist == true)
		//			{
		//				eMessage = "Holiday Type and Abbreviated Holiday Type are Already Exist!";
		//				return false;
		//			}
		//			else if (isHolidayTypeExist == true)
		//			{
		//				eMessage = "Holiday Type Already Exist!";
		//				return false;
		//			}
		//			else if (isAbbreviationExist == true)
		//			{
		//				eMessage = "Abbreviated Holiday Type Already Exist!";
		//				return false;
		//			}
		//			holidayType.HolidayTypeName = holidayTypeBO.HolidayTypeName;
		//			holidayType.IconCode = holidayTypeBO.IconCode;
		//			holidayType.AbbriviatedHolidayType = holidayTypeBO.AbbriviatedHolidayType;
		//			db.SaveChanges();
		//			eMessage = "Successfully Updated!";
		//			return true;
		//		}
		//		eMessage = "Unable To Find The Item To Update!";
		//		return false;
		//	}
		//	catch (Exception exc)
		//	{
		//		eMessage = exc.Message;
		//		return false;
		//	}
		//}

		/// <summary>
		/// Check whether the HolidayType is already exist in the database
		/// </summary>
		/// <param name="holidayTypeName"></param>
		/// <param name="holidayTypeID"></param>
		/// <returns></returns>
		public bool CheckHolidayType(string holidayTypeName, int holidayTypeID)
		{
			var isHolidaytypeExist = db.Resource_Holiday_Type.Where(h => (h.HolidayTypeName == holidayTypeName && h.HolidayTypeID != holidayTypeID))
				.SingleOrDefault() != null;
			return isHolidaytypeExist;
		}
		/// <summary>
		/// Check whether the Abbreviated Holiday type is already exist in the database
		/// </summary>
		/// <param name="abbriviatedHolidayType"></param>
		/// <param name="holidayTypeID"></param>
		/// <returns></returns>
		public bool CheckAbbreviation(string abbriviatedHolidayType, int holidayTypeID)
		{
			var isAbbreviationExist = db.Resource_Holiday_Type.Where(h => (h.AbbriviatedHolidayType == abbriviatedHolidayType && h.HolidayTypeID != holidayTypeID))
				.SingleOrDefault() != null;
			return isAbbreviationExist;

		}

		/// <summary>
		/// Delete Holiday Type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteResourceHolidayType(int id, out string msg)
		{
			msg = "";
			var item = db.Resource_Holiday_Type.SingleOrDefault(r => r.HolidayTypeID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_Holiday.Count == 0 && item.Branch_Holidays.Count == 0)
					{

						db.Resource_Holiday_Type.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Holiday Type Is Already In Use So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the detail of selected holiday type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ResourceHolidayTypeBO GetResourceHolidayType(int id)
		{
			var holidayType = (from h in db.Resource_Holiday_Type
							   where h.HolidayTypeID == id
							   select new ResourceHolidayTypeBO()
							   {
								   HolidayTypeID = h.HolidayTypeID,
								   AbbriviatedHolidayType = h.AbbriviatedHolidayType,
								   HolidayTypeName = h.HolidayTypeName,
								   IconCode = h.IconCode
							   }).SingleOrDefault();
			if (holidayType != null)
			{
				return holidayType;
			}
			return new ResourceHolidayTypeBO();
		}
		#endregion
		#region Shift Pattern
		/// <summary>
		/// Gets Shift Pattern List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ShiftPatternBO> GetShiftPatternList()
		{
			// TimeSpan.FromMinutes method not recognized in Linq query
			var shiftlist = db.Resource_ShiftPattern.ToList();
			var shiftPattern = (from s in shiftlist
								select new ShiftPatternBO
								{
									BreakDuration = s.BreakDuration,
									Description = s.Description,
									Duration = TimeSpan.FromMinutes((int)s.Duration),//convert minutes in integer to timespan
									IsEnable = (bool)s.IsEnable,
									ShiftPatternID = s.ShiftPatternID,
									ShiftStartTime = TimeSpan.FromMinutes((int)s.ShiftStartTime),
									ResourceCount = s.Resource_ShiftSchedule.Count
								}).OrderBy(s => s.Description).ToList();
			return shiftPattern;
		}

		/// <summary>
		/// Save and Update Shift Pattern to "Resource_ShiftPattern" Table
		/// </summary>
		/// <param name="shiftPatternBO"></param>
		/// <param name="eMsg"></param>
		/// <returns></returns>
		public bool SaveShiftPattern(ShiftPatternBO shiftPatternBO, out string eMsg)
		{
			eMsg = "";
			bool isShiftPatternNameExist = CheckShiftPatternNameExist(shiftPatternBO.Description, shiftPatternBO.ShiftPatternID);
			bool isShiftPatternExist = CheckShiftPatternIsExist(shiftPatternBO.ShiftStartTime, shiftPatternBO.Duration, shiftPatternBO.ShiftPatternID);
			try
			{
				//check shift Duration field and Shift Start Time fields have valid Time
				if (CheckShiftStartTimeEntered(shiftPatternBO.ShiftStartTime) == false && CheckShiftDurationEntered(shiftPatternBO.Duration) == false)
				{
					eMsg = "Please Enter Shift Start Time And Shift Duration!";
					return false;
				}
				else if (CheckShiftStartTimeEntered(shiftPatternBO.ShiftStartTime) == false)
				{
					eMsg = "Please Enter Shift Start Time!";
					return false;
				}
				else if (CheckShiftDurationEntered(shiftPatternBO.Duration) == false)
				{
					eMsg = "Please Enter Shift Duration";
					return false;
				}

				//both shift pattern name and shift Patterns(same shift Start time and Duration) are not exist in database
				if (isShiftPatternExist == false && isShiftPatternNameExist == false)
				{
					if (shiftPatternBO.ShiftPatternID == 0)//save Shift Pattern
					{
						var saveShiftpattern = new Resource_ShiftPattern();
						saveShiftpattern.Description = shiftPatternBO.Description;
						saveShiftpattern.BreakDuration = shiftPatternBO.BreakDuration;
						saveShiftpattern.Duration = (int)shiftPatternBO.Duration.TotalMinutes;
						saveShiftpattern.IsEnable = shiftPatternBO.IsEnable;
						saveShiftpattern.ShiftStartTime = (int)shiftPatternBO.ShiftStartTime.TotalMinutes;
						db.Resource_ShiftPattern.Add(saveShiftpattern);
						eMsg = "Successfully Saved!";
					}
					else
					{
						var pattern = db.Resource_ShiftPattern.SingleOrDefault(x => x.ShiftPatternID == shiftPatternBO.ShiftPatternID);
						if (pattern != null)//Update SHift Pattern
						{
							pattern.Description = shiftPatternBO.Description;
							pattern.BreakDuration = shiftPatternBO.BreakDuration;
							pattern.Duration = (int)shiftPatternBO.Duration.TotalMinutes;
							pattern.IsEnable = shiftPatternBO.IsEnable;
							pattern.ShiftStartTime = (int)shiftPatternBO.ShiftStartTime.TotalMinutes;
							eMsg = "Successfully Updated!";
						}
					}
					db.SaveChanges();
					return true;
				}
				else if (isShiftPatternNameExist == false)//shift Name not exist and shift pattern Exist
				{
					eMsg = "Same Shift Start Time And Shift Duration Already Exist!";
				}
				else if (isShiftPatternExist == false)//shift pattern not exist, shift Name exist
				{
					eMsg = "Shift Pattern Already Exist!";
				}
				else
				{
					eMsg = "Shift Pattern And, Shift Start Time And Shift Duration Already Exist!";
				}
				return false;
			}
			catch (Exception ex)
			{
				eMsg = ex.Message;
				return false;
			}
		}

		/// <summary>
		/// Deletes Shift Pattern from "Resource_ShiftPattern" Table
		/// </summary>
		/// <param name="id"></param>
		/// <param name="eMsg"></param>
		/// <returns></returns>
		public bool DeleteShiftPattern(int id, out string eMsg)
		{
			eMsg = "";
			var pattern = db.Resource_ShiftPattern.SingleOrDefault(t => t.ShiftPatternID == id);
			try
			{
				if (pattern != null)
				{
					db.Resource_ShiftPattern.Remove(pattern);
					db.SaveChanges();
					eMsg = "Successfully Removed!";
					return true;
				}
				else
				{
					eMsg = "Already Removed!";
					return false;
				}
			}
			catch (Exception exc)
			{
				eMsg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Edits Shift Pattern
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ShiftPatternBO EditShiftPattern(int id, out string eMsg)
		{
			eMsg = "";
			var shiftPattern = (from s in db.Resource_ShiftPattern.Where(x => x.ShiftPatternID.Equals(id)) select s).SingleOrDefault();
			var shiftPatternView = new ShiftPatternBO()
			{
				BreakDuration = shiftPattern.BreakDuration,
				Description = shiftPattern.Description,
				Duration = TimeSpan.FromMinutes((int)shiftPattern.Duration),
				IsEnable = (bool)shiftPattern.IsEnable,
				ShiftPatternID = shiftPattern.ShiftPatternID,
				ShiftStartTime = TimeSpan.FromMinutes((int)shiftPattern.ShiftStartTime),
				ResourceCount = shiftPattern.Resource_ShiftSchedule.Count
			};

			if (shiftPatternView == null)
			{
				eMsg = "Unable to Find The Shift Pattern!";
				return null;
			}
			return shiftPatternView;
		}
		/// <summary>
		/// check whether Shift Start Time field is entered
		/// </summary>
		/// <param name="strtTime"></param>
		/// <returns></returns>
		public bool CheckShiftStartTimeEntered(TimeSpan strtTime)
		{
			var isSrtTimeZero = (int)strtTime.TotalMinutes != 0;
			return isSrtTimeZero;
		}

		/// <summary>
		/// check whether Shift Duration field is entered
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public bool CheckShiftDurationEntered(TimeSpan duration)
		{
			var isDurationZero = (int)duration.TotalMinutes != 0;
			return isDurationZero;
		}

		/// <summary>
		/// Check whether the entered Shift Pattern Name is exist in Database
		/// </summary>
		/// <param name="shftPatternname"></param>
		/// <param name="patternID"></param>
		/// <returns></returns>
		public bool CheckShiftPatternNameExist(string shftPatternname, int patternID)
		{
			var isExist = db.Resource_ShiftPattern.Where(x => x.Description == shftPatternname && x.ShiftPatternID != patternID)
				.FirstOrDefault() != null;
			return isExist;
		}

		/// <summary>
		/// Check whether the entered Shift Start Time and Duration is exist in Database
		/// </summary>
		/// <param name="strtTime"></param>
		/// <param name="duration"></param>
		/// <param name="patternID"></param>
		/// <returns></returns>
		public bool CheckShiftPatternIsExist(TimeSpan strtTime, TimeSpan duration, int patternID)
		{
			int shftSrtTime = (int)strtTime.TotalMinutes;
			int shftDuration = (int)duration.TotalMinutes;
			var dbShiftPattern = db.Resource_ShiftPattern.Where(x => (x.ShiftStartTime == shftSrtTime && x.Duration == shftDuration && x.ShiftPatternID != patternID))
				.FirstOrDefault();
			var isShiftTimeExist = (dbShiftPattern != null) ? true : false;
			return isShiftTimeExist;
		}
		#endregion
		#region Title
		/// <summary>
		/// Get Title List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TitleBO> GetTitleList()
		{
			var titleList = (from t in db.Common_Title
							 select new TitleBO()
							 {
								 Title = t.TitleName,
								 TitleID = t.TitleID
							 }).OrderBy(t => t.Title).ToList();
			return titleList;
		}

		/// <summary>
		/// Save Title
		/// </summary>
		/// <param name="titleBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveTitle(TitleBO titleBO, out string eMessage)
		{
			//var existingTitle = db.Common_Title.SingleOrDefault(t => t.TitleName == titleBO.Title);
			var existingTitle = (from t in db.Common_Title
								 where t.TitleName.Equals(titleBO.Title) && t.TitleID != titleBO.TitleID
								 select t).SingleOrDefault();			
			try
			{
				if (existingTitle == null)
				{
					var title = db.Common_Title.SingleOrDefault(ti => ti.TitleID == titleBO.TitleID);//get the Title from database to Update
					if (titleBO.TitleID == 0)//Save
					{
						var t = new Common_Title();
						t.TitleName = titleBO.Title;
						db.Common_Title.Add(t);
						db.SaveChanges();
						eMessage = "Successfully Saved!";
						return true;
					}
					else if (title != null)//Update
					{
						title.TitleName = titleBO.Title;
						db.SaveChanges();
						eMessage = "Successfully Updated!";
						return true;
					}
					eMessage = "Unable To Find The Item To Update!";
				}
				eMessage = "Title Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Title
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteTitle(int id, out string msg)
		{
			msg = "";
			var item = db.Common_Title.SingleOrDefault(t => t.TitleID == id);
			try
			{
				if (item != null)
				{
					if (item.Resource_Employee.Count == 0 && item.User_UserDetails.Count==0)//Title is not used in any Resource
					{
						db.Common_Title.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Title Is Already In Use. So Cannot Be Deleted.";
						return false; 
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// get the title detail
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TitleBO GetTitle(int id)
		{
			var title = (from t in db.Common_Title
						 where t.TitleID == id
						 select new TitleBO()
						 {
							 Title = t.TitleName,
							 TitleID = t.TitleID
						 }).SingleOrDefault();
			if (title != null)
			{
				return title;
			}
			return new TitleBO();
		}
		#endregion
		#region Timesheet Frequency
		/// <summary>
		/// Get the Time sheet frequency List
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TSFrequencyBO> GetFrequencyList()
		{
			var frequencyList = (from f in db.Common_TimeSheetFrequency
								 select new TSFrequencyBO
								 {
									 TimeSheetFrequencyDeadline = f.TimeSheetFrequencyDeadline,
									 TimeSheetFrequencyID = f.TimeSheetFrequencyID,
									 TimeSheetFrequencyName = f.TimeSheetFrequencyName
								 }).OrderBy(f => f.TimeSheetFrequencyName).ToList();
			return frequencyList;
		}

		/// <summary>
		/// update Time sheet frequency
		/// </summary>
		/// <param name="frequencyBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool UpdateTSFrequency(TSFrequencyBO frequencyBO, out string eMessage)
		{
			eMessage = "";
			var frequency = db.Common_TimeSheetFrequency.SingleOrDefault(f => f.TimeSheetFrequencyID == frequencyBO.TimeSheetFrequencyID);
			try
			{
				//when update the weekly frequency dead line update the same in fortnightly
				if (frequency.TimeSheetFrequencyName.ToLower() == "weekly")
				{
					frequency.TimeSheetFrequencyDeadline = frequencyBO.TimeSheetFrequencyDeadline;

					var fortnighlyfrequency = db.Common_TimeSheetFrequency.SingleOrDefault(f => f.TimeSheetFrequencyName.ToLower() == "Fortnightly");
					fortnighlyfrequency.TimeSheetFrequencyDeadline = frequencyBO.TimeSheetFrequencyDeadline;
					db.SaveChanges();
					eMessage = "Successfully Updated!";
					return true;
				}
				frequency.TimeSheetFrequencyDeadline = frequencyBO.TimeSheetFrequencyDeadline;
				db.SaveChanges();
				eMessage = "Successfully Updated!";
				return true;
			}
			catch (Exception exc)
			{
				eMessage = exc.Message;
				return false;
			}
		}
		/// <summary>
		///Get one Time sheet frequency name
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TSFrequencyBO GetTSFrequency(int id)
		{
			var frequency = (from f in db.Common_TimeSheetFrequency
							 where f.TimeSheetFrequencyID == id
							 select new TSFrequencyBO()
							 {
								 TimeSheetFrequencyName = f.TimeSheetFrequencyName,
								 TimeSheetFrequencyID = f.TimeSheetFrequencyID,
								 TimeSheetFrequencyDeadline = f.TimeSheetFrequencyDeadline
							 }).SingleOrDefault();
			if (frequency != null)
			{
				return frequency;
			}
			return new TSFrequencyBO();
		}

		/// <summary>
		/// Get the days from Common_ Day Table, to use in drop-down list
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Common_Day> GetDayofweek()
		{
			var Days = (from d in db.Common_Day select d).ToList();
			return Days;
		}
		#endregion
		#region Skill Category

		/// <summary>
		/// Get the Skill CAtegory list from database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SkillCategoryBO> GetSkillCategoryList()
		{
			var skillCategoryList = (from c in db.Common_SkillCategory
									 select new SkillCategoryBO
									 {
										 CategoryName = c.CategoryName,
										 Description = c.Description,
										 SkillCategoryID = c.SkillCategoryID
									 }).OrderBy(c => c.CategoryName).ToList();
			return skillCategoryList;
		}

		/// <summary>
		/// Save Skill Category Details to Common_SkillCategory Table
		/// </summary>
		/// <param name="skillCategoryBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveSkillCategory(SkillCategoryBO skillCategoryBO, out string eMessage)
		{
			//var existingTitle = db.Common_Title.SingleOrDefault(t => t.TitleName == titleBO.Title);
			var existingSkillCategory = (from s in db.Common_SkillCategory
										 where s.CategoryName.Equals(skillCategoryBO.CategoryName) && s.SkillCategoryID != skillCategoryBO.SkillCategoryID
										 select s).SingleOrDefault();
			try
			{
				if (existingSkillCategory == null)//skill category is not already exist in Database
				{
					var skillCategory = db.Common_SkillCategory.SingleOrDefault(s => s.SkillCategoryID == skillCategoryBO.SkillCategoryID);
					if (skillCategoryBO.SkillCategoryID == 0)//Save
					{
						var s = new Common_SkillCategory();
						s.CategoryName = skillCategoryBO.CategoryName;
						s.Description = skillCategoryBO.Description;
						db.Common_SkillCategory.Add(s);
						db.SaveChanges();
						eMessage = "Successfully Saved!";
						return true;
					}
					else if (skillCategory != null)//Update
					{
						skillCategory.CategoryName = skillCategoryBO.CategoryName;
						skillCategory.Description = skillCategoryBO.Description;
						db.SaveChanges();
						eMessage = "Successfully Updated!";
						return true;
					}
					eMessage = "Unable To Find The Item To Update!";
				}
				eMessage = "Category Name Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Skill Category from the database
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteSkillCategory(int id, out string msg)
		{
			msg = "";
			var item = db.Common_SkillCategory.SingleOrDefault(s => s.SkillCategoryID == id);//select category from SkillCategory Table																							
			try
			{
				if (item != null)
				{
					if (item.Resource_SkillDetails.Count == 0)
					{
						db.Common_SkillCategory.Remove(item);
						var skillGroupItems = db.Common_SkillGroup.Where(sg => sg.SkillCategoryID == id).Delete();//delete skill groups which include the selected skillcategory

						//if (skillGroupItems != null)
						//{

						//}
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Category Is Already In Use. So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// get the Skill Category detail from the Common_SkillCategory table
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SkillCategoryBO GetSkillCategory(int id)
		{
			var skillCategory = (from s in db.Common_SkillCategory
								 where s.SkillCategoryID == id
								 select new SkillCategoryBO()
								 {
									 CategoryName = s.CategoryName,
									 SkillCategoryID = s.SkillCategoryID,
									 Description = s.Description
								 }).SingleOrDefault();
			if (skillCategory != null)
			{
				return skillCategory;
			}
			return new SkillCategoryBO();
		}

		/// <summary>
		/// Get the skill and Skill Level List which belongs to the particular category from SkillGrop Table
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<SkillGroupBO> GetSkill(int? id)
		{
			var skillList = db.Common_SkillGroup.Where(s => s.SkillCategoryID == id).Select(s => new SkillGroupBO
			{
				SkillID = s.SkillID,
				SkillLevelID = s.SkillLevelID,
				SkillName = db.Common_Skill.Where(ss => ss.SkillID == s.SkillID).Select(ss => ss.SkillName).FirstOrDefault(),
				SkillLevel = db.Common_SkillLevelScheme.Where(sl => sl.SkillLevelID == s.SkillLevelID).Select(sl => sl.SkillLevelName).FirstOrDefault()
			}).ToList();
			return skillList;
		}
		#endregion

		#region SkillLevelSheme
		/// <summary>
		/// Get the Skill Level list from database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SkillLevelBO> GetSkillLevelList()
		{
			var skillLevelList = (from s in db.Common_SkillLevelScheme
								  select new SkillLevelBO
								  {
									  Description = s.Description,
									  MaxSkillLevel = s.MaxSkillLevel,
									  MinSkillLevel = s.MinSkillLevel,
									  SkillLevelID = s.SkillLevelID,
									  SkillLevelName = s.SkillLevelName
								  }).OrderBy(sl => sl.SkillLevelName).ToList();
			return skillLevelList;
		}

		/// <summary>
		/// Save/Update Skill Level Details to Database
		/// </summary>
		/// <param name="skillLevelBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveSkillLevel(SkillLevelBO skillLevelBO, out string eMessage)
		{
			var existingSkillLevel = (from s in db.Common_SkillLevelScheme
									  where s.SkillLevelName.Equals(skillLevelBO.SkillLevelName) && s.SkillLevelID != skillLevelBO.SkillLevelID
									  select s).SingleOrDefault();
			var skillLevel = db.Common_SkillLevelScheme.SingleOrDefault(s => s.SkillLevelID == skillLevelBO.SkillLevelID);
			try
			{
				if (existingSkillLevel == null)
				{
					if ((skillLevelBO.MaxSkillLevel < skillLevelBO.MinSkillLevel) /*|| (skillLevelBO.MaxSkillLevel == skillLevelBO.MinSkillLevel)*/)
					{
						eMessage = "Max Skill Level Should Be Greater Than Min Skill Level!";
						return false;
					}
					else if (skillLevelBO.SkillLevelID == 0)//Save
					{
						var s = new Common_SkillLevelScheme();
						s.SkillLevelName = skillLevelBO.SkillLevelName;
						s.Description = skillLevelBO.Description;
						s.MaxSkillLevel = skillLevelBO.MaxSkillLevel;
						s.MinSkillLevel = skillLevelBO.MinSkillLevel;
						db.Common_SkillLevelScheme.Add(s);
						db.SaveChanges();
						eMessage = "Successfully Saved!";
						return true;
					}
					else if (skillLevel != null)//Update
					{
						skillLevel.SkillLevelName = skillLevelBO.SkillLevelName;
						skillLevel.Description = skillLevelBO.Description;
						skillLevel.MinSkillLevel = skillLevelBO.MinSkillLevel;
						skillLevel.MaxSkillLevel = skillLevelBO.MaxSkillLevel;
						db.SaveChanges();
						eMessage = "Successfully Updated!";
						return true;
					}
					eMessage = "Unable To Find The Item To Update!";
				}
				eMessage = "Skill Level Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Delete Skill Level from the database
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteSkillLevel(int id, out string msg)
		{
			msg = "";
			var item = db.Common_SkillLevelScheme.SingleOrDefault(s => s.SkillLevelID == id);
			try
			{
				if (item != null)
				{
					var resourceSkill = db.Resource_SkillDetails.Where(s => s.SkillLevel == id).Count();

					if (resourceSkill == 0)//this skill is not used in any Resource
					{
						db.Common_SkillLevelScheme.Remove(item);
						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Skill Level Is Already In Use. So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the Skill Level detail from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SkillLevelBO GetSkillLevel(int id)
		{
			var skillLevel = (from s in db.Common_SkillLevelScheme
							  where s.SkillLevelID == id
							  select new SkillLevelBO()
							  {
								  MaxSkillLevel = s.MaxSkillLevel,
								  MinSkillLevel = s.MinSkillLevel,
								  SkillLevelName = s.SkillLevelName,
								  Description = s.Description,
								  SkillLevelID = s.SkillLevelID
							  }).SingleOrDefault();
			if (skillLevel != null)
			{
				return skillLevel;
			}
			return new SkillLevelBO();
		}
		#endregion

		#region Skill
		/// <summary>
		/// Get Skill List from common_Skill  table
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SkillBO> GetSkillList()
		{
			var skillList = (from s in db.Common_Skill
							 select new SkillBO
							 {
								 SkillID = s.SkillID,
								 SkillName = s.SkillName,
								 SkillDescription = s.SkillDescription,
								 NoOfResources = s.Resource_SkillDetails.Count()							 
							 }).OrderBy(s => s.SkillName).ToList();
			return skillList;
		}

		/// <summary>
		/// Get All Skill Category data from database and return as a list
		/// </summary>
		/// <returns></returns>
		public List<SkillGroup> GetSkillCatList()
		{
			var skillCategoryList = (from c in db.Common_SkillCategory
									 select new SkillGroup
									 {
										 CategoryName = c.CategoryName,
										 CategoryID = c.SkillCategoryID
									 }).OrderBy(sc => sc.CategoryName).ToList();
			return skillCategoryList;
		}

		/// <summary>
		/// Save the skill and associated categories to the Common_Skill and Commonn_SkillGroup Tables
		/// </summary>
		/// <param name="skillcreationBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveSkill(SkillView skillcreationBO, out string eMessage)
		{
			eMessage = "";
			var existingSkill = (from s in db.Common_Skill
								 where s.SkillName.Equals(skillcreationBO.Skill.SkillName) && s.SkillID != skillcreationBO.Skill.SkillID
								 select s).SingleOrDefault();
			try
			{				
				//get the selected category count for this Skill
				var SelectedCategoryCount = skillcreationBO.SkillCategoryGroup.Where(s => s.isSelected == true).Count();
				//No skill Name same as in Database && to validate, skill should be included in at least one skill
				if (existingSkill == null && SelectedCategoryCount > 0)
				{
					//get the skill detail from Database, match with the ID in SkillCreationBO object
					var dbSkill = db.Common_Skill.SingleOrDefault(s => s.SkillID == skillcreationBO.Skill.SkillID);
					if (skillcreationBO.Skill.SkillID == 0 || skillcreationBO.Skill.SkillID == null)//Save to Common_Skill Table
					{//skillID is declared as nullable in model. thats y check the null.
						var skill = new Common_Skill();
						skill.SkillName = skillcreationBO.Skill.SkillName;
						skill.SkillDescription = skillcreationBO.Skill.SkillDescription;
						db.Common_Skill.Add(skill);
						db.SaveChanges();

						var skillID = (from s in db.Common_Skill where s.SkillName == skillcreationBO.Skill.SkillName select s.SkillID).SingleOrDefault();//save the skill to database and return the ID

						foreach (var item in skillcreationBO.SkillCategoryGroup)
						{
							if (item.isSelected == true)//save selected category names and Skill Level
							{
								var skillGroup = new Common_SkillGroup();
								skillGroup.SkillID = skillID;
								skillGroup.SkillLevelID = item.SkillLevelID;
								skillGroup.SkillCategoryID = item.CategoryID;
								db.Common_SkillGroup.Add(skillGroup);
							}
						}
						eMessage = "Successfully Saved!";
					}
					else if (dbSkill != null)//Update Common_Skill Table and Common_SkillGroup Table
					{
						//update Common_Skill Table Detail
						dbSkill.SkillName = skillcreationBO.Skill.SkillName;
						dbSkill.SkillDescription = skillcreationBO.Skill.SkillDescription;

						//Update Common_SkillGroup Table Detail					
						var skillGroup = (from s in db.Common_SkillGroup where s.SkillID == skillcreationBO.Skill.SkillID select s).ToList();//select record with the skillId which we need to update
						foreach (var item in skillGroup)
						{
							db.Common_SkillGroup.Remove(item);//remove records with that skillID
						}
						foreach (var item in skillcreationBO.SkillCategoryGroup)//newly add data to Common_SkillGroup Table
						{
							if (item.isSelected == true)//save selected categories
							{
								var updatedSkillGroup = new Common_SkillGroup();
								updatedSkillGroup.SkillID = skillcreationBO.Skill.SkillID;
								updatedSkillGroup.SkillLevelID = item.SkillLevelID;
								updatedSkillGroup.SkillCategoryID = item.CategoryID;
								db.Common_SkillGroup.Add(updatedSkillGroup);
							}
						}
						eMessage = "Successfully Updated!";
					}

					db.SaveChanges();
					return true;
				}
				else if (SelectedCategoryCount == 0)
				{
					eMessage = "Please Select At least One Category!!";
					return false;
				}
				eMessage = "Skill Name Already Exist.";
				return false;

			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// get the Skill and associated Category detail from the database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SkillView GetSkill(int id)
		{
			SkillView skillCreation = new SkillView();
			skillCreation.SkillCategoryGroup = new List<SkillGroup>();
			skillCreation.Skill = (from s in db.Common_Skill
								   where s.SkillID == id
								   select new SkillBO()
								   {
									   SkillID = s.SkillID,
									   SkillDescription = s.SkillDescription,
									   SkillName = s.SkillName
								   }).SingleOrDefault();//get the skill match with the id from the DB
			var skillGroup = (from sg in db.Common_SkillGroup
							  where sg.SkillID == id
							  select new SkillGroup()
							  {
								  CategoryID = (int)sg.SkillCategoryID,
								  SkillLevelID = (int)sg.SkillLevelID,
								  SkillLevelName = db.Common_SkillLevelScheme.FirstOrDefault(l =>l.SkillLevelID==sg.SkillLevelID).SkillLevelName,
								  SkillGroupID = sg.SkillGroupID
							  }).ToList();//get the associated skill category which includes that skill from the Common_SkillGroup table.

			var SkillCategory = db.Common_SkillCategory;
			foreach (var item in SkillCategory)
			{
				SkillGroup sg = new SkillGroup();
				sg.CategoryID = item.SkillCategoryID;
				sg.CategoryName = item.CategoryName;
				sg.isSelected = skillGroup.FirstOrDefault(s => s.CategoryID == item.SkillCategoryID) != null ? true : false;
				sg.SkillLevelID = sg.isSelected == true ? skillGroup.FirstOrDefault(s => s.CategoryID == item.SkillCategoryID).SkillLevelID : 0;
				sg.SkillLevelName = sg.isSelected == true ? skillGroup.FirstOrDefault(s => s.CategoryID == item.SkillCategoryID).SkillLevelName: "";
				skillCreation.SkillCategoryGroup.Add(sg);
			}
			return skillCreation;
		}

		/// <summary>
		/// Delete a skill from Database
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteSkill(int id, out string msg)
		{
			msg = "";
			var item = db.Common_Skill.SingleOrDefault(s => s.SkillID == id);//select Skill from Skill Table																							
			try
			{
				if (item != null)
				{
					if (item.Resource_SkillDetails.Count == 0)
					{						
						//if (item.Common_SkillGroup.Count != 0)
						//{
						//	db.Common_SkillGroup.Where(sg => sg.SkillID == id).Delete();//select and delete skill groups which include the selected skill
						//}
						db.Common_Skill.Remove(item);
						db.Common_SkillGroup.Where(sg => sg.SkillID == id).Delete();//select and delete skill groups which include the selected skill

						db.SaveChanges();
						msg = "Successfully Removed!";
						return true;
					}
					else
					{
						msg = "This Skill Is Already In Use. So Cannot Be Deleted.";
						return false;
					}
				}
				msg = "Already Removed!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}


		/// <summary>
		/// get the details of skill category and skill level associated with the particular skill
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public List<SkillGroup> GetSkillCategoryLevel(int? skillId)
		{
			var skillCatgoryList = (from s in db.Common_SkillGroup
									where s.SkillID == skillId
									select new SkillGroup()
									{
										CategoryBO = db.Common_SkillCategory.Where(c => c.SkillCategoryID == s.SkillCategoryID).Select(c => new SkillCategoryBO { CategoryName = c.CategoryName, Description = c.Description }).FirstOrDefault(),
										CategoryID = (int)s.SkillCategoryID,
										LevelBO = db.Common_SkillLevelScheme.Where(l => l.SkillLevelID == s.SkillLevelID).Select(l => new SkillLevelBO { SkillLevelName = l.SkillLevelName, SkillLevelID = l.SkillLevelID, Description = l.Description }).FirstOrDefault()
									}).ToList();
			return skillCatgoryList;
		}

		/// <summary>
		/// Return the list of Employees who have that Skill
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<EmployeeView> GetEmployees(int id)
		{
			var x = db.Resource_SkillDetails.Where(s => s.SkillID == id).Select(s => new EmployeeView
			{
				FName = s.Resource_Employee.FirstName,
				LName = s.Resource_Employee.LastName,
				EmployeeID = s.ResourceID
			}).ToList();
			return x;
		}
		#endregion

		#region Designation
		/// <summary>
		/// Get Designation list from DB
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DesignationBO> GetDesignationList()
		{
			var designationList = (from d in db.Common_Designation
						select new DesignationBO()
						{
							Designation = d.DesignationName,
							DesignationID = d.DesignationID
						}).OrderBy(d =>d.Designation).ToList();
			return designationList;
		}

		/// <summary>
		/// Save and Update Designation to DB
		/// </summary>
		/// <param name="designationBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveDesignation(DesignationBO designationBO, out string eMessage)
		{
			eMessage = "";
			var existingDesignation = db.Common_Designation.SingleOrDefault(d => d.DesignationName == designationBO.Designation && d.DesignationID != designationBO.DesignationID);
			try
			{
				if (existingDesignation == null)//Designation is not exist in the DB
				{
					var dbDesignation = db.Common_Designation.SingleOrDefault(d => d.DesignationID == designationBO.DesignationID);//get the designation from DB to update
					if (designationBO.DesignationID == 0)//Save
					{
						var designation = new Common_Designation();
						designation.DesignationName = designationBO.Designation;
						db.Common_Designation.Add(designation);
						eMessage = "Successfully Saved!";
					}
					else if (dbDesignation != null)//update
					{
						dbDesignation.DesignationName = designationBO.Designation;
						eMessage = "Successfully Updated!";
					}
					else
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				eMessage = "Designation Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the selected Designation Detail from the Db
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DesignationBO GetDesignation(int id)
		{
			var designation = (from d in db.Common_Designation
						 where d.DesignationID == id
						 select new DesignationBO()
						 {
							 Designation = d.DesignationName,
							 DesignationID = d.DesignationID,
						 }).SingleOrDefault();
			if (designation != null)
			{
				return designation;
			}
			return new DesignationBO();
		}

		/// <summary>
		/// Delete Designation from DB
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteDesignation(int id, out string msg)
		{
			msg = "";
			var designation = db.Common_Designation.SingleOrDefault(d => d.DesignationID == id);
			try
			{
				if (designation != null)
				{
					db.Common_Designation.Remove(designation);
					db.SaveChanges();
					msg = "Successfully Removed!";
					return true;
					//if (designation.Resource_Employee.Count == 0)
					//{
					//	db.Common_Designation.Remove(designation);
					//	db.SaveChanges();
					//	msg = "Successfully Removed!";
					//	return true;
					//}
					//else
					//{
					//	msg = "This Nationality Is Already In Use So Cannot Be Deleted.";
					//	return false;
					//}
				}
				msg = "Unable To Find The Designation!!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		#endregion

		#region Company
		/// <summary>
		/// Get Company list from the DB
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CompanyBO> GetCompanyList()
		{
			var companyList = (from c in db.Common_Company
								   select new CompanyBO()
								   {
									   CompanyID = c.CompanyID,
									   CompanyName = c.CompanyName
								   }).OrderBy(c => c.CompanyName).ToList();
			return companyList;
		}

		/// <summary>
		/// Save and Update the Company Info Detail to DB
		/// </summary>
		/// <param name="companyBO"></param>
		/// <param name="eMessage"></param>
		/// <returns></returns>
		public bool SaveCompany(CompanyBO companyBO, out string eMessage)
		{
			eMessage = "";
			var existingCompany = db.Common_Company.SingleOrDefault(c => c.CompanyName == companyBO.CompanyName && c.CompanyID != companyBO.CompanyID);
			try
			{
				if (existingCompany == null)
				{
					var dbCompany = db.Common_Company.SingleOrDefault(c => c.CompanyID == companyBO.CompanyID);//get the selected item from the DB
					if (companyBO.CompanyID == 0)//save
					{
						var company = new Common_Company();
						company.CompanyName = companyBO.CompanyName;
						db.Common_Company.Add(company);
						eMessage = "Successfully Saved!";
					}
					else if (dbCompany != null)//update
					{
						dbCompany.CompanyName = companyBO.CompanyName;
						eMessage = "Successfully Updated!";
					}
					else//selected item is not available in the DB
					{
						eMessage = "Unable To Find The Item To Update!";
						return false;
					}
					db.SaveChanges();
					return true;
				}
				eMessage = "Company Already Exist!";
				return false;
			}
			catch (Exception e)
			{
				eMessage = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Get the selected company detail from the DB
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CompanyBO GetCompany(int id)
		{
			var company = (from c in db.Common_Company
							   where c.CompanyID == id
							   select new CompanyBO()
							   {
								  CompanyID= c.CompanyID,
								  CompanyName = c.CompanyName
							   }).SingleOrDefault();
			if (company != null)
			{
				return company;
			}
			return new CompanyBO();
		}

		/// <summary>
		/// Delete the selected company from DB
		/// </summary>
		/// <param name="id"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool DeleteCompany(int id, out string msg)
		{
			msg = "";
			var company = db.Common_Company.SingleOrDefault(c => c.CompanyID == id);
			try
			{
				if (company != null)
				{
					db.Common_Company.Remove(company);
					db.SaveChanges();
					msg = "Successfully Removed!";
					return true;
					//if (designation.Resource_Employee.Count == 0)
					//{
					//	db.Common_Designation.Remove(designation);
					//	db.SaveChanges();
					//	msg = "Successfully Removed!";
					//	return true;
					//}
					//else
					//{
					//	msg = "This Nationality Is Already In Use So Cannot Be Deleted.";
					//	return false;
					//}
				}
				msg = "Unable To Find The Company!!";
				return false;
			}
			catch (Exception exc)
			{
				msg = exc.Message;
				return false;
			}
		}
		#endregion
	}
}
