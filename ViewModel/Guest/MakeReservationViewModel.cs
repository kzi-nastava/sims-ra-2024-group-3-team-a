using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class MakeReservationViewModel: ViewModel
    {
        private List<AccommodationReservationDTO> _accommodationReservationsDTO;
        public static ObservableCollection<AccommodationReservationDTO> _freeDates { get; set; }
        private AccommodationDTO _accommodationDTO;
        private UserDTO _userDTO;

        public int DaysToStay;
        private AccommodationReservationService _accommodationReservationService;

        private AccommodationReservationDTO _selectedDates;
        private RelayCommand _searchDatesCommand;
        private RelayCommand _showReservationDetailsPageCommand;
        public MakeReservationViewModel(AccommodationDTO selectedAccommodationDTO, UserDTO loggedInGuest)
        {
            _userDTO = loggedInGuest;
            _accommodationReservationService = new AccommodationReservationService();
            _freeDates = new ObservableCollection<AccommodationReservationDTO>();
            _accommodationDTO = selectedAccommodationDTO;
            _searchDatesCommand = new RelayCommand(SearchAvailableDates);
            _showReservationDetailsPageCommand = new RelayCommand(ShowReservationDetailsPage);
            _accommodationReservationsDTO = new List<AccommodationReservationDTO>();
            _accommodationReservationsDTO = _accommodationReservationService.GetAll().Select(reservation => new AccommodationReservationDTO(reservation)).ToList();
        }

        private DateOnly _selectedBeginDate;
        public DateOnly SelectedBeginDate
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

        private DateOnly _selectedEndDate;
        public DateOnly SelectedEndDate
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

        private void ShowReservationDetailsPage()
        {
            AccommodationReservationDTO acc_resDTO = _selectedDates;
            if (acc_resDTO != null)
            {
                DateOnly beginOfStay = acc_resDTO.BeginDate;
                DateOnly endOfStay = acc_resDTO.EndDate;
                MakeAccommodationReservationPage.Instance.frameReserve.Content = new ReservationDetailsPage(_accommodationDTO, _userDTO, beginOfStay, endOfStay);
            }
            else
            {
                MessageBox.Show("Please pick dates!");
                MakeAccommodationReservationPage.Instance.frameReserve.Content = null;
            }
        }

        private void SearchAvailableDates()
        {
            int spanIncrement = Int32.Parse(MakeAccommodationReservationPage.Instance.DaysToStayTextBox.Text);
            int increment = 0;
            while (true)
            {
                if (PerformSearchForDates(increment))
                {
                    break;
                }
                increment += spanIncrement;
            }
        }
        public bool PerformSearchForDates(int timeSpanIncrement)
        {
            _freeDates.Clear();

            DateTime? date = MakeAccommodationReservationPage.Instance.datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                _selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }
            _selectedBeginDate = _selectedBeginDate.AddDays(-timeSpanIncrement);

            DateTime? dateEnd = MakeAccommodationReservationPage.Instance.datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }
            _selectedEndDate = _selectedEndDate.AddDays(timeSpanIncrement);

            if (_selectedEndDate < _selectedBeginDate)
            {
                _freeDates.Clear();
                MessageBox.Show("Error! Improper TimeSpan - Begin Date must be before End Date!");
                return true;
            }

            DaysToStay = Int32.Parse(MakeAccommodationReservationPage.Instance.DaysToStayTextBox.Text);

            if (DaysToStay < _accommodationDTO.MinDaysReservation)
            {
                _freeDates.Clear();
                MessageBox.Show($"Improper no of days, enter minimum {_accommodationDTO.MinDaysReservation} days!");
                return true;
            }

            List<DateOnly> unavailableDates = FindUnavailableDates(_selectedBeginDate, _selectedEndDate);

            return AvailableDatesFound(unavailableDates);
        }
        List<DateOnly> FindUnavailableDates(DateOnly beginDate, DateOnly endDate)
        {
            List<DateOnly> unavailableDates = new List<DateOnly>();
            DateOnly dateIterator = beginDate;
            for (; dateIterator <= endDate; dateIterator = dateIterator.AddDays(1))
            {

                foreach (AccommodationReservationDTO accommodationReservationDTO in _accommodationReservationsDTO)
                {
                    if (dateIterator >= accommodationReservationDTO.BeginDate && dateIterator <= accommodationReservationDTO.EndDate)
                    {
                        unavailableDates.Add(dateIterator);
                    }
                }
            }
            return unavailableDates;
        }
        public bool AvailableDatesFound(List<DateOnly> unavailableDates)
        {
            bool found = false;
            int availableDatesCounter = 0;
            DateOnly i = _selectedBeginDate;
            DateOnly j = _selectedBeginDate;
            for (; i <= _selectedEndDate; i = i.AddDays(1))
            {
                availableDatesCounter = 0;
                for (j = i; j <= _selectedEndDate; j = j.AddDays(1))
                {
                    AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO();
                    if (unavailableDates.Any(c => c == j))
                    {
                        availableDatesCounter = 0;
                        continue;
                    }
                    availableDatesCounter++;
                    if (availableDatesCounter == DaysToStay && !_freeDates.Any(c => c.EndDate == j))
                    {
                        accommodationReservationDTO.BeginDate = j.AddDays(-DaysToStay + 1);
                        accommodationReservationDTO.EndDate = j;
                        _freeDates.Add(accommodationReservationDTO);
                        availableDatesCounter = 0;
                        found = true;
                    }
                }
            }
            return found;
        }
        public ObservableCollection<AccommodationReservationDTO> FreeDates 
        {
            get
            {
                return _freeDates;
            }
            set
            {
                _freeDates = value;
                OnPropertyChanged();
            }
        }
        public AccommodationReservationDTO SelectedDates
        {
            get
            {
                return _selectedDates;
            }
            set
            {
                _selectedDates = value;
                OnPropertyChanged();
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
        public RelayCommand ShowReservationDetailsPageCommand
        {
            get
            {
                return _showReservationDetailsPageCommand;
            }
            set
            {
                _showReservationDetailsPageCommand = value;
                OnPropertyChanged();
            }
        }
    }
    
}
