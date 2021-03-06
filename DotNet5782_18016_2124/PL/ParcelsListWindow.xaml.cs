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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private BlApi.IBL myBl { get; }
        private ObservableCollection<BO.ParcelForList> collection;
        private List<IGrouping<int, ParcelForList>> GroupingData { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="MyBl"></param>
        public ParcelsListWindow(BlApi.IBL MyBl)
        {
            myBl = MyBl;
            DataContext = this;
            InitializeComponent();

            //to remove close box from window
            Loaded += ToolWindow_Loaded;

            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(BO.ParcelStatuses));
            comboPrioritySelector.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

            // DronesListView.ItemsSource = myBl.GetDrones();
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels());
            ParcelsListView.ItemsSource = collection;
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
            parcelWindow.Update += ParcelWindow_Update;


        }
        /// <summary>
        /// a click open a window to add parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelWindow pw = new ParcelWindow(myBl);
            pw.Show();
            pw.Update += ParcelWindow_Update;

        }
        /// <summary>
        /// close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// a click open a window to updat parcel
        /// </summary>
        private void ParcelWindow_Update()
        {
            collection = new ObservableCollection<BO.ParcelForList>(myBl.GetParcels(null));
            ParcelsListView.ItemsSource = collection;
        }
        /// <summary>
        /// piriorty selector -put the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboPrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Priorities pr = (BO.Priorities)comboPrioritySelector.SelectedItem;
            
            this.ParcelsListView.ItemsSource = myBl.GetParcels(p => p.Priority==pr);
        }
        /// <summary>
        /// status selector -put the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.ParcelStatuses ps = (BO.ParcelStatuses)comboStatusSelector.SelectedItem;

            this.ParcelsListView.ItemsSource = myBl.GetParcels(p => p.Status == ps);
        }
        /// <summary>
        /// group the parcel by sendrId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSender_Click(object sender, RoutedEventArgs e)
        {
          
            GroupingData = myBl.GetParcels().GroupBy(x => x.SenderId).ToList();
            ParcelsListView.ItemsSource = GroupingData;

        }
        /// <summary>
        /// wehigt selector -put the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboWehigtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.WeightCategories pw = (BO.WeightCategories)comboWeightSelector.SelectedItem;

            this.ParcelsListView.ItemsSource = myBl.GetParcels(p => p.Longitude == pw);
        }
    }
}
