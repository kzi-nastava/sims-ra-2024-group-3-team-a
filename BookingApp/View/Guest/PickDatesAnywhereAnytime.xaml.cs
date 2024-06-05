using BookingApp.DTO;
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
    /// Interaction logic for PickDatesAnywhereAnytime.xaml
    /// </summary>
    public partial class PickDatesAnywhereAnytime : Page
    {
        PickDatesAnywhereAnytimeViewModel _pickDatesAnywhereAnytimeViewModel;
        public static PickDatesAnywhereAnytime Instance;
        public PickDatesAnywhereAnytime(AccommodationDTO selectedAccommodationDTO, UserDTO loggedInGuest, DateTime selectedBeginDate, DateTime selectedEndDate, int daysToStay)
        {
            InitializeComponent();
            _pickDatesAnywhereAnytimeViewModel = new PickDatesAnywhereAnytimeViewModel(selectedAccommodationDTO, loggedInGuest, selectedBeginDate, selectedEndDate, daysToStay);
            DataContext = _pickDatesAnywhereAnytimeViewModel;
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
