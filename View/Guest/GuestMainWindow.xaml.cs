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
        private UserDTO _userDTO;

        public GuestMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();
            _userDTO = new UserDTO(user);  

            Update();
        }

        private void Update()
        {
            AccommodationsDTO.Clear();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                AccommodationsDTO.Add(new AccommodationDTO(accommodation));
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

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Name.ToString().Contains(searchNameInput)
                    && accommodation.Type.ToString().ToLower().Contains(searchTypeInput)
                    && accommodation.PlaceDTO.Country.ToLower().Contains(searchCountryInput)
                    && accommodation.PlaceDTO.City.ToLower().Contains(searchCityInput)
                    && accommodation.Capacity.ToString().Contains(searchCapacityInput)
                    && accommodation.MinDaysReservation.ToString().Contains(searchMinDaysInput))
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
    }
}
