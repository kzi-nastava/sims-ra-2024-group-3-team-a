using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class FinishedToursViewModel : ViewModel
    {

        private TourService _tourService { get; set; }
        private TourReviewService _tourReviewService { get; set; }

        private ObservableCollection<TourDTO> _finishedTourDTO;

        private TourDTO _selectedTourDTO = null;

        private UserDTO _userDTO;

        private RelayCommand _showTourReviewWindowCommand;
        private RelayCommand _closeWindowCommand;
        private RelayCommand _showTouristMainWindowCommand;
        public Action CloseAction { get; set; }

        public FinishedToursViewModel(UserDTO loggedInUser)
        {

            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _tourReviewService = new TourReviewService(tourReviewRepository);
            _userDTO = loggedInUser;
            List<TourDTO> finishedTours = _tourService.GetFinishedTours().Select(finishedTours => new TourDTO(finishedTours)).ToList();
            _finishedTourDTO = new ObservableCollection<TourDTO>(finishedTours);
            _showTourReviewWindowCommand = new RelayCommand(ShowTourReviewWindow);
            _closeWindowCommand = new RelayCommand(CloseWindow);
            _showTouristMainWindowCommand = new RelayCommand(ShowTouristMainWindow);
        }

        public ObservableCollection<TourDTO> FinishedToursDTO
        {
            get
            {
                return _finishedTourDTO;
            }

            set
            {
                _finishedTourDTO = value;
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

        public RelayCommand ShowTourReviewWindowCommand
        {
            get
            {
                return _showTourReviewWindowCommand;
            }
            set
            {
                _showTourReviewWindowCommand = value;
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
        public RelayCommand ShowTouristMainWindowCommand
        {
            get
            {
                return _showTouristMainWindowCommand;
            }
            set
            {
                _showTouristMainWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowTourReviewWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as TourDTO;

            if(_tourReviewService.IsTourRated(selectedItem.ToTourAllParam(), _userDTO.ToUser()))
            {
                MessageBox.Show("This tour is allready rated!");
            }
            else
            {
                TourReviewWindow tourReviewWindow = new TourReviewWindow(new TourDTO(selectedItem), _userDTO);
                tourReviewWindow.ShowDialog();
            }
            
        }
        public void ShowTouristMainWindow()
        {

            
                TouristMainWindow touristMainWindow = new TouristMainWindow( _userDTO.ToUser());
                  touristMainWindow.ShowDialog();
            

        }

        public void CloseWindow()
        {
            CloseAction();
        }
    }
}
