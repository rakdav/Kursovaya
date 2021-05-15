namespace Kursovaya.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Table("Client")]
    public partial class Client:INotifyPropertyChanged
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Sdelka = new HashSet<Sdelka>();
        }

        
        private int id_client;
        [Key]
        public int Id_client
        {
            get { return id_client; }
            set
            {
                id_client = value;
                OnPropertyChanged("Id_client");
            }
        }

       
        private string fio;
        [Required]
        [StringLength(100)]
        public string Fio
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChanged("Fio");
            }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        
        private string phone;
        [Required]
        [StringLength(50)]
        public String Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
       
        private string city;

        

        [Required]
        [StringLength(50)]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("city");
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sdelka> Sdelka { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
