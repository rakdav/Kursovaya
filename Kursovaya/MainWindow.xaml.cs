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
    }
}
