using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TourView : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }
        private static TourDTO tour { get; set; }

        private readonly TourRepository _repository;

        public TourView()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            tour= new TourDTO();
            Update();
        }
        private void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _repository.GetAll()) Tours.Add(new TourDTO(tour));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = textboxSearch.Text.ToLower();
            string[] resultArray = searchTerm.Split(',').Select(s => s.Trim()).ToArray();

            if (comboBoxFilter.SelectedItem == null)
            {
                MessageBox.Show("You didn't choose an option for filtration!");
                return;
            }

            string selectedFilter;

            if (comboBoxFilter.SelectedItem == comboBoxItemDuration)
            {
                selectedFilter = comboBoxFilter.SelectedItem.ToString();
                var filtered = Tours.Where(tour => tour.Duration.ToString().Contains(resultArray[0])).ToList();
                dataGridTour.ItemsSource = filtered;
            }
            else if (comboBoxFilter.SelectedItem == comboBoxItemLanguage)
            {
                selectedFilter = comboBoxFilter.SelectedItem.ToString();
                var filtered = Tours.Where(tour => tour.Language.ToLower().Contains(resultArray[0])).ToList();
                dataGridTour.ItemsSource = filtered;
            }
            else if (comboBoxFilter.SelectedItem == comboBoxItemNumberOfPeople)
            {
                
                    selectedFilter = comboBoxFilter.SelectedItem.ToString();
                    var filtered = Tours.Where(tour => tour.MaxTouristNumber.ToString().Contains(resultArray[0])).ToList();
                    dataGridTour.ItemsSource = filtered;
                
                
            }
            else if(comboBoxFilter.SelectedItem == comboBoxItemPlace)
            {
                
                if (resultArray.Length >= 2)
                {
                    string city = resultArray[0];
                    string country = resultArray[1];

                    var filtered = Tours.Where(tour => tour.LocationDTO.City.ToLower() == city && tour.LocationDTO.Country.ToLower() == country).ToList();
                    dataGridTour.ItemsSource = filtered;
                }
                else if (searchTerm == "")
                {
                   
                    dataGridTour.ItemsSource = Tours;
                }
                else
                {
                    MessageBox.Show("U didn't input city and contry in right format!");
                }



            }
            else if (comboBoxFilter.SelectedItem == comboBoxItemBeginningTime)
            { 
                selectedFilter = comboBoxFilter.SelectedItem.ToString();
                var filtered = Tours.Where(tour => tour.BeginingTime.ToString().Contains(resultArray[0])).ToList();
                dataGridTour.ItemsSource = filtered;
            }
            else if(comboBoxFilter.SelectedItem == null)
            {
                MessageBox.Show("U didn't input anything to search!");
            }
            else
            {
                MessageBox.Show("Invalid filter selected!");
            }
          
        }
    }
}
