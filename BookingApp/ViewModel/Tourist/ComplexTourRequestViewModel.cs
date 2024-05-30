using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
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
    public class ComplexTourRequestViewModel:ViewModel
    {

        private OrdinaryTourRequestService _ordinaryTourRequestService { get; set; }

        private ComplexTourRequestService _complexTourRequestService { get; set; }
        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }

        private ComplexTourRequestDTO _complexTourRequestDTO { get; set; }

        private ObservableCollection<OrdinaryTourRequestDTO> _ordinaryTourRequestsDTO;
        private List<OrdinaryTourRequest> ordinaryTourRequestList;
        private List<OrdinaryTourRequestDTO> ordinaryTourRequests { get; set; }
        private UserDTO _userDTO { get; set; }
        private RelayCommand _closeWindowCommand;
        private RelayCommand _openOrdinaryTourRequestWindowCommand;
        private RelayCommand _confirmRequestCommand;
        public Action CloseAction { get; set; }
        private ComplexTourRequest complexTourRequest { get; set; }

        public ComplexTourRequestViewModel(UserDTO loggedInUser)
        {
            _userDTO = loggedInUser;
            _complexTourRequestDTO = new ComplexTourRequestDTO();
            IComplexTourRequestRepository complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
            IOrdinaryTourRequestRepository ordinaryTourRequestRepository = Injector.CreateInstance<IOrdinaryTourRequestRepository>();
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _ordinaryTourRequestService = new OrdinaryTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _complexTourRequestService = new ComplexTourRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, ordinaryTourRequestRepository, tourRepository, messageRepository, touristRepository, userRepository, tourReservationRepository, tourReviewRepository, voucherRepository, complexTourRequestRepository);
            _ordinaryTourRequestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>();
            complexTourRequest =  _complexTourRequestService.Save(_complexTourRequestDTO.ToComplexTourRequest());
            ordinaryTourRequests = new List<OrdinaryTourRequestDTO>();
            ordinaryTourRequestList = new List<OrdinaryTourRequest>();
            _openOrdinaryTourRequestWindowCommand = new RelayCommand(OpenOrdinaryTourRequestWindow);
            _confirmRequestCommand = new RelayCommand(ConfirmRequest);
            _closeWindowCommand = new RelayCommand(CloseWindow);
        }

        public ObservableCollection<OrdinaryTourRequestDTO> OrdinaryTourRequestsDTO
        {
            get
            {
                return _ordinaryTourRequestsDTO;
            }

            set
            {
                _ordinaryTourRequestsDTO = value;
                OnPropertyChanged();

            }
        }
        

        public RelayCommand OpenOrdinaryTourRequestWindowCommand
        {
            get
            {
                return _openOrdinaryTourRequestWindowCommand;
            }
            set
            {
                _openOrdinaryTourRequestWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmRequestCommand
        {
            get
            {
                return _confirmRequestCommand;
            }
            set
            {
                _confirmRequestCommand = value;
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

        public void OpenOrdinaryTourRequestWindow()
        {
           OrdinaryTourRequestWindow ordinaryTourRequestWindow = new OrdinaryTourRequestWindow(_userDTO, complexTourRequest.Id);
           ordinaryTourRequestWindow.ShowDialog();
            loadOrdinaryTourRequests();


        }
        public void loadOrdinaryTourRequests()
        {
            ordinaryTourRequestList = _complexTourRequestService.getOrdinaryTourRequestsForUser(complexTourRequest.Id, _userDTO.Id);
            ordinaryTourRequests = _complexTourRequestService.getOrdinaryTourRequestsForUser(complexTourRequest.Id, _userDTO.Id).Select(ordinaryTourRequests=> new OrdinaryTourRequestDTO(ordinaryTourRequests)).ToList();
            OrdinaryTourRequestsDTO = new ObservableCollection<OrdinaryTourRequestDTO>(ordinaryTourRequests);
        }
        public void ConfirmRequest()
        {
            if(OrdinaryTourRequestsDTO.Count==0)
            {
                MessageBox.Show("Complex tour is made of two or more ordinary tours!");
            }

            MessageBox.Show("Complex tour succesfully created");

        }
        public void CloseWindow()
        {

            foreach (OrdinaryTourRequest ordinaryTourRequest in ordinaryTourRequestList)
            {
                if(ordinaryTourRequest.ComplexTourRequestId == complexTourRequest.Id)
                {
                    _ordinaryTourRequestService.Delete(ordinaryTourRequest);
                }
            }

            _complexTourRequestService.Delete(complexTourRequest);

            CloseAction();
        }
    }
}
