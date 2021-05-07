namespace Kursovaya.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sdelka")]
    public partial class Sdelka
    {
        [Key]
        [Column("sdelka")]
        public int sdelka1 { get; set; }

        public DateTime data { get; set; }

        public int count { get; set; }

        public decimal sum { get; set; }

        public int id_tovar { get; set; }

        public int id_client { get; set; }

        public virtual Client Client { get; set; }

        public virtual Tovar Tovar { get; set; }
    }
}
