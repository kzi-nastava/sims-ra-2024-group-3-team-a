using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class FindAvailableDatesViewModel: ViewModel
    {
        private AccommodationDTO _selectedAccommodationDTO;
        private int _daysToStay;
        private UserDTO _loggedGuest;

        private RelayCommand _searchDatesCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _nextImageCommand;
        private RelayCommand _previousImageCommand;
        private List<string> _images;
        private string _selectedImage;
        public FindAvailableDatesViewModel(AccommodationDTO selectedAccommodationDTO, UserDTO loggedGuest) 
        {
            _selectedAccommodationDTO = selectedAccommodationDTO;
            _searchDatesCommand = new RelayCommand(SearchDates);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _nextImageCommand = new RelayCommand(NextImage);
            _previousImageCommand = new RelayCommand(PreviousImage);
            _loggedGuest = loggedGuest;
            _images = _selectedAccommodationDTO.Images;
            SelectedImage = _images[0];
        }

        private void SearchDates()
        {
            if (!IsSearchValid())
            {
                return;
            }
            GuestMainViewWindow.MainFrame.Content = new MakeReservationPage(_selectedAccommodationDTO, _loggedGuest, _selectedBeginDate, _selectedEndDate, DaysToStay);
        }

        private bool IsSearchValid()
        {
            DateTime? date = FindAvailableDatesPage.Instance.datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                  DateOnly _selectedBeginDateOnly = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }
            //_selectedBeginDate = _selectedBeginDate.AddDays(-timeSpanIncrement);

            DateTime? dateEnd = FindAvailableDatesPage.Instance.datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                DateOnly _selectedEndDateOnly = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }
            //_selectedEndDate = _selectedEndDate.AddDays(timeSpanIncrement);

            if (_selectedEndDate < _selectedBeginDate)
            {
                MessageBox.Show("Error! Improper TimeSpan - Begin Date must be before End Date!");
                return false;
            }

            DaysToStay = Int32.Parse(FindAvailableDatesPage.Instance.DaysToStayTextBox.Text);

            if (DaysToStay < _selectedAccommodationDTO.MinDaysReservation)
            {
                //_freeDates.Clear();
                MessageBox.Show($"Improper no of days, enter minimum {_selectedAccommodationDTO.MinDaysReservation} days!");
                return false;
            }
            return true;
        }
        public int DaysToStay
        {
            get
            {
                return _daysToStay;
            }
            set
            {
                _daysToStay = value;
                OnPropertyChanged();
                //ShowAccommodationStatisticsYear();
                //_selectedAccommodationDTO = null;
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
                //ShowAccommodationStatisticsYear();
                _selectedAccommodationDTO = null;
            }
        }

        private DateTime _selectedBeginDate = DateTime.Today;
        public DateTime SelectedBeginDate
        {
            get { return _selectedBeginDate; }
            set
            {
                if (_selectedBeginDate != value)
                {
                    _selectedBeginDate = value;
                    OnPropertyChanged("SelectedBeginDate");
                }
            }
        }

        private DateTime _selectedEndDate = DateTime.Today;
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged("SelectedEndDate");
                }
            }
        }

        public RelayCommand SearchDatesCommand
        {
            get
            {
                return _searchDatesCommand;
            }
            set
            {
                _searchDatesCommand = value;
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
            GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
        }
        public void CloseSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = null;
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
    }
}
