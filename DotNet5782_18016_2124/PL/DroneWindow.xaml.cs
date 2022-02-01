
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BO;

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

        private DroneWindow windowDrones;

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        private BlApi.IBL myBl;
        private BO.Drone drone;
        bool closing;
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Update = delegate { };
        BackgroundWorker worker;
        bool charge;
       
        public Drone Drone { get => drone; }

        public DroneWindow(BlApi.IBL myBl)
        {
            InitializeComponent();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            this.myBl = myBl;
            drone = new Drone();
            DataContext = drone;

            comboStatus.ItemsSource = myBl.GetStations();
            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            grdAdd.Visibility = Visibility.Visible;
            grdRelease.Visibility = Visibility.Hidden;
            grdUpdate.Visibility = Visibility.Hidden;

            btnShowParcel.Visibility = Visibility.Hidden;
            txtMaxWeight.Visibility = Visibility.Hidden;
            txtStatus.Visibility = Visibility.Hidden;
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
        private void WindowUp()
        {
            btnShowParcel.Visibility = Visibility.Hidden;

            grdAdd.Visibility = Visibility.Hidden;
            comboMaxWeight.Visibility = Visibility.Hidden;
            comboStatus.Visibility = Visibility.Hidden;
            grdRelease.Visibility = Visibility.Hidden;
            //fillTextbox(drone);
            if (drone.Status == DroneStatuses.Free)
            {
                btnCharge.Visibility = Visibility.Visible;
                btnAssignment.Visibility = Visibility.Visible;
            }

            if (drone.Status == DroneStatuses.Maintenance)
            {
                btnRelease.Visibility = Visibility.Visible;
                btnCharge.Visibility = Visibility.Hidden;
                btnAssignment.Visibility = Visibility.Hidden;
                btnDelivery.Visibility = Visibility.Hidden;
                btnPickedup.Visibility = Visibility.Hidden;
                btnShowParcel.Visibility = Visibility.Hidden;
            }

            if (drone.Status == DroneStatuses.Shipping)
            {
                btnCharge.Visibility = Visibility.Hidden;
                btnAssignment.Visibility = Visibility.Hidden;
                btnDelivery.Visibility = Visibility.Visible;
                btnPickedup.Visibility = Visibility.Visible;
                btnShowParcel.Visibility = Visibility.Visible;

            }

            txtStatus.IsEnabled = false;
            txtMaxWeight.IsEnabled = false;
            txtId.IsEnabled = false;
            comboStatus.IsEnabled = false;
            comboMaxWeight.IsEnabled = false;
            txtBattery.IsEnabled = false;

            txtLongtitude.IsEnabled = false;
            txtLatitude.IsEnabled = false;
            if (windowDrones != null)
                Update();
        }
        public DroneWindow(BlApi.IBL myBl, Drone d)
        {

            InitializeComponent();

            //to remove close box from window
            Loaded += ToolWindow_Loaded;
            this.myBl = myBl;
            DataContext = d;
            drone = new Drone();
            drone = d;
            txtBattery.Text = d.Battery.ToString() + "%";
            WindowUp();
        }
        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                drone.Model = txtModel.Text;
                myBl.UpdateDrone(drone);
                MessageBox.Show("the model of the drone was successfully updated");

                DroneForList dr = myBl.GetDroneForList(drone.Id);
                //fillTextbox(dr);
                Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                    MaxWeight = (WeightCategories)comboMaxWeight.SelectedItem,
                    Model = txtModel.Text,
                };


                myBl.AddDrone(dr, Convert.ToInt32(s.Id));
                MessageBox.Show("the drone was successfully added");
                Update();

            }
            catch (BLAlreadyExistExeption ex)
            {
                MessageBox.Show("this id already exist");
                flag = false;

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                flag = false;
            }
            if (!flag)
            {
                var bc = new BrushConverter();
                txtId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

            }
            new DroneListWindow(myBl);
            if (flag)
                this.Close();
        }









        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            myBl.ReleaseDroneFromRecharge(drone.Id, Convert.ToInt32(txtTime.Text));
            MessageBox.Show("the drone was relase from charge");
            DroneForList dr = myBl.GetDroneForList(drone.Id);
            //fillTextbox(dr);
            btnRelease.Visibility = Visibility.Hidden;

            btnCharge.Visibility = Visibility.Visible;
            btnAssignment.Visibility = Visibility.Visible;

            grdUpdate.Visibility = Visibility.Visible;
            grdRelease.Visibility = Visibility.Hidden;
            Update();

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
                //fillTextbox(dr);
                btnRelease.Visibility = Visibility.Visible;
                btnCharge.Visibility = Visibility.Hidden;
                btnAssignment.Visibility = Visibility.Hidden;
                Update();
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
                //fillTextbox(dr);
                btnPickedup.Visibility = Visibility.Visible;
                btnDelivery.Visibility = Visibility.Visible;
                btnCharge.Visibility = Visibility.Hidden;
                btnAssignment.Visibility = Visibility.Hidden;
                Update();
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
                // fillTextbox(dr);

                btnPickedup.Visibility = Visibility.Hidden;
                btnDelivery.Visibility = Visibility.Hidden;
                btnCharge.Visibility = Visibility.Visible;
                btnAssignment.Visibility = Visibility.Visible;
                Update();

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
                myBl.PickedupParcel(drone.Id);
                DroneForList dr = myBl.GetDroneForList(drone.Id);
                //fillTextbox(dr);
                MessageBox.Show("the parcel was collected by the parcel");
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }


        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            //if (worker != null)
            //{
            //    closing = true;
            //    e.Cancel = true;
            //}
        }



        private void btnShowParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel p = myBl.GetParcel(drone.Package.Id);
            ParcelWindow pw = new ParcelWindow(myBl, p);
            pw.Show();
            //pw.Update += ParcelWindow_Update;
        }
 
        private void UpdateWidowDrone()
        {
            drone = myBl.GetDrone(drone.Id);
            DataContext = drone;
            WindowUp();
        }
        private void btnAutomatic_Click(object sender, RoutedEventArgs e)
        {
            btnManual.IsEnabled = true;
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
            worker.DoWork += (sender, args) => myBl.StartDroneSimulator((int)args.Argument, () => worker.ReportProgress(0), () => worker.CancellationPending);
            worker.ProgressChanged += (sender, args) => UpdateWidowDrone();
            worker.RunWorkerAsync(drone.Id);
        }
        private void Manual_Click(object sender, RoutedEventArgs e) 
            {
            worker.CancelAsync();
            btnManual.IsEnabled = false;
            }

    }




}

