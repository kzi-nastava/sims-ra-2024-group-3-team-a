using BookingApp.DTO;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class OwnerMainViewModel : ViewModel
    {
        private ObservableCollection<AccommodationReservationDTO> _finishedAccommodationReservationsDTO;

        private AccommodationReservationService _accommodationReservationService;
        private UserService _userService;
        private UserDTO _loggedInOwner;

        public OwnerMainViewModel(UserDTO loggedInOwner)
        {
            _loggedInOwner = loggedInOwner;
            _userService = new UserService();

            _accommodationReservationService = new AccommodationReservationService();
            UpdateFinishedReservations();
        }

        public void UpdateFinishedReservations()
        {
            List<AccommodationReservationDTO> finishedAccommodationReservationsList = _accommodationReservationService.GetFinishedAccommodationReservations(_loggedInOwner.ToUser()).Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            _finishedAccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>(finishedAccommodationReservationsList);
        }

        public UserDTO GetUserDTOById(int id)
        {
            return new UserDTO(_userService.GetById(id));
        }

        public ObservableCollection<AccommodationReservationDTO> FinishedAccommodationReservationsDTO
        {
            get
            {
                return _finishedAccommodationReservationsDTO;
            }
            set
            {
                _finishedAccommodationReservationsDTO = value;
                OnPropertyChanged();
            }
        }

    }
}
