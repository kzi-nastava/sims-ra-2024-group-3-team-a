using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guest;
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

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationsViewModel: ViewModel
    {
        public static ObservableCollection<AccommodationReservationDTO> _myReservations { get; set; }
        public static ObservableCollection<AccommodationReservationChangeRequestDTO> _myChangeRequests { get; set; }

        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private UserDTO _loggedInGuest;

        private DateTime _selectedNewBeginDate;
        private DateTime _selectedNewEndDate;
        private AccommodationReservationDTO _selectedReservation;

        private List<string> _images;

        private bool _isFrameRateVisible;
        private bool _isFrameMyRequestsVisible;

        private RelayCommand _addImagesCommand;
        private RelayCommand _submitRateOwnerCommand;
        private RelayCommand _rateOwnerCommand;
        private RelayCommand _requestChangeCommand;
        private RelayCommand _submitRequestCommand;
        private RelayCommand _cancelReservationCommand;
        private RelayCommand _renovationRatingCommand;
        
        private int _selectedRating;


        public GuestReservationsViewModel(UserDTO loggedInGuest) 
        {
            _loggedInGuest = loggedInGuest;

            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);

            _images = new List<string>();

            _myChangeRequests = new ObservableCollection<AccommodationReservationChangeRequestDTO>();

            _addImagesCommand = new RelayCommand(AddImages);
            _rateOwnerCommand = new RelayCommand(RateOwner);
            _submitRateOwnerCommand = new RelayCommand(SubmitRateOwner);
            _requestChangeCommand = new RelayCommand(RequestChange);
            _submitRequestCommand = new RelayCommand(SubmitRequest);
            _cancelReservationCommand = new RelayCommand(CancelReservation);
            _renovationRatingCommand = new RelayCommand(ChangeRating);
            UpdateMyReservations();
        }

        public void UpdateMyReservations()
        {
            List<AccommodationReservationDTO> AccommodationReservationsList = _accommodationReservationService.GetAllByGuestId(_loggedInGuest.Id).Select(accommodationReservation => new AccommodationReservationDTO(accommodationReservation)).ToList();
            _myReservations = new ObservableCollection<AccommodationReservationDTO>(AccommodationReservationsList);
        }

        private void CancelReservation()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to cancel!");
            }
            else
            {
                Accommodation selectedAccommodation = _accommodationService.GetById(_selectedReservation.AccommodationId);
                if (_selectedReservation.Canceled == true)
                {
                    MessageBox.Show("Reservation already canceled!");
                }
                else if (_selectedReservation.BeginDate >= DateOnly.FromDateTime(DateTime.Today).AddDays(selectedAccommodation.CancellationPeriod))
                { 
                    _selectedReservation.Canceled = true;
                    _accommodationReservationService.Update(_selectedReservation.ToAccommodationReservation());
                    MessageBox.Show("Cancellation successful!");
                }
                else
                {
                    MessageBox.Show($"Too late to cancel. Cancelation period is {selectedAccommodation.CancellationPeriod} days!");
                }
            }
        }

        private void RequestChange()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to request date change!");
            }
            else
            {
                if (_selectedReservation.BeginDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("This reservation is already over!");
                    return;
                }
                else
                {
                    IsFrameMyRequestsVisible = true;
                    IsFrameRateVisible = false;
                }
            }
        }

        private void RateOwner()
        {
            if (_selectedReservation == null)
            {
                MessageBox.Show("Please select reservation to rate!");
            }
            else
            {
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
                    IsFrameRateVisible = true;
                    IsFrameMyRequestsVisible = false;
                }

            }
        }

        private void SubmitRequest()
        {
            DateOnly selectedBeginDate = DateOnly.FromDateTime(_selectedNewBeginDate);
            DateOnly selectedEndDate = DateOnly.FromDateTime(_selectedNewEndDate);

            AccommodationReservationChangeRequest changeRequest = new AccommodationReservationChangeRequest(0, _selectedReservation.Id, selectedBeginDate, selectedEndDate, Model.Enums.AccommodationChangeRequestStatus.WaitingForApproval, "");
            _accommodationReservationChangeRequestService.Save(changeRequest);
            _myChangeRequests.Add(new AccommodationReservationChangeRequestDTO(changeRequest));
            MessageBox.Show("Submitted request");
        }

        private void SubmitRateOwner()
        {
            _selectedReservation.RatingDTO.GuestImages = _images;
            _selectedReservation.RatingDTO.GuestRenovationRating = SelectedRating;
            _accommodationReservationService.Update(_selectedReservation.ToAccommodationReservation());
            MessageBox.Show("Rated Owner successfully!");

        }
        
        private void AddImages()
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) return;

            switch (rb.Content.ToString())
            {
                case "1":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case "2":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Minor wear and tear observed.";
                    break;
                case "3":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Moderate renovation may be required.";
                    break;
                case "4":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Major improvements needed soon.";
                    break;
                case "5":
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Accommodation needs to be renovated ASAP.";
                    break;
                default:
                    GuestReservationsPage.Instance.lblRecommendation.Content = "Unknown rating.";
                    break;
            }
        }

        public ObservableCollection<AccommodationReservationDTO> MyReservationsDTO
        {
            get
            {
                return _myReservations;
            }
            set
            {
                _myReservations = value;
                OnPropertyChanged();
            }
        }

        public bool IsFrameRateVisible
        {
            get
            {
                return _isFrameRateVisible;
            }
            set
            {
                _isFrameRateVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsFrameMyRequestsVisible
        {
            get
            {
                return _isFrameMyRequestsVisible;
            }
            set
            {
                _isFrameMyRequestsVisible = value;
                OnPropertyChanged();
            }
        }

        public AccommodationReservationDTO SelectedReservation
        {
            get
            {
                return _selectedReservation;
            }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
            }

        }

        public DateTime SelectedNewBeginDate
        {
            get
            {
                return _selectedNewBeginDate;
            }
            set
            {
                _selectedNewBeginDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedNewEndDate
        {
            get
            {
                return _selectedNewEndDate;
            }
            set
            {
                _selectedNewEndDate = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CancelReservationCommand
        {
            get
            {
                return _cancelReservationCommand;
            }
            set
            {
                _cancelReservationCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand RateOwnerCommand
        {
            get
            {
                return _rateOwnerCommand;
            }
            set
            {
                _rateOwnerCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SubmitRateOwnerCommand
        {
            get
            {
                return _submitRateOwnerCommand;
            }
            set
            {
                _submitRateOwnerCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SubmitRequestCommand
        {
            get
            {
                return _submitRequestCommand;
            }
            set
            {
                _submitRequestCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddImagesCommand
        {
            get
            {
                return _addImagesCommand;
            }
            set
            {
                _addImagesCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RequestChangeCommand
        {
            get
            {
                return _requestChangeCommand;
            }
            set
            {
                _requestChangeCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RenovationRatingCommand
        {
            get
            {
                return _renovationRatingCommand;
            }
            set
            {
                _renovationRatingCommand = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged(nameof(SelectedRating));
                    UpdateRecommendation();
                }
            }
        }

        private string _recommendationText;
        public string RecommendationText
        {
            get => _recommendationText;
            set
            {
                if (_recommendationText != value)
                {
                    _recommendationText = value;
                    OnPropertyChanged(nameof(RecommendationText));
                }
            }
        }

        private void ChangeRating(object parameter)
        {
            if (parameter is string ratingString && int.TryParse(ratingString, out int rating))
            {
                SelectedRating = rating;
            }
        }

        private void UpdateRecommendation()
        {
            // Update your recommendation logic here based on `SelectedRating`
            switch (SelectedRating)
            {
                case 1:
                    RecommendationText = "All is ok, some minor improvements needed only.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    //GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case 2:
                    RecommendationText = "Minor wear and tear observed.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    //GuestReservationsPage.Instance.lblRecommendation.Content = "All is ok, some minor improvements needed only.";
                    break;
                case 3:
                    RecommendationText = "Moderate renovation may be required.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                case 4:
                    RecommendationText = "Major improvements needed soon.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                case 5:
                    RecommendationText = "Accommodation needs to be renovated ASAP.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
                default:
                    RecommendationText = "Unknown rating.";
                    GuestReservationsPage.Instance.lblRecommendation.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

