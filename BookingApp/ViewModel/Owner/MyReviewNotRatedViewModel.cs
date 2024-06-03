using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Owner
{
    public class MyReviewNotRatedViewModel : Validation.ValidationBase
    {
        private AccommodationReservationService _accommodationReservationService;
        private AccommodationReservationDTO _accommodationReservationDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _rateCommand;

        public MyReviewNotRatedViewModel(AccommodationReservationDTO reservation)
        {
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);

            _accommodationReservationDTO = new AccommodationReservationDTO(reservation);

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _rateCommand = new RelayCommand(Rate);
        }

        public AccommodationReservationDTO AccommodationReservationDTO
        {
            get
            {
                return _accommodationReservationDTO;
            }
            set
            {
                _accommodationReservationDTO = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand;
            }
            set
            {
                _showSideMenuCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand RateCommand
        {
            get
            {
                return _rateCommand;
            }
            set
            {
                _rateCommand = value;
                OnPropertyChanged();
            }
        }

        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
        private void Rate()
        {
            Validate1();

            if(IsValid)
            {
                _accommodationReservationService.Update(_accommodationReservationDTO.ToAccommodationReservation());
                OwnerMainWindow.MainFrame.Content = new ReviewsPage();
            }
        }

        protected override void ValidateSelf1()
        {
            if (!int.TryParse(_accommodationReservationDTO.RatingDTO.OwnerCleannessRating.ToString(), out int OwnerCleannessRating) || OwnerCleannessRating <= 0)
            {
                ValidationErrors["OwnerCleannessRating"] = "You didn't rate Guest's cleanliness";
            }

            if (!int.TryParse(_accommodationReservationDTO.RatingDTO.OwnerRulesRespectRating.ToString(), out int OwnerRulesRespectRating) || OwnerRulesRespectRating <= 0)
            {
                ValidationErrors["OwnerRulesRespectRating"] = "You didn't rate Guest's rules respect";
            }

            OnPropertyChanged(nameof(ValidationErrors));
        }

        protected override void ValidateSelf2()
        {
            throw new NotImplementedException();
        }
    }
}
