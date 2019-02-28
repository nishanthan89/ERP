///-----------------------------------------------------------------
///   Namespace:      ERP.Admin.BL
///   Class:          CompanyInfoBL
///   Description:    Company  Business Logic  Of ERP Companies 
///   Author:         S.Sathiyan                   Date: 02/06/2016
///   Notes:          
///   Revision History: Last Update ON 22/07/2016
///   Name:   s.sathiyan        Date: 21/06/2016       Description: 
///-----------------------------------------------------------------

using ERP.Admin.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;
using ERP.Address.ViewModels;
using ERP.Address.BL;
using System.Web;
using System.Data;

namespace ERP.Admin.BL
{
   
    public class CompanyInfoBL
    {
        private ERPEntities db = new ERPEntities();
        AddressBL addressBL = new AddressBL();
       CompanyInfoViewModel companyInfo = new CompanyInfoViewModel();

        /// <summary>
        /// dropdown Common date
        /// </summary>
        public List<DateFormatModel> CommonDate()  
        {
            var DateFormat = (from dt in db.Common_DateFormat select new DateFormatModel
              { DateFormat = dt.DateFormat, DateFormatID=dt.DateFormatID }).ToList();
            return DateFormat;
        }

        /// <summary>
        ///  dropDown Common Currency
        /// </summary>
        public List<CurrencyFormat> CommonCurrency()
        {
            var currency = (from cuy in db.Common_CurrencyFormat select new CurrencyFormat {
                CurrencyID=cuy.CurrencyID,
                Currency=cuy.Currency
            }).ToList();                    
            return currency;
        }

        /// <summary>
        ///  DropDOwn Common Time
        /// </summary>
        public List<Common_TimeFormat> CommonTime()
        {

            var timeFormat = (db.Common_TimeFormat).ToList();
            
            return timeFormat;
        }

        /// <summary>
        ///  DropDown Get TimeZone of Global level 
        /// </summary>
        public ReadOnlyCollection<TimeZoneInfo> GetTimeZone() 
        {
            ReadOnlyCollection<TimeZoneInfo> TimeZones = TimeZoneInfo.GetSystemTimeZones();

            return TimeZones;
        }

        /// <summary>
        /// Other common  details about Company  
        /// </summary>
        /// <returns></returns>
        public CompanyInfoViewModel CmyInfoDets() 
        {
            var companyInfoview =(from d in db.Common_HostSettings select d).ToList();
           int addressID= int.Parse(companyInfoview.Where(x => x.HostKey == "AddressID").Single().HostValue);
            
            if (companyInfoview != null)
            {

                var AddressDetails = (from a in db.Common_Address where (a.AddressID == addressID) select a).SingleOrDefault();
                companyInfo.AddressID = int.Parse(companyInfoview.Where(x => x.HostKey == "AddressID").Single().HostValue);
                companyInfo.CompanyName = companyInfoview.Where(x => x.HostKey == "CompanyName").Single().HostValue;
                companyInfo.Currency = companyInfoview.Where(x => x.HostKey.Equals("Currency")).Single().HostValue;
                companyInfo.CurrencyID = CommonCurrency().Where(x => x.Currency.Equals(companyInfo.Currency)).Single().CurrencyID;
                companyInfo.DateFormat = companyInfoview.Where(x => x.HostKey.Equals("DateFormat")).Single().HostValue;
                companyInfo.DateFormatID = CommonDate().Where(x => x.DateFormat.Equals(companyInfo.DateFormat)).Single().DateFormatID;
                companyInfo.EmailAddress = companyInfoview.Where(x => x.HostKey.Equals("EmailAddress")).Single().HostValue;
                companyInfo.FaxNo = companyInfoview.Where(x => x.HostKey.Equals("FaxNo")).Single().HostValue;
                companyInfo.RegistrationNo = companyInfoview.Where(x => x.HostKey.Equals("RegistrationNo")).Single().HostValue;
                companyInfo.TelephoneNo = Int64.Parse(companyInfoview.Where(x => x.HostKey.Equals("TelephoneNo")).Single().HostValue);
                companyInfo.TimeZone = companyInfoview.Where(x => x.HostKey.Equals("TimeZone")).Single().HostValue;
                companyInfo.TimeZoneID = GetTimeZone().Where(x => x.DisplayName.Equals(companyInfo.TimeZone)).Single().Id;
                companyInfo.VATNo = companyInfoview.Where(x => x.HostKey.Equals("VATNo")).Single().HostValue;
                companyInfo.Website = companyInfoview.Where(x => x.HostKey.Equals("Website")).Single().HostValue;
                companyInfo.TimeFormat = companyInfoview.Where(x => x.HostKey.Equals("TimeFormat")).Single().HostValue;
                companyInfo.TimeID = CommonTime().Where(x => x.TimeFormat.Equals(companyInfo.TimeFormat)).Single().TimeID;
                companyInfo.Address = new AddressViewModel()// Add Address Details 
                {

                    AddressID = AddressDetails.AddressID,
                    BuildingName = AddressDetails.AddressLine1,
                    StreetName = AddressDetails.AddressLine2,
                    Locality = AddressDetails.AddressLine3,
                    Town = AddressDetails.Town,
                    County = AddressDetails.County,
                    Postcode = AddressDetails.PostCode,
                    CountryID = AddressDetails.CountryID.Value,
                };
      
                if(db.Common_HostLogo.Where(x => x.LogoID == 8).Select(x => x.HostLogoContent).SingleOrDefault() != null)
                {
                    companyInfo.PhotoString = "data: image / png; base64," + Convert.ToBase64String(db.Common_HostLogo.Where(x => x.LogoID == 8).Select(x => x.HostLogoContent).SingleOrDefault()); // res
                    companyInfo.LogoID = db.Common_HostLogo.Select(x => x.LogoID).SingleOrDefault();
                }

                
                return companyInfo;
            }
            return null;
        }

