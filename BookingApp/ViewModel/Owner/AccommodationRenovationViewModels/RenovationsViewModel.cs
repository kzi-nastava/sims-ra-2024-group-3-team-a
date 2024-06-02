using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AccommodationRenovationPages;
using BookingApp.View.Owner.WizardAndHelp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.AccommodationRenovationViewModels
{
    public class RenovationsViewModel : ViewModel
    {
        private UserDTO _loggedInUser;
        private ObservableCollection<AccommodationRenovationDTO> _accommodationRenovationsDTO;
        private AccommodationRenovationService _renovationService;
        private AccommodationRenovationDTO _selectedAccommodationRenovationDTO;

        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showRenovationHelpCommand;

        public RenovationsViewModel(UserDTO loggedInUser)
        {
            _loggedInUser = loggedInUser;
            _accommodationRenovationsDTO = new ObservableCollection<AccommodationRenovationDTO>();

            IAccommodationRenovationRepository accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _renovationService = new AccommodationRenovationService(accommodationRenovationRepository, accommodationReservationRepository, accommodationRepository);

            List<AccommodationRenovationDTO> renovationsDTO = _renovationService.GetRenovationsForOwner(loggedInUser.ToUser()).Select(renovation => new AccommodationRenovationDTO(renovation)).ToList();
            _accommodationRenovationsDTO = new ObservableCollection<AccommodationRenovationDTO>(renovationsDTO);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showRenovationHelpCommand = new RelayCommand(ShowRenovationHelp);
        }

        public AccommodationRenovationDTO SelectedAccommodationRenovationDTO
        {
            get
            {
                return _selectedAccommodationRenovationDTO;
            }
            set
            {
                _selectedAccommodationRenovationDTO = value;
                OnPropertyChanged();
                ShowRenovationDetailsPage();
                _selectedAccommodationRenovationDTO = null;
            }
        }
        public ObservableCollection<AccommodationRenovationDTO> AccommodationRenovationsDTO
        {
            get
            {
                return _accommodationRenovationsDTO;
            }
            set
            {
                _accommodationRenovationsDTO = value;
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
        public RelayCommand ShowRenovationHelpCommand
        {
            get
            {
                return _showRenovationHelpCommand;
            }
            set
            {
                _showRenovationHelpCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        public void ShowRenovationDetailsPage()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationRenovationDetailsPage(_selectedAccommodationRenovationDTO);
        }

        public void ShowRenovationHelp()
        {
            OwnerMainWindow.MainFrame.Content = new RenovationHelpPage(_loggedInUser);
        }
    }
}
