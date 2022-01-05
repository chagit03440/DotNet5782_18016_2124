using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.CustomerForList> collection;

        public CustomersListWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            InitializeComponent();
            DataContext = this;
        
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            collection = new ObservableCollection<BO.CustomerForList>(myBl.GetCustomers());
            CustomersListView.ItemsSource = collection;
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
        
       

       
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomersListView.SelectedItem == null)
                return;
            BO.Customer cs = new BO.Customer();
            BO.CustomerForList csL = CustomersListView.SelectedItem as BO.CustomerForList;

            try
            {
                cs = myBl.GetCustomer(csL.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            CustomerWindow customerWindow = new CustomerWindow(myBl, cs);
            customerWindow.Show();
            customerWindow.Update += CustomerWindow_Update;


        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CustomerWindow_Update()
        {
            collection = new ObservableCollection<BO.CustomerForList>(myBl.GetCustomers());
            CustomersListView.ItemsSource = collection;
        }

        
        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow cw = new CustomerWindow(myBl);
            cw.Show();
            cw.Update += CustomerWindow_Update;
        }
    }
}
