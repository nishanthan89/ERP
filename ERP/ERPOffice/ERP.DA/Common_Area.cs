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
    
    public partial class Common_Area
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Common_Area()
        {
            this.Common_AreaPostCodeDetails = new HashSet<Common_AreaPostCodeDetails>();
            this.Resource_DepartmentAreaDetails = new HashSet<Resource_DepartmentAreaDetails>();
        }
    
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public Nullable<int> RegionID { get; set; }
    
        public virtual Common_Region Common_Region { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Common_AreaPostCodeDetails> Common_AreaPostCodeDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource_DepartmentAreaDetails> Resource_DepartmentAreaDetails { get; set; }
    }
}
