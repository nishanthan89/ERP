using ERP.MvcBinder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MVCHelpers
{
    public static class DateHelper
    {
        public static MvcHtmlString DateDisplay(this HtmlHelper htmlHelper, DateTime date)
        {
           // DateTime covertedDateTime = ConvertTimeZone.ConvertServerTimeToTimeZone(date);
            string datein = date.ToString(MvcApplication.DateFormat);
            //TagBuilder builder = new TagBuilder("span");
            //builder.SetInnerText(datein);
            return new MvcHtmlString(datein);//builder.ToString());
        }

        public static MvcHtmlString DateTimeDisplay(this HtmlHelper htmlHelper, DateTime date)
        {
            //DateTime covertedDateTime = ConvertTimeZone.ConvertServerTimeToTimeZone(date);
            string datein = date.ToString(MvcApplication.DateFormat);
            //TagBuilder builder = new TagBuilder("span");
            //builder.SetInnerText(datein);
            datein = datein + " " + date.ToString(MvcApplication.TimeFormat);
            return new MvcHtmlString(datein);//builder.ToString());
        }
    }
}