using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.WizardAndHelp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class AccommodationsViewModel : ViewModel
    {
        private AccommodationService _accommodationService;
        private ObservableCollection<AccommodationDTO> _accommodationsDTO;

        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showAddAccommodationPageCommand;
        private RelayCommand _showAccommodationHelpCommand;

        private UserDTO _loggedInUser;

        private AccommodationDTO _selectedAccommodationDTO = null;

        public AccommodationsViewModel(UserDTO loggedInUser)
        {
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _accommodationService = new AccommodationService(accommodationRepository);

            List<AccommodationDTO> accommodationsDTO = _accommodationService.GetAccommodationsForOwner(loggedInUser.ToUser()).Select(accommodation => new AccommodationDTO(accommodation)).ToList(); ;
            _accommodationsDTO = new ObservableCollection<AccommodationDTO>(accommodationsDTO);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showAddAccommodationPageCommand = new RelayCommand(ShowAddAccommodationPage);
            _showAccommodationHelpCommand = new RelayCommand(ShowAccommodationHelp);

            _loggedInUser = loggedInUser;
        }

        public ObservableCollection<AccommodationDTO> AccommodationsDTO
        {
            get
            {
                return _accommodationsDTO;
            }
            set
            {
                _accommodationsDTO = value;
                OnPropertyChanged();
            }
        }
        public AccommodationDTO SelectedAccommodationDTO
        {
            get
            {
                return _selectedAccommodationDTO;
            }
            set
            {
                _selectedAccommodationDTO = value;
                OnPropertyChanged();
                ShowAccommodationStatisticsYear();
                _selectedAccommodationDTO = null;
            }
        }

        public RelayCommand ShowAddAccommodationPageCommand
        {
            get
            {
                return _showAddAccommodationPageCommand;
            }
            set
            {
                _showAddAccommodationPageCommand = value;
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

        public RelayCommand ShowAccommodationHelpCommand
        {
            get
            {
                return _showAccommodationHelpCommand;
            }
            set
            {
                _showAccommodationHelpCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowAccommodationStatisticsYear()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationsStatisticsYearsPage(_selectedAccommodationDTO);
        }

        public void ShowAddAccommodationPage()
        {
            OwnerMainWindow.MainFrame.Content = new AddAccommodationPage(_loggedInUser);
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        public void ShowAccommodationHelp()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationHelpPage(_loggedInUser);
        }
    }
}
