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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL myBl;
        private Parcel pr;

        public ParcelWindow(IBL myBl)
        {
            InitializeComponent();
            this.myBl = myBl;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
        }
        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public event Action Update = delegate { };

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        public ParcelWindow(IBL myBl, Parcel pr)
        {
            this.myBl = myBl;
            this.pr = pr;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
        }

        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDroneWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTargetWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSenderWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            try
            {
                ParcelForList pr = new ParcelForList()
                {
                    Id = Convert.ToInt32(txtId.Text),
                    Longitude = (WeightCategories)comboWeight.SelectedItem,
                    SenderId = Convert.ToInt32(txtSenderId.Text),
                    TargetId= Convert.ToInt32(txtTargetId.Text),
                    Priority=(Priorities)comboPriority.SelectedItem,
                    Status=(ParcelStatuses)comboStatus.SelectedItem

                };


                myBl.AddParcel(pr);
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
