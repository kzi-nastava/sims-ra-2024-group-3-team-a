using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BookingApp.ViewModel.Guide
{
    public class RequestDetailsViewModel : ViewModel
    {
        private OrdinaryTourRequestDTO _requestDTO;
        private UserDTO _userDTO;


        public RequestDetailsViewModel(OrdinaryTourRequestDTO requestDTO, UserDTO user)
        {
            _requestDTO = requestDTO;
            _userDTO = user;
        }

        public OrdinaryTourRequestDTO Request
        {
            get { return _requestDTO; }
            set
            {
                _requestDTO = value;
                OnPropertyChanged();
            }
        }
    }
}
