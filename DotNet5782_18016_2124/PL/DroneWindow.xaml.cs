
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
        private IBL.BO.Drone drone ;
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
            drone = new Drone();
            comboStatus.ItemsSource = myBl.GetStations();
            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            grdAdd.Visibility = Visibility.Visible;
            grdUpdate.Visibility = Visibility.Hidden;
            this.drone = new IBL.BO.Drone();
            this.myBl = myBl;
            DataContext = drone;

            lblStatus.Content = "Station";
            txtBattery.Visibility = Visibility.Hidden;
            comboPackage.Visibility = Visibility.Hidden;
            txtLongtitude.Visibility = Visibility.Hidden;
            txtLatitude.Visibility = Visibility.Hidden;
            lblBattery.Visibility = Visibility.Hidden;
            lblPackage.Visibility = Visibility.Hidden;
            lblLatitude.Visibility = Visibility.Hidden;
            lblLongtitude.Visibility = Visibility.Hidden;
        }
        
        public DroneWindow(IBL.IBL myBl, Drone d)
        {

            InitializeComponent();
            this.myBl = myBl;
            drone = new Drone();
            drone = d;
            grdAdd.Visibility = Visibility.Hidden;
            new DroneWindow(myBl).Show();
            grdRelease.Visibility = Visibility.Hidden;
            fillTextbox(drone);
            //if (d.Status == DroneStatuses.Free)
            //{
            //    btnCharge.Visibility = Visibility.Visible;
            //    btnAssignment.Visibility = Visibility.Visible;
            //}

            //if (d.Status == DroneStatuses.Maintenance)
            //{
            //    btnRelease.Visibility = Visibility.Visible;
            //}

            //if (d.Status == DroneStatuses.Shipping)
            //{
            //     btnDelivery.Visibility = Visibility.Visible;
            //    btnPickedup.Visibility = Visibility.Visible;
            //}
            txtId.IsEnabled = false;
            comboStatus.IsEnabled = false;
            comboMaxWeight.IsEnabled = false;
            txtBattery.IsEnabled = false;
            comboPackage.IsEnabled = false;
            txtLongtitude.IsEnabled = false;
            txtLatitude.IsEnabled = false;
        }
        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            drone.Model = txtModel.Text;
            myBl.UpdateDrone(drone);
            MessageBox.Show("the model of the drone was successfully updated");

                DroneForList dr = myBl.GetDroneForList(drone.Id);
                fillTextbox(dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void fillTextbox(DroneForList d)
        {

            comboStatus.Text = d.Status.ToString();
            comboMaxWeight.Text = d.MaxWeight.ToString();
            txtId.Text = d.Id.ToString();
            txtModel.Text = d.Model.ToString();
            txtBattery.Text = d.Battery.ToString() + "%";
            comboPackage.Text = d.ParcelId.ToString();
            txtLongtitude.Text = d.Location.Longitude.ToString();
            txtLatitude.Text = d.Location.Lattitude.ToString();
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
                if (d.Package != null)
                {
                    comboPackage.Text = d.Package.Id.ToString();

                }
                else
                    comboPackage.Text = 0.ToString();
                txtLongtitude.Text = d.Location.Longitude.ToString();
                txtLatitude.Text = d.Location.Lattitude.ToString();
                return;
            }
             
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StationForList s = (StationForList)comboStatus.SelectedItem;
                DroneForList dr = new DroneForList()
                {
                    Id = Convert.ToInt32(txtId.Text),
                    MaxWeight =(WeightCategories) comboMaxWeight.SelectedItem,
                    Model = txtModel.Text,
                };
               
                   
                myBl.AddDrone(dr, Convert.ToInt32(s.Id));
                MessageBox.Show("the drone was successfully added");

                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        
         

        

       
         
      
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            myBl.ReleaseDroneFromRecharge(drone.Id, Convert.ToInt32(txtTime.Text));
            MessageBox.Show("the drone was relase from charge");
            DroneForList dr = myBl.GetDroneForList(drone.Id);
            fillTextbox(dr);
            btnRelease.Visibility = Visibility.Hidden;

            btnCharge.Visibility = Visibility.Visible;
            btnAssignment.Visibility = Visibility.Visible;

            grdUpdate.Visibility = Visibility.Visible;
           
           

        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            grdUpdate.Visibility = Visibility.Hidden;
            grdRelease.Visibility = Visibility.Visible;
        }

        private void btnCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBl.SendDroneToRecharge(drone.Id);
                MessageBox.Show("the drone was sent to charge");
                DroneForList dr = myBl.GetDroneForList(drone.Id);
                fillTextbox(dr);
                btnRelease.Visibility = Visibility.Visible;
                btnCharge.Visibility = Visibility.Hidden;
                btnAssignment.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void btnAssignment_Click(object sender, RoutedEventArgs e)
        {
               try
                {
                    myBl.AssignmentParcelToDrone(drone.Id);
                    MessageBox.Show("the drone belongs to parcel");
                    DroneForList dr = myBl.GetDroneForList(drone.Id);
                    fillTextbox(dr);
                    btnPickedup.Visibility = Visibility.Visible;
                    btnDelivery.Visibility = Visibility.Visible;
                    btnCharge.Visibility = Visibility.Hidden;
                    btnAssignment.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
           }

        

        private void btnDelivery_Click(object sender, RoutedEventArgs e)
        {
             
                try
                {

                    myBl.PackageDeliveryByDrone(drone.Id);
                    MessageBox.Show("the parcel was delivered to the customer");
                    Drone dr = myBl.GetDrone(drone.Id);
                    fillTextbox(dr);

                    btnPickedup.Visibility = Visibility.Hidden;
                    btnDelivery.Visibility = Visibility.Hidden;
                    btnCharge.Visibility = Visibility.Visible;
                    btnAssignment.Visibility = Visibility.Visible;

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            

        }

        private void btnPickedup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBl.PickedupParcel( drone.Package.Id);
                DroneForList dr = myBl.GetDroneForList(drone.Id);
                fillTextbox(dr);
                MessageBox.Show("the parcel was collected by the parcel");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
           

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

