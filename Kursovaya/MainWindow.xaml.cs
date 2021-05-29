using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ModelDB"].
            ConnectionString;
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
            sdelkaList.ItemsSource = null;
            using (ModelDB db = new ModelDB())
            {
                var result = from sdelka in db.Sdelka
                             join tovar in db.Tovar on sdelka.id_tovar equals tovar.id_tovar
                             join client in db.Client on sdelka.id_client equals client.Id_client
                             select new SdelkaModel
                             {
                                 data = sdelka.data,
                                 count = sdelka.count,
                                 sum = sdelka.count*tovar.price,
                                 name = sdelka.Tovar.name,
                                 fio=sdelka.Client.Fio,
                                 id=sdelka.sdelka1
                             };
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

        private void sdelkaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sdelkaList.SelectedItem!=null)
            {
                SdelkaModel sdelkaModel = (SdelkaModel)sdelkaList.SelectedItem;
                using (ModelDB db = new ModelDB())
                {
                    Sdelka sdelka = db.Sdelka.Where(p => p.sdelka1 == sdelkaModel.id).FirstOrDefault();
                    id_client.Text = db.Client.Where(p => p.Id_client == sdelka.id_client).FirstOrDefault().Fio;
                    id_tovar.Text = db.Tovar.Where(p => p.id_tovar == sdelka.id_tovar).FirstOrDefault().name;
                    count.Text = sdelka.count.ToString();
                    sum.Text = sdelka.sum.ToString();
                    Date.SelectedDate = sdelka.data;
                    sdelkaModel = null;
                }
            }
          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SdelkaModel sdelkaModel = (SdelkaModel)sdelkaList.SelectedItem;
            using (ModelDB db = new ModelDB())
            {
                Sdelka sdelka = db.Sdelka.Where(p => p.sdelka1 == sdelkaModel.id).FirstOrDefault();
                sdelka.sum = decimal.Parse(sum.Text);
                sdelka.sdelka1 = sdelkaModel.id;
                sdelka.count = int.Parse(count.Text);
                sdelka.data = Date.SelectedDate.Value;
                string name = id_tovar.SelectedValue.ToString();
                string fio = id_client.SelectedValue.ToString();
                sdelka.id_tovar = db.Tovar.Where(p => p.name.Equals(name)).FirstOrDefault().id_tovar;
                sdelka.id_client = db.Client.Where(p => p.Fio.Equals(fio)).FirstOrDefault().Id_client;
                db.Entry(sdelka).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            UpdateSdelka();
            sdelkaList.SelectedItem = null;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SdelkaModel sdelkaModel = (SdelkaModel)sdelkaList.SelectedItem;
            using (ModelDB db = new ModelDB())
            {
                Sdelka sdelka = db.Sdelka.Where(p => p.sdelka1 == sdelkaModel.id).FirstOrDefault();
                if (sdelka != null)
                {
                    db.Entry(sdelka).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            sdelkaList.SelectedItem = null;
            UpdateSdelka();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Filter filter = new Filter();
            if (filter.ShowDialog() == true)
            {
                List<SdelkaModel> sdelkaModels = new List<SdelkaModel>();
                string query = filter.Querry;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var cells = row.ItemArray;
                            SdelkaModel sdelka = new SdelkaModel();
                            sdelka.id = int.Parse(cells[0].ToString());
                            sdelka.data = DateTime.Parse(cells[1].ToString());
                            sdelka.count = int.Parse(cells[2].ToString());
                            sdelka.sum = decimal.Parse(cells[3].ToString());
                            using (ModelDB db = new ModelDB())
                            {
                                int id_t = int.Parse(cells[4].ToString());
                                sdelka.name= db.Tovar.Where(p => p.id_tovar==id_t).FirstOrDefault().name;
                                int id_c = int.Parse(cells[5].ToString());
                                sdelka.fio = db.Client.Where(p => p.Id_client ==id_c).FirstOrDefault().Fio;
                            }
                            sdelkaModels.Add(sdelka);
                        }
                    }
                    sdelkaList.ItemsSource = null;
                    sdelkaList.ItemsSource = sdelkaModels;               
                }
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            UpdateSdelka();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Text files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
                if (openFile.ShowDialog() == true)
                {
                    string path = openFile.FileName;
                    ISheet sheet;
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        stream.Position = 0;
                        XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                        sheet = xssWorkbook.GetSheetAt(0);
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            Client client = new Client();
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                   if(j==0) client.Fio = row.GetCell(j).ToString();
                                   if (j == 1) client.Age =int.Parse(row.GetCell(j).ToString());
                                   if (j == 2) client.Phone = row.GetCell(j).ToString();
                                   if (j == 3) client.City = row.GetCell(j).ToString();
                                }
                            }
                            using (ModelDB db=new ModelDB())
                            {
                                db.Client.Add(client);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
