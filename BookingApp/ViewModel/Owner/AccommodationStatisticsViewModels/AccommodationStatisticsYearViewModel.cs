﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Reports.Owner;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AccommodationRenovationPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.ViewModel.Owner.AccommodationStatisticsViewModels
{
    public class AccommodationStatisticsYearViewModel : ViewModel
    {
        private AccommodationStatisticsService _accommodationStatisticsService;
        private AccommodationDTO _accommodationDTO;
        private Dictionary<int, AccommodationStatisticsDTO> _accommodationStatisticsDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showAddAccommodationRenovationPageCommand;
        private RelayCommand _generateReportCommand;
        private RelayCommand _nextImageCommand;
        private RelayCommand _previousImageCommand;
        
        private int[] _years = { 2022, 2023, 2024, 2025 };
        private int _selectedYear;
        private int _mostOccupiedYear;

        private List<string> _images;
        private string _selectedImage;

        public AccommodationStatisticsYearViewModel(AccommodationDTO accommodationDTO)
        {
            IAccommodationReservationRepository accommodationResrvationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            _accommodationStatisticsService = new AccommodationStatisticsService(accommodationResrvationRepository, accommodationReservationChangeRequestRepository);

            _accommodationDTO = accommodationDTO;
            _accommodationStatisticsDTO = new Dictionary<int, AccommodationStatisticsDTO>();

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showAddAccommodationRenovationPageCommand = new RelayCommand(ShowAddAccommodationRenovationPage);
            _generateReportCommand = new RelayCommand(GenerateReport);
            _nextImageCommand = new RelayCommand(NextImage);
            _previousImageCommand = new RelayCommand(PreviousImage);

            _images = accommodationDTO.Images;
            _selectedImage = _images[0];

            _mostOccupiedYear = _accommodationStatisticsService.GetMostOccupiedYear(_accommodationDTO.Id, _years);
            SetStatistics();
        }

        public string SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }
        public int MostOccupiedYear
        {
            get
            {
                return _mostOccupiedYear;
            }
            set
            {
                _mostOccupiedYear = value;
                OnPropertyChanged();
            }
        }
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                ShowAccommodationStatisticsMonths();
                _selectedYear = 0;
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
            }
        }


        public Dictionary<int, AccommodationStatisticsDTO> AccommodationStatisticsDTO
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
        public RelayCommand GenerateReportCommand
        {
            get
            {
                return _generateReportCommand;
            }
            set
            {
                _generateReportCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand NextImageCommand
        {
            get
            {
                return _nextImageCommand;
            }
            set
            {
                _nextImageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand PreviousImageCommand
        {
            get
            {
                return _previousImageCommand;
            }
            set
            {
                _previousImageCommand = value;
                OnPropertyChanged();
            }
        }

        private void SetStatistics()
        {
            foreach (int year in _years)
            {
                AccommodationStatisticsDTO accommodationStatisticsDTO = new AccommodationStatisticsDTO();
                accommodationStatisticsDTO.Reservations = _accommodationStatisticsService.GetReservationsNumber(_accommodationDTO.Id, year);
                accommodationStatisticsDTO.Cancellations = _accommodationStatisticsService.GetCancellationsNumber(_accommodationDTO.Id, year);
                accommodationStatisticsDTO.AccommodationRenovationRecommendations = _accommodationStatisticsService.GetRenovationRecommendationsNumber(_accommodationDTO.Id, year);
                accommodationStatisticsDTO.AccommodationReservationChanges = _accommodationStatisticsService.GetAccommodationReservationChangeRequestsNumber(_accommodationDTO.Id, year);

                if (year == _mostOccupiedYear)
                {
                    accommodationStatisticsDTO.IsMostOccupied = true;
                }

                _accommodationStatisticsDTO.Add(year, accommodationStatisticsDTO);
            }
        }

        private void GenerateReport()
        {
            AccommodationStatisticsReport accommodationStatisticsReport = new AccommodationStatisticsReport();
            accommodationStatisticsReport.GenerateReport(_accommodationStatisticsDTO, _accommodationDTO);
        }

        public void ShowAccommodationStatisticsMonths()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationStatisticsMonthsPage(_accommodationDTO, _selectedYear);
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
        
        private void NextImage()
        {
            int index = _images.IndexOf(_selectedImage);
            if (index == _images.Count - 1)
            {
                SelectedImage = _images[0];
            }
            else
            {
                SelectedImage = _images[index + 1];
            }
        }

        private void PreviousImage()
        {
            int index = _images.IndexOf(_selectedImage);
            if (index == 0)
            {
                SelectedImage = _images[_images.Count - 1];
            }
            else
            {
                SelectedImage = _images[index - 1];
            }
        }
    }
}
