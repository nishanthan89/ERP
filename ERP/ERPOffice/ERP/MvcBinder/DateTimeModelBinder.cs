using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERP.MvcBinder
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            try
            {
                if (!String.IsNullOrEmpty(MvcApplication.DateFormat) && String.IsNullOrEmpty(value.AttemptedValue))
                {
                    DateTime date;

                    if (DateTime.TryParseExact(value.AttemptedValue,
                        MvcApplication.DateFormat.Replace("{0:", "").Replace("}", ""),
                        new DateTimeFormatInfo(),
                        DateTimeStyles.None,
                        out date))
                    {
                        return date.Date;
                    }
                    else
                    {
                        bindingContext.ModelState.AddModelError(
                            bindingContext.ModelName, String.Format("{0} is an invalid date format", value.AttemptedValue)
                            );
                    }
                }
            }
            catch { }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
