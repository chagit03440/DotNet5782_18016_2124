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

        }

        public ParcelWindow(IBL myBl, Parcel pr)
        {
            this.myBl = myBl;
            this.pr = pr;
        }
    }
}
