using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Owner;
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

        private User _loggedInUser;

        public AccommodationsViewModel(User loggedInUser)
        {
            _accommodationService = new AccommodationService();
            List<AccommodationDTO> accommodationsDTO = _accommodationService.GetAccommodationsForOwner(loggedInUser).Select(accommodation => new AccommodationDTO(accommodation)).ToList(); ;
            _accommodationsDTO = new ObservableCollection<AccommodationDTO>(accommodationsDTO);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showAddAccommodationPageCommand = new RelayCommand(ShowAddAccommodationPage);

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

        public void ShowAddAccommodationPage()
        {
            OwnerMainWindow.MainFrame.Content = new AddAccommodationPage(new UserDTO(_loggedInUser));
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
    }
}
