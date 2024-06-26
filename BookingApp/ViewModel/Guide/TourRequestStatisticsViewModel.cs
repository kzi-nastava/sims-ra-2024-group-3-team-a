﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class TourRequestStatisticsViewModel : ViewModel
    {
        private OrdinaryTourRequestService _tourRequestService;
        private int _number;
        private RelayCommand _searchCommand;
        private RelayCommand _filterCommand;
        private RelayCommand _resetSearchParametersCommand;
        private RelayCommand _resetSearchCommand;
        private RelayCommand _mostWantedLanguageCommand;
        private RelayCommand _mostWantedLocationCommand;
        private List<string> _years;
        private List<string> _months;
        private UserDTO _loggedInGuide;
        public TourRequestStatisticsViewModel(UserDTO user) 
        {
            _loggedInGuide = user;
            IOrdinaryTourRequestRepository requestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();

            _tourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, requestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _searchCommand = new RelayCommand(Search);
            _filterCommand = new RelayCommand(Filter);
            _mostWantedLanguageCommand = new RelayCommand(SuggestMostWantedLanguage);
            _mostWantedLocationCommand = new RelayCommand(SuggestMostWantedLocation);
            _resetSearchParametersCommand = new RelayCommand(ResetSearchParameters);
            _resetSearchCommand = new RelayCommand(ResetSearch);
            _years  = new List<string> { "2028", "2027" , "2026" , "2025" , "2024" , "2023" , "2022" , "2021", "2020" };
            _months = new List<string> {"1","2","3","4","5","6","7","8","9","10","11","12" };

        }
        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Languages> Languages
        {
            get
            {
                return Enum.GetValues(typeof(Languages)).Cast<Languages>();
            }
            set { }
        }
        public List<string> Years
        {
            get
            {
                return _years;
            }
            set { }
        }
        public List<string> Months
        {
            get
            {
                return _months;
            }
            set { }
        }
        public RelayCommand FilterCommand
        {
            get
            {
                return _filterCommand;
            }
            set
            {
                _filterCommand = value;
                OnPropertyChanged();
            }
        }
        private void Filter()
        {
            if(_searchLocationInput == String.Empty && _searchLanguageInput != String.Empty  && _searchMonthInput==String.Empty && _searchYearInput!=String.Empty)
            {
                Number = _tourRequestService.CountByLanguage(_searchLanguageInput, _tourRequestService.GetByYear(Convert.ToInt32( _searchYearInput)));
            }else if (_searchLocationInput != String.Empty && _searchLanguageInput == String.Empty && _searchMonthInput == String.Empty && _searchYearInput != String.Empty)
            {
                Number = _tourRequestService.CountByLocation(_searchLocationInput, _tourRequestService.GetByYear(Convert.ToInt32(_searchYearInput)));
            }else if(_searchLocationInput == String.Empty && _searchLanguageInput != String.Empty && _searchMonthInput != String.Empty && _searchYearInput != String.Empty)
                {
                Number = _tourRequestService.CountByLanguage(_searchLanguageInput, _tourRequestService.GetByMonth(Convert.ToInt32(_searchYearInput), Convert.ToInt32(_searchMonthInput)));
            }
            else if(_searchLocationInput != String.Empty && _searchLanguageInput == String.Empty && _searchMonthInput != String.Empty && _searchYearInput != String.Empty)
            {
                Number = _tourRequestService.CountByLocation(_searchLocationInput, _tourRequestService.GetByMonth(Convert.ToInt32(_searchYearInput), Convert.ToInt32(_searchMonthInput)));
            }
        }
        public RelayCommand MostWantedLanguageCommand
        {
            get { return _mostWantedLanguageCommand; }
            set
            {
                _mostWantedLanguageCommand = value;
                OnPropertyChanged();
            }
        }
        private void SuggestMostWantedLanguage()
        {
            MostWantedLanguageWindow suggestions = new MostWantedLanguageWindow(_loggedInGuide);
            suggestions.Show();
        }
        public RelayCommand MostWantedLocationCommand
        {
            get { return _mostWantedLocationCommand; }
            set
            {
                _mostWantedLocationCommand = value;
                OnPropertyChanged();
            }
        }
        private void SuggestMostWantedLocation()
        {
            MostWantedLocation suggestions = new MostWantedLocation(_loggedInGuide);
            suggestions.Show();
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand;
            }
            set
            {
                _searchCommand = value;
                OnPropertyChanged();
            }
        }
        private void Search()
        {
            if (_searchLocationInput == String.Empty && _searchLanguageInput != String.Empty)
            {
                Number = _tourRequestService.CountByLanguage(_searchLanguageInput, _tourRequestService.GetAll());
            }
            else if (_searchLocationInput != String.Empty && _searchLanguageInput == String.Empty)
            {
                Number = _tourRequestService.CountByLocation(_searchLocationInput, _tourRequestService.GetAll());
            }
        }
        private string _searchLocationInput = String.Empty;
        public string SearchLocationInput
        {
            get
            {
                return _searchLocationInput;
            }
            set
            {
                _searchLocationInput = value;
                OnPropertyChanged();
            }
        }
        private string _searchYearInput = String.Empty;
        public string SearchYearInput
        {
            get
            {
                return _searchYearInput;
            }
            set
            {
                _searchYearInput = value;
                OnPropertyChanged();
            }
        }
        private string _searchMonthInput = String.Empty;
        public string SearchMonthInput
        {
            get
            {
                return _searchMonthInput;
            }
            set
            {
                _searchMonthInput = value;
                OnPropertyChanged();
            }
        }
        private string _searchLanguageInput = String.Empty;
        public string SearchLanguageInput
        {
            get
            {
                return _searchLanguageInput;
            }
            set
            {
                _searchLanguageInput = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ResetSearchParametersCommand
        {

            get { return _resetSearchParametersCommand; }
            set
            {
                _resetSearchParametersCommand = value;
                OnPropertyChanged();
            }
        }
        private void ResetSearchParameters()
        {

            SearchLocationInput = string.Empty;
            TourRequestStatisticsWindow.GetInstance().languageComboBox.SelectedIndex = -1;
            SearchLanguageInput = string.Empty;
            Search();
        }
        public RelayCommand ResetSearchCommand
        {

            get { return _resetSearchCommand; }
            set
            {
                _resetSearchCommand = value;
                OnPropertyChanged();
            }
        }
        private void ResetSearch()
        {
            SearchMonthInput = string.Empty;
            TourRequestStatisticsWindow.GetInstance().monthComboBox.SelectedIndex = -1;
            SearchYearInput = string.Empty;
            TourRequestStatisticsWindow.GetInstance().yearComboBox.SelectedIndex = -1;
            Search();
        }
    }
}
