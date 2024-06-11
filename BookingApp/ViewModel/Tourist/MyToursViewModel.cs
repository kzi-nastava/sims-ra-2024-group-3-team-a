using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.ViewModel.Tourist
{
    public class MyToursViewModel: ViewModel
    {
        private TourService _tourService {  get; set; }

        private ObservableCollection<TourDTO> _activeTourDTO;

        private ObservableCollection<TourDTO> _unactiveTourDTO;

        private TourDTO _selectedTourDTO = null;

      
        private UserDTO _userDTO;
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;
        private RelayCommand _showTrackTourWindowCommand;
        public MyToursViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            List<TourDTO> activeTours = _tourService.GetActiveToursForUser(loggedInUser.Id).Select(activeTours => new TourDTO(activeTours)).ToList();
            List<TourDTO> unactiveTours = _tourService.GetUnactiveToursForUser(loggedInUser.Id).Select(unactiveTours => new TourDTO(unactiveTours)).ToList();
            _activeTourDTO = new ObservableCollection<TourDTO>(activeTours);
            _unactiveTourDTO = new ObservableCollection<TourDTO>(unactiveTours);
            _closeWindowCommand = new RelayCommand(CloseWindow);
            _showTrackTourWindowCommand = new RelayCommand(ShowTourTrackingWindow);
        }
        public ObservableCollection<TourDTO> ActiveToursDTO
        {
            get 
            {
                return _activeTourDTO;
            }

            set
            {
                _activeTourDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TourDTO> UnactiveToursDTO
        {
            get
            {
                return _unactiveTourDTO;
            }

            set
            {
                _unactiveTourDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand;
            }
            set
            {
                _closeWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowTourTrackingWindowCommand
        {
            get
            {
                return _showTrackTourWindowCommand;
            }
            set
            {
                _showTrackTourWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public TourDTO SelectedTourDTO
        {
            get
            {
                return _selectedTourDTO;
            }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
               
            }
        }
        public void ShowTourTrackingWindow(object parameter)
        {
            var selectedItem = parameter as TourDTO;
            if (selectedItem == null)
            {
                return;
            }

            
            TrackTourWindow trackTourWindow = new TrackTourWindow(new TourDTO(selectedItem));
            trackTourWindow.ShowDialog();
        }

        public void CloseWindow()
        {


            CloseAction();
        }


    }
}
