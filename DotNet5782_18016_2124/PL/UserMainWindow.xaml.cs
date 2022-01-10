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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        BlApi.IBL myBl;
        BO.User user;
        CustomerForList c;
        private ObservableCollection<BO.ParcelForList> collection;
        
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

        public UserMainWindow(BlApi.IBL MyBl, BO.User curUser)
        {
            myBl = MyBl;
            DataContext = curUser;
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
            txtName.IsReadOnly = true;
            user = curUser;
            c = myBl.GetCustomers().FirstOrDefault(x => x.Name == user.UserName);
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels(p => p.SenderId == c.Id || p.TargetId == c.Id));
            ListViewParcels.ItemsSource = collection;
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelWindow pw = new ParcelWindow(myBl);
            pw.Show();
            pw.Update += ParcelWindow_Update;
        }

        private void ParcelWindow_Update()
        {
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels(p=>p.SenderId==c.Id||p.TargetId == c.Id));
            ListViewParcels.ItemsSource = collection;
        }

        private void ListViewParcels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewParcels.SelectedItem == null)
                return;
            BO.Parcel p = new BO.Parcel();
            BO.ParcelForList pfL = ListViewParcels.SelectedItem as BO.ParcelForList;

            try
            {
                p = myBl.GetParcel(pfL.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            ParcelWindow parcelWindow = new ParcelWindow(myBl, p);
            parcelWindow.Show();
            parcelWindow.Update += ParcelWindow_Update;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
