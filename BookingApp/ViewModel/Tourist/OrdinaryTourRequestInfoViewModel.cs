using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class OrdinaryTourRequestInfoViewModel : ViewModel
    {

      

        private OrdinaryTourRequestDTO _ordinaryTourRequestDTO { get; set; }
        private UserDTO _userDTO { get; set; }
        private UserService _userService { get; set; }
        private User user { get; set; }
        
        public ObservableCollection<TouristDTO> _touristsDTO;
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;
        public OrdinaryTourRequestInfoViewModel(OrdinaryTourRequestDTO ordinaryTourRequestDTO)
        {
            
            _ordinaryTourRequestDTO = new OrdinaryTourRequestDTO(ordinaryTourRequestDTO);
            _touristsDTO = new ObservableCollection<TouristDTO>(ordinaryTourRequestDTO.TouristsDTO);
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _userService = new UserService(userRepository);
            _ordinaryTourRequestDTO = ordinaryTourRequestDTO;
            if (_ordinaryTourRequestDTO.Status.Equals(TourRequestStatus.Accepted)) 
            {
                _ordinaryTourRequestDTO.GuideId = 5;
                _userDTO = new UserDTO();
                user = _userService.GetById(ordinaryTourRequestDTO.GuideId);
                _userDTO = new UserDTO(user);
            }
            else
            {
                _userDTO = new UserDTO();
            }
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

        public UserDTO  UserDTO
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
        public void CloseWindow()
        {


            CloseAction();
        }



    }
}
