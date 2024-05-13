using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class OrdinaryTourRequestViewModel : ViewModel
    {

        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }

        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }

        private UserDTO _userDTO { get; set; }

        private TouristDTO _touristDTO { get; set; }

        private TouristDTO _selectedTouristDTO = null;

        private Languages _selectedLanguage;

        public ObservableCollection<TouristDTO> _touristsDTO;

        private RelayCommand _addTouristCommand;
        private RelayCommand _removeTouristCommand;
        private RelayCommand _confirmTourRequestCommand;
        private RelayCommand _closeWindowCommand;
        public Action CloseAction { get; set; }

        public OrdinaryTourRequestViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _touristDTO = new TouristDTO();
            _ordinaryTourRequestDTO = new OrdinaryTourRequestDTO();
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
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
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _touristsDTO = new ObservableCollection<TouristDTO>();
            _addTouristCommand = new RelayCommand(AddTourist);
            _removeTouristCommand = new RelayCommand(RemoveTourist);
            _confirmTourRequestCommand = new RelayCommand(ConfirmTourRequest);
            _closeWindowCommand = new RelayCommand(CloseWindow);

        }

        public OrdinaryTourRequestDTO OrdinaryTourRequestDTO
        {
            get
            {
                return _ordinaryTourRequestDTO;
            }
            set
            {
                _ordinaryTourRequestDTO = value;
                OnPropertyChanged();
            }
        }

        public UserDTO UserDTO
        {
            get
            {
                return _userDTO;
            }
            set
            {
                _userDTO = value;
                OnPropertyChanged();
            }
        }

        public TouristDTO TouristDTO
        {
            get
            {
                return _touristDTO;
            }
            set
            {
                _touristDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get
            {
                return _touristsDTO;
            }

            set
            {
                _touristsDTO = value;
                OnPropertyChanged();

            }
        }

        public TouristDTO SelectedTouristDTO
        {
            get { return _selectedTouristDTO; }
            set
            {
                _selectedTouristDTO = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddTouristCommand
        {
            get
            {
                return _addTouristCommand;
            }
            set
            {
                _addTouristCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand RemoveTouristCommand
        {
            get
            {
                return _removeTouristCommand;
            }
            set
            {
                _removeTouristCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ConfirmTourRequestCommand
        {
            get
            {
                return _confirmTourRequestCommand;
            }
            set
            {
                _confirmTourRequestCommand = value;
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
        public IEnumerable<Languages> Languages
        {
            get
            {
                return Enum.GetValues(typeof(Languages)).Cast<Languages>();
            }
            set { }
        }

        public Languages SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        public void ConfirmTourRequest()
        {
            _ordinaryTourRequestDTO.TouristsDTO = TouristsDTO.ToList();
            _ordinaryTourRequestDTO.UserId = _userDTO.Id;
            _ordinaryTourRequestDTO.NumberOfTourists = TouristsDTO.Count;
            _ordinaryTourRequestDTO.RequestSentDate = DateTime.Now;
            _ordinaryTourRequestService.Save(_ordinaryTourRequestDTO.ToOrdinaryTourRequest());
            MessageBox.Show("made!");
            
        }


        public void AddTourist()
        {
           TouristsDTO.Add(new TouristDTO(_touristDTO));
            
        }
        public void RemoveTourist()
        {
            if (_selectedTouristDTO == null)
            {
                return;
            }
            var selectedItem = _selectedTouristDTO as TouristDTO;
            TouristsDTO.Remove(selectedItem);
          
        }
        public void CloseWindow()
        {
            CloseAction();
        }
    }
}
