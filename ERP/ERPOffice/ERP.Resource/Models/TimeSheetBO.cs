using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
   public class TimeSheetBO
    {
        public int? FrequencyID { get; set; }
      
        public int? ResourcesID { get; set; }
        [Display(Name = "Resource")]
        public string ResourcesName { get; set; }
        public int  MonthID { get; set; }
        [Display(Name = "Month ")]
        public string MonthName { get; set; }
        public int? YearID { get; set; }
        [Display(Name = "Year ")]
        public string Year { get; set; }
        public int? StatusID { get; set; }
        [Display(Name = "Resource. Confm")]
        public string Status { get; set; }
        [Display(Name = "Confm. Date")]
        public DateTime? StatusDate { get; set; }
        [Display(Name = " Month Of Timesheet")]
        public DateTime? TimeSheetDate { get; set; }
        public int? TimesheetID { get; set; }
        public int  dethLine { get; set; }
        public string dethLineNew { get; set; }
        [Display(Name = "No. Of Shift")]
        public int NoOFShift { get; set; }
        [Display(Name = "Total Shift Hours ")]
        public double ? TolShiftHrs { get; set; }
        [Display(Name = "No. Of Holiday ")]
        public double? TolHly { get; set; }
        [Display(Name = "No. Of Holiday Hours")]
        public double? TolHlyHours { get; set; }
        [Display(Name = "Remarks ")]
        public string Comments { get; set; }
        [Display(Name = "Admin. Confm ")]
        public string locked { get; set; }
        [Display(Name = "Locked By")]
        public string LockedBy  { get; set; }
        [Display(Name = "Locked Date Time")]
        public DateTime? LockedDateTime { get; set; }
        [Display(Name =" Time Sheet ")]
        public string TimeSheetFrequency { get; set; }
        [Display(Name ="HandOver Hours")]
        public double? HandOverHours { get; set; }
        [Display(Name = "Payment")]
        public double? Payment { get; set; }
        public List<double> PaymentList { get; set; }
        [Display(Name = "List Of NoOFShift")]
        public IEnumerable<DateTime> ListOFShift { get; set; }

        [Display(Name = "List of Total ShiftHours")]
        public IEnumerable<int> ListTolShiftHrs { get; set; }
        [Display(Name = "Total Holiday Hours")]
        public IEnumerable<Double?> ListOFTolHolidayHours { get; set; }
        [Display(Name = "Total Holidays")]
        public IEnumerable<DateTime> ListOFTolHoliday{ get; set; }
        [Display(Name = "Shift Pattern")]
        public List<TimeSheetShiftPattern> shiftPattern { get; set; }

        public List<TimeSheetHolidayPattern> holidayPattern { get; set; }

        public DateTime dtDethTime1 { get; set; }
        public DateTime dtDethTime2 { get; set; }
        public IEnumerable<TimeSheetPayment> PaymentValueList { get; set; }


        public TimeSheetAuthBO TimeSheetAuthBO { get; set; }
    }

    //public class TimeSearchBO
    //{

    //    public int? ResourcesID { get; set; }
    //    [Display(Name = "Resource")]
    //    public string ResourcesName { get; set; }
    //    public int MonthID { get; set; }
    //    [Display(Name ="Month ")]
    //    public string MonthName { get; set; }
    //    public int? YearID { get; set; }
    //    [Display(Name ="Year")]
    //    public string Year { get; set; }
    //    public int? StatusID { get; set; }
    //    [Display(Name ="Status")]
    //    public string Status { get; set; }

    //    public int? FrequencyID { get; set; }
    //    [Display(Name = "Frequency")]
    //    public string Frequency { get; set; }


    //}

    //public class ShiftPattern
    //{
    //    public int? ShiftStartTime { get; set; }
    //    public int? Duration { get; set; }
    //    public int ShiftpatternId { get; set; }
    //    public string Description { get; set; }
    //    public int Breakduration { get; set; }
    //    public DateTime? ShiftDate { get; set; }
    //}


    //public class HolidayPattern
    //{
    //      public DateTime? HolidayDate { get; set; }
    //    public string HolidayType { get; set; }
    //    public string Comment { get; set; }
    //    public string Authorized { get; set; }
    //    public double? HolidayHours { get; set; }
    //}

    //public class PaymentValue
    //{
    //    public Tuple<DateTime, DateTime> dateRange { get; set; }
    //    public double? Payment { get; set; }
    //    public int Pfrequency { get; set; }
    //    public DateTime? PstartDate { get; set; }
    //    public DateTime? PendDate { get; set; }
    //    public int  PEmpID { get; set; }
    //    public string PEmpName { get; set; }
    //}
}
