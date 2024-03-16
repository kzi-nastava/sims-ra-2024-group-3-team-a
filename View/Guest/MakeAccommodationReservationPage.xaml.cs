using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for MakeAccommodationReservationPage.xaml
    /// </summary>
    public partial class MakeAccommodationReservationPage : Page, INotifyPropertyChanged
    {
        private List<AccommodationReservationDTO> _accommodationReservationsDTO;
        public static ObservableCollection<AccommodationReservationDTO> _freeDates { get; set; }

        private AccommodationReservationRepository _accommodationReservationRepository;
        private AccommodationDTO _accommodationDTO;
        private UserDTO _userDTO;

        public int DaysToStay;

        public MakeAccommodationReservationPage(AccommodationDTO accommodationDTO, UserDTO userDTO)
        {
            InitializeComponent();
            DataContext = this;
            _accommodationDTO = accommodationDTO;
            _userDTO = userDTO;

            _accommodationReservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> accommodationReservations = _accommodationReservationRepository.GetAllByAccommodationId(accommodationDTO.Id);
            _accommodationReservationsDTO = new List<AccommodationReservationDTO>();

            foreach(AccommodationReservation accommodationReservation in accommodationReservations)
            {
                
                _accommodationReservationsDTO.Add(new AccommodationReservationDTO(accommodationReservation));
            }

            _freeDates = new ObservableCollection<AccommodationReservationDTO>();
            dataGridAvailableDates.ItemsSource = _freeDates;
        }

        public DateOnly _selectedBeginDate;
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

        public DateOnly _selectedEndDate;
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

        private void SearchAvailableDates_Click(object sender, RoutedEventArgs e)
        {
            int spanIncrement = Int32.Parse(DaysToStayTextBox.Text);
            if (!PerformSearchForDates(0))
            {
                PerformSearchForDates(spanIncrement);
            }
        }

        private void ShowReservationDetailsPage(object sender, RoutedEventArgs e)
        {
            AccommodationReservationDTO acc_resDTO = (AccommodationReservationDTO)dataGridAvailableDates.SelectedItem;
            DateOnly beginOfStay = acc_resDTO.BeginDate;
            DateOnly endOfStay = acc_resDTO.EndDate;
            frameReserve.Content = new ReservationDetailsPage(_accommodationDTO, _userDTO, beginOfStay, endOfStay); 
        }

        public bool PerformSearchForDates(int timeSpanIncrement)
        {
            _freeDates.Clear();
            bool found = false;

            DateTime? date = datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                _selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }

            _selectedBeginDate = _selectedBeginDate.AddDays(-timeSpanIncrement);
            
            DateTime? dateEnd = datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }
            _selectedEndDate=_selectedEndDate.AddDays(timeSpanIncrement);

            if (_selectedEndDate < _selectedBeginDate)
            {
                _freeDates.Clear();
                MessageBox.Show("Error! Improper TimeSpan - Begin Date must be before End Date!");
                return true;
            }

            DaysToStay = Int32.Parse(DaysToStayTextBox.Text);

            if(DaysToStay < _accommodationDTO.MinDaysReservation)
            {
                _freeDates.Clear();
                MessageBox.Show($"Improper no of days, enter minimum {_accommodationDTO.MinDaysReservation} days!");
                return true;
            }

            DateOnly dateIterator = _selectedBeginDate;
            int availableDatesCounter = 0;
            
            List<DateOnly> unavailableDates = new List<DateOnly>();

            for (; dateIterator <= _selectedEndDate; dateIterator = dateIterator.AddDays(1))
            {
                AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO();
                foreach (AccommodationReservationDTO accommodationReservationDTO2 in _accommodationReservationsDTO)
                {
                    if (dateIterator >= accommodationReservationDTO2.BeginDate && dateIterator <= accommodationReservationDTO2.EndDate)
                    {
                        unavailableDates.Add(dateIterator);
                    }
                }
            }
            
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
