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
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        private readonly AccommodationRepository _repository;


        public GuestMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<AccommodationDTO>();

            Update();
        }

        private void Update()
        {
            Accommodations.Clear();
            foreach (var accommodation in _repository.GetAll())
            {
                Accommodations.Add(new AccommodationDTO(accommodation));
            }
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            Update();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = textboxSearch.Text.ToLower();
            string[] resultArray = searchInput.Split(',').Select(s => s.Trim()).ToArray();

            var filtered = Accommodations;

            if (resultArray.Length == 0 || string.IsNullOrWhiteSpace(searchInput))
            {
                dataGridAccommodation.ItemsSource = Accommodations;
            }

            foreach (string input in resultArray)
            {
                string value = input;

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                filtered = FilterAccommodations(filtered, value);
            }

            dataGridAccommodation.ItemsSource = filtered;
        }

        private ObservableCollection<BookingApp.DTO.AccommodationDTO> FilterAccommodations(ObservableCollection<BookingApp.DTO.AccommodationDTO> accommodations, string value)
        {
            var filtered = new ObservableCollection<BookingApp.DTO.AccommodationDTO>();

            foreach (var accommodation in accommodations)
            {
                int result;

                if (int.TryParse(value, out result))
                {
                    if (
                         accommodation.Capacity >= result
    
                        || accommodation.MinDaysReservation <= result)
                    {
                        filtered.Add(accommodation);
                    }
                }
                else
                {
                    if (accommodation.Name.ToLower().Contains(value)
                        || accommodation.Type.ToString().ToLower().Contains(value) 
                        || (accommodation.PlaceDTO.City.ToLower() + ", " + accommodation.PlaceDTO.Country.ToLower()).Contains(value))
                       
                    {
                        filtered.Add(accommodation);
                    }
                }
            }

            return filtered;
        }

        private void MakeReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridAccommodation.SelectedItem != null && frameMain.Content == null)
            {
                    frameMain.Content = new MakeAccommodationReservationPage((AccommodationDTO)dataGridAccommodation.SelectedItem as AccommodationDTO);
                
            }
            else if(frameMain.Content == null)
            {
                MessageBox.Show("Accommodation not selected");
                //AccommodationDTO acc = (AccommodationDTO)dataGridAccommodation.SelectedItem;
                
            }
            else
            {
                frameMain.Content = null;
            }
        }
    }
}
