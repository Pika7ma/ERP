//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lplfw.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.Status = "可用";
            this.MaterialStock = new HashSet<MaterialStock>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string Unit { get; set; }
        public int ClassId { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialStock> MaterialStock { get; set; }
    }
}
