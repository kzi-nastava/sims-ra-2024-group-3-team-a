using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    class TourStatisticsViewModel : ViewModel
    {
        
        private TourDTO _mostVisitedTourDTO;
        private TourDTO _selectedTourDTO = null;
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private RelayCommand _showTouristsStatistcsCommand;
        private RelayCommand _showMostVisitedByYearCommand;
        private int _chosenYear;

        public static ObservableCollection<TourDTO> _finishedToursDTO { get; set; }

        public TourStatisticsViewModel(UserDTO user)
        {

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
             years = new List<string>
             {
            "ukupno", "2024", "2023", "2022", "2021", "2020",
            "2019", "2018", "2017", "2016", "2015",
            "2014", "2013", "2012", "2011", "2010",
            "2009", "2008", "2007", "2006", "2005",
            "2004", "2003", "2002", "2001", "2000"
            };
            chosenYear = "ukupno";

            List<TourDTO> toursDTO = _tourService.GetAllFinishedTours(user.ToUser()).Select(tour => new TourDTO(tour)).ToList();
            _finishedToursDTO = new ObservableCollection<TourDTO>(toursDTO);
            if (_tourService.GetMostVisitedTour() != null)
            {
                _mostVisitedTourDTO = new TourDTO(_tourService.GetMostVisitedTour());
            }
            else
            {
                _mostVisitedTourDTO = null;
            }
            _showTouristsStatistcsCommand = new RelayCommand(ShowTouristStatistics);
            _showMostVisitedByYearCommand = new RelayCommand(ShowMostVisitedByYear);
        }
        public TourDTO SelectedTourDTO
        {
            get { return _selectedTourDTO; }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
            }
        }
        private List<string> years;
        public List<string> Years
        {
            get { return years; }
            set
            {
                years = value;
                OnPropertyChanged();
            }
        }
        private string chosenYear;
        public string ChosenYear
        {
            get { return chosenYear; }
            set
            {
                if (chosenYear != value)
                {
                    chosenYear = value;
                    OnPropertyChanged();
                    ShowMostVisitedByYear();
                }
              
            }
        }

        public TourDTO MostVisitedTourDTO
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TourDTO> FinishedToursDTO
        {
            get { return _finishedToursDTO; }
            set
            {
                _finishedToursDTO = value;
                OnPropertyChanged();
            }
        }
        private void ShowMostVisitedByYear()
        {
            if(chosenYear == "ukupno")
            {
                MostVisitedTourDTO = new TourDTO(_tourService.GetMostVisitedTour());
            }
            else{
                string date = chosenYear;
                int year = Convert.ToInt32(date);
                if (_tourService.GetMostVisitedByYear(year) != null)
                {
                    MostVisitedTourDTO = new TourDTO(_tourService.GetMostVisitedByYear(year));
                }
                else
                {
                    TourDTO tour = new TourDTO();
                    tour.Name = "";
                    tour.LocationDTO.City = "";
                    tour.LocationDTO.Country = "";
                    tour.TouristsPresent = 0;
                    MostVisitedTourDTO = tour;
                    
                }
            }
        }
        public RelayCommand ShowTouristStatisticsCommand
        {
            get { return _showTouristsStatistcsCommand; }
            set
            {
                _showTouristsStatistcsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTouristStatistics()
        {
            if (_selectedTourDTO == null)
            {
                return;
            }
            var selectedTour = _selectedTourDTO as TourDTO;
            if (_tourReservationService.GetJoinedTourists(selectedTour.ToTourAllParam()).Count() == 0)
            {
                MessageBox.Show("There are no joined tourists on this tour", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            TouristStatisticsWindow touristStatisticsWindow = new TouristStatisticsWindow(selectedTour);
            touristStatisticsWindow.Show();
        }
    }
}
