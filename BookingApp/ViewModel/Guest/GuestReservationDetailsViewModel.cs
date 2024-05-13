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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationDetailsViewModel: ViewModel
    {
        private DateOnly _selectedBeginDate;
        private DateOnly _selectedEndDate;

        private UserDTO _userDTO;
        private AccommodationDTO _accommodationDTO;
        private AccommodationReservationService _accommodationReservationService;
        private SuperGuestService _superGuestService;
        private RelayCommand _newReservationCommand;

        public GuestReservationDetailsViewModel(AccommodationDTO accommodationDTO, UserDTO userDTO, DateOnly begin, DateOnly end)
        {
            isSuper = true;
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _superGuestService = new SuperGuestService(superGuestRepository);

            _accommodationDTO = accommodationDTO;
            _userDTO = userDTO;
            _selectedBeginDate = begin;
            _selectedEndDate = end;
            _newReservationCommand = new RelayCommand(NewReservation);

            if((_superGuestService.GetByUserId(userDTO.Id) == null) || _superGuestService.GetByUserId(userDTO.Id).Points == 0)
            {
                IsSuper = false;
                
            }
            else
            {
                IsSuper = true;
                
            }
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
                AccommodationReservation acc = new AccommodationReservation(0, _userDTO.Id, _accommodationDTO.Id, _selectedBeginDate, _selectedEndDate, false,usedPoint, rating);
                _accommodationReservationService.Save(acc);
                MessageBox.Show("Reservation successful!");
                if (UsedPoint)
                {
                    SuperGuest superGuest = _superGuestService.GetByUserId(_userDTO.Id);
                    superGuest.Points--;
                    _superGuestService.Update(superGuest);
                }
            }
            else
            {
                MessageBox.Show("Please enter number of guests!");
            }
        }
        private bool isSuper;
        public bool IsSuper
        {
            get
            {
                return isSuper;
            }
            set
            {
                isSuper = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsToggleEnabled));
            }
        }
        public bool IsToggleEnabled => IsSuper;
        private bool usedPoint;
        public bool UsedPoint
        {
            get
            {
                return usedPoint;
            }
            set
            {
                usedPoint = value;
                OnPropertyChanged();
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
