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
        //private  List<AccommodationReservationDTO> _freeDates;
        public static ObservableCollection<AccommodationReservationDTO> _freeDates { get; set; }
        //public static ObservableCollection<Comment> Comments { get; set; }

        private AccommodationReservationRepository _accommodationReservationRepository;
        private AccommodationDTO _accommodationDTO;
        private UserDTO _userDTO;

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
            _freeDates = new  ObservableCollection<AccommodationReservationDTO>();
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

        public int DaysToStay;


        private void SearchAvailableDatesClick(object sender, RoutedEventArgs e)
        {
           if(!PerformSearchForDates(0))
           {
                PerformSearchForDates(5);
           }
        }
        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            AccommodationReservationDTO acc_resDTO = (AccommodationReservationDTO)dataGridAvailableDates.SelectedItem;
            DateOnly beginOfStay = acc_resDTO.BeginDate;
            DateOnly endOfStay = acc_resDTO.EndDate;
            frameReserve.Content = new ReservationDetailsPage(_accommodationDTO, _userDTO, beginOfStay, endOfStay); 
        }

        

        public bool PerformSearchForDates(int area)
        {
            _freeDates.Clear();
            bool found = false;

            DateTime? date = datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                _selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }

            _selectedBeginDate = _selectedBeginDate.AddDays(-area);
            
            DateTime? dateEnd = datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }
            _selectedEndDate=_selectedEndDate.AddDays(area);

            if (_selectedEndDate < _selectedBeginDate)
            {
                _freeDates.Clear();
                //ReserveButton.IsEnabled = false;
                MessageBox.Show("Los opseg - krajnji datum je pre pocetnog");
                return true;
            }

            DaysToStay = Int32.Parse(DaysToStayTextBox.Text);

            if(DaysToStay < _accommodationDTO.MinDaysReservation)
            {
                _freeDates.Clear();
                //ReserveButton.IsEnabled = false;
                MessageBox.Show($"Los broj dana, minimum je {_accommodationDTO.MinDaysReservation} dana!");
                return true;
            }

            DateOnly i = _selectedBeginDate;
            int availableDatesCounter = 0;
            
            List<DateOnly> unavailableDates = new List<DateOnly>();

            for (; i <= _selectedEndDate; i = i.AddDays(1))
            {
                AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO();
                foreach (AccommodationReservationDTO accommodationReservationDTO2 in _accommodationReservationsDTO)
                {
                    if (i >= accommodationReservationDTO2.BeginDate && i <= accommodationReservationDTO2.EndDate)
                    {
                        unavailableDates.Add(i);
                    }
                }
            }

            i = _selectedBeginDate;
            DateOnly g = i;

            for (; g <= _selectedEndDate; g = g.AddDays(1))
            {
                availableDatesCounter = 0;
                for (i = g; i <= _selectedEndDate; i = i.AddDays(1))
                {
                    AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO();
                    if (unavailableDates.Any(c => c == i))
                    {
                        availableDatesCounter = 0;
                        continue;
                    }
                    availableDatesCounter++;
                    if (availableDatesCounter == DaysToStay && !_freeDates.Any(c => c.EndDate == i))
                    {
                        accommodationReservationDTO.BeginDate = i.AddDays(-DaysToStay + 1);
                        accommodationReservationDTO.EndDate = i;
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
