using ERP.Admin.BL;
using ERP.DA;
using ERP.Resource.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.Resource.BL
{
    public class HolidayBL
    {
        private ERPEntities _db = new ERPEntities();
        private GlobalApplicationVariables globalVariables = new GlobalApplicationVariables();
        /// <summary>
        /// Get Resource List
        /// </summary>
        /// <returns></returns>
        public List<Resource_Employee> GetResourceList()
        {
            return _db.Resource_Employee.ToList();
        }
        /// <summary>
        /// Get Holiday Type List
        /// </summary>
        /// <returns></returns>
        public List<Resource_Holiday_Type> GetHolidayTypeList()
        {
            return _db.Resource_Holiday_Type.ToList();
        }
        /// <summary>
        /// Get Holiday Status List
        /// </summary>
        /// <returns></returns>
        public List<Resource_HolidayStatusType> GetHolidayStatusList()
        {
            return _db.Resource_HolidayStatusType.ToList();
        }
        /// <summary>
        /// Assign Holiday
        /// </summary>
        /// <param name="createHoliday"></param>
        public bool AssignHoliday(CreateHolidayViewModel createHoliday,out string msg)
        {
            msg = "";
            int datediffrent = createHoliday.HolidayEndDate.Date.Subtract(createHoliday.HolidayStartDate.Date).Days;
            datediffrent = datediffrent + 1;
            int numofDays = createHoliday.NoofDays;
            DateTime startDate = (DateTime)createHoliday.HolidayStartDate;

            string assigntype = createHoliday.AssignType;

            if (assigntype == "Weekly")
            {
                for (int i = 0; i < datediffrent; i++)
                {

                    if (startDate.DayOfWeek==DayOfWeek.Monday && createHoliday.WeeklyMonday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }

                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Tuesday && createHoliday.WeeklyTuesday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if(CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Wednesday && createHoliday.WeeklyWednesday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Thursday && createHoliday.WeeklyThursday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Friday && createHoliday.WeeklyFriday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Saturday && createHoliday.WeeklySaturday == true)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (startDate.DayOfWeek == DayOfWeek.Sunday && createHoliday.WeeklySunday == true)
                        {
                            createHoliday.HolidayStartDate = startDate;
                            createHoliday.HolidayEndDate = startDate;
                            if (CreateAssignHoliday(createHoliday, out msg) == false)
                            {
                                return false;
                            }
                        }
                    }
                    startDate = startDate.AddDays(1);
                }
            }

            else
            {
                if (datediffrent >= numofDays)
                {
                    for (int i = 0; i < numofDays; i++)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                        startDate = startDate.AddDays(1);
                    }
                    startDate = startDate.AddDays(numofDays);
                    datediffrent = datediffrent - 2 * numofDays;

                }
                else
                {
                    for (int i = 0; i < datediffrent; i++)
                    {
                        createHoliday.HolidayStartDate = startDate;
                        createHoliday.HolidayEndDate = startDate;
                        if (CreateAssignHoliday(createHoliday, out msg) == false)
                        {
                            return false;
                        }
                        startDate = startDate.AddDays(1);
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Create Assign Holiday
        /// </summary>
        /// <param name="createHoliday"></param>
        /// <returns></returns>
        public bool CreateAssignHoliday(CreateHolidayViewModel createHoliday,out string msg)
        {
            msg = "";
            try
            {
                Resource_Holiday resourceHoliday = new Resource_Holiday();

                resourceHoliday.ResourceID = createHoliday.ResourceID;
                resourceHoliday.HolidayTypeID = createHoliday.HolidayTypeID;
                resourceHoliday.BookingHrs = createHoliday.BookingHrs;
                resourceHoliday.HolidayDate = createHoliday.HolidayStartDate;
                resourceHoliday.Comments = createHoliday.Comment;
                _db.Resource_Holiday.Add(resourceHoliday);
               // _db.SaveChanges();

                Resource_HolidayStatus resourceHolidayStatus = new Resource_HolidayStatus();
                resourceHolidayStatus.HolidayID = resourceHoliday.HolidayID;
                resourceHolidayStatus.StatusChangedBy = HttpContext.Current.User.Identity.Name;
                resourceHolidayStatus.StatusDateTime = createHoliday.RequestedOn.Add(DateTime.Now.TimeOfDay);
                resourceHolidayStatus.HolidayStatusTypeID =createHoliday.StatusID==0? _db.Resource_HolidayStatusType.Where(x=>x.HolidayStatusType== "Request").Select(x=>x.HolidayStatusTypeID).FirstOrDefault(): createHoliday.StatusID;
                 //resourceHolidayStatus.Comments = createHoliday.Comment;
                _db.Resource_HolidayStatus.Add(resourceHolidayStatus);
                _db.SaveChanges();
                msg = "Successfully Created";
                return true;
            }
            catch ( Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Get Holiday List
        /// </summary>
        /// <param name="searchHoliday"></param>
        /// <returns></returns>
        public List<CreateHolidayViewModel> GetHolidayList(SearchHolidayViewModel searchHoliday)
        {
            var holidayList = (from rhd in _db.Resource_Holiday
                               select new CreateHolidayViewModel
                               {
                                   ResourceID = (int)rhd.ResourceID,
                                   HolidayID=rhd.HolidayID,
                                   BookingHrs=(float)rhd.BookingHrs,
                                   Resource=rhd.Resource_Employee.FirstName,
                                   HolidayDate=(DateTime)rhd.HolidayDate,
                                   HolidayTypeID=(int)rhd.HolidayTypeID,
                                   HoliColor=rhd.Resource_Holiday_Type.IconCode,
                                   HolidayType=rhd.Resource_Holiday_Type.HolidayTypeName,
                                  // StatusChangedByID=rhd.Resource_HolidayStatus.OrderByDescending(x=>x.StatusDateTime).Select(x=>x.StatusChangedBy).FirstOrDefault(),
                                   StatusChangedBy= rhd.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.StatusChangedBy).FirstOrDefault(),
                                   Comment=rhd.Comments,
                                   StatusID=rhd.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.HolidayStatusTypeID).FirstOrDefault(),
                                   Status=_db.Resource_HolidayStatusType.Where(y=>y.HolidayStatusTypeID== rhd.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.HolidayStatusTypeID).FirstOrDefault()).Select(y=>y.HolidayStatusType).FirstOrDefault(),
                               });
            if (searchHoliday.SearchResourceID == 0)
            {
                searchHoliday.SearchResourceID = null;
            }
            if (searchHoliday.SearchResourceID != null && searchHoliday.Month != null && searchHoliday.Year!=null)
            {
                holidayList = holidayList.Where(x => x.ResourceID == searchHoliday.SearchResourceID && x.HolidayDate.Month==searchHoliday.Month &&x.HolidayDate.Year==searchHoliday.Year);
            }
           else if (searchHoliday.SearchResourceID == null && searchHoliday.Month != null && searchHoliday.Year != null)
            {
                holidayList = holidayList.Where(x => x.HolidayDate.Month == searchHoliday.Month && x.HolidayDate.Year == searchHoliday.Year);
            }
            else if (searchHoliday.SearchResourceID != null && searchHoliday.Month == null && searchHoliday.Year == null)
            {
                holidayList = holidayList.Where(x => x.ResourceID==searchHoliday.SearchResourceID);
            }
            else if (searchHoliday.SearchResourceID == null && searchHoliday.Month != null && searchHoliday.Year == null)
            {
                holidayList = holidayList.Where(x => x.HolidayDate.Month == searchHoliday.Month);
            }
            else if (searchHoliday.SearchResourceID == null && searchHoliday.Month == null && searchHoliday.Year != null)
            {
                holidayList = holidayList.Where(x =>x.HolidayDate.Year == searchHoliday.Year);
            }
            else if (searchHoliday.SearchResourceID != null && searchHoliday.Month == null && searchHoliday.Year != null)
            {
                holidayList = holidayList.Where(x => x.ResourceID == searchHoliday.SearchResourceID && x.HolidayDate.Year == searchHoliday.Year);
            }
            else if (searchHoliday.SearchResourceID != null && searchHoliday.Month != null && searchHoliday.Year == null)
            {
                holidayList = holidayList.Where(x => x.HolidayDate.Month == searchHoliday.Month && x.ResourceID == searchHoliday.SearchResourceID);
            }
            return holidayList.ToList();       
        }
        /// <summary>
        /// Remove Holiday
        /// </summary>
        /// <param name="searchHoliday"></param>
        /// <returns></returns>
        public bool RemoveHoliday(SearchHolidayViewModel searchHoliday,out string  msg)
        {
            msg = "";
            try
            {
                if (searchHoliday.HolidayID != 0)
                {
                    var resourceHoli = _db.Resource_HolidayStatus.Where(x => x.HolidayID == searchHoliday.HolidayID).ToList();
                    if (resourceHoli.Count != 0)
                    {
                        resourceHoli.ForEach(x =>
                        {
                            _db.Resource_HolidayStatus.Remove(x);
                        });

                    }
                    var resourceStatus = _db.Resource_Holiday.Where(x => x.HolidayID == searchHoliday.HolidayID).Select(x => x).FirstOrDefault();
                    if (resourceStatus != null)
                    {
                        _db.Resource_Holiday.Remove(resourceStatus);
                    }
                    if(resourceHoli.Count == 0&& resourceStatus == null)
                    {
                        msg = "Already Removed";
                        return false;
                    }
                }
                _db.SaveChanges();
                msg = "Successfully Removed";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            
        }
        /// <summary>
        /// Edit Holiday Detail
        /// </summary>
        /// <param name="createHoliday"></param>
        /// <returns></returns>
        public CreateHolidayViewModel EditHolidayDetail(CreateHolidayViewModel createHoliday,out string msg)
        {
            msg = "";
            try
            {
                var editHoli = (from rh in _db.Resource_Holiday 
                                //join rhs in _db.Resource_HolidayStatus on rh.HolidayID equals rhs.HolidayID 
                                where rh.HolidayID==createHoliday.HolidayID
                               // orderby rhs.StatusDateTime descending
                                select new CreateHolidayViewModel
                                {
                                    HolidayID=rh.HolidayID,
                                    ResourceID = (int)rh.ResourceID,
                                    HolidayTypeID = (int)rh.HolidayTypeID,
                                    BookingHrs = (float)rh.BookingHrs,
                                    HolidayDate=(DateTime)rh.HolidayDate,
                                    StatusChangedOn = (DateTime)rh.Resource_HolidayStatus.OrderByDescending(x=>x.StatusDateTime).Select(x=>x.StatusDateTime).FirstOrDefault(),
                                    //StatusChangedByID = rh.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.StatusChangedBy).FirstOrDefault(),
                                    StatusChangedBy = rh.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.StatusChangedBy).FirstOrDefault(),
                                    StatusID = rh.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.HolidayStatusTypeID).FirstOrDefault(),
                                    Status = _db.Resource_HolidayStatusType.Where(y => y.HolidayStatusTypeID == rh.Resource_HolidayStatus.OrderByDescending(x => x.StatusDateTime).Select(x => x.HolidayStatusTypeID).FirstOrDefault()).Select(y => y.HolidayStatusType).FirstOrDefault(),
                                    Comment = rh.Comments,
                                }).FirstOrDefault();
                if (editHoli == null)
                {
                    msg = "Already Removed";
                }
                return editHoli;
            }
            catch (Exception ex)
            {
                 msg = ex.Message;
                return createHoliday;
            }
           
        }
        /// <summary>
        /// Add Edit Holiday
        /// </summary>
        /// <param name="createHoliday"></param>
        /// <returns></returns>
        public bool AddEditHoliday(CreateHolidayViewModel createHoliday,out string msg)
        {
            msg = "";
            try
            {
                var checkRes = _db.Resource_Holiday.Where(x => x.HolidayDate == createHoliday.HolidayDate && x.ResourceID == createHoliday.ResourceID).FirstOrDefault();
                if (checkRes== null||checkRes.HolidayID==createHoliday.HolidayID)
                {
                    var addResHoli = (from rh in _db.Resource_Holiday where rh.HolidayID == createHoliday.HolidayID select rh).FirstOrDefault();

                    if (addResHoli != null)
                    {
                        addResHoli.BookingHrs = createHoliday.BookingHrs;
                        addResHoli.HolidayDate = createHoliday.HolidayDate;
                        addResHoli.HolidayTypeID = createHoliday.HolidayTypeID;
                        addResHoli.ResourceID = createHoliday.ResourceID;
                        addResHoli.Comments = createHoliday.Comment;
                    }
                    var addReHoliSta = _db.Resource_HolidayStatus.Where(x => x.HolidayID == createHoliday.HolidayID && x.HolidayStatusTypeID == createHoliday.StatusID).Select(x => x).FirstOrDefault();
                    if (addReHoliSta != null)
                    {
                        addReHoliSta.HolidayStatusTypeID = createHoliday.StatusID;
                        addReHoliSta.StatusChangedBy = HttpContext.Current.User.Identity.Name;
                        addReHoliSta.StatusDateTime = DateTime.Now;

                    }
                    else
                    {
                        Resource_HolidayStatus resourceHolidayStatus = new Resource_HolidayStatus();
                        resourceHolidayStatus.HolidayID = createHoliday.HolidayID;
                        resourceHolidayStatus.StatusChangedBy = HttpContext.Current.User.Identity.Name;
                        resourceHolidayStatus.StatusDateTime = DateTime.Now;
                        resourceHolidayStatus.HolidayStatusTypeID = createHoliday.StatusID == 0 ? _db.Resource_HolidayStatusType.Where(x => x.HolidayStatusType == "Request").Select(x => x.HolidayStatusTypeID).FirstOrDefault() : createHoliday.StatusID;
                        //resourceHolidayStatus.Comments = createHoliday.Comment;
                        _db.Resource_HolidayStatus.Add(resourceHolidayStatus);

                    }
                    _db.SaveChanges();
                    msg = "Successfully Updated";
                    
                    return true;
                }
                msg = checkRes.Resource_Employee.FirstName+" already booked on "+createHoliday.HolidayDate.Date.ToString(globalVariables.GetCommonDate());
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
           
        }
        /// <summary>
        /// Holiday Detail
        /// </summary>
        /// <param name="detailsHoliday"></param>
        /// <returns></returns>
        public List<DetailsHolidayViewModel> HolidayDetail(DetailsHolidayViewModel detailsHoliday)
        {
            var holiDetail = (from rhs in _db.Resource_HolidayStatus where rhs.HolidayID==detailsHoliday.HolidayID
                              select new DetailsHolidayViewModel
                              {
                                  HolidayID=detailsHoliday.HolidayID,
                                 // StatusChangedByID=rhs.StatusChangedBy,
                                  StatusChangedOn=rhs.StatusDateTime,
                                  StatusID=rhs.HolidayStatusTypeID,
                                  Status=rhs.Resource_HolidayStatusType.HolidayStatusType,
                                  StatusChangedBy=rhs.StatusChangedBy,
                              }).ToList();
            if (holiDetail != null)
            {
                return holiDetail;
            }
            return null;
        }
        /// <summary>
        /// Get Working Age
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        public int GetWorkingAge(int resourceID)
        {
            if (resourceID != 0)
            {
                var joinDate = _db.Resource_Employee.Where(x => x.EmployeeID == resourceID).Select(x => x.DateOfJoin).FirstOrDefault();
                if (joinDate != null)
                {
                    var noDays = joinDate.Value.AddYears(1).Subtract(joinDate.Value.Date).Days;
                    var age = DateTime.Now.Date.Subtract(joinDate.Value.Date).Days / noDays;
                    return age;
                }
            }
             return 0;
        }
        /// <summary>
        /// Get Taken Holiday
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        public int GetTakenHoliday(int resourceID)
        {
            if (resourceID != 0)
            {
                int noHoliday;
                var joinDate = _db.Resource_Employee.Where(x => x.EmployeeID == resourceID).Select(x => x.DateOfJoin).FirstOrDefault();
                var workAge = GetWorkingAge(resourceID);
                if (workAge == 0)
                {
                    var dateVal = joinDate.Value.AddYears(1);
                    noHoliday = _db.Resource_Holiday.Where(x => x.ResourceID == resourceID && x.HolidayDate >= joinDate && x.HolidayDate <= dateVal).Select(x => x.HolidayID).ToList().Count;

                }
                else
                {
                    var dateStart = joinDate.Value.AddYears(workAge);
                    var dateEnd = joinDate.Value.AddYears(workAge + 1);
                    noHoliday = _db.Resource_Holiday.Where(x => x.ResourceID == resourceID && x.HolidayDate >= dateStart && x.HolidayDate <= dateEnd).Select(x => x.HolidayID).ToList().Count;
                }
                return noHoliday;
            }
            return 0;
        }
        /// <summary>
        /// Get Holiday
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        public bool GetDayHoliday(DateTime startDate,int resourceID)
        {
            if (resourceID != 0)
            {
                var takeHoli = _db.Resource_Holiday.Where(x => x.ResourceID == resourceID && x.HolidayDate == startDate).FirstOrDefault();
                if (takeHoli != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
