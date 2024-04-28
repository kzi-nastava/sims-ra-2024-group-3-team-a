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

namespace BookingApp.ViewModel.Tourist
{
    public class TourInformationView : ViewModel
    {
        private static TourDTO _tourDTO;
        private static UserDTO _userDTO;
        private static TourReservationService _tourReservationService;
        private static RelayCommand _showTourReservationWindow;
        private static List<string> images;
        public ObservableCollection<string> imagesCollection;

        public TourInformationView(TourDTO tourDTO, UserDTO loggedInUser)
        {
           _tourDTO = tourDTO;
           _userDTO = loggedInUser;

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _showTourReservationWindow = new RelayCommand(ShowTourReservationWindow);
           images = _tourDTO.Images;
           imagesCollection = new ObservableCollection<string>(images);
            

   
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
        public void ShowTourReservationWindow()
        {


            TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationService, _tourDTO, _userDTO);
            tourReservationWindow.ShowDialog();
        }

    }
}
