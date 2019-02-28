using ERP.Utility.Models;
using System.Collections.Generic;

namespace ERP.Utility.ViewModels
{
	public class PaymentTypeView
    {
        public PaymentTypeBO PaymentType { get; set; }
        public IEnumerable<PaymentTypeBO> PaymentTypeList  { get; set; }

    }
}
