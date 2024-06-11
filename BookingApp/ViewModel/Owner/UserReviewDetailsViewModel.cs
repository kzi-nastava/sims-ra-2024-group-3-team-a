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
        private RelayCommand _nextImageCommand;
        private RelayCommand _previousImageCommand;

        private List<string> _images;
        private string _selectedImage;

        public UserReviewDetailsViewModel(AccommodationReservationDTO accommodationReservationDTO)
        {
            _accommodationReservationDTO = accommodationReservationDTO;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);

            _images = accommodationReservationDTO.RatingDTO.GuestImages;
            _selectedImage = _images[0];

            _nextImageCommand = new RelayCommand(NextImage);
            _previousImageCommand = new RelayCommand(PreviousImage);
        }

        public string SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
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
        public RelayCommand NextImageCommand
        {
            get
            {
                return _nextImageCommand;
            }
            set
            {
                _nextImageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand PreviousImageCommand
        {
            get
            {
                return _previousImageCommand;
            }
            set
            {
                _previousImageCommand = value;
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

        private void NextImage()
        {
            int index = _images.IndexOf(_selectedImage);
            if (index == _images.Count - 1)
            {
                SelectedImage = _images[0];
            }
            else
            {
                SelectedImage = _images[index + 1];
            }
        }

        private void PreviousImage()
        {
            int index = _images.IndexOf(_selectedImage);
            if (index == 0)
            {
                SelectedImage = _images[_images.Count - 1];
            }
            else
            {
                SelectedImage = _images[index - 1];
            }
        }
    }
}
