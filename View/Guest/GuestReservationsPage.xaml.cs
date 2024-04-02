using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
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
using static System.Net.Mime.MediaTypeNames;

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
        private List<string> _images;
        public GuestReservationsPage(UserDTO userDTO, ObservableCollection<AccommodationReservationChangeRequestDTO> myChangeRequests)
        {
            InitializeComponent();
            DataContext = this;
            _userDTO = userDTO;
            _myReservationsDTO = new List<AccommodationReservationDTO>();
            _myReservations = new ObservableCollection<AccommodationReservationDTO>();
            _myChangeRequests = myChangeRequests;
            _images = new List<string>();
            
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
                if (selectedReservation.Canceled == true)
                {
                    MessageBox.Show("Reservation already canceled!");
                }
                else if (selectedReservation.BeginDate >= DateOnly.FromDateTime(DateTime.Today).AddDays(selectedAccommodation.CancellationPeriod))
                {
                    //_accommodationReservationRepository.Delete(selectedReservation.ToAccommodationReservation());
                    //_myReservations.Remove(selectedReservation);
                    selectedReservation.Canceled = true;
                    _accommodationReservationRepository.Update(selectedReservation.ToAccommodationReservation());
                    MessageBox.Show("Cancellation successful!");
                }
                else
                {
                    MessageBox.Show($"Too late to cancel. Cancelation period is {selectedAccommodation.CancellationPeriod} days!");
                }
            }
        }

        private void RateOwner_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMyReservations.SelectedItem == null)
            {
                MessageBox.Show("Please select reservation to rate!");
            }
            else
            {
                _selectedReservation = (AccommodationReservationDTO)dataGridMyReservations.SelectedItem;
                if (_selectedReservation.EndDate.AddDays(5) < DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("You can't rate this - it was over more than 5 days ago!");
                    return;
                }
                else if (_selectedReservation.EndDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("You can't rate this - reservation not over yet!");
                    return;
                }
                else
                {
                    frameRateOwner.Visibility = Visibility.Visible;
                }
                
            }
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

        private void AddImages(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                _images = openFileDialog.FileNames.ToList();

                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, _images[i]).ToString();
                }
            }
        }

        private void SubmitRateOwner_Click(object sender, RoutedEventArgs e)
        {
            AccommodationReservationDTO selectedReservation = (AccommodationReservationDTO)dataGridMyReservations.SelectedItem;
            int cleanliness = Convert.ToInt32(cleanlinessTextBox.Text);
            int correctness = Convert.ToInt32(ownerCorrectnessTextBox.Text);
            string comment = CommentForOwnerTextBox.Text;
            List<string> images = new List<string>();

            //ReviewDTO guestReview = new ReviewDTO(cleanliness, correctness, comment, images);
            selectedReservation.RatingDTO.GuestCleanlinessRating = cleanliness;
            selectedReservation.RatingDTO.GuestHospitalityRating = correctness;
            selectedReservation.RatingDTO.GuestComment = comment;
            selectedReservation.RatingDTO.GuestImages = _images;
            _accommodationReservationRepository.Update(selectedReservation.ToAccommodationReservation());
            MessageBox.Show("Rated Owner successfully!");

        }
    }
}
