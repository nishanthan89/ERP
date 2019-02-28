using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace ERP.MvcBinder
{
    public class ConvertTimeZone
    {
        ////Convert server time to timezone time to display
        //public static DateTime ConvertServerTimeToTimeZone(DateTime date)
        //{
        //    DateTime utcSourceTime = TimeZoneInfo.ConvertTimeToUtc(date, TimeZoneInfo.Local);
        //    DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcSourceTime, TimeZoneInfo.FindSystemTimeZoneById(MvcApplication.TimeZoneID));
        //    return convertedDateTime;
        //}

        ////Convert time zone time ti server time to store
        //public static DateTime ConvertTimeZoneToServerTime(DateTime date)
        //{
        //    DateTime utcSourceTime = TimeZoneInfo.ConvertTimeToUtc(date, TimeZoneInfo.FindSystemTimeZoneById(MvcApplication.TimeZoneID));
        //    DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcSourceTime, TimeZoneInfo.Local);
        //    return convertedDateTime;
        //}

        public static TimeSpan ConvertTimeServertoTimeZone()
        {
            DateTime serverTime = DateTime.Now;
            // CompanyDA stringValue = new CompanyDA();
            //var hostSetting = stringValue.FindCompany();
            var timeZone = (from tz in TimeZoneInfo.GetSystemTimeZones() select tz).ToList();
            string timeZoneID = timeZone.Where(x => x.DisplayName == "GMT Standard Time").Select(x => x.Id).FirstOrDefault();

            DateTime utcSourceTime = TimeZoneInfo.ConvertTimeToUtc(serverTime, TimeZoneInfo.Local);
            DateTime destTime = utcSourceTime;
            if (!String.IsNullOrEmpty(timeZoneID))
            {
                TimeZoneInfo destTZ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
                destTime = TimeZoneInfo.ConvertTimeFromUtc(utcSourceTime, destTZ);
            }

            var time = new TimeSpan(destTime.Hour, destTime.Minute, 0);
            return time;
        }
    }
}