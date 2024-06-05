using BookingApp.ViewModel.Guest;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for AnywhereAnytimePage.xaml
    /// </summary>
    public partial class AnywhereAnytimePage : Page
    {
        public static AnywhereAnytimePage Instance;
        public static AnywhereAnytimeViewModel _anywhereAnytimeViewModel;
        public AnywhereAnytimePage()
        {
            InitializeComponent();
            if (Instance == null)
            {
                Instance = this;
            }
            _anywhereAnytimeViewModel = new AnywhereAnytimeViewModel(GuestMainViewWindow.LoggedInGuest);
            DataContext = _anywhereAnytimeViewModel;
            
        }
    }
}
