using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.View;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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

namespace BookingApp.View
{
    public partial class AllToursWindow : Window
    {
        public static AllToursWindow Instance;
        public AllToursWindow(UserDTO guide)
        {
            InitializeComponent();
            DataContext = new AllToursViewModel(guide);

            Instance = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
        public static AllToursWindow GetInstance()
        {
            return Instance;
        }
    }
}
