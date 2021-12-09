
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
using IBL.BO;

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
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    new (myBl).ShowDialog();
        //    fillListView();
        //}
        //private void fillListView()
        //{
        //    IEnumerable<DroneForList> d = new List<DroneForList>();
        //    d = myBl.GetDrones();
        //    if (statusSelector.Text != "")
        //        d = this.myBl.droneFilterStatus((DroneStatuses)statusSelector.SelectedItem);
        //    if (weightSelector.Text != "")
        //        d = bl.droneFilterWheight((WeightCategories)weightSelector.SelectedItem);
        //    DroneListWindow.ItemsSource = d;
        //}

        public DroneWindow(IBL.IBL myBl)
        {
            InitializeComponent();
            comboStations.ItemsSource = myBl.GetStations();
            comboStatus.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));

            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            grdAdd.Visibility = Visibility.Visible;
            grdUpdate.Visibility = Visibility.Hidden;
            this.drone = new IBL.BO.Drone();
            this.myBl = myBl;
            DataContext = drone;
            
        }
        
        public DroneWindow(IBL.IBL myBl, Drone d)
        {

            InitializeComponent();
            this.myBl = myBl;
            drone = d;
            grdAdd.Visibility = Visibility.Hidden;
            new DroneWindow(myBl).Show();
          //  realeseFromCharg.Visibility = Visibility.Hidden;
            fillTextbox(drone);
            //if (d.Status == DroneStatuses.Free)
            //{
            //  //  droneChargeBtn.Visibility = Visibility.Visible;
            //   // sendToDeliveryBtn.Visibility = Visibility.Visible;
            //}

            //if (d.Status ==DroneStatuses.Maintenance)
            //{
            //  //  relaseBtn.Visibility = Visibility.Visible;
            //}

            //if (d.Status == DroneStatuses.Shipping)
            //{
            //   // collectBtn.Visibility = Visibility.Visible;
            //   // parcelDeliveryBtn.Visibility = Visibility.Visible;
            //}
        }

        private void fillTextbox(Drone d)
        {
            if (d != null)
            {
                comboStatus.Text = d.Status.ToString();
                comboMaxWeight.Text = d.MaxWeight.ToString();
                txtId.Text = d.Id.ToString();
                txtModel.Text = d.Model.ToString();
                txtBattery.Text = d.Battery.ToString() + "%";
                comboPackage.Text = d.Package.Id.ToString();
                txtLongtitude.Text = d.Location.Longitude.ToString();
                txtLatitude.Text = d.Location.Lattitude.ToString();
                return;
            }
            comboStatus.Text =" ";
            comboMaxWeight.Text = " ";
            txtId.Text = " ";
            txtModel.Text = " ";
            txtBattery.Text = " ";
            comboPackage.Text = " ";
            txtLongtitude.Text = " ";
            txtLatitude.Text = " ";
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {

        }
    }



    //private void btnAddDrone_Click(object sender, RoutedEventArgs e)
    //{
    //    // int sId =(int)comboStations.SelectedItem;
    //    IBL.BO.DroneForList df = new IBL.BO.DroneForList()
    //    {
    //        Id = drone.Id,
    //        Battery = drone.Battery,
    //        Location = drone.Location,
    //        MaxWeight = drone.MaxWeight,
    //        Model = drone.Model,
    //        ParcelId = drone.Package.Id,
    //        Status = drone.Status
    //    };
    //    //myBl.AddDrone(df,sId);
    //}
}

