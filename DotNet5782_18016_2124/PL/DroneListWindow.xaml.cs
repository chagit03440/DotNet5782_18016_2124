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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {


        private IBL.IBL myBl { get; }

        public DroneListWindow(IBL.IBL MyBl)
        {
            myBl = MyBl;
            InitializeComponent();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            DronesListView.ItemsSource = myBl.GetDrones();
            comboMaxWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.DroneStatuses status = (IBL.BO.DroneStatuses)comboStatusSelector.SelectedItem;
            Func<IBL.BO.DroneForList, bool> func;
            //  this.DronesListView.ItemsSource = myBl.GetDrones(func);

        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.WeightCategories status = (IBL.BO.WeightCategories)comboStatusSelector.SelectedItem;
            Func<IBL.BO.DroneForList, bool> func;
            //  this.DronesListView.ItemsSource = myBl.GetDrones(func);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = 0;
            IBL.BO.DroneForList drone = myBl.GetDrone(id);

            new DroneWindow(myBl, drone).Show();

        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(myBl).Show();
        }
    }
}
