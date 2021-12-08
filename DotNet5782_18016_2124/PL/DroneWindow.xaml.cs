using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL myBl;
        private IBL.BO.Drone drone;
        public DroneWindow()
        {
            InitializeComponent();
        }

        public DroneWindow(IBL.IBL myBl)
        {
            InitializeComponent();
            comboStations.ItemsSource= myBl.GetStations();
            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            grdAdd.Visibility = Visibility.Visible;
            grdUpdate.Visibility = Visibility.Hidden;
            this.drone = new IBL.BO.Drone();
            this.myBl = myBl;
            DataContext = drone;
            
        }
        public droneView(IBL.IBL bl, DroneToList d)
        {
            InitializeComponent();
            this.bl = bl;
            dr = d;
            addGrid.Visibility = Visibility.Hidden;
            realeseFromCharg.Visibility = Visibility.Hidden;
            fillTextbox(d);
            if (dr.status == DroneStatus.Available)
            {
                droneChargeBtn.Visibility = Visibility.Visible;
                sendToDeliveryBtn.Visibility = Visibility.Visible;
            }

            if (dr.status == DroneStatus.Maintenace)
            {
                relaseBtn.Visibility = Visibility.Visible;
            }

            if (dr.status == DroneStatus.Delivery)
            {
                collectBtn.Visibility = Visibility.Visible;
                parcelDeliveryBtn.Visibility = Visibility.Visible;
            }
        }
        public DroneWindow(IBL.IBL myBl, IBL.BO.Drone drone)
        {
            
            
            InitializeComponent();
            grdAdd.Visibility = Visibility.Hidden;
            grdUpdate.Visibility = Visibility.Visible;
            this.myBl = myBl;
            this.drone = drone;
            txtId.Text = drone.Id.ToString();
            txtBattery.Text = drone.Battery.ToString();
            txtModel.Text = drone.Model;
            txtLatitude.Text = drone.Location.Lattitude.ToString();
            txtLongtitude.Text = drone.Location.Longitude.ToString();
            comboMaxWeight.SelectedItem = drone.MaxWeight;
            comboPackage.SelectedItem = drone.Package;
            comboStatus.SelectedItem = drone.Status;
            
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            int sId =(int)comboStations.SelectedItem;
            IBL.BO.DroneForList df = new IBL.BO.DroneForList()
            {
                Id = drone.Id,
                Battery = drone.Battery,
                Location = drone.Location,
                MaxWeight = drone.MaxWeight,
                Model = drone.Model,
                ParcelId = drone.Package.Id,
                Status=drone.Status
            };
            myBl.AddDrone(df,sId);
        }
    }
}
