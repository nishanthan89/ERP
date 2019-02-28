///-----------------------------------------------------------------
///   Namespace:      ERP.Resource.BL
///   Class:          TimeSheetBL
///   Description:    TimeSheet Details Of ERP Companies Resources
///   Author:         S.Sathiyan                   Date: 03/09/2016
///   Notes:          Calculate Shift Details ,Holiday Details & Salary Details
///   Revision History: Last Update ON 27/09/2016
///   Name:   s.sathiyan        Date: 27/09/2016     
///-----------------------------------------------------------------
using ERP.DA;
using ERP.Resource.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.Resource.BL
{
    public class TimeSheetBL
    {
        private ERPEntities db = new ERPEntities();


        //Get TimeSheet Frequency Details
        public List<Common_TimeSheetFrequency> getTimeSheetFrz()
        {
            var getTimeSheetFrz = (from fz in db.Common_TimeSheetFrequency select fz).OrderBy(x => x.TimeSheetFrequencyName).ToList();

            return getTimeSheetFrz;
        }

        //Get Resources Details
        public List<Resource_Employee> getResources()
        {

            var getResources = (from re in db.Resource_Employee.Where(x => x.DateOfLeave == null) select re).OrderBy(x => x.FirstName).ToList();

            return getResources;
        }

        //Get Status Details of Holiday
        public List<Resource_HolidayStatusType> getStatus()
        {

            var getStatus = (from st in db.Resource_HolidayStatusType select st).OrderBy(x => x.HolidayStatusType).ToList();

            return getStatus;
        }


        //Get Timesheet  List using Filter Options 
        public List<TimeSheetBO> getTimeListSample(TimeSearchBO tsearchBO)
        {

            int Intyear = int.Parse(tsearchBO.Year);
            var MonthLastDate = DateTime.DaysInMonth(Intyear, tsearchBO.MonthID);
            List<int> MonthDateList = Enumerable.Range(1, MonthLastDate).ToList();
            var StrMonthDateList = MonthDateList.ConvertAll(x => x.ToString());
            var DBDatedethLine = from fq in db.Common_TimeSheetFrequency where (fq.TimeSheetFrequencyID == 1) select (fq.TimeSheetFrequencyDeadline);
            string xxx = DBDatedethLine.SingleOrDefault();
            int dethLine = int.Parse(xxx);
            //define a Death date for TimeSheet
            int DBdethLine;
            if (MonthDateList.Contains(dethLine))
            {
                DBdethLine = dethLine; 

            }

            else
            {
                DBdethLine = MonthDateList.LastOrDefault();

            }
            int yy = int.Parse(tsearchBO.Year);
            int mm = tsearchBO.MonthID;
            int dd = DBdethLine;

            DateTime dtDethTime = new DateTime(yy, mm, dd);
            DateTime dtDethTime2 = dtDethTime.AddMonths(-1);

            int daysInMonth = DateTime.DaysInMonth(yy, mm);
            int daysInMonth2 = DateTime.DaysInMonth(yy, mm - 1);

            int DateGaps = (dtDethTime - dtDethTime2).Days;

            

                var timeSheetList = (from shift in db.Resource_ShiftSchedule
                                     join rs in db.Resource_Employee on shift.EmployeeID equals rs.EmployeeID
                                     join ts in db.Resource_TimeSheet.Where(x => x.TimeSheetDate > dtDethTime2 && x.TimeSheetDate <= dtDethTime) on rs.EmployeeID equals ts.ResourceID into authorised
                                     from ts in authorised.DefaultIfEmpty()
                                     where ((shift.ShiftDate >= dtDethTime2 && shift.ShiftDate < dtDethTime) && shift.ActualOffTime != null)
                                     select new TimeSheetBO()
                                     {
                                         ResourcesID = rs.EmployeeID,
                                         ResourcesName = rs.FirstName,
                                         FrequencyID = rs.TimeSheetFrequencyID,
                                         TimeSheetFrequency = rs.Common_TimeSheetFrequency.TimeSheetFrequencyName,
                                         PaymentValueList = db.Resource_EmployeePayment.Where(x => x.EmployeeID == rs.EmployeeID &&
                                         ((x.StartDate <= dtDethTime2 && x.EndDate >= dtDethTime2) ||
                                         (x.StartDate < dtDethTime && x.EndDate > dtDethTime) ||
                                         (x.StartDate >= dtDethTime2 && x.EndDate < dtDethTime) ||
                                         (x.StartDate < dtDethTime && x.EndDate == null)))
                                            .Select(x => new TimeSheetPayment()
                                            {
                                                PEmpID = rs.EmployeeID,
                                                PEmpName = rs.FirstName,

                                                Payment = x.PaymentAmount,
                                                PstartDate = x.StartDate,
                                                PendDate = x.EndDate,
                                                Pfrequency = x.Common_PaymentType.PaymentTypeID,
                                            }).OrderBy(x => x.PstartDate),
                                         PaymentList = db.Resource_TimeSheet.Where(x => x.ResourceID == rs.EmployeeID && x.TimeSheetDate > dtDethTime2 && x.TimeSheetDate <= dtDethTime && x.TimeSheetAuthorizationDate != null).Select(x => x.Payment).ToList(),
                                         Payment = ts.Payment,
                                         dtDethTime1 = dtDethTime,
                                         dtDethTime2 = dtDethTime2,
                                         ListOFShift = db.Resource_ShiftSchedule.Where(x => x.EmployeeID == rs.EmployeeID && x.ShiftDate != null && (dtDethTime2 <= x.ShiftDate) && (dtDethTime > x.ShiftDate)).Select(x => x.ShiftDate).ToList(),
                                         NoOFShift = db.Resource_ShiftSchedule.Where(x => x.EmployeeID == rs.EmployeeID && x.ShiftDate != null && (dtDethTime2 <= x.ShiftDate) && (dtDethTime > x.ShiftDate)).Select(x => x.ShiftDate).Count(),
                                         ListOFTolHoliday = db.Resource_Holiday.Where(x => x.ResourceID == rs.EmployeeID && x.HolidayDate != null && (dtDethTime2 <= x.HolidayDate) && (dtDethTime > x.HolidayDate)).Select(x => x.HolidayDate).Distinct().ToList(),
                                         TolHly = db.Resource_Holiday.Where(x => x.ResourceID == rs.EmployeeID && x.HolidayDate != null && (dtDethTime2 <= x.HolidayDate) && (dtDethTime > x.HolidayDate)).Distinct().Count(),
                                         ListTolShiftHrs = db.Resource_ShiftSchedule.Where(x => x.EmployeeID == rs.EmployeeID && x.ShiftDate != null && (dtDethTime2 <= x.ShiftDate) && (dtDethTime > x.ShiftDate)).Select(x => x.Resource_ShiftPattern.Duration).ToList(),
                                         TolHlyHours = db.Resource_Holiday.Where(x => x.ResourceID == rs.EmployeeID && x.HolidayDate != null && (dtDethTime2 <= x.HolidayDate) && (dtDethTime > x.HolidayDate)).Sum(x => (x.BookingHrs * 60)),
                                         ListOFTolHolidayHours = db.Resource_Holiday.Where(x => x.ResourceID == rs.EmployeeID && x.HolidayDate != null && (dtDethTime2 <= x.HolidayDate) && (dtDethTime > x.HolidayDate)).Select(x => x.BookingHrs).ToList(),
                                         holidayPattern = db.Resource_Holiday.Where(x => x.ResourceID == rs.EmployeeID && x.HolidayDate != null && (dtDethTime2 <= x.HolidayDate) && (dtDethTime > x.HolidayDate))
                                                           .Select(x => new TimeSheetHolidayPattern { HolidayDate = x.HolidayDate, HolidayType = x.Resource_Holiday_Type.HolidayTypeName, Authorized = "", Comment = "", HolidayHours = x.BookingHrs }).ToList(),
                                         shiftPattern = db.Resource_ShiftSchedule.Where(x => x.EmployeeID == rs.EmployeeID && x.ShiftDate != null
                                         && (dtDethTime2 <= x.ShiftDate) && (dtDethTime > x.ShiftDate))
                                                            .Select(x => new TimeSheetShiftPattern { ShiftDate = x.ShiftDate, ShiftStartTime = x.Resource_ShiftPattern.ShiftStartTime, Duration = x.Resource_ShiftPattern.Duration, Description = x.Resource_ShiftPattern.Description }).ToList(),


                                         TolShiftHrs = db.Resource_ShiftSchedule.Where(x => x.EmployeeID == rs.EmployeeID && x.ShiftDate != null && (dtDethTime2 <= x.ShiftDate) && (dtDethTime > x.ShiftDate)).Sum(x => x.Resource_ShiftPattern.Duration),
                                         dethLine = DBdethLine,
                                         TimeSheetDate =ts.TimeSheetDate,
                                         TimesheetID = ts.TimeSheetID,
                                         locked = ts.LockedDateTime != null ? "Yes" : "No",
                                         LockedBy = ts.LockedBy,
                                         LockedDateTime = ts.LockedDateTime,
                                         Status = ts.TimeSheetAuthorizationDate != null ? "Authorized" : "UnAuthorized",// Authorized,UnAuthorized
                                         StatusDate = ts.TimeSheetAuthorizationDate,
                                         StatusID=ts.TimeSheetAuthorizationDate!=null ? 2 : 4,
                                     }).GroupBy(p => p.ResourcesID).Select(grp => grp.FirstOrDefault()).ToList();


                return timeSheetList.Where(x=>x.dtDethTime1 <= DateTime.Now).ToList();



            }
        
    
                

        //Take a Value from controller & filter the Timesheet List
        public IEnumerable<TimeSheetBO> GetfilterTimeSheet(TimeSearchBO TsearchBO)
        {
            var filterTimeSheetList = getTimeListSample(TsearchBO);



            if (TsearchBO != null && TsearchBO.ResourcesID != null && TsearchBO.StatusID == null)
            {
                return filterTimeSheetList.Where(x => (x.ResourcesID.Equals(TsearchBO.ResourcesID)));

            }

            if (TsearchBO != null && TsearchBO.ResourcesID == null && TsearchBO.StatusID != null)
            {
                return filterTimeSheetList.Where(x => (x.StatusID.Equals(TsearchBO.StatusID)));
            }

            if (TsearchBO != null && TsearchBO.ResourcesID != null && TsearchBO.StatusID != null)
            {
                return filterTimeSheetList.Where(x => (x.StatusID.Equals(TsearchBO.StatusID) && x.ResourcesID.Equals(TsearchBO.ResourcesID)));
            }


            return filterTimeSheetList;

        }


        //Authorized & UnAuthorized the Timesheet Resources Details 
        public bool TimeSheetAuthorizedFun(TimeSheetBO timeSheetBO, out string Msg, string ButtonType ,out string status,out DateTime StatusDate)
        {

            
            Msg = "";
            // Authorized 
            if (ButtonType == "Authorized")
            {

                if (timeSheetBO.TimesheetID ==null)
                {
                    try
                    {

                        Resource_TimeSheet authorized = new Resource_TimeSheet();
                        
                        authorized.ResourceID =db.Resource_Employee.Where(x => x.EmployeeID == timeSheetBO.ResourcesID).Select(x => x.EmployeeID).FirstOrDefault();
                        authorized.TimeSheetDate = (DateTime)(timeSheetBO.TimeSheetDate==null ? timeSheetBO.dtDethTime1 :timeSheetBO.TimeSheetDate);
                        authorized.TimeSheetFrequencyID = db.Common_TimeSheetFrequency.Where(x => x.TimeSheetFrequencyID == timeSheetBO.FrequencyID).Select(x => x.TimeSheetFrequencyID).FirstOrDefault();
                        authorized.TimeSheetAuthorizationDate = DateTime.Now;
                        authorized.Approved = "Yes";
                        authorized.LockedBy = HttpContext.Current.User.Identity.Name;
                        authorized.LockedDateTime = DateTime.Now;
                        authorized.Comments = "No Comments ";
                        authorized.NoOfShift = timeSheetBO.NoOFShift;
                        authorized.TotalShiftHours =(double)(timeSheetBO.TolShiftHrs==null ? 0: timeSheetBO.TolShiftHrs);
                        authorized.NoOfHolidays = (double)(timeSheetBO.TolHly == null ? 0 : timeSheetBO.TolHly); 
                        authorized.NoOfHolidayHours = (double)(timeSheetBO.TolHlyHours == null ? 0 : timeSheetBO.TolHlyHours); 
                        authorized.IsAct = true;
                        authorized.Payment = (double)timeSheetBO.Payment;
                        db.Resource_TimeSheet.Add(authorized);
                        db.SaveChanges();
                        Msg = "Timesheet Is Successfully Authorized For  " + timeSheetBO.TimeSheetDate + " Date";
                        status = "Authorized";
                        StatusDate = DateTime.Now;
                        return true;
                    }

                    catch (Exception e)
                    {
                        StatusDate = DateTime.Now;
                        status = timeSheetBO.Status;
                        Msg = e.Message;
                        return false;
                    }

                }
                else
                {
                    //Re Authorized 
                    var isExitTimesheet = db.Resource_TimeSheet.Where(x => x.TimeSheetID == timeSheetBO.TimesheetID && x.TimeSheetAuthorizationDate ==null ).SingleOrDefault();
                    if (isExitTimesheet != null)
                    {

                        try
                        {    

                            isExitTimesheet.ResourceID = db.Resource_Employee.Where(x => x.EmployeeID == timeSheetBO.ResourcesID).Select(x => x.EmployeeID).FirstOrDefault();
                            isExitTimesheet.TimeSheetDate = (DateTime)(timeSheetBO.TimeSheetDate == null ? timeSheetBO.dtDethTime1 : timeSheetBO.TimeSheetDate);
                            isExitTimesheet.TimeSheetFrequencyID = db.Common_TimeSheetFrequency.Where(x => x.TimeSheetFrequencyID == timeSheetBO.FrequencyID).Select(x => x.TimeSheetFrequencyID).FirstOrDefault();
                            isExitTimesheet.TimeSheetAuthorizationDate = DateTime.Now;
                            isExitTimesheet.Approved = "Yes";
                            isExitTimesheet.LockedBy = HttpContext.Current.User.Identity.Name;
                            isExitTimesheet.LockedDateTime = DateTime.Now;
                            isExitTimesheet.Comments = "No Comments ";
                            isExitTimesheet.NoOfShift = timeSheetBO.NoOFShift;
                            isExitTimesheet.TotalShiftHours = (double)(timeSheetBO.TolShiftHrs == null ? 0 : timeSheetBO.TolShiftHrs);
                            isExitTimesheet.NoOfHolidays = (double)(timeSheetBO.TolHly == null ? 0 : timeSheetBO.TolHly);
                            isExitTimesheet.NoOfHolidayHours = (double)(timeSheetBO.TolHlyHours == null ? 0 : timeSheetBO.TolHlyHours);
                            isExitTimesheet.IsAct = true;
                            isExitTimesheet.Payment = (double)timeSheetBO.Payment;
                           
                            db.SaveChanges();
                            Msg = "Timesheet Is Successfully Authorized For  " + timeSheetBO.TimeSheetDate + " Date ";
                            status = "Authorized";
                            StatusDate = DateTime.Now;
                            return true;
                        }

                        catch (Exception e)
                        {
                            status = timeSheetBO.Status;
                            Msg = e.Message;
                            StatusDate = DateTime.Now;
                            return false;
                        }
                    }
                    else
                    {
                        Msg = " Timesheet Is Already Authorized For " + db.Resource_TimeSheet.Where(x => x.ResourceID == timeSheetBO.ResourcesID && x.TimeSheetDate.Month==timeSheetBO.MonthID).Select(x => x.TimeSheetDate).SingleOrDefault()+ "Date";
                        status = timeSheetBO.Status;
                        StatusDate = DateTime.Now;
                        return false;
                    }
                }
            }
            //UnAuthorized The Resources Details 
            else
            {

                var isExitUnAuzedTimesheet = db.Resource_TimeSheet.SingleOrDefault(x => x.TimeSheetID == timeSheetBO.TimesheetID && x.TimeSheetAuthorizationDate!=null);
                if (isExitUnAuzedTimesheet != null)
                {

                    try
                    {
                        isExitUnAuzedTimesheet.TimeSheetAuthorizationDate = null;
                        isExitUnAuzedTimesheet.IsAct = false;

                        db.SaveChanges();
                        Msg = "Timesheet Is Successfully UnAuthorized  For" + timeSheetBO.TimeSheetDate + "Date";
                        status = "UnAuthorized";
                        StatusDate = DateTime.Now;
                        return true;
                    }
                    catch (Exception e)
                    {
                        status = timeSheetBO.Status;
                        Msg = e.Message;
                        StatusDate = DateTime.Now;
                        return false;
                    }


                }
                // Fail UnAuthorized
                else {
                    status = timeSheetBO.Status;
                    Msg = "Timesheet Is Already UnAuthorized For" + timeSheetBO.TimeSheetDate + "Date";
                    StatusDate = DateTime.Now;
                    return false;

                }
            }

            

        }




    }
}
