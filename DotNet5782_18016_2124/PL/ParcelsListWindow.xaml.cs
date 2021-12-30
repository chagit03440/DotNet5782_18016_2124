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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        public ParcelsListWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            DataContext = this;
            InitializeComponent();

            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            comboSenderSelector.ItemsSource = myBl.GetCustomers();
            // DronesListView.ItemsSource = myBl.GetDrones();
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels());
            ParcelsListView.ItemsSource = collection;
            comboTargetSelector.ItemsSource = myBl.GetCustomers();
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
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.ParcelForList> collection;
       

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.CustomerInParcel cus = (BO.CustomerInParcel)comboSenderSelector.SelectedItem;
            // IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)comboStatusSelector.SelectedItem;

            this.ParcelsListView.ItemsSource = myBl.GetParcels();

        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            BO.CustomerInParcel weight = (BO.CustomerInParcel)comboTargetSelector.SelectedItem;
            //  IBL.BO.DroneStatuses status = (IBL.BO.DroneStatuses)comboStatusSelector.SelectedItem;
            this.ParcelsListView.ItemsSource = myBl.GetParcels();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelsListView.SelectedItem == null)
                return;
            BO.Parcel pr = new BO.Parcel();
            BO.ParcelForList prL = ParcelsListView.SelectedItem as BO.ParcelForList;

            try
            {
                pr = myBl.GetParcel(prL.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            ParcelWindow parcelWindow = new ParcelWindow(myBl, pr);
            parcelWindow.Show();
            //parcelWindow.Update += DroneWindow_Update;


        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow dw = new DroneWindow(myBl);
            dw.Show();
            dw.Update += DroneWindow_Update;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void DroneWindow_Update()
        {
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels(null));
            ParcelsListView.ItemsSource = collection;
        }

    }
}
