﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    class TourDetailsViewModel : ViewModel
    {
        private TourDTO _tourDTO;
        private TourService _tourService;
        private KeyPointService _keyPointService;
        private RelayCommand _showMainWindowCommand;
        private RelayCommand _showTourStatisticsCommand;
        private RelayCommand _logoutCommand;
        private RelayCommand _showAllToursCommand;
        private UserDTO _loggedGuide;
        private string _points;

        public TourDetailsViewModel(TourDTO tourDTO, UserDTO guide)
        {
            _tourDTO = tourDTO;
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _keyPointService = new KeyPointService(keyPointsRepository);
            List<KeyPointDTO> keypointsDTO = _keyPointService.GetKeyPointsForTour(tourDTO.ToTourAllParam()).Select(k => new KeyPointDTO(k)).ToList();
            _points = "";
            foreach (var keypoint in keypointsDTO)
            {
                _points += keypoint.Name + ", ";
            }
            _showAllToursCommand = new RelayCommand(ShowAllTours);
            _logoutCommand = new RelayCommand(Logout);
            _showMainWindowCommand = new RelayCommand(ShowMainWindow);
            _points = _points.Remove(_points.Length - 2);
        }
        public string Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }
        public TourDTO Tour
        {
            get { return _tourDTO; }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
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
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow();
            tourStatistics.Show();
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
            TourDetailsWindow.GetInstance().Close();
        }
        public RelayCommand ShowAllToursCommand
        {
            get { return _showAllToursCommand; }
            set
            {
                _showAllToursCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowAllTours()
        {
            AllToursWindow allToursView = new AllToursWindow(_loggedGuide);

            allToursView.Show();
            TourDetailsWindow.GetInstance().Close();
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
            TourDetailsWindow.GetInstance().Close();
        }
    }
}