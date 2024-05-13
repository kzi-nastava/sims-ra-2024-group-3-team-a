using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AccommodationRenovationPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.AccommodationRenovationViewModels
{
    public class AccommodationRenovationDetailsViewModel : ViewModel
    {
        private AccommodationRenovationDTO _accommodationRenovationDTO;
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _cancelRenovationCommand;

        private AccommodationRenovationService _accommodationRenovationService;
        public AccommodationRenovationDetailsViewModel(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            _accommodationRenovationDTO = accommodationRenovationDTO;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _cancelRenovationCommand = new RelayCommand(CancelReservation);


            IAccommodationRenovationRepository accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _accommodationRenovationService = new AccommodationRenovationService(accommodationRenovationRepository, accommodationReservationRepository, accommodationRepository);
        }

        public bool IsCancellable
        {
            get
            {
                if(DateTime.Now < AccommodationRenovationDTO.BeginDate.AddDays(-5))
                {
                    return true;
                }
                return false;
            }
            set { }
        }
        public AccommodationRenovationDTO AccommodationRenovationDTO
        {
            get { return _accommodationRenovationDTO; }
            set
            {
                if (value != _accommodationRenovationDTO)
                {
                    _accommodationRenovationDTO = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand CancelRenovationCommand
        {
            get
            {
                return _cancelRenovationCommand;
            }
            set
            {
                _cancelRenovationCommand = value;
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

        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
        private void CancelReservation()
        {
            _accommodationRenovationService.Delete(_accommodationRenovationDTO.ToAccommodationRenovation());
            OwnerMainWindow.MainFrame.Content = new RenovationsPage(new UserDTO(OwnerMainWindow.LoggedInOwner));
        }
    }
}
