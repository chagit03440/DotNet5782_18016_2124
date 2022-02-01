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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    { 
        private BlApi.IBL myBl;
        private BO.Station s;
        public event Action Update = delegate { };
        


       
        //constructor to update
            public StationWindow(BlApi.IBL myBl, BO.Station station)
        {
            
            InitializeComponent();

            Loaded += ToolWindow_Loaded;

            comboDrone.ItemsSource = myBl.GetDrones(d => d.DroneLocation == station.Location);
            this.myBl = myBl;
           
            DataContext = station;
            s = station;
            Add_btn.Visibility = Visibility.Hidden;
            txtId.IsEnabled=false;
            txtlatitude.IsEnabled=false;

            txtlongenttitude.IsEnabled = false;
            

            txtCharge.IsEnabled = false;

        }
        //constructor to add
        public StationWindow(IBL myBl)
        {
            this.myBl = myBl;
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
            Update_btn.Visibility = Visibility.Hidden;
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


        //a click that close the window
        private void closbtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //a click to add station
        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                DroneForList d = (DroneForList)comboDrone.SelectedItem;
                Station st = new Station
                {
                    Id = Convert.ToInt32(txtId.Text),
                    Name = txtName.Text,
                    Location = new Location()
                    {
                        Longitude = Convert.ToInt32(txtlongenttitude.Text),
                        Lattitude = Convert.ToInt32(txtlatitude.Text)
                    },
                    ChargeSlots= Convert.ToInt32(txtCharge.Text)
                };


                myBl.AddStation(st);
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
            new StationWindow(myBl);
            if (flag)
                this.Close();
        }
        // a click to updat the station
        private void Updat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s.Name = txtName.Text;
                myBl.UpdateStation(s,-1);
                Update();
                MessageBox.Show("the name of the station was successfully updated");
                Station dr = myBl.GetStation(s.Id);
                //fillTextbox(s);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
 
        //a click that delet the station
        private void deletbtn_Click(object sender, RoutedEventArgs e)
        {

            myBl.DeleteStation(s);
            Update();
            this.Close();
        }
    }
}
