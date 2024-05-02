using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.AccommodationRenovationViewModels
{
    public class AddAccommodationRenovationViewModel : ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _confirmAccommodationRenovationCommand;

        private AccommodationRenovationService _accommodationRenovationService;
        private AccommodationDTO _accommodationDTO;
        private DateTime _fromDate;
        private DateTime _toDate;
        private int _length;
        private ObservableCollection<DateTime> _availableDates;

        private AccommodationRenovationDTO _accommodationRenovationDTO;

        public AddAccommodationRenovationViewModel(AccommodationDTO accommodationDTO)
        {
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRenovationRepository accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _accommodationRenovationService = new AccommodationRenovationService(accommodationRenovationRepository, accommodationReservationRepository, accommodationRepository);
            _accommodationRenovationDTO = new AccommodationRenovationDTO();
            _accommodationRenovationDTO.AccommodationId = accommodationDTO.Id;

            _accommodationDTO = accommodationDTO;
            _availableDates = new ObservableCollection<DateTime>();

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _confirmAccommodationRenovationCommand = new RelayCommand(ConfirmAccommodationRenovation);
        }

        public AccommodationRenovationDTO AccommodationRenovationDTO
        {
            get
            {
                return _accommodationRenovationDTO;
            }
            set
            {
                _accommodationRenovationDTO = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DateTime> AvailableDates
        {
            get
            {
                return _availableDates;
            }
            set
            {
                _availableDates = value;
                OnPropertyChanged();
            }
        }
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged();
                FindAvailableDates();
            }
        }
        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value;
                OnPropertyChanged();
                FindAvailableDates();
            }
        }
        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                _toDate = value;
                OnPropertyChanged();
                FindAvailableDates();
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
        public RelayCommand ConfirmAccommodationRenovationCommand
        {
            get
            {
                return _confirmAccommodationRenovationCommand;
            }
            set
            {
                _confirmAccommodationRenovationCommand = value;
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

        public void ConfirmAccommodationRenovation()
        {
            _accommodationRenovationDTO.EndDate = _accommodationRenovationDTO.BeginDate.AddDays(_length);
            _accommodationRenovationService.Save(_accommodationRenovationDTO.ToAccommodationRenovation());
            OwnerMainWindow.MainFrame.GoBack();
        }
        public void FindAvailableDates()
        {
            AvailableDates = new ObservableCollection<DateTime>(_accommodationRenovationService.FindAvailableDates(_accommodationDTO.Id, FromDate, ToDate, Length));
        }
        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
    }
}
