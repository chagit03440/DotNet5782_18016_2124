using System;
using System.Collections.Generic;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL myBl;
        private IBL.BO.DroneForList drone;
        public DroneWindow()
        {
            InitializeComponent();
        }

        public DroneWindow(IBL.IBL myBl)
        {
            InitializeComponent();
            this.drone = new IBL.BO.DroneForList();
            this.myBl = myBl;
            DataContext = drone;
        }
        public DroneWindow(IBL.IBL myBl, IBL.BO.DroneForList drone)
        {
            InitializeComponent();
            this.myBl = myBl;
            this.drone = drone;
        }
    }
}
