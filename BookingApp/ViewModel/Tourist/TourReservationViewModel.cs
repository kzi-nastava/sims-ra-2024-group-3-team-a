using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
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
        public Action CloseAction { get; set; }
        public TourReservationViewModel(TourReservationService tourReservationService,TourDTO tourDTO, UserDTO loggedInUser)
        {
            _tourDTO = tourDTO;
            _userDTO = loggedInUser;
            _touristDTO = new TouristDTO();
         /*   _touristDTO.PropertyChanged += (sender, e) =>
            {
               
                    Validate1();
                
            };*/
            _tourReservationDTO = new TourReservationDTO();
            voucher = new Voucher();
           // _keyPoints = new List<KeyPointDTO>();
          //  List<KeyPointDTO> keypoints = 

            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();

            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _voucherService = new VoucherService(voucherRepository);
           
         
         

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
            _touristsDTO.Add(new TouristDTO("Milena","Mima", 22));
            _showTouristMainWindowCommand = new RelayCommand(ShowTouristMainWindow);
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


        public void AddTourist()
        {

            IsAddButtonEnabled = _tourDTO.CurrentCapacity > 0;
                 Validate1();
                if (IsValid)
                {
                _touristsDTO.Add(new TouristDTO(_touristDTO));
                _tourDTO.CurrentCapacity--;

               
                }

               // _touristDTO.Age = 0;   
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
                MessageBox.Show("Voucher allready used!");
            }
            else
            {
                _voucherDTO.IsUsed = true;
                voucher = _voucherService.GetById(_voucherDTO.Id);
                voucher.TourId = _tourDTO.Id;
                _voucherService.Update(voucher);
                MessageBox.Show("Voucher used!");

            }
           
        }
        public void ConfirmTourReservation()
        {
            _tourReservationDTO.TouristsDTO = _touristsDTO.ToList();
            _tourReservationDTO.TourId = _tourDTO.Id;
            _tourReservationDTO.UserId = _userDTO.Id;
            _tourReservationDTO.NumberOfTourists = _tourReservationDTO.TouristsDTO.Count();

            _tourReservationService.Save(_tourReservationDTO.ToTourReservation());

            if(_voucherDTO != null)
            {
                _voucherService.Delete(voucher);
            }

            MessageBox.Show("Successfully added reservation!");
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
           

            OnPropertyChanged(nameof(ValidationErrors));
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Validate1();
        }
        public void CloseWindow()
        {
            CloseAction();
        }
    }
}
