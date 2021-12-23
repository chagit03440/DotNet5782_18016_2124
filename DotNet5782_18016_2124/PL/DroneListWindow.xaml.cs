using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
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
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.DroneForList> collection;
        public DroneListWindow( BlApi.IBL MyBl)
        {
            myBl = MyBl;
            DataContext = this;
            InitializeComponent();

            //to remove close box from window
            Loaded += ToolWindow_Loaded;
     
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
          // DronesListView.ItemsSource = myBl.GetDrones();
            collection = new ObservableCollection<BO.DroneForList>(myBl.GetDrones());
            DronesListView.ItemsSource = collection;
            comboMaxWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.DroneStatuses status = (BO.DroneStatuses)comboStatusSelector.SelectedItem;
           // IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)comboStatusSelector.SelectedItem;

            this.DronesListView.ItemsSource = myBl.GetDrones(dr => dr.Status == status );

        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
                BO.WeightCategories weight = (BO.WeightCategories)comboMaxWeightSelector.SelectedItem;
              //  IBL.BO.DroneStatuses status = (IBL.BO.DroneStatuses)comboStatusSelector.SelectedItem;
                this.DronesListView.ItemsSource = myBl.GetDrones(dr=> dr.MaxWeight== weight);
           }

    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DronesListView.SelectedItem == null)
                return;
            BO.Drone dr = new BO.Drone() ;
            BO.DroneForList drL = DronesListView.SelectedItem as BO.DroneForList;
            
            try
            {
                dr = myBl.GetDrone(drL.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            DroneWindow droneWindow = new DroneWindow(myBl, dr);
            droneWindow.Show();
            droneWindow.Update += DroneWindow_Update;


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
            collection = new ObservableCollection<BO.DroneForList>(myBl.GetDrones(null));
            DronesListView.ItemsSource = collection;
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
