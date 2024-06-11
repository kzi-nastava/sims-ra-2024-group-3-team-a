using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Reports.Owner;
using BookingApp.Reports.Tourist;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.Validation;
using BookingApp.View;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReservationViewModel: Validation.ValidationBase
    {
        private TourReservationService _tourReservationService;
     
        private TourDTO _tourDTO;
        private TourReservationDTO _tourReservationDTO;
        private UserDTO _userDTO;
        private TouristDTO _touristDTO;
        private VoucherDTO _voucherDTO;
        private Voucher voucher;
        private VoucherService _voucherService;
        private KeyPointService _keyPointService ;
        private UserService _userService ;
        private ObservableCollection<VoucherDTO> _vouchersDTO;
        public  ObservableCollection<TouristDTO> _touristsDTO;
        private TouristDTO _selectedTouristDTO = null;
        private VoucherDTO _selectedVoucherDTO = null;
        private RelayCommand _addTouristCommand;
        private RelayCommand _removeTouristCommand;
        private RelayCommand _confirmTourReservationCommand;
        private RelayCommand _useVoucherCommand;
        private RelayCommand _closeWindowCommand;
        private RelayCommand _showTouristMainWindowCommand;
        private string _currentLanguage;
        private Dictionary<int, TourReservationDTO> _tourReservationDTOForReport;
        public Action CloseAction { get; set; }
        public TourReservationViewModel(TourReservationService tourReservationService,TourDTO tourDTO, UserDTO loggedInUser)
        {
            _tourDTO = tourDTO;
            _userDTO = loggedInUser;
            _touristDTO = new TouristDTO();
            
            _tourReservationDTO = new TourReservationDTO();
            voucher = new Voucher();
           
          

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository keyPointRepository = Injector.CreateInstance<IKeyPointRepository>();
            
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _voucherService = new VoucherService(voucherRepository);
            _tourReservationDTOForReport =  new Dictionary<int, TourReservationDTO>();
            _keyPointService = new KeyPointService(keyPointRepository);
            _userService = new UserService(userRepository);
            _isVoucherUsed = false;



            _voucherService.UpdateHeader();
            _voucherService.UpdateVouchers();
            List<VoucherDTO> vouchers = _voucherService.GetAll().Select(vouchers => new VoucherDTO(vouchers)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>();
            _vouchersDTO = new ObservableCollection<VoucherDTO>(vouchers);
            _addTouristCommand = new RelayCommand(AddTourist);
            _removeTouristCommand = new RelayCommand(RemoveTourist);
            _confirmTourReservationCommand = new RelayCommand(ConfirmTourReservation);
            _useVoucherCommand = new RelayCommand(UseVoucher);
            IsAddButtonEnabled = tourDTO.CurrentCapacity > 0;
            _closeWindowCommand = new RelayCommand(CloseWindow);
            FinUserInfo(_userDTO);
            _showTouristMainWindowCommand = new RelayCommand(ShowTouristMainWindow);
            _exportToPDFCommand = new RelayCommand(GenerateReport);
            var currentLanguage = App.Instance.CurrentLanguage.Name;
            _currentLanguage = currentLanguage;
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
        public TourReservationDTO TourReservationDTO
        {
            get
            {
                return _tourReservationDTO;
            }
            set
            {
                _tourReservationDTO = value;
                OnPropertyChanged();
            }
        }
        public VoucherDTO VoucherDTO
        {
            get
            {
                return _voucherDTO;
            }
            set
            {
                _voucherDTO = value;
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

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get { return _isAddButtonEnabled; }
            set
            {
                _isAddButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool _isVoucherUsed;
        public bool IsVoucherUsed
        {
            get { return _isVoucherUsed; }
            set
            {
                _isVoucherUsed = value;
                OnPropertyChanged();
            }
        }
        private bool _isConfirmButtonEnabled;
        public bool IsConfirmButtonEnabled
        {
            get { return _isConfirmButtonEnabled; }
            set
            {
                _isConfirmButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _exportToPDFCommand;
        public RelayCommand ExportToPDFCommand
        {
            get { return _exportToPDFCommand; }
            set
            {
                _exportToPDFCommand = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<VoucherDTO> VouchersDTO
        {
            get
            {
                return _vouchersDTO;
            }
            set 
            {
                _vouchersDTO = value;
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
        public VoucherDTO SelectedVoucherDTO
        {
            get { return _selectedVoucherDTO; }
            set
            {
                _selectedVoucherDTO = value;
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
        public RelayCommand ComfirmTourReservationCommand
        {
            get
            {
                return _confirmTourReservationCommand;
            }
            set
            {
                _confirmTourReservationCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand UseVoucherCommand
        {
            get
            {
                return _useVoucherCommand;
            }
            set
            {
                _useVoucherCommand = value;
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
        public RelayCommand ExportToPdfFile
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

        public void AddTourist()
        {

            IsAddButtonEnabled = _tourDTO.CurrentCapacity > 0;
                 Validate1();
                if (IsValid)
                {
                _touristsDTO.Add(new TouristDTO(_touristDTO));
                _tourDTO.CurrentCapacity--;

               
                }

                _touristDTO.Age = 0;   
        }
        public void RemoveTourist()
        {
            if(_selectedTouristDTO == null) 
            {
                return;
            }
            var selectedItem = _selectedTouristDTO as TouristDTO;
            _touristsDTO.Remove(selectedItem);
            _tourDTO.CurrentCapacity ++;

            IsAddButtonEnabled = _tourDTO.CurrentCapacity > 0;
        }
        public void UseVoucher()
        {
            _voucherDTO = _selectedVoucherDTO as VoucherDTO;
            _tourReservationDTO.TouristsDTO = _touristsDTO.ToList();

            if (_voucherDTO == null)
            {
                return;
            }

            if(_tourReservationService.IsVoucherUsed(_tourDTO.ToTourAllParam()))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    MessageBox.Show("Voucher allready used!");
                }
                else
                {
                    MessageBox.Show("Vaucer je vec iskoristen!");
                }
            }
            
            else
            {
                _isVoucherUsed = true;
                _voucherDTO.IsUsed = true;
                voucher = _voucherService.GetById(_voucherDTO.Id);
                voucher.TourId = _tourDTO.Id;
                _voucherService.Update(voucher);
                if (_currentLanguage.Equals("en-US"))
                {
                    MessageBox.Show("Voucher used!");
                }
                else
                {
                    MessageBox.Show("Vaucer iskoristen!");
                }
                  

            }
           
        }
        public void ConfirmTourReservation()
        {
           
                if(_touristsDTO.Count!=0)
                {
                    _tourReservationDTO.TouristsDTO = _touristsDTO.ToList();
                    _tourReservationDTO.TourId = _tourDTO.Id;
                    _tourReservationDTO.UserId = _userDTO.Id;
                    _tourReservationDTO.NumberOfTourists = _tourReservationDTO.TouristsDTO.Count();

                _tourReservationService.Save(_tourReservationDTO.ToTourReservation());

                if (_voucherDTO != null)
                {
                    _voucherService.Delete(voucher);
                }
                    if(_currentLanguage.Equals("en-US"))
                    {
                        MessageBox.Show("Successfully added reservation!");
                        CloseWindow();
                    }
                    else
                    {
                        MessageBox.Show("Uspjesno dodata rezervacija!");
                        CloseWindow();
                    }
                    
                }
            else
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    MessageBox.Show("Reservation not added.You didn't add any tourist!");
                    CloseWindow();
                }
                else
                {
                    MessageBox.Show("Rezervacija nije dodata. Niste unijeli ni jednog turistu!");
                    CloseWindow();
                }

            }



        }
        public void ShowTouristMainWindow()
        {


            TouristMainWindow touristMainWindow = new TouristMainWindow(_userDTO.ToUser());
            touristMainWindow.ShowDialog();


        }

        protected override void ValidateSelf1()
        {
            if (string.IsNullOrWhiteSpace(_touristDTO.Name))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Name"] = "First name is required.";
                }
                else
                {
                    ValidationErrors["Name"] = "Ime je obavezno.";
                }

                
            }
            else if (!Regex.IsMatch(_touristDTO.Name, @"^[a-zA-Z]+$"))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Name"] = "Name must contain only letters.";
                }
                else
                {
                    ValidationErrors["Name"] = "Ime moze sadrzati samo slova.";
                }
                

            }

            if (string.IsNullOrWhiteSpace(_touristDTO.Surname))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Surname"] = "Surname is required.";
                }
                else
                {
                    ValidationErrors["Surname"] = "Prezime je obavezno.";
                }
              
            }
            else if (!Regex.IsMatch(_touristDTO.Surname, @"^[a-zA-Z]+$"))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Surname"] = "Surname must contain only letters.";
                }
                else
                {
                    ValidationErrors["Surname"] = "Prezima moze sadrzati samo slova.";
                }

            }
           
            int age;
            if (!int.TryParse(_touristDTO.Age.ToString(), out age) || age<=0)
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Age"] = "Enter a valid age number.";
                }
                else
                {
                    ValidationErrors["Age"] = "Broj godina nije validan";
                }
               
            }
            



            OnPropertyChanged(nameof(ValidationErrors));
        }
        protected override void ValidateSelf2()
        {
            throw new NotImplementedException();
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _touristDTO.PropertyChanged += (sender, e) =>
            {

                Validate1();

            };

        }

        public List<KeyPoint> GetKeyPoints()
        {
            List<KeyPoint > keyPoints = new List<KeyPoint>();
            return _keyPointService.GetKeyPointsForTour(_tourDTO.ToTourAllParam());
        }
        private void GenerateReport()
        {
            List<KeyPoint> keyPoints = new List<KeyPoint>();
            keyPoints = GetKeyPoints();
            TourReservationReport tourReservationReport = new TourReservationReport();
            tourReservationReport.GenerateReport(_tourReservationDTO,_tourDTO, keyPoints, _touristsDTO, _isVoucherUsed);
        }
        public void CloseWindow()
        {
            CloseAction();
        }
        public void FinUserInfo(UserDTO loggedInUser)
        {
            TouristProfile profile = new TouristProfile();
            profile = _userService.GetTouristProfileById(loggedInUser.Id);
            if(profile!=null)
            {
                _touristDTO.Surname = profile.Surname;
                _touristDTO.Name = profile.Name;
                _touristDTO.Age = profile.Age;
                _touristsDTO.Add(new TouristDTO(_touristDTO));
                _tourDTO.CurrentCapacity--;
            }
           
        }
    }
}
