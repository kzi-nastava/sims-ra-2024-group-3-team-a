using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.ViewModel.Tourist
{
    public class TourInformationView : ViewModel
    {
        private static TourDTO _tourDTO;
        private static UserDTO _userDTO;
        private static TourReservationService _tourReservationService;
        private static RelayCommand _showTourReservationWindow;

        public TourInformationView(TourDTO tourDTO, UserDTO loggedInUser)
        {
           _tourDTO = tourDTO;
           _userDTO = loggedInUser;
           _tourReservationService = new TourReservationService();
           _showTourReservationWindow = new RelayCommand(ShowTourReservationWindow);
   
        }
        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowTourReservationWindowCommand
        {
            get
            {
                return _showTourReservationWindow;
            }
            set
            {
                _showTourReservationWindow = value;
                OnPropertyChanged();
            }
        }
        public void ShowTourReservationWindow()
        {


            TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationService, _tourDTO, _userDTO);
            tourReservationWindow.ShowDialog();
        }

    }
}
