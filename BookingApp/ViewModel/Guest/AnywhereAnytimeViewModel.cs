using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guest;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class AnywhereAnytimeViewModel: ViewModel
    {
        private List<AccommodationReservationDTO> _accommodationReservationsDTO;
        private AccommodationDTO _selectedAccommodationDTO;
        public static ObservableCollection<AccommodationReservationDTO> _freeDates { get; set; }
        public static ObservableCollection<AccommodationDTO> _foundAccommodations { get; set; }
        private AccommodationDTO _accommodationDTO;
        private UserDTO _userDTO;

        public int DaysToStay;
        private AccommodationReservationService _accommodationReservationService;
        private SuperGuestService _superGuestService;
        private AccommodationService _accommodationService;

        private AccommodationReservationDTO _selectedDates;
        private RelayCommand _showReservationDetailsPageCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _newReservationCommand;
        private RelayCommand _searchAccommodationsCommand;



        public AnywhereAnytimeViewModel(UserDTO loggedInGuest)
        {
            isSuper = true;
            _userDTO = loggedInGuest;
             _selectedBeginDate = null;
             _selectedEndDate = null;
            //DaysToStay = daysToStay;
            //_selectedAccommodationDTO = selectedAccommodationDTO;
            //SelectedAccommodationDTO = _selectedAccommodationDTO;


            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ISuperGuestRepository superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();

            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _superGuestService = new SuperGuestService(superGuestRepository);
            _accommodationService = new AccommodationService(accommodationRepository);

            _freeDates = new ObservableCollection<AccommodationReservationDTO>();
            _foundAccommodations = new ObservableCollection<AccommodationDTO>();

            _accommodationDTO = new AccommodationDTO();
            
            _showReservationDetailsPageCommand = new RelayCommand(ShowReservationDetailsPage);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _searchAccommodationsCommand = new RelayCommand(SearchAccommodations);

            _accommodationReservationsDTO = new List<AccommodationReservationDTO>();
            _accommodationReservationsDTO = _accommodationReservationService.GetAllByAccommodationId(_accommodationDTO.Id).Where(c => c.Canceled == false).Select(reservation => new AccommodationReservationDTO(reservation)).ToList();

            if ((_superGuestService.GetByUserId(_userDTO.Id) == null) || _superGuestService.GetByUserId(_userDTO.Id).Points == 0)
            {
                IsSuper = false;

            }
            else
            {
                IsSuper = true;

            }
            //SearchAvailableDates();
        }

        private DateOnly? _selectedBeginDate = null;
        public DateOnly? SelectedBeginDate
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

        private DateOnly? _selectedEndDate = null;
        public DateOnly? SelectedEndDate
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
                return _selectedAccommodationDTO;
            }
            set
            {
                _selectedAccommodationDTO = value;
                
                
                OnPropertyChanged();
                ReserveAccommodation();
                _selectedBeginDate = null;
                _selectedBeginDate = null;
                DaysToStay = 0;
                _selectedAccommodationDTO = null;
            }
        }

        private void ShowReservationDetailsPage()
        {
            AccommodationReservationDTO acc_resDTO = _selectedDates;
            if (acc_resDTO != null)
            {
                DateOnly beginOfStay = acc_resDTO.BeginDate;
                DateOnly endOfStay = acc_resDTO.EndDate;
                //MakeAccommodationReservationPage.Instance.frameReserve.Content = new ReservationDetailsPage(_accommodationDTO, _userDTO, beginOfStay, endOfStay);
            }
            else
            {
                MessageBox.Show("Please pick dates!");
                //MakeAccommodationReservationPage.Instance.frameReserve.Content = null;
            }
        }
        private void SearchAccommodations()
        {
            _freeDates.Clear();
            _foundAccommodations.Clear();
            var accommodations = _accommodationService.GetAll();

            foreach(var accommodation in accommodations)
            {
                PerformSearchForDates(accommodation, 0);
            }
        }
        public void ReserveAccommodation()
        {
           // if (_selectedAccommodationDTO != null)
            //{
                DateOnly dateOnlyBeginDate = new DateOnly(_selectedBeginDate.Value.Year, _selectedBeginDate.Value.Month, _selectedBeginDate.Value.Day);
                DateOnly dateOnlyEndDate = new DateOnly(_selectedEndDate.Value.Year, _selectedEndDate.Value.Month, _selectedEndDate.Value.Day);
                GuestMainViewWindow.MainFrame.Content = new PickDatesAnywhereAnytime(_selectedAccommodationDTO, _userDTO, dateOnlyBeginDate.ToDateTime(TimeOnly.MinValue), dateOnlyEndDate.ToDateTime(TimeOnly.MinValue), DaysToStay);
                //_selectedBeginDate = DateOnly.FromDateTime(DateTime.Now);
                //_selectedEndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(180);

            //}
        }

        public bool PerformSearchForDates(Accommodation accommodation, int timeSpanIncrement)
        {
            _freeDates.Clear();

            DateTime? date = AnywhereAnytimePage.Instance.datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                _selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }
            //_selectedBeginDate = _selectedBeginDate.AddDays(-timeSpanIncrement);
            else
            {
                _selectedBeginDate = DateOnly.FromDateTime(DateTime.Now);
            }
            DateTime? dateEnd = AnywhereAnytimePage.Instance.datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }
            //_selectedEndDate = _selectedEndDate.AddDays(timeSpanIncrement);
            else
            {
                _selectedEndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(180));
            }

            if (_selectedEndDate < _selectedBeginDate)
            {
                _freeDates.Clear();
                MessageBox.Show("Error! Improper TimeSpan - Begin Date must be before End Date!");
                return false;
            }

            DaysToStay = Int32.Parse(AnywhereAnytimePage.Instance.searchMinDaysAnywhereTextBox.Text);

            if (DaysToStay < accommodation.MinDaysReservation)
            {
                return false;
            }

            int numberOfGuests = Int32.Parse(AnywhereAnytimePage.Instance.searchCapacityAnywhereTextBox.Text);
            if (numberOfGuests > accommodation.Capacity)
            {
                return false;
            }

            _accommodationReservationsDTO = _accommodationReservationService.GetAllByAccommodationId(accommodation.Id).Where(c => c.Canceled == false).Select(reservation => new AccommodationReservationDTO(reservation)).ToList();
            if(_accommodationReservationsDTO.Count == 0) 
            {
                _foundAccommodations.Add(new AccommodationDTO(accommodation));
                return true;
            }

            DateOnly dateOnlyBeginDate = new DateOnly(_selectedBeginDate.Value.Year, _selectedBeginDate.Value.Month, _selectedBeginDate.Value.Day);
            DateOnly dateOnlyEndDate = new DateOnly(_selectedEndDate.Value.Year, _selectedEndDate.Value.Month, _selectedEndDate.Value.Day);

            List<DateOnly> unavailableDates = FindUnavailableDates(dateOnlyBeginDate, dateOnlyEndDate);

            return AvailableDatesFound(unavailableDates, accommodation);
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
        public bool AvailableDatesFound(List<DateOnly> unavailableDates, Accommodation accommodation)
        {
            bool found = false;
            int availableDatesCounter = 0;
            DateOnly dateOnlyBeginDate = new DateOnly(_selectedBeginDate.Value.Year, _selectedBeginDate.Value.Month, _selectedBeginDate.Value.Day);
            DateOnly dateOnlyEndDate = new DateOnly(_selectedEndDate.Value.Year, _selectedEndDate.Value.Month, _selectedEndDate.Value.Day);

            DateOnly i = dateOnlyBeginDate;
            DateOnly j = dateOnlyEndDate;
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
                        //_freeDates.Add(accommodationReservationDTO);
                        _foundAccommodations.Add( new AccommodationDTO(accommodation));
                        
                        availableDatesCounter = 0;
                        found = true;
                        return true;
                    }
                }
            }
            return found;
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
        public RelayCommand SearchAccommodationsCommand
        {
            get
            {
                return _searchAccommodationsCommand;
            }
            set
            {
                _searchAccommodationsCommand = value;
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
        public ObservableCollection<AccommodationDTO> FoundAccommodations
        {
            get
            {
                return _foundAccommodations;
            }
            set
            {
                _foundAccommodations = value;
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
