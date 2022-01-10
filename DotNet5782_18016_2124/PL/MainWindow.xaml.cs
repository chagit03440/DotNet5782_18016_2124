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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BL;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL myBl;
        BO.User curUser = new User();

        public MainWindow()
        {
            myBl = BlFactory.GetBl();
            InitializeComponent();
        }

       

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void btnGuest_Click(object sender, RoutedEventArgs e)
        //{
        //    ManagerMainWindow managementWin = new ManagerMainWindow(myBl);
        //    this.Close();
        //    managementWin.ShowDialog();
        //}

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            curUser.UserName = txtUserName.Text;
            curUser.Password = (string)txtPassword.Text;

            bool verify;


            try
            {
                verify = myBl.LogInVerify(curUser);

                if (verify == true)
                {
                    verify = myBl.isWorker(curUser);
                    if (verify == true)
                    {
                        UserMainWindow userMainWin = new UserMainWindow(myBl);
                        this.Close();
                        userMainWin.ShowDialog();

                    }
                    else
                    {
                        ManagerMainWindow managementWin = new ManagerMainWindow(myBl);
                        this.Close();
                        managementWin.ShowDialog();


                    }

                    
                }
            }
            catch (BO.BLInVaildIdException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
