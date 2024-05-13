using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class UserReviewDetailsViewModel : ViewModel
    {
        private AccommodationReservationDTO _accommodationReservationDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;

        public UserReviewDetailsViewModel(AccommodationReservationDTO accommodationReservationDTO)
        {
            _accommodationReservationDTO = accommodationReservationDTO;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
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

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
    }
}
