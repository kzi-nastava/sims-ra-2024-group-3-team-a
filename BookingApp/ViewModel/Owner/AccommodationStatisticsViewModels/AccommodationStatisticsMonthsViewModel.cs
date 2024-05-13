using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AccommodationRenovationPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.AccommodationStatisticsViewModels
{
    public class AccommodationStatisticsMonthsViewModel : ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showAddAccommodationRenovationPageCommand;

        private AccommodationDTO _accommodationDTO;
        private int _year;

        private Dictionary<string, AccommodationStatisticsDTO> _accommodationStatisticsDTO;
        private AccommodationStatisticsService _accommodationStatisticsService;
        private int _mostOccupiedMonth;

        private int[] _months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        public AccommodationStatisticsMonthsViewModel(AccommodationDTO accommodationDTO, int year)
        {
            IAccommodationReservationRepository accommodationResrvationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            _accommodationStatisticsService = new AccommodationStatisticsService(accommodationResrvationRepository, accommodationReservationChangeRequestRepository);

            _accommodationDTO = accommodationDTO;
            _year = year;
            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showAddAccommodationRenovationPageCommand = new RelayCommand(ShowAddAccommodationRenovationPage);

            _accommodationStatisticsDTO = new Dictionary<string, AccommodationStatisticsDTO>();

            _mostOccupiedMonth = _accommodationStatisticsService.GetMostOccupiedMonth(_accommodationDTO.Id, _year, _months);
            SetStatistics();
        }

        public Dictionary<string, AccommodationStatisticsDTO> AccommodationStatisticsDTO
        {
            get
            {
                return _accommodationStatisticsDTO;
            }
            set
            {
                _accommodationStatisticsDTO = value;
            }
        }
        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }
        public AccommodationDTO AccommodationDTO
        {
            get
            {
                return _accommodationDTO;
            }
            set
            {
                _accommodationDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand;
            }
            set
            {
                _showSideMenuCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowAddAccommodationRenovationPageCommand
        {
            get
            {
                return _showAddAccommodationRenovationPageCommand;
            }
            set
            {
                _showAddAccommodationRenovationPageCommand = value;
                OnPropertyChanged();
            }
        }

        private void SetStatistics()
        {
            foreach (int month in _months)
            {
                AccommodationStatisticsDTO accommodationStatisticsDTO = new AccommodationStatisticsDTO();
                accommodationStatisticsDTO.Reservations = _accommodationStatisticsService.GetReservationsNumber(_accommodationDTO.Id, _year, month);
                accommodationStatisticsDTO.Cancellations = _accommodationStatisticsService.GetCancellationsNumber(_accommodationDTO.Id, _year, month);
                accommodationStatisticsDTO.AccommodationRenovationRecommendations = _accommodationStatisticsService.GetRenovationRecommendationsNumber(_accommodationDTO.Id, _year, month);
                accommodationStatisticsDTO.AccommodationReservationChanges = _accommodationStatisticsService.GetAccommodationReservationChangeRequestsNumber(_accommodationDTO.Id, _year, month);

                if (month == _mostOccupiedMonth)
                {
                    accommodationStatisticsDTO.IsMostOccupied = true;
                }

                _accommodationStatisticsDTO.Add(ConvertMonthToString(month), accommodationStatisticsDTO);
            }
        }

        private string ConvertMonthToString(int month)
        {
            string monthString = "";

            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "Invalid month number";
            }
        }
        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        private void ShowAddAccommodationRenovationPage()
        {
            OwnerMainWindow.MainFrame.Content = new AddAccommodationRenovationPage(_accommodationDTO);
        }
        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
    }
}
