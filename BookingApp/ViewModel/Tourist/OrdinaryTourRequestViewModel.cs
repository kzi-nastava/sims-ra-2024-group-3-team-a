using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.ViewModel.Tourist
{
    public class OrdinaryTourRequestViewModel : ValidationBase
    {

        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }

        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }

        private UserDTO _userDTO { get; set; }

        private TouristDTO _touristDTO { get; set; }

        private TouristDTO _selectedTouristDTO = null;

        private Languages _selectedLanguage;

        private string comboBoxInput { get; set; }

        private string beginDateInput { get; set; }
        private string endDateInput { get; set; }
        private string dateFormatPattern = @"^\d{4}-\d{2}-\d{2}$";
        public int _complexTourRequestId { get; set; }

        public ObservableCollection<TouristDTO> _touristsDTO;

        private RelayCommand _addTouristCommand;
        private RelayCommand _removeTouristCommand;
        private RelayCommand _confirmTourRequestCommand;
        private RelayCommand _closeWindowCommand;
        public RelayCommand LostFocusCommand { get; private set; }
        public RelayCommand LostFocusBeginDateCommand { get; private set; }
        public RelayCommand LostFocusEndDateCommand { get; private set; }
        private LocationDTO _locationDTO;
        public Action CloseAction { get; set; }

        public OrdinaryTourRequestViewModel(UserDTO loggedInUser, int complexTourRequestId)
        {
            _userDTO = loggedInUser;
            _touristDTO = new TouristDTO();
            _locationDTO = new LocationDTO();
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
            _complexTourRequestId = complexTourRequestId;
            LostFocusCommand = new RelayCommand(OnLostFocus);
            LostFocusBeginDateCommand = new RelayCommand(OnLostFocusBeginDate);
            LostFocusEndDateCommand = new RelayCommand(OnLostFocusEndDate);

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
            Validate12();
            
            if(IsValid)
            {
                _ordinaryTourRequestDTO.TouristsDTO = TouristsDTO.ToList();
                _ordinaryTourRequestDTO.UserId = _userDTO.Id;
                _ordinaryTourRequestDTO.NumberOfTourists = TouristsDTO.Count;
                _ordinaryTourRequestDTO.RequestSentDate = DateTime.Now;
                _ordinaryTourRequestDTO.ComplexTourRequestId = _complexTourRequestId;
                _ordinaryTourRequestService.Save(_ordinaryTourRequestDTO.ToOrdinaryTourRequest());
                MessageBox.Show("made!");
            }
         
            
        }


        public void AddTourist()
        {
            Validate1();
            if(IsValid)
            {
                TouristsDTO.Add(new TouristDTO(_touristDTO));
            }
         
            
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

        protected override void ValidateSelf1()
        {
            if (string.IsNullOrWhiteSpace(_touristDTO.Name))
            {
                ValidationErrors["Name"] = "First name is required.";
            }

            if (string.IsNullOrWhiteSpace(_touristDTO.Surname))
            {
                ValidationErrors["Surname"] = "Last name is required.";
            }

            int age;
            if (!int.TryParse(_touristDTO.Age.ToString(), out age) || age==0)
            {
                ValidationErrors["Age"] = "Enter a valid age number.";
            }
           
           



            OnPropertyChanged(nameof(ValidationErrors));
        }
        protected override void ValidateSelf2()
        {

            if (string.IsNullOrWhiteSpace(_ordinaryTourRequestDTO.LocationDTO.Country))
            {
                ValidationErrors["Country"] = "Contry  is required.";
            }

            if (string.IsNullOrWhiteSpace(_ordinaryTourRequestDTO.LocationDTO.City))
            {
                ValidationErrors["City"] = "City is required.";
            }
           
            if(string.IsNullOrWhiteSpace(_ordinaryTourRequestDTO.BeginDate.ToString()))
            {
                ValidationErrors["BeginDate"] = "Begin date i required.";
            }

            if(_ordinaryTourRequestDTO.BeginDate>_ordinaryTourRequestDTO.EndDate)
            {
                ValidationErrors["BeginDate"] = "Incorrect interval: begin date must come before end date";
            }

            if (string.IsNullOrWhiteSpace(_ordinaryTourRequestDTO.EndDate.ToString()))
            {
                ValidationErrors["EndDate"] = "End date i required.";
            }

            if (_ordinaryTourRequestDTO.BeginDate > _ordinaryTourRequestDTO.EndDate)
            {
                ValidationErrors["EndDate"] = "Incorrect interval: end date must come after before date";
            }
            Languages selectedLanguage;
            bool isValidLanguage = Enum.TryParse(comboBoxInput, out selectedLanguage);

            if (!isValidLanguage || !Languages.Contains(selectedLanguage))
            {
                ValidationErrors["Language"] = "Please select one of the options";
            }
            DateTime beginDate;
            bool isValidBeginDate = Enum.TryParse(endDateInput, out beginDate);
            if (!isValidBeginDate)
            {
                ValidationErrors["BeginDate"] = "BeginDate is not in correct format";
            }
            DateTime endDate;
            bool isValidEndDate = Enum.TryParse(endDateInput, out endDate);
            if (!isValidEndDate)
            {
                ValidationErrors["EndDate"] = "EndDate is not in correct format";
            }


        }


        public void CloseWindow()
        {
            CloseAction();
        }

        private string _userInput;

        public string UserInput
        {
            get { return _userInput; }
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged(nameof(UserInput));
                }
            }
        }

        private string _userBeginDateInput;

        public string UserBeginDateInput
        {
            get { return _userBeginDateInput; }
            set
            {
                if (_userBeginDateInput != value)
                {
                    _userBeginDateInput = value;
                    OnPropertyChanged(nameof(UserBeginDateInput));
                }
            }
        }
        private string _userEndDateInput;

        public string UserEndDateInput
        {
            get { return _userEndDateInput; }
            set
            {
                if (_userEndDateInput != value)
                {
                    _userEndDateInput = value;
                    OnPropertyChanged(nameof(UserEndDateInput));
                }
            }
        }

        private void OnLostFocusBeginDate()
        {
            beginDateInput = _userBeginDateInput;
        }
        private void OnLostFocusEndDate()
        {
            endDateInput = _userEndDateInput;
        }

        private void OnLostFocus()
        {
            comboBoxInput = _userInput;
        }

    }
}
