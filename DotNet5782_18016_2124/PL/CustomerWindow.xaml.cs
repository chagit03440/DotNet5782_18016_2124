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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window, INotifyPropertyChanged
    {
        private BlApi.IBL myBl;
        private Customer selectedCustomer;
        public event Action Update = delegate { };
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private Customer customerToAdd;
        private Customer customer;

        public Customer CustomerToAdd
        {
            get { return customerToAdd; }
            set
            {
                customerToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerToAdd"));
            }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { selectedCustomer = value; }
        }

        public CustomerWindow(BlApi.IBL myBl)
        {
            InitializeComponent();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            customerToAdd = new Customer();
            btnUpdateModel.Visibility = Visibility.Hidden;
            this.myBl = myBl;
            DataContext = this;
            comboParcelsTo.Visibility = Visibility.Hidden;
            comboParcelsFrom.Visibility = Visibility.Hidden;
            lblParcelsFromTheCustomer.Visibility = Visibility.Hidden;
            lblParcelsToTheCustomer.Visibility = Visibility.Hidden;
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
            DataContext = c;
           
            customer = c;
            selectedCustomer = c;
            btnAddCustomer.Visibility = Visibility.Hidden;
            //fillTextbox(c);

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
                MessageBox.Show("the name of the customer was successfully updated");

                Customer cs = myBl.GetCustomer(customer.Id);
               // fillTextbox(customer);
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
                    Location = new Location()
                    {
                        Longitude = Convert.ToDouble(txtLongtitude.Text),
                        Lattitude = Convert.ToDouble(txtLatitude.Text)
                    }

                };



                myBl.AddCustomer(cus);
                MessageBox.Show("the customer was successfully added");
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

        private void comboParcelsFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboParcelsFrom.SelectedItem == null)
                return;
            BO.Customer cs = new BO.Customer();
            int id = (int)comboParcelsFrom.SelectedItem;

            try
            {
                cs = myBl.GetCustomer(id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            CustomerWindow customerWindow = new CustomerWindow(myBl, cs);
            customerWindow.Show();

        }

        private void comboParcelsTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboParcelsTo.SelectedItem == null)
                return;
            BO.Customer cs = new BO.Customer();
            int id = (int)comboParcelsTo.SelectedItem;

            try
            {
                cs = myBl.GetCustomer(id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            CustomerWindow customerWindow = new CustomerWindow(myBl, cs);
            customerWindow.Show();

        }
    }
}

