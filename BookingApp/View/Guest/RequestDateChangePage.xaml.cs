using BookingApp.DTO;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for RequestDateChangePage.xaml
    /// </summary>
    public partial class RequestDateChangePage : Page
    {
        public static RequestDateChangePage Instance;
        public static ReservationViewModel _reservationViewModel;

        public RequestDateChangePage(AccommodationReservationDTO selectedReservation)
        {
            InitializeComponent();
            _reservationViewModel = new ReservationViewModel(selectedReservation);
            DataContext = _reservationViewModel;

            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
}
