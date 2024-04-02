using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }
        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        public static ObservableCollection<AccommodationReservationChangeRequestDTO> _myChangeRequestsDTO {  get; set; }
        private readonly AccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private UserDTO _userDTO;
        public static ObservableCollection<AccommodationReservationDTO> _myReservations { get; set; }
        private List<AccommodationReservationDTO> _myReservationsDTO;
        //private List<AccommodationReservationChangeRequestDTO> _

        public GuestMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();
            _myChangeRequestsDTO = new ObservableCollection<AccommodationReservationChangeRequestDTO>();
            _userDTO = new UserDTO(user);
            _myReservationsDTO = new List<AccommodationReservationDTO>();
            _myReservations = new ObservableCollection<AccommodationReservationDTO>();

            _accommodationReservationChangeRequestRepository = new AccommodationReservationChangeRequestRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> myReservations = _accommodationReservationRepository.GetAllByGuestId(_userDTO.Id);

            foreach (AccommodationReservation accommodationReservation in myReservations)
            {
                _myReservationsDTO.Add(new AccommodationReservationDTO(accommodationReservation));
            }
            foreach (AccommodationReservationDTO accommodationReservationDTO in _myReservationsDTO)
            {
                _myReservations.Add(accommodationReservationDTO);
            }
            dataGridMyRequests.ItemsSource = _myChangeRequestsDTO;
            Update();
        }

        private void Update()
        {
            AccommodationsDTO.Clear();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                AccommodationsDTO.Add(new AccommodationDTO(accommodation));
            }
            _myChangeRequestsDTO.Clear();
            foreach (var request in _accommodationReservationChangeRequestRepository.GetAll())
            {
               foreach (var reservation in _myReservations)
                {
                    if (request.AccommodationReservationId == reservation.Id)
                    {
                        _myChangeRequestsDTO.Add(new AccommodationReservationChangeRequestDTO(request));
                    }
                }
            }

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchNameInput = searchNameTextBox.Text.ToLower();
            string searchCountryInput = searchCountryTextBox.Text.ToLower();
            string searchCityInput = searchCityTextBox.Text.ToLower();
            string searchTypeInput = searchTypeTextBox.Text.ToLower();

            string searchCapacityInput = searchCapacityTextBox.Text.ToLower();
            string searchMinDaysInput = searchMinDaysTextBox.Text.ToLower();

            string allParams = searchCityInput + searchCountryInput + searchNameInput + searchTypeInput + searchCapacityInput + searchMinDaysInput;

            var filtered = AccommodationsDTO;

            if (allParams.Length == 0 || string.IsNullOrWhiteSpace(allParams))
            {
                dataGridAccommodation.ItemsSource = AccommodationsDTO;
            }

            filtered = FilterAccommodations(filtered, searchCountryInput, searchCityInput, searchNameInput, searchTypeInput, searchCapacityInput, searchMinDaysInput);

            dataGridAccommodation.ItemsSource = filtered;
        }

        private ObservableCollection<AccommodationDTO> FilterAccommodations(ObservableCollection<AccommodationDTO> accommodations, string searchCountryInput,string searchCityInput,string searchNameInput,string searchTypeInput,string searchCapacityInput,string searchMinDaysInput )
        {
            var filtered = new ObservableCollection<AccommodationDTO>();

            int? CapacityConvert = string.IsNullOrEmpty(searchCapacityInput) ? (int?)null : int.Parse(searchCapacityInput);
            int? MinDaysConvert = string.IsNullOrEmpty(searchMinDaysInput) ? (int?)null : int.Parse(searchMinDaysInput);

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Name.ToLower().Contains(searchNameInput)
                    && accommodation.Type.ToString().ToLower().Contains(searchTypeInput)
                    && accommodation.PlaceDTO.Country.ToLower().Contains(searchCountryInput)
                    && accommodation.PlaceDTO.City.ToLower().Contains(searchCityInput)
                    && (accommodation.Capacity >= CapacityConvert || CapacityConvert == null )
                    && (accommodation.MinDaysReservation <= MinDaysConvert || MinDaysConvert == null))
                {
                    filtered.Add(accommodation);
                }
            }
            return filtered;
        }

        private void ShowAccommodationReservationPage(object sender, RoutedEventArgs e)
        {
            if (dataGridAccommodation.SelectedItem != null && frameMain.Content == null)
            {
                frameMain.Content = new MakeAccommodationReservationPage((AccommodationDTO)dataGridAccommodation.SelectedItem, _userDTO);
                
            }
            else if(frameMain.Content == null)
            {
                MessageBox.Show("Accommodation not selected");      
            }
            else
            {
                frameMain.Content = null;
            }
        }

        private void ShowGuestReservationsPage(object sender, RoutedEventArgs e)
        {
            
            frameMain.Content = new GuestReservationsPage(_userDTO, _myChangeRequestsDTO);
            frameMyRequests.Visibility = Visibility.Collapsed;
            frameMain.Visibility = Visibility.Visible;

        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchCapacityTextBox.Text, out int value))
            {
                searchCapacityTextBox.Text = (value + 1).ToString();
            }
        }
        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchCapacityTextBox.Text, out int value))
            {
                searchCapacityTextBox.Text = (value - 1).ToString();
            }
        }

        private void IncreaseButtonMinDays_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchMinDaysTextBox.Text, out int value))
            {
                searchMinDaysTextBox.Text = (value + 1).ToString();
            }
        }
        private void DecreaseButtonMinDays_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchMinDaysTextBox.Text, out int value))
            {
                searchMinDaysTextBox.Text = (value - 1).ToString();
            }
        }

        private void ShowMyRequests_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Visibility = Visibility.Collapsed;
            frameMyRequests.Visibility = Visibility.Visible;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            this.Close();
            signInForm.Show();
        }

        private void MyInbox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
