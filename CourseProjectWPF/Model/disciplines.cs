//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseProjectWPF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class disciplines : ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public disciplines()
        {
            this.timetable = new HashSet<timetable>();
        }
    
        public int discipline_id { get; set; }
        public string discipline_name { get; set; }
        public int department_id { get; set; }
    
        public virtual departments departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<timetable> timetable { get; set; }

        public object Clone()
        {
            return new disciplines
            {
                department_id = this.department_id,
                discipline_name = this.discipline_name,
                discipline_id = this.department_id,
                departments = this.departments,
                timetable = this.timetable
            };
        }
    }
}