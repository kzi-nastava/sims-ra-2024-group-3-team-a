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
    /// Interaction logic for TourRequestsWindow.xaml
    /// </summary>
    public partial class TourRequestWindow : Window
    {
        public static TourRequestWindow Instance;
        public TourRequestWindow(UserDTO user)
        {
            InitializeComponent();
            DataContext = new TourRequestViewModel(user);
            Instance = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        public static TourRequestWindow GetInstance()
        {
            return Instance;
        }
    }
}