        /// <summary>
        /// update Company Information  
        /// </summary>
        /// <param name="companyUpdateInfo"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool  UpdateCompanyInfo( CompanyInfoViewModel companyUpdateInfo ,out string errorMsg, HttpPostedFileBase UploadFile)
        {
            errorMsg = "";


            if (companyUpdateInfo.Address.CountryID != 1)
            {
                errorMsg = "PAF Is Only Used For UK, So please Select a Country As a UK..!! ";
                return false;

            }

            try
            {
                var companyDB=db.Common_HostSettings.ToList();
               
                companyDB.Where(x => x.HostKey.Equals( "CompanyName")).Single().HostValue = companyUpdateInfo.CompanyName;
                companyDB.Where(x => x.HostKey.Equals("EmailAddress")).Single().HostValue = companyUpdateInfo.EmailAddress;
                companyDB.Where(x => x.HostKey.Equals("FaxNo")).Single().HostValue = companyUpdateInfo.FaxNo;   
                companyDB.Where(x => x.HostKey.Equals("RegistrationNo")).Single().HostValue = companyUpdateInfo.RegistrationNo;
                companyDB.Where(x => x.HostKey.Equals("TelephoneNo")).Single().HostValue = companyUpdateInfo.TelephoneNo.ToString();
                companyDB.Where(x => x.HostKey.Equals("VATNo")).Single().HostValue = companyUpdateInfo.VATNo;
                companyDB.Where(x => x.HostKey.Equals("Website")).Single().HostValue = companyUpdateInfo.Website;
                companyDB.Where(x => x.HostKey.Equals("DateFormat")).Single().HostValue = CommonDate().Where(x => x.DateFormatID.Equals(companyUpdateInfo.DateFormatID)).Select(x => x.DateFormat).Single();
                companyDB.Where(x => x.HostKey.Equals("Currency")).Single().HostValue = CommonCurrency().Where(x => x.CurrencyID.Equals(companyUpdateInfo.CurrencyID)).Select(x => x.Currency).Single();
                companyDB.Where(x => x.HostKey.Equals("TimeZone")).Single().HostValue = GetTimeZone().Where(x=>x.Id.Equals(companyUpdateInfo.TimeZoneID)).Select(x=>x.DisplayName).Single();
                companyDB.Where(x => x.HostKey.Equals("TimeFormat")).Single().HostValue = CommonTime().Where(x => x.TimeID.Equals(companyUpdateInfo.TimeID)).Select(x => x.TimeFormat).Single();
                if (companyUpdateInfo.Address.Postcode != null)
                {
                    companyUpdateInfo.Address.AddressID = addressBL.CheckAddress(companyUpdateInfo.Address);
                    companyUpdateInfo.AddressID = companyUpdateInfo.Address.AddressID;
                    if (companyUpdateInfo.Address.AddressID == 0)
                    {
                        errorMsg = "Company Details is Not Updated.......!";
                        return false;
                    }
                    companyDB.Where(x => x.HostKey.Equals("AddressID")).Single().HostValue = companyUpdateInfo.Address.AddressID.ToString();
                    

                }


                if (UploadFile != null)
                {
                    
                    if (UploadFile.ContentType == "image/jpeg" || UploadFile.ContentType == "image/png" || UploadFile.ContentType == "image/gif")
                    {

                        Common_HostLogo ComLogo = db.Common_HostLogo.SingleOrDefault(x => x.LogoID == 8);
                        ComLogo.HostLogoContentType = UploadFile.ContentType;
                        ComLogo.HostLogoContent = ConvertToBytes(UploadFile);
                     
                        db.SaveChanges();
                        errorMsg = "Successfully Saved.......!";
                    }
                    else
                    {
                        errorMsg = "ImageType Should Be 'jpeg' Or 'png' Or 'gif' ";
                        return false;
                    }

                }
                else
                {
                    Common_HostLogo CmHostlogo = new Common_HostLogo();
                    CmHostlogo.HostLogoContentType = null;
                    CmHostlogo.HostLogoContent= null;

                    db.SaveChanges();
                    errorMsg = "SuccessFully Saved.......!";
                }
                return true;
            }
            catch (Exception ex)
            {

                errorMsg = ex.Message;
                return false;
            }
            
        }
        /// <summary>
        /// Convert to byte (Logo Image)
        /// </summary>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        public byte[] ConvertToBytes(HttpPostedFileBase UploadFile)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(UploadFile.InputStream);
            imageBytes = reader.ReadBytes((int)UploadFile.ContentLength);
            return imageBytes;
        }


