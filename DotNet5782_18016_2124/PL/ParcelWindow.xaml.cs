using BlApi;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL myBl;
        private Parcel parcel;
        public event Action Update = delegate { };

        public ParcelWindow(IBL myBl)
        {
            InitializeComponent();
            this.myBl = myBl;

            DataContext = parcel;

            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            btnCollect.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnDroneWindow.Visibility = Visibility.Hidden;
            btnSenderWindow.Visibility = Visibility.Hidden;
            btnTargetWindow.Visibility = Visibility.Hidden;

            comboPriority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            comboDrone.ItemsSource = myBl.GetDrones(d=>d.Status==DroneStatuses.Free);

        }
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
        public ParcelWindow(IBL myBl, Parcel pr)
        {
            InitializeComponent();
            DataContext = pr;
            this.myBl = myBl;
            parcel = pr;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
            comboDrone.ItemsSource = myBl.GetDrones(d => d.Status == DroneStatuses.Free);


            btnAddParcel.Visibility = Visibility.Hidden;
            comboPriority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

        }

       

        private void btnDroneWindow_Click(object sender, RoutedEventArgs e)
        {
            Parcel p = myBl.GetParcel(parcel.Id);
            
            Drone d = myBl.GetDrone(p.DroneP.Id);
            DroneWindow droneWindow= new DroneWindow(myBl, d);
            droneWindow.Show();
        }

        private void btnTargetWindow_Click(object sender, RoutedEventArgs e)
        {
            
            Customer c = myBl.GetCustomer(parcel.Target.Id);
            CustomerWindow customerWindow = new CustomerWindow(myBl, c);
            customerWindow.Show();
        }

        private void btnSenderWindow_Click(object sender, RoutedEventArgs e)
        {
            Customer c = myBl.GetCustomer(parcel.Sender.Id);
            CustomerWindow customerWindow = new CustomerWindow(myBl, c);
            customerWindow.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            myBl.DeleteParcel(parcel);
            Update();
            this.Close();
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                //ParcelForList parcel = new ParcelForList()
                //{
                //    Id = Convert.ToInt32(txtId.Text),
                //    Longitude = (WeightCategories)comboWeight.SelectedItem,
                //    SenderId = Convert.ToInt32(txtSenderId.Text),
                //    TargetId= Convert.ToInt32(txtTargetId.Text),
                //    Priority=(Priorities)comboPriority.SelectedItem,

                //};
                DroneForList d = comboDrone.SelectedItem as DroneForList;
                DroneInParcel dp = new DroneInParcel() { Id = d.Id, Battery = d.Battery, Location = d.DroneLocation };
                Parcel p = new Parcel()
                {
                    Priority = (Priorities)comboPriority.SelectedItem,
                    DroneP = dp,
                    AssociationTime = Convert.ToInt32(txtAssociationTime.Text),
                    CollectionTime = Convert.ToInt32(txtCollectionTime.Text),
                   // CreationTime =(DateTime)Convert.ToInt32(txtCreationTime.Text),
                    Id = Convert.ToInt32(txtId.Text),
                    Longitude = (WeightCategories)comboWeight.SelectedItem,
                    Sender =new CustomerInParcel() { Id= Convert.ToInt32(txtSenderId.Text), Name=myBl.GetCustomer(Convert.ToInt32(txtSenderId.Text)).Name},
                    SupplyTime= Convert.ToInt32(txtSupplyTime.Text),
                    Target=new CustomerInParcel() { Id = Convert.ToInt32(txtTargetId.Text), Name = myBl.GetCustomer(Convert.ToInt32(txtTargetId.Text)).Name }
                    
                };

                myBl.AddParcel(p);
                MessageBox.Show("the parcel was successfully added");
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
            new ParcelsListWindow(myBl);
            if (flag)
                this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
