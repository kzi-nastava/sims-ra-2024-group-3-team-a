using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationDetailsViewModel: ViewModel
    {
        private DateOnly _selectedBeginDate;
        private DateOnly _selectedEndDate;

        private UserDTO _userDTO;
        private AccommodationDTO _accommodationDTO;
        private AccommodationReservationService _accommodationReservationService;
        private RelayCommand _newReservationCommand;

        public GuestReservationDetailsViewModel(AccommodationDTO accommodationDTO, UserDTO userDTO, DateOnly begin, DateOnly end)
        {
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);

            _accommodationDTO = accommodationDTO;
            _userDTO = userDTO;
            _selectedBeginDate = begin;
            _selectedEndDate = end;
            _newReservationCommand = new RelayCommand(NewReservation);
        }
        private void NewReservation()
        {
            int _guestNumber;
            if (int.TryParse(ReservationDetailsPage.Instance.GuestNumberTextBox.Text, out _guestNumber))
            {
                if (_guestNumber < 0 || _guestNumber > _accommodationDTO.Capacity)
                {
                    MessageBox.Show($"Error! Capacity is {_accommodationDTO.Capacity} guests!");
                    return;
                }
                Review rating = new Review();
                AccommodationReservation acc = new AccommodationReservation(0, _userDTO.Id, _accommodationDTO.Id, _selectedBeginDate, _selectedEndDate, false, rating);
                _accommodationReservationService.Save(acc);
                MessageBox.Show("Reservation successful!");
            }
            else
            {
                MessageBox.Show("Please enter number of guests!");
            }
        }
        public RelayCommand NewReservationCommand
        {
            get
            {
                return _newReservationCommand;
            }
            set
            {
                _newReservationCommand = value;
                OnPropertyChanged();
            }
        }
    }
}
