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
    
    public partial class Common_ImmigrationStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Common_ImmigrationStatus()
        {
            this.Resource_Employee = new HashSet<Resource_Employee>();
        }
    
        public int ImmigrationStatusID { get; set; }
        public string ImmigrationName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource_Employee> Resource_Employee { get; set; }
    }
}
