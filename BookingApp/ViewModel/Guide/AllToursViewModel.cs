using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Commands;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using System.Diagnostics.Metrics;
using BookingApp.View.Guide;

namespace BookingApp.ViewModel.Guide
{
    class AllToursViewModel : ViewModel
    {
        private static ObservableCollection<TourDTO> _allToursDTO { get; set; }
        private static ObservableCollection<TourDTO> _finishedToursDTO { get; set; }
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private TourDTO _selectedTourDTO = null;
        private TourDTO _mostVisitedTourDTO;
        private RelayCommand _showAddTourWindowCommand; 
        private RelayCommand _showTourDetailsCommand; 
        private RelayCommand _showTourStatisticsCommand; 
        private RelayCommand _logoutCommand; 
        private RelayCommand _showMainWindowCommand; 
        private RelayCommand _cancelTourCommand;
        private UserDTO _loggedGuide;
        public AllToursViewModel(UserDTO guide)
        {
            _loggedGuide = guide;
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            List<TourDTO> toursFinishedDTO = _tourService.GetAllFinishedTours(guide.ToUser()).Select(tour => new TourDTO(tour)).ToList();
            List<TourDTO> toursDTO = _tourService.GetUpcoming(guide.ToUser()).Select(tour => new TourDTO(tour)).ToList();
            _allToursDTO = new ObservableCollection<TourDTO>(toursDTO);
            _finishedToursDTO = new ObservableCollection<TourDTO>(toursFinishedDTO);
            _showAddTourWindowCommand = new RelayCommand(ShowAddTourWindow);
            _showTourDetailsCommand = new RelayCommand(ShowTourDetails);
            _showTourStatisticsCommand = new RelayCommand(ShowTourStatistics);
            _cancelTourCommand = new RelayCommand(CancelTour);
            _showMainWindowCommand = new RelayCommand(ShowMainWindow);
            _logoutCommand = new RelayCommand(Logout);
            if (_tourService.GetMostVisitedTour() != null)
            {
                _mostVisitedTourDTO = new TourDTO(_tourService.GetMostVisitedTour());
            }
            else
            {
                _mostVisitedTourDTO = null;
            }
        }
        public RelayCommand ShowTourDetailsCommand
        {
            get { return _showTourDetailsCommand; }
            set
            {
                _showTourDetailsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourDetails(object parameter)
        {
            TourDTO selectedTourDTO = parameter as TourDTO;
            TourDetailsWindow details = new TourDetailsWindow(selectedTourDTO, _loggedGuide);
            details.Show();

        }
        public RelayCommand ShowTourStatisticsCommand
        {
            get { return _showTourStatisticsCommand; }
            set
            {
                _showTourStatisticsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourStatistics()
        {
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow(_loggedGuide);
            tourStatistics.Show();
        }
        public ObservableCollection<TourDTO> AllToursDTO
        {
            get { return _allToursDTO; }
            set
            {
                _allToursDTO = value;
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
        public UserDTO User
        {
            get { return _loggedGuide; }
            set
            {
                _loggedGuide = value;
                OnPropertyChanged();
            }
        }
        public TourDTO MostVisitedTour
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
            }
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
        public RelayCommand ShowAddTourWindowCommand
        {
            get { return _showAddTourWindowCommand; }
            set
            {
                _showAddTourWindowCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowAddTourWindow()
        {
            AddTourWindow addTourWindow = new AddTourWindow(_loggedGuide);
            addTourWindow.Show();
        }
        public RelayCommand ShowMainWindowCommand
        {
            get { return _showMainWindowCommand; }
            set
            {
                _showMainWindowCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowMainWindow()
        {
            GuideMainWindow mainWindow = new GuideMainWindow(_loggedGuide.ToUser());
            mainWindow.Show();
            AllToursWindow.GetInstance().Close();
        }
        public RelayCommand CancelTourCommand
        {
            get { return _cancelTourCommand; }
            set
            {
                _cancelTourCommand = value;
                OnPropertyChanged();
            }
        }
        private void CancelTour()
        {
            if (_selectedTourDTO == null)
            {
                return;
            }
            TourDTO selectedTour = _selectedTourDTO as TourDTO;

            DateTime currentTimePlus48Hours = DateTime.Now.AddHours(48);
            if (selectedTour.BeginingTime > currentTimePlus48Hours)
            {
                _tourReservationService.MakeTourReservationVoucher(selectedTour.ToTourAllParam());
                selectedTour.CurrentKeyPoint = "canceled";
                _tourService.Update(selectedTour.ToTourAllParam());
                _allToursDTO.Remove(selectedTour);
                MessageBox.Show("Tour has been successfully canceled", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Tour cannot be canceled as it is less than 48 hours away from its beginning time. ", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set
            {
                _logoutCommand = value;
                OnPropertyChanged();
            }
        }
        private void Logout()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            AllToursWindow.GetInstance().Close();
        }

    }
}
