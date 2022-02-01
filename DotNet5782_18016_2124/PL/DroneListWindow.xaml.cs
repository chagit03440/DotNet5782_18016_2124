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
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.DroneForList> collection;

        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        /// <summary>
        /// a function to remove the close box from the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
       
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="MyBl"></param>
        public DroneListWindow(BlApi.IBL MyBl)
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
        /// <summary>
        /// a function that change the list by the selected status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.DroneStatuses status = (BO.DroneStatuses)comboStatusSelector.SelectedItem;
            // IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)comboStatusSelector.SelectedItem;

            this.DronesListView.ItemsSource = myBl.GetDrones(dr => dr.Status == status);

        }
        /// <summary>
        /// a function that change the list by the selected weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            BO.WeightCategories weight = (BO.WeightCategories)comboMaxWeightSelector.SelectedItem;
            //  IBL.BO.DroneStatuses status = (IBL.BO.DroneStatuses)comboStatusSelector.SelectedItem;
            this.DronesListView.ItemsSource = myBl.GetDrones(dr => dr.MaxWeight == weight);
        }
        /// <summary>
        /// a function that open  window with the checked drone 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DronesListView.SelectedItem == null)
                return;
            BO.Drone dr = new BO.Drone();
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
        /// <summary>
        /// a function that open a drone window in state add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow dw = new DroneWindow(myBl);
            dw.Show();
            dw.Update += DroneWindow_Update;

        }
        /// <summary>
        /// a function to close the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// a function that update the listview in the window
        /// </summary>
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

