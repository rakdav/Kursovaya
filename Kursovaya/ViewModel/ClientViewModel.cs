using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Kursovaya.Model;

namespace Kursovaya.ViewModel
{
    class ClientViewModel:INotifyPropertyChanged
    {
        private Client selectedClient;
        public ObservableCollection<Client> Clients { get; set; }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        public ClientViewModel()
        {
            Clients = new ObservableCollection<Client>();
            using (ModelDB db = new ModelDB())
            {
                List<Client> clients = db.Client.ToList();
                foreach (Client c in clients) Clients.Add(c);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    
    }
}
