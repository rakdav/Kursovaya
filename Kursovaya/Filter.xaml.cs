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
using System.Windows.Shapes;
using System.Configuration;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public Filter()
        {
            InitializeComponent();
            first.Visibility = Visibility.Collapsed;
            second.Visibility = Visibility.Collapsed;
            Querry = "Select * from Sdelka";
        }

        public string FilterPriceCount { get; set; }
        public string Querry { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (one.IsChecked == true)
                Querry += "=" + first.Text;
            if (two.IsChecked == true)
                Querry += ">" + first.Text + " and " + FilterPriceCount + "<" + second.Text;
            this.DialogResult = true;
        }

        private void filterPriceCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(filterPriceCount.SelectedIndex==0)
            {
                FilterPriceCount ="sum";
            }
            else
            {
                FilterPriceCount = "count";
            }
            Querry += " where " + FilterPriceCount;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Visible;
        }

       
    }
}
