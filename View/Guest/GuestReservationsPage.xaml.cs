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
    /// Interaction logic for GuestReservationsPage.xaml
    /// </summary>
    /// 

    public partial class GuestReservationsPage : Page, INotifyPropertyChanged
    {
        public static ObservableCollection<AccommodationReservationDTO> _myReservations { get; set; }
        public static ObservableCollection<AccommodationReservationChangeRequestDTO> _myChangeRequests { get; set; }
        private List<AccommodationReservationDTO> _myReservationsDTO;
        private AccommodationReservationRepository _accommodationReservationRepository;
        private AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private UserDTO _userDTO;

        public DateOnly _selectedNewBeginDate;
        public DateOnly _selectedNewEndDate;
        private AccommodationReservationDTO _selectedReservation;
        public GuestReservationsPage(UserDTO userDTO, ObservableCollection<AccommodationReservationChangeRequestDTO> myChangeRequests)
        {
            InitializeComponent();
            DataContext = this;
            _userDTO = userDTO;
            _myReservationsDTO = new List<AccommodationReservationDTO>();
            _myReservations = new ObservableCollection<AccommodationReservationDTO>();
            _myChangeRequests = myChangeRequests;
            
            _accommodationReservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> myReservations = _accommodationReservationRepository.GetAllByGuestId(_userDTO.Id);

            _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();
            //List<AccommodationReservation> myReservations = _accommodationReservationRepository.GetAllByGuestId(_userDTO.Id);

            foreach (AccommodationReservation accommodationReservation in myReservations)
            {
                _myReservationsDTO.Add(new AccommodationReservationDTO(accommodationReservation));
            }
            foreach (AccommodationReservationDTO accommodationReservationDTO in _myReservationsDTO)
            {
                _myReservations.Add(accommodationReservationDTO);
            }
            
            dataGridMyReservations.ItemsSource = _myReservations;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            if(dataGridMyReservations.SelectedItem == null) 
            {
                MessageBox.Show("Please select reservation to cancel!");
            }
            else
            {
                AccommodationReservationDTO selectedReservation = (AccommodationReservationDTO)dataGridMyReservations.SelectedItem;
                AccommodationRepository accommodationRepository = new AccommodationRepository();
                Accommodation selectedAccommodation = accommodationRepository.GetById(selectedReservation.AccommodationId);
                if (selectedReservation.BeginDate >= DateOnly.FromDateTime(DateTime.Today).AddDays(selectedAccommodation.CancellationPeriod))
                {
                    _accommodationReservationRepository.Delete(selectedReservation.ToAccommodationReservation());
                    _myReservations.Remove(selectedReservation);
                }
                else
                {
                    MessageBox.Show($"Too late to cancel. Cancelation period is {selectedAccommodation.CancellationPeriod} days!");
                }
            }
        }

        private void RateOwner_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequestChange_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMyReservations.SelectedItem == null)
            {
                MessageBox.Show("Please select reservation to request date change!");
            }
            else
            {
                _selectedReservation = (AccommodationReservationDTO)dataGridMyReservations.SelectedItem;
                if (_selectedReservation.BeginDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("This reservation is already over!");
                    return;
                }
                else
                {
                    frameRequestChange.Visibility = Visibility.Visible;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedBeginDate = datePickerNewBegin.SelectedDate;
            if (selectedBeginDate.HasValue)
            {
                DateTime dateTime = selectedBeginDate.Value;
                _selectedNewBeginDate = DateOnly.FromDateTime(dateTime);
            }
            DateTime? selectedEndDate = datePickerNewEnd.SelectedDate;
            if (selectedEndDate.HasValue)
            {
                DateTime dateTime = selectedEndDate.Value;
                _selectedNewEndDate = DateOnly.FromDateTime(dateTime);
            }

            AccommodationReservationChangeRequest changeRequest = new AccommodationReservationChangeRequest(0, _selectedReservation.Id, _selectedNewBeginDate, _selectedNewEndDate, Model.Enums.AccommodationChangeRequestStatus.WaitingForApproval,"");
            _accommodationReservationChangeRequestRepository.Save(changeRequest);
            _myChangeRequests.Add(new AccommodationReservationChangeRequestDTO(changeRequest));
            MessageBox.Show("Submitted request");
            
        }
    }
}
