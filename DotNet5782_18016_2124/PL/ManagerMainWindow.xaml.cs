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
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="MyBl"></param>
        public ManagerMainWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            InitializeComponent();
        }
        /// <summary>
        ///  a click to open the dronelist window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)

        {

            new DroneListWindow(myBl).Show();
        }

        /// <summary>
        /// a click to open the stationlist window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStation_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(myBl).Show();
        }
        /// <summary>
        ///  a click to open the customerlist window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(myBl).Show();
        }
        /// <summary>
        /// a click to open the parcellist window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(myBl).Show();
        }
    }
}
