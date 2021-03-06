using BO;
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
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private List<IGrouping<int, StationForList>> GroupingData { get; set; }
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.StationForList> collection;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="MyBl"></param>
        public StationListWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            DataContext = this;
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
            collection = new ObservableCollection<BO.StationForList>(myBl.GetStations());
           
            stationListView.ItemsSource =collection;
        }
        


        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        /// <summary>
        ///   close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closbtn_Click(object sender, RoutedEventArgs e)
        {
           
            Close();
        }
        /// <summary>
        /// a click open window to updat station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (stationListView.SelectedItem == null)
                return;
            BO.Station station = new BO.Station();
            BO.StationForList drL = stationListView.SelectedItem as BO.StationForList;

            try
            {
                station = myBl.GetStation(drL.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            StationWindow stationWindow = new StationWindow(myBl, station);
            stationWindow.Show();
            stationWindow.Update += StationWindow_Update;
           
        }
 /// <summary>
 /// update the window
 /// </summary>
        private void StationWindow_Update()
        {
            collection = new ObservableCollection<BO.StationForList>(myBl.GetStations( ));
            stationListView.ItemsSource = collection;
        }
 
        /// <summary>
        /// the click open the station window to add station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            StationWindow s = new StationWindow(myBl);
            s.Show();
        }

        private void stationListView_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        ///  a function to group a station by Available Charge Slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void group_Click(object sender, RoutedEventArgs e)
        {
            GroupingData = myBl.GetStations().GroupBy(x => x.AvailableChargeSlots).ToList();
            stationListView.ItemsSource = GroupingData;
        }
    }
}
