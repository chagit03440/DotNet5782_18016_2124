
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
    /// 
    public partial class DroneWindow : Window
    {
        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        private IBL.IBL myBl;
        private IBL.BO.Drone drone ;
        public event Action Update=delegate { };
        public DroneWindow()
        {

            InitializeComponent();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
        }
        

        public DroneWindow(IBL.IBL myBl)
        {
            InitializeComponent();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            drone = new Drone();
            comboStatus.ItemsSource = myBl.GetStations();
            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            grdAdd.Visibility = Visibility.Visible;
            grdRelease.Visibility = Visibility.Hidden;
            grdUpdate.Visibility = Visibility.Hidden;
            this.drone = new IBL.BO.Drone();
            this.myBl = myBl;
            DataContext = drone;
            txtMaxWeight.Visibility= Visibility.Hidden;
            txtStatus.Visibility= Visibility.Hidden;
            lblStatus.Content = "Station";
            txtBattery.Visibility = Visibility.Hidden;
            //comboPackage.Visibility = Visibility.Hidden;
            txtLongtitude.Visibility = Visibility.Hidden;
            txtLatitude.Visibility = Visibility.Hidden;
            lblBattery.Visibility = Visibility.Hidden;
            //lblPackage.Visibility = Visibility.Hidden;
            lblLatitude.Visibility = Visibility.Hidden;
            lblLongtitude.Visibility = Visibility.Hidden;
        }
        
        public DroneWindow(IBL.IBL myBl, Drone d)
        {

            InitializeComponent();
           
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
            this.myBl = myBl;
            drone = new Drone();
            drone = d;
            grdAdd.Visibility = Visibility.Hidden;
            comboMaxWeight.Visibility= Visibility.Hidden;
            comboStatus.Visibility= Visibility.Hidden;
            grdRelease.Visibility = Visibility.Hidden;
            fillTextbox(drone);
            if (d.Status == DroneStatuses.Free)
            {
                btnCharge.Visibility = Visibility.Visible;
                btnAssignment.Visibility = Visibility.Visible;
            }

            if (d.Status == DroneStatuses.Maintenance)
            {
                btnRelease.Visibility = Visibility.Visible;
            }

            if (d.Status == DroneStatuses.Shipping)
            {
                btnDelivery.Visibility = Visibility.Visible;
                btnPickedup.Visibility = Visibility.Visible;
            }
            txtStatus.IsEnabled = false;
            txtMaxWeight.IsEnabled = false;
            txtId.IsEnabled = false;
            comboStatus.IsEnabled = false;
            comboMaxWeight.IsEnabled = false;
            txtBattery.IsEnabled = false;
            //comboPackage.IsEnabled = false;
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
                Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        

        private void fillTextbox(DroneForList d)
        {

            txtStatus.Text = d.Status.ToString();
            txtMaxWeight.Text = d.MaxWeight.ToString();
            txtId.Text = d.Id.ToString();
            txtModel.Text = d.Model.ToString();
            txtBattery.Text = d.Battery.ToString() + "%";
           // comboPackage.Text = d.ParcelId.ToString();
            txtLongtitude.Text = d.DroneLocation.Longitude.ToString();
            txtLatitude.Text = d.DroneLocation.Lattitude.ToString();
        }
        private void fillTextbox(Drone d)
        {
            if (d != null)
            {
                txtStatus.Text = d.Status.ToString();
                txtMaxWeight.Text = d.MaxWeight.ToString();
                txtId.Text = d.Id.ToString();
                txtModel.Text = d.Model.ToString();
                txtBattery.Text = d.Battery.ToString() + "%";
                //if (d.Package != null)
                //{
                //    comboPackage.Text = d.Package.Id.ToString();

                //}
                //else
                //    comboPackage.Text = 0.ToString();
                txtLongtitude.Text = d.Location.Longitude.ToString();
                txtLatitude.Text = d.Location.Lattitude.ToString();
                return;
            }
             
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
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
                Update();

            }
            catch(BLAlreadyExistExeption ex)
            {
                MessageBox.Show("this id already exist");
                flag = false;
               
            }
           
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                flag = false;
            }
           if(!flag)
            {
                var bc = new BrushConverter();
                txtId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

            }
            new DroneListWindow(myBl);
            if(flag)
                this.Close();
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
                myBl.PickedupParcel( drone.Id);
                DroneForList dr = myBl.GetDroneForList(drone.Id);
                fillTextbox(dr);
                MessageBox.Show("the parcel was collected by the parcel");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
           

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }



  
}

