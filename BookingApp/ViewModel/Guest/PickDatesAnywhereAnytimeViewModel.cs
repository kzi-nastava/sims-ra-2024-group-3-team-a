using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
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
    public class PickDatesAnywhereAnytimeViewModel : ViewModel
    {
        private List<AccommodationReservationDTO> _accommodationReservationsDTO;
        private AccommodationDTO _selectedAccommodationDTO;
        public static ObservableCollection<AccommodationReservationDTO> _freeDates { get; set; }
        private AccommodationDTO _accommodationDTO;
        private UserDTO _userDTO;

        public int DaysToStay;
        private AccommodationReservationService _accommodationReservationService;
        private SuperGuestService _superGuestService;

        private AccommodationReservationDTO _selectedDates;
        private RelayCommand _searchDatesCommand;
        private RelayCommand _showReservationDetailsPageCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _newReservationCommand;


        public PickDatesAnywhereAnytimeViewModel(AccommodationDTO selectedAccommodationDTO, UserDTO loggedInGuest, DateTime selectedBeginDate, DateTime selectedEndDate, int daysToStay)
        {
            isSuper = true;
            _userDTO = loggedInGuest;
            _selectedBeginDate = DateOnly.FromDateTime(selectedBeginDate);
            _selectedEndDate = DateOnly.FromDateTime(selectedEndDate);
            DaysToStay = daysToStay;
            _selectedAccommodationDTO = selectedAccommodationDTO;
            SelectedAccommodationDTO = _selectedAccommodationDTO;
            _accommodationDTO = selectedAccommodationDTO;

            //MakeReservationPage.Instance.DaysToStayTextBox.Text = DaysToStay.ToString();
            //AnywhereAnytimePage.Instance.datePickerBegin.Text = _selectedBeginDate.ToString();
            //AnywhereAnytimePage.Instance.datePickerEnd.Text = _selectedEndDate.ToString();
            //AnywhereAnytimePage.Instance. = _selectedEndDate.ToString();

            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();

            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _superGuestService = new SuperGuestService(superGuestRepository);

            _freeDates = new ObservableCollection<AccommodationReservationDTO>();

            _searchDatesCommand = new RelayCommand(SearchAvailableDates);
            _showReservationDetailsPageCommand = new RelayCommand(ShowReservationDetailsPage);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _newReservationCommand = new RelayCommand(NewReservation);
            _accommodationReservationsDTO = new List<AccommodationReservationDTO>();
            _accommodationReservationsDTO = _accommodationReservationService.GetAllByAccommodationId(_selectedAccommodationDTO.Id).Where(c => c.Canceled == false).Select(reservation => new AccommodationReservationDTO(reservation)).ToList();
            if ((_superGuestService.GetByUserId(_userDTO.Id) == null) || _superGuestService.GetByUserId(_userDTO.Id).Points == 0)
            {
                IsSuper = false;
            }
            else
            {
                IsSuper = true;

            }
            SearchAvailableDates();
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
        public AccommodationDTO SelectedAccommodationDTO
        {
            get
            {
                return _accommodationDTO;
            }
            set
            {
                _accommodationDTO = value;
                OnPropertyChanged();
                //ShowAccommodationStatisticsYear();
                //_accommodationDTO = null;
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
            int spanIncrement = AnywhereAnytimePage.Instance.searchMinDaysAnywhereTextBox.Text != null ? Int32.Parse(AnywhereAnytimePage.Instance.searchMinDaysAnywhereTextBox.Text) : Int32.Parse(AnywhereAnytimePage.Instance.searchMinDaysAnywhereTextBox.Text);
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

            //DateTime? date = AnywhereAnytimePage.Instance.datePickerBegin.SelectedDate;
            //if (date.HasValue)
            //{
                //_selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            //}
            _selectedBeginDate = _selectedBeginDate.AddDays(-timeSpanIncrement);

            //DateTime? dateEnd = AnywhereAnytimePage.Instance.datePickerEnd.SelectedDate;
            //if (dateEnd.HasValue)
            //{
             //   _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            //}
            _selectedEndDate = _selectedEndDate.AddDays(timeSpanIncrement);

            if (_selectedEndDate < _selectedBeginDate)
            {
                _freeDates.Clear();
                MessageBox.Show("Error! Improper TimeSpan - Begin Date must be before End Date!");
                return true;
            }

            DaysToStay = Int32.Parse(AnywhereAnytimePage.Instance.searchMinDaysAnywhereTextBox.Text);

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
        private void NewReservation()
        {
            int _guestNumber;
            if (int.TryParse(MakeReservationPage.Instance.GuestNumberTextBox.Text, out _guestNumber))
            {
                if (_guestNumber < 0 || _guestNumber > _accommodationDTO.Capacity)
                {
                    MessageBox.Show($"Error! Capacity is {_accommodationDTO.Capacity} guests!");
                    return;
                }
                Review rating = new Review();
                AccommodationReservation acc = new AccommodationReservation(0, _userDTO.Id, _accommodationDTO.Id, SelectedDates.BeginDate, SelectedDates.EndDate, false, usedPoint, rating);
                _accommodationReservationService.Save(acc);
                MessageBox.Show("Reservation successful!");
                if (UsedPoint)
                {
                    SuperGuest superGuest = _superGuestService.GetByUserId(_userDTO.Id);
                    superGuest.Points--;
                    _superGuestService.Update(superGuest);
                }
            }
            else
            {
                MessageBox.Show("Please enter number of guests!");
            }
        }
        private bool isSuper;
        public bool IsSuper
        {
            get
            {
                return isSuper;
            }
            set
            {
                isSuper = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsToggleEnabled));
            }
        }
        public bool IsToggleEnabled => IsSuper;
        private bool usedPoint;
        public bool UsedPoint
        {
            get
            {
                return usedPoint;
            }
            set
            {
                usedPoint = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand NewReservationCommand
        {
            get
            {
                return _newReservationCommand;
            }
            set
            {
                _newReservationCommand = value;
                OnPropertyChanged();
            }
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

