using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;
using System.Windows;
using BookingApp.View.Tourist;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class TourInformationViewModel : ViewModel
    {
        private static TourDTO _tourDTO;
        private static UserDTO _userDTO;
        private static TourReservationService _tourReservationService;
        private RelayCommand _showFinishedToursWindowCommand;
        private KeyPointService _keyPointService;
        private ObservableCollection<KeyPointDTO> _keyPointsDTO;
        private static RelayCommand _showTourReservationWindow;
        private static List<string> images;
        public ObservableCollection<string> imagesCollection;
        private RelayCommand _closeWindowCommand;
        private RelayCommand _showTouristMainWindowCommand;
        private ScrollViewer _scrollViewer;
        public RelayCommand ScrollLeftCommand { get; }
        public RelayCommand ScrollRightCommand { get; }
        public Action CloseAction { get; set; }

        public TourInformationViewModel(TourDTO tourDTO, UserDTO loggedInUser)
        {
           _tourDTO = tourDTO;
           _userDTO = loggedInUser;
           

             IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IKeyPointRepository keyPointRepository = Injector.CreateInstance<IKeyPointRepository>();
       
            
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _keyPointService = new KeyPointService(keyPointRepository);
            List<KeyPointDTO> keypoints = _keyPointService.GetKeyPointsForTour(_tourDTO.ToTourAllParam()).Select(keypoints => new KeyPointDTO(keypoints)).ToList();
            _keyPointsDTO = new ObservableCollection<KeyPointDTO>(keypoints);
            _showTourReservationWindow = new RelayCommand(ShowTourReservationWindow);
            _closeWindowCommand = new RelayCommand(CloseWindow);
           images = _tourDTO.Images;
           imagesCollection = new ObservableCollection<string>(images);
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showTouristMainWindowCommand = new RelayCommand(ShowTouristMainWindow);
            ScrollLeftCommand = new RelayCommand(ScrollLeft);
            ScrollRightCommand = new RelayCommand(ScrollRight);
         
        }
        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ImagesCollection
        {
            get
            {
                return imagesCollection;
            }
            set
            {
                imagesCollection = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<KeyPointDTO> KeyPointsDTO
        {
            get
            {
                return _keyPointsDTO;
            }
            set
            {
                _keyPointsDTO = value;
                OnPropertyChanged();
            }
        }
        public string KeyPointsString
        {
            get { return GetKeyPointsAsString(); }
        }
       
        private string GetKeyPointsAsString()
        {
            // Extract names of keypoints and join them into a single string
            var keyPointNames = KeyPointsDTO.Select(kp => kp.Name.Trim());
            return string.Join(" - ", keyPointNames);
        }


        public RelayCommand ShowFinishedToursWindowCommand
        {
            get
            {
                return _showFinishedToursWindowCommand;
            }
            set
            {
                _showFinishedToursWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowTourReservationWindowCommand
        {
            get
            {
                return _showTourReservationWindow;
            }
            set
            {
                _showTourReservationWindow = value;
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


       
        
        
      
       
      
        private int _focusedImageIndex;
        public int FocusedImageIndex
        {
            get { return _focusedImageIndex; }
            set
            {
                if (_focusedImageIndex != value)
                {
                    _focusedImageIndex = value;
                    OnPropertyChanged(nameof(FocusedImageIndex));
                }
            }
        }
      

        public void SetScrollViewer(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
        }

        private void ScrollRight(object obj)
        {

           _scrollViewer?.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset+150);
        }
        private void ScrollLeft(object obj)
        {

            _scrollViewer?.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset-150);
        }
        public void ShowFinishedToursWindow()
        {
            FinishedToursWindow finishedToursWindow = new FinishedToursWindow(_userDTO);

            finishedToursWindow.ShowDialog();
        }

        public void ShowTourReservationWindow()
        {


            TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationService, _tourDTO, _userDTO);
            tourReservationWindow.Owner = Application.Current.MainWindow;
            tourReservationWindow.ShowDialog();
        }
        public void ShowTouristMainWindow()
        {


            TouristMainWindow touristMainWindow = new TouristMainWindow(_userDTO.ToUser());
            touristMainWindow.ShowDialog();


        }
        public void CloseWindow()
        {
            CloseAction();
        }


    }
}
