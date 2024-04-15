using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MakeAccommodationReservationPage.xaml
    /// </summary>
    public partial class MakeAccommodationReservationPage : Page
    {
        private UserDTO _userDTO;
        public static MakeAccommodationReservationPage Instance;

        public MakeAccommodationReservationPage(AccommodationDTO selectedAccommodationDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _userDTO = userDTO;
            DataContext = new MakeReservationViewModel(selectedAccommodationDTO, _userDTO);
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
}