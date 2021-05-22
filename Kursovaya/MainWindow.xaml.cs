using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kursovaya.Model;
using Kursovaya.ViewModel;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateTovar();
            edit.Visibility = Visibility.Collapsed;
            delete.Visibility = Visibility.Collapsed;
            phonesList.SelectedIndex = 0;
            DataContext = new ClientViewModel();
            UpdateSdelka();
            using (ModelDB db = new ModelDB())
            {
                List<Client> clients = db.Client.ToList();
                foreach(Client client in clients)
                {
                    id_client.Items.Add(client.Fio);
                }
                List<Tovar> tovars = db.Tovar.ToList();
                foreach (Tovar tovar in tovars)
                {
                    id_tovar.Items.Add(tovar.name);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ModelDB db = new ModelDB())
            {
                Tovar tovar = new Tovar();
                tovar.name = name.Text;
                tovar.price = decimal.Parse(price.Text);
                tovar.sort = sort.Text;
                tovar.description = description.Text;
                db.Tovar.Add(tovar);
                db.SaveChanges();
                UpdateTovar();
            }
        }
        private void UpdateTovar()
        {
            using (ModelDB db = new ModelDB())
            {
                var Tovars = db.Tovar.ToList();
                TovarTable.ItemsSource = null;
                TovarTable.ItemsSource = Tovars;
            }
            name.Text = "";
            price.Text = "";
            sort.Text = "";
            description.Text = "";
        }

        private void UpdateSdelka()
        {
            using (ModelDB db = new ModelDB())
            {
                var result = from sdelka in db.Sdelka
                             join tovar in db.Tovar on sdelka.id_tovar equals tovar.id_tovar
                             join client in db.Client on sdelka.id_client equals client.Id_client
                             select new
                             {
                                 data = sdelka.data,
                                 count = sdelka.count,
                                 summa = sdelka.count*tovar.price,
                                 id_tovar = tovar.name,
                                 id_client=client.Fio,
                                 sdelka1=sdelka.sdelka1
                             };
                sdelkaList.ItemsSource = null;
                sdelkaList.ItemsSource = result.ToList();
            }
        }

        private void TovarTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            edit.Visibility = Visibility.Visible;
            delete.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Collapsed;

            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                name.Text = tovar.name;
                price.Text = tovar.price.ToString();
                sort.Text = tovar.sort;
                description.Text = tovar.description;
            }
            
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                tovar.name = name.Text;
                tovar.price = decimal.Parse(price.Text);
                tovar.sort = sort.Text;
                tovar.description = description.Text;
                using (ModelDB db = new ModelDB())
                {
                    db.Entry(tovar).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                UpdateTovar();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                using (ModelDB db = new ModelDB())
                {
                    db.Entry(tovar).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                UpdateTovar();
            }
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            String str = filter.Text;
            if (phonesList.SelectedIndex==0)
            {
                if (str.Length != 0)
                {
                    using (ModelDB db = new ModelDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.name.StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
            else if(phonesList.SelectedIndex == 1)
            {
                if (str.Length != 0)
                {
                    using (ModelDB db = new ModelDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.price.ToString().StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
            else
            {
                if (str.Length != 0)
                {
                    using (ModelDB db = new ModelDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.sort.StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateTovar();
            filter.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataContext = new ClientViewModel(super.Text);
        }

        private void id_tovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (ModelDB db = new ModelDB())
            {
                string tovarName = id_tovar.SelectedItem.ToString();
                decimal price = db.Tovar.Where(p => p.name.Equals(tovarName)).FirstOrDefault().price;
                Price.Text = price.ToString();
            }
        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (count.Text.Length != 0)
                sum.Text = (decimal.Parse(Price.Text) * int.Parse(count.Text)).ToString();
            else sum.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (ModelDB db = new ModelDB())
            {
                int id_cl = db.Client.Where(p => p.Fio.Equals(id_client.SelectedItem.ToString())).FirstOrDefault().Id_client;
                int id_tv = db.Tovar.Where(p => p.name.Equals(id_tovar.SelectedItem.ToString())).FirstOrDefault().id_tovar;
                Sdelka sdelka = new Sdelka();
                sdelka.id_client = id_cl;
                sdelka.id_tovar = id_tv;
                sdelka.sum = decimal.Parse(sum.Text);
                sdelka.count = int.Parse(count.Text);
                sdelka.data = Date.SelectedDate.Value;
                db.Sdelka.Add(sdelka);
                db.SaveChanges();
                UpdateSdelka();
            }
        }
    }
}