        /// <summary>
        /// Save the Logo to DB 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveLogo(byte[] content, out string errorMsg) 
        {
            errorMsg = "";
            try
            {
                var hostLogo = db.Common_HostLogo.Where(x => x.LogoID == 8).FirstOrDefault();
                hostLogo.HostLogoContent = content;
                hostLogo.HostLogoContentType = "image/png";
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get Logo from DB
        /// </summary>
        /// <returns></returns>
        /// 
        public byte[] GetImage(string imageId)
        {
            var img = db.Common_HostLogo.Where(x => x.LogoID == 8).FirstOrDefault();
            var Logo = img.HostLogoContent;
            Stream stream = new MemoryStream(Logo);
            var ResizeLogo = ScaleBySize(stream,imageId);
            return ResizeLogo;
        }

        /// <summary>
        /// To resize the logo
        ///  Resize the Image for your Purpose 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] ScaleBySize(Stream stream ,string imageId)
        {
            using (Bitmap origImage = new Bitmap(stream))
            {
                int LogoSize = 0;
                float SourceWidth = 0;
                float SourceHeight = 0;
                float DestHeight = 0;
                float DestWidth =0;
                int SourceX = 0;
                int SourceY = 0;
                int DestX = 0;
                int DestY = 0;


                if (imageId == "large")
                {
                  LogoSize = 50;  //20
                  SourceWidth =origImage.Width; 
                  SourceHeight = origImage.Height;
                  DestHeight = LogoSize;
                  DestWidth =(SourceWidth/ SourceHeight) * LogoSize;
                   SourceX = 0;
                   SourceY = 0;
                   DestX = 0;
                   DestY = 0;
                }
                else {
                     LogoSize = 20;  //20
                     SourceWidth = origImage.Width;
                     SourceHeight = origImage.Height;
                     DestHeight = LogoSize;
                     DestWidth =(SourceWidth / SourceHeight) * LogoSize;
                     SourceX = 0;
                     SourceY = 0;
                     DestX = 0;
                     DestY = 0;
                }
                ////Resize Image Height > Width
                //if (SourceWidth > (2 * SourceHeight))
                //{
                //    DestHeight = LogoSize;
                //    DestWidth = (float)(SourceWidth * LogoSize / SourceHeight);
                //}
                //// Resize Image Height = Width
                //else if (SourceWidth == SourceHeight)
                //{
                //    int h = LogoSize;
                //    DestWidth = h;
                //    DestHeight = (float)(SourceHeight * h / SourceWidth);
                //}
                //// Resize Image Height < Width
                //else
                //{
                //    int h = LogoSize;
                //    DestWidth = h / 2;
                //    DestHeight = (float)(SourceHeight * h / SourceWidth);
                //}

                //Draw the logo image
                Bitmap BmPhoto = new Bitmap((int)DestWidth, (int)DestHeight, PixelFormat.Format32bppPArgb);
                BmPhoto.SetResolution(origImage.HorizontalResolution, origImage.VerticalResolution);
                Graphics GrPhoto = Graphics.FromImage(BmPhoto);
                GrPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                GrPhoto.DrawImage(origImage,
                    new Rectangle(DestX, DestY, (int)DestWidth, (int)DestHeight),
                    new Rectangle(SourceX, SourceY, (int)SourceWidth, (int)SourceHeight),
                    GraphicsUnit.Pixel);
                GrPhoto.Dispose();


                using (Bitmap newImage = new Bitmap((int)DestWidth, (int)DestHeight))
                {
                    using (Graphics gr = Graphics.FromImage(newImage))
                    {
                        gr.SmoothingMode = SmoothingMode.AntiAlias;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.DrawImage(origImage, new Rectangle(0, 0, (int)DestWidth, (int)DestHeight));

                        MemoryStream ms = new MemoryStream();
                        newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
        }




        public bool DeleteImage(int id, out string eMsg)
        {
            eMsg = "";


            try
            {
                

                var hostLogo = db.Common_HostLogo.Where(x => x.LogoID == id).FirstOrDefault();
                hostLogo.HostLogoContent = null;
                hostLogo.HostLogoContentType = null;
                db.SaveChanges();
                eMsg = "SuccessFully Deleted..!";
            }
            catch (Exception ex)
            {

                eMsg = ex.Message;
                return false;

            }
            return true; 

        }
    }
}
