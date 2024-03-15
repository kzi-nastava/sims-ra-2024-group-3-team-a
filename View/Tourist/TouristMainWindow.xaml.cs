﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }
        private static TourDTO _tourDTO { get; set; }

        private static UserDTO _userDTO { get; set; }

        private readonly TourRepository _tourRepository;
        private static TourReservationRepository _tourReservationRepository { get; set; }
        public int CurrentCapacity { get; set; }
        public TouristMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _tourReservationRepository = new TourReservationRepository();
            Tours = new ObservableCollection<TourDTO>();
            _tourDTO = new TourDTO();
            _userDTO= new UserDTO(user);
            Update();
        }
        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _tourRepository.GetAll()) Tours.Add(new TourDTO(tour));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

            string searchCountryInput = searchCountryTextBox.Text.ToLower();
            string sarchCityInput = searchCityTextBox.Text.ToLower();
            string searchDurationInput = searchDurationTextBox.Text.ToString();
            string searchLanguageInput = languageComboBox.Text.ToLower();
            string searchMaxTouristNumberInput = searchMaxTouristsTextBox.Text.ToLower();
            var filtered = Tours;
            string allParams = sarchCityInput + searchCountryInput + searchDurationInput + searchLanguageInput + searchMaxTouristNumberInput;

            if ((allParams).Length == 0 || string.IsNullOrWhiteSpace(allParams))
            {
                dataGridTour.ItemsSource = Tours;
            }

            filtered = FilterTours(filtered, searchCountryInput, sarchCityInput, searchDurationInput, searchLanguageInput, searchMaxTouristNumberInput);
            dataGridTour.ItemsSource = filtered;
        }

        private ObservableCollection<BookingApp.DTO.TourDTO> FilterTours(ObservableCollection<BookingApp.DTO.TourDTO> tours, string searchCountryInput, string searchCityInput, string searchDurationInput,string searchLanguageInput, string searchMaxTouristNumberInput)
        {
            var filtered = new ObservableCollection<BookingApp.DTO.TourDTO>();

            foreach (var tour in tours)
            {
                if (tour.Duration.ToString().Contains(searchDurationInput)
                    && tour.Language.ToLower().Contains(searchLanguageInput)
                    && tour.LocationDTO.Country.ToLower().Contains(searchCountryInput)
                    && tour.LocationDTO.City.ToLower().Contains(searchCityInput)
                    && tour.MaxTouristNumber.ToString().Contains(searchMaxTouristNumberInput))
                {
                    filtered.Add(tour);
                }
            }

            return filtered;
        }
        
        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            _tourDTO = dataGridTour.SelectedItem as TourDTO;

            if(_tourDTO != null) 
            {
                if(_tourDTO.CurrentCapacity != 0)
                {
                    TourReservationWindow tourReservationWindow = new TourReservationWindow(this,_tourReservationRepository, _tourDTO, _userDTO);
                    tourReservationWindow.ShowDialog();
                }
                else if (_tourDTO.CurrentCapacity == 0)
                {
                    AlternativeToursWindow alternativeToursWidow = new AlternativeToursWindow(this, _tourDTO, _userDTO);
                    alternativeToursWidow.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("You didn't choose any tour!");
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchMaxTouristsTextBox.Text, out int value))
            {
                searchMaxTouristsTextBox.Text = (value + 1).ToString();
            }
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(searchMaxTouristsTextBox.Text, out int value) && value > 0)
            {
                searchMaxTouristsTextBox.Text = (value - 1).ToString();
            }
        }
    }
}
