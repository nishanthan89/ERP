//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.DA
{
    using System;
    using System.Collections.Generic;
    
    public partial class Common_PaymentType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Common_PaymentType()
        {
            this.Resource_EmployeePayment = new HashSet<Resource_EmployeePayment>();
        }
    
        public int PaymentTypeID { get; set; }
        public string PaymentName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource_EmployeePayment> Resource_EmployeePayment { get; set; }
    }
}
