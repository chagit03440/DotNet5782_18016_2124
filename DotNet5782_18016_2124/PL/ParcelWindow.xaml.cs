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
        /// <summary>
        /// constructor to add
        /// </summary>
        /// <param name="myBl"></param>
        public ParcelWindow(IBL myBl)
        {
            InitializeComponent();
            this.myBl = myBl;

            DataContext = parcel;

            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            txtId.Text = myBl.getParcelId();//the next id
            txtId.IsEnabled = false;
            btnCollect.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnDroneWindow.Visibility = Visibility.Hidden;
            btnSenderWindow.Visibility = Visibility.Hidden;
            btnTargetWindow.Visibility = Visibility.Hidden;
            txtAssociationTime.Visibility = Visibility.Hidden;
            txtCollectionTime.Visibility = Visibility.Hidden;
            txtCreationTime.Visibility = Visibility.Hidden;
            lblAssociationTime.Visibility = Visibility.Hidden;
            comboDrone.Visibility = Visibility.Hidden;
            lblCollectionTime.Visibility = Visibility.Hidden;
            lblCreationTime.Visibility = Visibility.Hidden;
            lblDrone.Visibility = Visibility.Hidden;
            lblSupplyTime.Visibility = Visibility.Hidden;
            txtSupplyTime.Visibility = Visibility.Hidden;

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
        /// <summary>
        /// constructor to updat parcel
        /// </summary>
        /// <param name="myBl"></param>
        /// <param name="pr">the parcel to updat</param>
        public ParcelWindow(IBL myBl, Parcel pr)
        {
            InitializeComponent();
            DataContext = pr;
            this.myBl = myBl;
            parcel = pr;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
           // comboDrone.ItemsSource = myBl.GetDrones(d => d.Status == DroneStatuses.Free);

            txtAssociationTime.IsEnabled = false;
            txtCollectionTime.IsEnabled = false;
            txtCreationTime.IsEnabled = false;
            txtSupplyTime.IsEnabled = false;
            txtId.IsEnabled = false;
            comboDrone.IsEnabled = false;
            btnAddParcel.Visibility = Visibility.Hidden;
            comboPriority.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

        }


        /// <summary>
        /// open the window with the drone that assingment parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneWindow_Click(object sender, RoutedEventArgs e)
        {
            Parcel p = myBl.GetParcel(parcel.Id);
            
            Drone d = myBl.GetDrone(p.DroneP.Id);
            DroneWindow droneWindow= new DroneWindow(myBl, d);
            droneWindow.Show();
        }
        /// <summary>
        /// open the window with who get the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTargetWindow_Click(object sender, RoutedEventArgs e)
        {
            
            Customer c = myBl.GetCustomer(parcel.Target.Id);
            CustomerWindow customerWindow = new CustomerWindow(myBl, c);
            customerWindow.Show();
        }
        /// <summary>
        /// open the window with who send the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSenderWindow_Click(object sender, RoutedEventArgs e)
        {
            Customer c = myBl.GetCustomer(parcel.Sender.Id);
            CustomerWindow customerWindow = new CustomerWindow(myBl, c);
            customerWindow.Show();
        }
        
        /// <summary>
        /// a click to delete parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            myBl.DeleteParcel(parcel);
            Update();
            this.Close();
        }
    
        /// <summary>
        /// a click to add parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                
                Parcel p = new Parcel()
                {
                    Priority = (Priorities)comboPriority.SelectedItem,
                    DroneP = null,
                    AssociationTime = DateTime.Now,
                    CollectionTime = null,
                   CreationTime =DateTime.Now,
                    Id = Convert.ToInt32(txtId.Text),
                    Longitude = (WeightCategories)comboWeight.SelectedItem,
                    Sender =new CustomerInParcel() { Id= Convert.ToInt32(txtSenderId.Text), Name=myBl.GetCustomer(Convert.ToInt32(txtSenderId.Text)).Name},
                    SupplyTime=null,
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
        /// <summary>
        /// close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
