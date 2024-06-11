using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Owner
{
    public class MyReviewRatedViewModel : ViewModel
    {
        private AccommodationReservationDTO _accommodationReservationDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;

        public MyReviewRatedViewModel(AccommodationReservationDTO accommodationReservationDTO)
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
