using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.WizardAndHelp
{
    public class AccommodationHelpViewModel : ViewModel
    {
        private List<string> _images;
        private string _currentImage;
        private int _currentIndex;
        private int _currentIndexForShow;
        private UserDTO _loggedInUser;

        private RelayCommand _nextImageCommand;
        private RelayCommand _previousImageCommand;
        public AccommodationHelpViewModel(UserDTO loggedInUser)
        {
            _images = new List<string>();
            _loggedInUser = loggedInUser;

            _images.Add("../../../Resources/Images/WizardAndHelp/AccommodationHelp1.png");
            _images.Add("../../../Resources/Images/WizardAndHelp/AccommodationHelp2.png");
            _images.Add("../../../Resources/Images/WizardAndHelp/AccommodationHelp3.png");
            _images.Add("../../../Resources/Images/WizardAndHelp/AccommodationHelp4.png");
            _images.Add("../../../Resources/Images/WizardAndHelp/AccommodationHelp5.png");

            _currentImage = _images[0];
            _currentIndex = 0;
            _currentIndexForShow = 1;

            _nextImageCommand = new RelayCommand(NextImage);
            _previousImageCommand = new RelayCommand(PreviousImage);
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
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
                OnPropertyChanged();
            }
        }
        public int CurrentIndexForShow
        {
            get
            {
                return _currentIndexForShow;
            }
            set
            {
                _currentIndexForShow = value;
                OnPropertyChanged();
            }
        }
        public string CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        private void NextImage()
        {
            if (_currentIndex < _images.Count() - 1)
            {
                CurrentIndex++;
                CurrentIndexForShow++;
                CurrentImage = _images[_currentIndex];
            }
            else
            {
                OwnerMainWindow.MainFrame.Content = new AccommodationsPage(_loggedInUser);
            }
        }

        private void PreviousImage()
        {
            if (_currentIndex > 0)
            {
                CurrentIndex--;
                CurrentIndexForShow--;
                CurrentImage = _images[_currentIndex];
            }
        }
    }
}

