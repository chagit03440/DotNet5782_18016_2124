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
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private BlApi.IBL myBl;
        private BO.Customer customer;
        public event Action Update = delegate { };
        public CustomerWindow(BlApi.IBL myBl)
        {
            InitializeComponent();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            customer = new Customer();
            comboStatus.ItemsSource = myBl.GetStations();
            comboMaxWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            grdUpdate.Visibility = Visibility.Hidden;
            this.customer = new Customer();
            this.myBl = myBl;
            DataContext = customer;
            //comboPackage.Visibility = Visibility.Hidden;
            txtLongtitude.Visibility = Visibility.Hidden;
            txtLatitude.Visibility = Visibility.Hidden;
            //lblPackage.Visibility = Visibility.Hidden;
            lblLatitude.Visibility = Visibility.Hidden;
            lblLongtitude.Visibility = Visibility.Hidden;
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





        public CustomerWindow(BlApi.IBL myBl, Customer c)
        {

            InitializeComponent();

            //to remove close box from window
            Loaded += ToolWindow_Loaded;
            this.myBl = myBl;
            customer = new Customer();
            customer = c;
            btnAddCustomer.Visibility = Visibility.Hidden;
            comboMaxWeight.Visibility = Visibility.Hidden;
            comboStatus.Visibility = Visibility.Hidden;
            fillTextbox(c);
          
            txtId.IsEnabled = false;
            txtPhone.IsEnabled = false;

            txtLongtitude.IsEnabled = false;
            txtLatitude.IsEnabled = false;
        }
        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                customer.Name = txtName.Text;
                myBl.UpdateCustomer(customer);
                MessageBox.Show("the model of the drone was successfully updated");

                Customer cs = myBl.GetCustomer(customer.Id);
                fillTextbox(cs);
                Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        private void fillTextbox(CustomerForList c)
        {

            txtId.Text = c.Id.ToString();
            txtName.Text = c.Name.ToString();
            txtPhone.Text = c.Phone.ToString();

            //txtLongtitude.Text = c.Location.Longitude.ToString();
            //txtLatitude.Text = c.Location.Lattitude.ToString();

           
        }
        private void fillTextbox(Customer c)
        {
            if (c != null)
            {
                txtId.Text = c.Id.ToString();
                txtName.Text = c.Name.ToString();
                txtPhone.Text = c.Phone.ToString();

                txtLongtitude.Text = c.Location.Longitude.ToString();
                txtLatitude.Text = c.Location.Lattitude.ToString();
                return;
            }

        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {

                Customer cus = new Customer()
                {
                    Id = Convert.ToInt32(txtId.Text),
                    Name = txtName.Text,
                    Phone = txtPhone.Text,
                 
                };


                myBl.AddCustomer(cus);
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
    }
}

