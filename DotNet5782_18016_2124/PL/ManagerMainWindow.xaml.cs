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
using BlApi;
using BL;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManagerMainWindow : Window
    {
        BlApi.IBL myBl;
//constructor
        public ManagerMainWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            InitializeComponent();
        }
        //a click to open the dronelist window
        private void Button_Click(object sender, RoutedEventArgs e)

        {

            new DroneListWindow(myBl).Show();
        }
        //a click to open the stationlist window
        private void BtnStation_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(myBl).Show();
        }
    //a click to open the customerlist window
        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(myBl).Show();
        }
        //a click to open the parcellist window
        private void BtnParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(myBl).Show();
        }
    }
}
