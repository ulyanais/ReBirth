//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReBirth
{
    using System;
    using System.Collections.Generic;
    
    public partial class Specialist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specialist()
        {
            this.Sessions = new HashSet<Session>();
        }
    
        public int ID { get; set; }
        public int userID { get; set; }
        public string nameS { get; set; }
        public string surnameS { get; set; }
        public string patronymicS { get; set; }
        public string TipeOfSpecialist { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual User User { get; set; }
    }
}
