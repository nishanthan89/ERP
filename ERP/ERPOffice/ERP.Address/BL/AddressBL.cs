using ERP.Address.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Address.BL
{
   public  class AddressBL
    {
        private ERPEntities db = new ERPEntities();

        public long CheckAddress(AddressViewModel addressViewModel)
        {
            long ChekAdd = 0;// to store address Id
            string BuildingName = null;
            string Locality = null;
            string StreetName = addressViewModel.StreetName.Trim();
            string Town = addressViewModel.Town.Trim();
            string Postcode = addressViewModel.Postcode.Trim();
            string County = addressViewModel.County.ToUpper().Trim();
            int Country = (short)addressViewModel.CountryID;
            if (addressViewModel.Locality != null && addressViewModel.BuildingName != null)
            {
                //To Find AddressID
                ChekAdd = (from Add in db.Common_Address
                           where Add.AddressLine1.ToUpper().Trim() == BuildingName &&
                                 Add.AddressLine2.ToUpper().Trim() == StreetName &&
                                 Add.AddressLine3.ToUpper().Trim() == Locality &&
                                 Add.Town.ToUpper().Trim() == Town &&
                                 Add.PostCode.ToUpper().Trim() == Postcode &&
                                 Add.County.ToUpper().Trim() == County &&
                                 Add.CountryID == Country
                           select Add.AddressID).FirstOrDefault();
            }
            else if (addressViewModel != null)
            {
                BuildingName = addressViewModel.BuildingName;

                ChekAdd = (from Add in db.Common_Address
                           where Add.AddressLine1.ToUpper().Trim() == BuildingName &&
                                 Add.AddressLine2.ToUpper().Trim() == StreetName &&
                                 Add.Town.ToUpper().Trim() == Town &&
                                 Add.PostCode.ToUpper().Trim() == Postcode &&
                                 Add.County.ToUpper().Trim() == County &&
                                 Add.CountryID == Country
                           select Add.AddressID).FirstOrDefault();
            }

            else if (addressViewModel.Locality != null)
            {
                Locality = addressViewModel.Locality.ToUpper().Trim();

                ChekAdd = (from Add in db.Common_Address
                           where Add.AddressLine2.ToUpper().Trim() == StreetName &&
                                 Add.AddressLine3.ToUpper().Trim() == Locality &&
                                 Add.Town.ToUpper().Trim() == Town &&
                                 Add.PostCode.ToUpper().Trim() == Postcode &&
                                 Add.County.ToUpper().Trim() == County &&
                                 Add.CountryID == Country
                           select Add.AddressID).FirstOrDefault();
            }
            else
            {
                ChekAdd = (from Add in db.Common_Address
                           where Add.AddressLine2.ToUpper().Trim() == StreetName &&
                                 Add.Town.ToUpper().Trim() == Town &&
                                 Add.PostCode.ToUpper().Trim() == Postcode &&
                                 Add.County.ToUpper().Trim() == County &&
                                 Add.CountryID == Country
                           select Add.AddressID).FirstOrDefault();
            }
            //Address is already exists.
            if (ChekAdd != 0)
            {
                return ChekAdd;//return address id
            }
            //Address is not already exists.
            else
            {
                Common_Address commonAddress = new Common_Address();

                commonAddress.AddressLine1 = addressViewModel.BuildingName;
                commonAddress.AddressLine2 = addressViewModel.StreetName;
                commonAddress.AddressLine3 = addressViewModel.Locality;
                commonAddress.Town = addressViewModel.Town;
                commonAddress.County =addressViewModel.County;
                commonAddress.PostCode = addressViewModel.Postcode;
                commonAddress.CountryID = (short)addressViewModel.CountryID;
                db.Common_Address.Add(commonAddress);
                db.SaveChanges();
                return commonAddress.AddressID;//return new address id
            }
        }

        /// <summary>
        /// Inserts Address
        /// </summary>
        /// <param name="commonAddress"></param>
        /// <returns></returns>
       

        //to get Address list
        //public IEnumerable<Common_Address> GetAddress()
        //{
        //    var Address = (from c in db.Common_Address select c).ToList();
        //    return Address;
        //}

        ////to get Address Details
        //public Common_Address GetAddressById(long? id)
        //{
        //    Common_Address GetAddress = db.Common_Address.Where(m => m.AddressID == id).FirstOrDefault();
        //    return GetAddress;
        //}

        //Retrieve the Country details to List
        public IEnumerable<Common_Country> GetCountryList()
        {
            var Country = (from c in db.Common_Country select c).ToList();
            return Country;
        }

        //To Get default country
        public int GetDefaultCountry()
        {
            return (from c in db.Common_Country.Where(d => d.IsSelected == true) select c.CountryID).FirstOrDefault();
        }

    }
}
