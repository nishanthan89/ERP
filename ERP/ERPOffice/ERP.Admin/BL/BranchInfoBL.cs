///-----------------------------------------------------------------
///   Namespace:      ERP.Admin.BL
///   Class:          BranchInfoBL
///   Description:    Branch Business Logic  Of ERP Companies 
///   Author:         S.Sathiyan                   Date: 15/06/2016
///   Notes:          
///   Revision History: Last Update ON 22/07/2016
///   Name:   s.sathiyan        Date: 21/06/2016      
///-----------------------------------------------------------------
using ERP.Address.BL;
using ERP.Address.ViewModels;
using ERP.Admin.Models;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.BL
{
  public  class BranchInfoBL
    {
        public ERPEntities db = new ERPEntities();
        AddressBL addressBL = new AddressBL();
        AddressViewModel address1 = new AddressViewModel();
        /// <summary>
        /// Branch Status View List 
        /// </summary>

        public IEnumerable<Branch_Status> BranchStatusView() 
        {

            var BranchStatus = (from bs in db.Branch_Status select bs).ToList();
            return BranchStatus;
        }
        /// <summary>
        ///  Branch Type List 
        /// </summary>

        public IEnumerable<Branch_Type> BranchTypeView() 
        {
            var BranchTypeview = (from bt in db.Branch_Type select bt).ToList();
            return BranchTypeview;
        }
        /// <summary>
        /// get a Branch List 
        /// </summary>


        public IEnumerable<BranchInfoBO> GetBranchList() 
        {
            var branchInfoList = (from br in db.Branch_Details 
                                  join bs in  db.Branch_Status on br.BranchStatusID equals bs.BranchStatusID
                                  join bt in db.Branch_Type on br.BranchTypeID equals bt.BranchTypeID
                                  join ad in db.Common_Address on br.AddressID equals ad.AddressID
                                    select new BranchInfoBO() {

                                        BranchName=br.BranchName,
                                        BranchID=br.BranchID,
                                        BranchCode=br.BranchCode,
                                        BranchStatusID=bs.BranchStatusID,
                                        BranchStatus=bs.BranchStatus,
                                        BranchType=bt.BranchType,
                                        BranchTypeID=bt.BranchTypeID,
                                        FaxNumber=br.Fax,
                                        TelephoneNumber= br.Telephone,
                                        VATnumber=br.VATNumber,
                                        AddressID=ad.AddressID,
                                        status=br.Status,

                                    } ).ToList().OrderBy(x=>x.BranchName);

            return branchInfoList;
        }

        /// <summary>
        ///  Save Branch Details to DB
        /// </summary>
        /// <param name="branchInfoBO"></param>
        /// <param name="eMessage"></param>
        /// <returns></returns>

        public bool SaveBranchDetails(BranchInfoBO branchInfoBO, out string eMessage) 
        {
            eMessage ="";
            var existingTypeBranchCode = db.Branch_Details.Where(t => t.BranchCode == branchInfoBO.BranchCode).SingleOrDefault();
            var existingTypeBranchName = db.Branch_Details.Where(t => t.BranchName == branchInfoBO.BranchName).SingleOrDefault();
            try
            {
                if (existingTypeBranchCode != null)
                {
                    eMessage = "Branch Code Is Already Exist...!!";
                    return false;
                }

                if (existingTypeBranchName!=null)
                {
                    eMessage = "Branch Name Is Already Exist..!!";
                    return false;
                }

                if (branchInfoBO.Address.CountryID != 1)
                {
                    eMessage = "PAF Is Only Used For UK, So please Select a Country As a UK..!! ";
                    return false;

                }
                var branchdetails = new Branch_Details();
                
                //branchdetails.AddressID = (int)branchInfoBO.AddressID;
                branchdetails.BranchCode = branchInfoBO.BranchCode;
                branchdetails.BranchName = branchInfoBO.BranchName;
                branchdetails.BranchTypeID = (int)branchInfoBO.BranchTypeID;
                branchdetails.BranchStatusID = (int)branchInfoBO.BranchStatusID;
                branchdetails.Fax = branchInfoBO.FaxNumber;
                branchdetails.Telephone = branchInfoBO.TelephoneNumber;
                branchdetails.VATNumber = branchInfoBO.VATnumber;
                branchdetails.Status = false;
                db.Branch_Details.Add(branchdetails);
                if (branchInfoBO.Address.Postcode != null)
                {   
                    branchdetails.AddressID = (int)addressBL.CheckAddress(branchInfoBO.Address);
                    branchInfoBO.AddressID = branchInfoBO.Address.AddressID=branchdetails.AddressID;
                    if (branchInfoBO.Address.AddressID == 0)
                    {
                        eMessage = "AddressId Is Not Saved ";
                        return false;
                    }
                }

                db.SaveChanges();
                eMessage = "Successfully Saved ";
                return true;

            }
            catch (Exception e)
            {
                eMessage = e.Message;
                return false;
            }
        }
        /// <summary>
        ///  Delete Branch details 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        public bool DeleteBranchdetails(int id, out string msg) 
        {
            msg = "";
            var item = db.Branch_Details.SingleOrDefault(t => t.BranchID == id);
            try
            {
                if (item != null)
                {
                    if (item.Resource_BranchDetails.Count == 0 && item.Branch_Holidays.Count==0 && item.Resource_ShiftSchedule.Count()==0 )
                    {
                        db.Branch_Details.Remove(item);
                        db.SaveChanges();
                        msg = "Successfully Removed";
                        return true;
                    }
                    else
                    {
                        msg = "This Branch Cannot Be Removed As It Is Being Used It Either Resources Or Holidays...!";
                        return false;
                    }
                }
                msg = "Unable To Remove!!";
                return false;
            }
            catch (Exception exc)
            {
                msg = exc.Message;
                return false;
            }


        }
        /// <summary>
        ///  Get Edit Branch details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>


       public bool ChkEditBranch(int Id)
        {
            var IsExit = (from br in db.Branch_Details.Where(x => x.BranchID.Equals(Id)) select br).Count()!=0;
            return IsExit;
        }
        public BranchInfoBO GetEditBranch(int Id)
        {

            var branchInfo = (from br in db.Branch_Details.Where(x => x.BranchID.Equals(Id))

                              select new BranchInfoBO()
                              {

                                  AddressID = br.AddressID,
                                  BranchName = br.BranchName,
                                  BranchStatusID = br.BranchStatusID,
                                  BranchTypeID = br.BranchTypeID,
                                  FaxNumber = br.Fax,
                                  TelephoneNumber = br.Telephone,
                                  VATnumber = br.VATNumber,
                                  BranchCode = br.BranchCode,
                                  BranchID = br.BranchID,
                                  BranchStatus = br.Branch_Status.BranchStatus,
                                  BranchType = br.Branch_Type.BranchType,
                                  Address = new AddressViewModel()
                                  {
                                      AddressID = br.AddressID,
                                      BuildingName = br.Common_Address.AddressLine1,
                                      StreetName = br.Common_Address.AddressLine2,
                                      Locality = br.Common_Address.AddressLine3,
                                      Town = br.Common_Address.Town,
                                      County=br.Common_Address.County,
                                      Postcode=br.Common_Address.PostCode,
                                      CountryID=br.Common_Address.CountryID.Value
                                 },

        }).SingleOrDefault();
            return branchInfo;
        }

        /// <summary>
        /// Update Branch details  
        /// </summary>
        /// <param name="branchInfoBO"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        public bool UpdateBranchInfo(BranchInfoBO branchInfoBO, out string msg)
        {


            msg = "";
            var UpdateBranchInfo = db.Branch_Details.SingleOrDefault(br => br.BranchID.Equals(branchInfoBO.BranchID));

            var isExistBranchCode = (from b in db.Branch_Details
                           where b.BranchCode.Equals(branchInfoBO.BranchCode) && b.BranchID != branchInfoBO.BranchID
                           select b).SingleOrDefault();

            var isExistBranchName = (from b in db.Branch_Details where b.BranchName.Equals(branchInfoBO.BranchName) && b.BranchID != branchInfoBO.BranchID select b).SingleOrDefault();
            try
            {
                if (UpdateBranchInfo != null)
                {
                    if (isExistBranchCode != null)
                    {
                        msg = "Branch  Code is  already exist!!";
                        return false;
                    }
                    if (isExistBranchName != null)
                    {
                        msg = "Branch Name is  already exist!!";
                        return false;
                    }
                    if (branchInfoBO.Address.CountryID != 1)
                    {
                        msg = "PAF Is Only Used For UK, So please Select a Country As a UK..!! ";
                        return false;

                    }
                    UpdateBranchInfo.BranchName = branchInfoBO.BranchName;
                    //UpdateBranchInfo.AddressID = (int)branchInfoBO.AddressID;
                    UpdateBranchInfo.BranchStatusID = (int)branchInfoBO.BranchStatusID;
                    UpdateBranchInfo.BranchTypeID = (int)branchInfoBO.BranchTypeID;
                    UpdateBranchInfo.Fax = branchInfoBO.FaxNumber;
                    UpdateBranchInfo.Telephone = branchInfoBO.TelephoneNumber;
                    UpdateBranchInfo.VATNumber = branchInfoBO.VATnumber;
                    UpdateBranchInfo.BranchCode = branchInfoBO.BranchCode;
                    UpdateBranchInfo.Status = branchInfoBO.status;
                  
                    UpdateBranchInfo.AddressID = (int)addressBL.CheckAddress(branchInfoBO.Address);
                    if (branchInfoBO.Address.AddressID == 0)
                    {
                        msg = "AddressId is Not Saved ";
                        return false;
                    }
                    db.SaveChanges();
                    msg = "Successfully Updated!!";
                    return true;
                }
                msg = "Unable to update! No records found with this Branch ID";

                return false;

            }
            catch (Exception exc)
            {
                msg = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Remote Validations For Branch Name 
        /// </summary>
        /// <param name="BranchName"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>

        public bool BranchRemoteNameChk(string BranchName,string BranchId)
        {

            var IsEXitResult = db.Branch_Details.SingleOrDefault(x => x.BranchName.Equals(BranchName) && x.BranchID.ToString() != (BranchId)) != null;

            return IsEXitResult;
        }
        /// <summary>
        /// Remote Validations For Branch Code 
        /// </summary>
        /// <param name="BranchCode"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public bool BranchRemoteCodeChk(string BranchCode ,string BranchId)
        {

            var IsExistResult = db.Branch_Details.SingleOrDefault(x => x.BranchCode.Equals(BranchCode) && x.BranchID.ToString() != BranchId) != null;

            return IsExistResult;
        }

        //public bool CountryRemoteChk( int CountryID)
        //{

        //    var IsExistResult =!(CountryID!=1);

        //    return IsExistResult;
        //}
    }
}
