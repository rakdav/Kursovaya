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

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Client client= new Client();
                      Clients.Add(client);
                      SelectedClient = client;
                }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      Client p1;
                      using (ModelDB db = new ModelDB())
                      {
                          p1 = db.Client.Where(p=>p.Id_client==SelectedClient.Id_client).FirstOrDefault();
                      }
                      using (ModelDB db = new ModelDB())
                      {
                          if (p1 != null)
                          {
                              p1.Id_client = SelectedClient.Id_client;
                              p1.Fio = SelectedClient.Fio;
                              p1.Age = SelectedClient.Age;
                              p1.Phone = SelectedClient.Phone;
                              p1.City = SelectedClient.City;
                              db.Entry(p1).State = System.Data.Entity.EntityState.Modified;
                              db.SaveChanges();
                          }
                          else
                          {
                              db.Client.Add(SelectedClient);
                              db.SaveChanges();
                          }
                      }
                  }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      using (ModelDB db = new ModelDB())
                      {
                          Client p1 = db.Client.Where(p => p.Id_client == SelectedClient.Id_client).FirstOrDefault();
                          if (p1 != null)
                          {
                              db.Entry(p1).State = System.Data.Entity.EntityState.Deleted;
                              db.SaveChanges();
                              Clients.Remove(SelectedClient);
                          }
                      }

                  }));
            }
        }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        public void FilterByFio(String fio)
        {
            using (ModelDB db = new ModelDB())
            {
                List<Client> clients = db.Client.Where(p=>p.Fio.StartsWith(fio)).ToList();
                foreach (Client c in clients) Clients.Add(c);
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

        public ClientViewModel(string fio)
        {
            Clients = new ObservableCollection<Client>();
            using (ModelDB db = new ModelDB())
            {
                List<Client> clients = db.Client.Where(p => p.Fio.StartsWith(fio)).ToList();
                 foreach (Client c in clients) Clients.Add(c);
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    //
    }
}
