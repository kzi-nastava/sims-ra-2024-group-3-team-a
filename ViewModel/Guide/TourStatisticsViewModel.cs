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
        public static ObservableCollection<TourDTO> _finishedToursDTO { get; set; }
        //private ObservableCollection<TouristDTO> _touristsDTO { get; set; }
        public TourStatisticsViewModel()
        {

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);

            List<TourDTO> toursDTO = _tourService.GetAllFinishedTours().Select(tour => new TourDTO(tour)).ToList();
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
        public TourDTO MostVisitedTourDTO
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
            }
        }
       /* public ObservableCollection<TouristDTO> TouristsDTO
        {
            get { return _touristsDTO; }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }*/
        public ObservableCollection<TourDTO> FinishedToursDTO
        {
            get { return _finishedToursDTO; }
            set
            {
                _finishedToursDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowMostVisitedByYearCommand
        {
            get { return _showMostVisitedByYearCommand; }
            set
            {
                _showMostVisitedByYearCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowMostVisitedByYear(object parameter)
        {
            string date = parameter as string;
            int year = Convert.ToInt32(date);
            MostVisitedTourWindow mostVisitedTour = new MostVisitedTourWindow(year);
            mostVisitedTour.Show();
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
