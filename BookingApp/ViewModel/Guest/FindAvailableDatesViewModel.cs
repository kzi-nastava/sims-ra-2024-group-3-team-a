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
        public FindAvailableDatesViewModel(AccommodationDTO selectedAccommodationDTO, UserDTO loggedGuest) 
        {
            _selectedAccommodationDTO = selectedAccommodationDTO;
            _searchDatesCommand = new RelayCommand(SearchDates);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _loggedGuest = loggedGuest;
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
    }
}
