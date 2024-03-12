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

        public MakeAccommodationReservationPage(AccommodationDTO accommodationDTO)
        {
            InitializeComponent();
            
            DataContext = this;
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
            _freeDates.Clear();
            
            DateTime? date = datePickerBegin.SelectedDate;
            if (date.HasValue)
            {
                _selectedBeginDate = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);
            }

            DateTime? dateEnd = datePickerEnd.SelectedDate;
            if (dateEnd.HasValue)
            {
                _selectedEndDate = new DateOnly(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
            }

            DaysToStay = Int32.Parse(DaysToStayTextBox.Text); 

            int k = 0;
            DateOnly i = _selectedBeginDate;
            DateOnly j = i;
            bool isAvailable = true;
            bool firstIteration = true;
            List<DateOnly> unavailableDates = new List<DateOnly>();
            for (; i <= _selectedEndDate; i=i.AddDays(1))
            {
                k = 0;

                for (j = i; j <= _selectedEndDate; j=j.AddDays(1))
                {
                    AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO();
                    if (firstIteration && !isAvailable )
                    {
                        unavailableDates.Add(j);
                    }

                    if (!firstIteration && unavailableDates.Any(c => c == j))
                    {
                        k = 0;
                        continue;
                    }

                    if (_accommodationReservationsDTO.Any(c => c.EndDate == j))
                    {
                        isAvailable = true;

                        continue;
                    }
                    else if (_accommodationReservationsDTO.Any(c => c.BeginDate == j))
                    {
                        isAvailable = false;
                        k = 0;
                        continue;
                    }
                    else if (isAvailable)
                    {
                        k++;
                        if (k == DaysToStay && !_freeDates.Any(c => c.EndDate == j))
                        {
                            
                            accommodationReservationDTO.BeginDate = j.AddDays(-DaysToStay+1);
                            accommodationReservationDTO.EndDate = j;
                            _freeDates.Add(accommodationReservationDTO);
                            k = 0;
                            
                        }
                        else if(k == DaysToStay)
                        {
                            k = 0;
                        }
                    }
                }
                firstIteration = false;
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
