using BookingApp.DTO;
using BookingApp.ViewModel.Guide;
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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for TourRequestStatisticsWindow.xaml
    /// </summary>
    public partial class TourRequestStatisticsWindow : Window
    {
        public static TourRequestStatisticsWindow Instance;
        public TourRequestStatisticsWindow( UserDTO user)
        {
            InitializeComponent();
            DataContext = new TourRequestStatisticsViewModel(user);
            Instance = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public static TourRequestStatisticsWindow GetInstance()
        {
            return Instance;
        }

    }
}
