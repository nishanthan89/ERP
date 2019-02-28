using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MvcBinder
{
    public class TimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                // Ensure there's incomming data
                var key = bindingContext.ModelName;
                var valueProviderResult = bindingContext.ValueProvider.GetValue(key);
                if (valueProviderResult == null || string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
                {
                    return null;
                }
                // Preserve it in case we need to redisplay the form 
                bindingContext.ModelState.SetModelValue(key, valueProviderResult);
                // Parse 

                string[] rowValue = (string[])valueProviderResult.RawValue;

                var hours = ((string[])rowValue[0].Split(':'))[0];
                var minutes = ((string[])rowValue[0].Split(':'))[1];
                // A TimeSpan represents the time elapsed since midnight
                var time = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), 0);
                return time;
            }
            catch { return null; }
        }
    }
}