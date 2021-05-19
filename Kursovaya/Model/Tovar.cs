namespace Kursovaya.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tovar")]
    public partial class Tovar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tovar()
        {
            Sdelka = new HashSet<Sdelka>();

        }

        [Key]
        public int id_tovar { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public decimal price { get; set; }

        [StringLength(50)]
        public string sort { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sdelka> Sdelka { get; set; }
    }
}
