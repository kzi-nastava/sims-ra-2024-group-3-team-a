﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Owner.AnswerRequestViewModels
{
    public class RejectedMessageViewModel : ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _confirmRejectRequestCommand;

        private string _rejectedMessage;

        private AccommodationReservationChangeRequestService _accommodationReservationChangeRequestService;
        private MessageService _messageService;
        private UserService _userService;

        private AccommodationReservationChangeRequestDTO _accommodationReservationChangeRequestDTO;
        private MessageDTO _messageDTO;

        public RejectedMessageViewModel(AccommodationReservationChangeRequestDTO accommodationReservationChangeRequestDTO, MessageDTO messageDTO)
        {
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _accommodationReservationChangeRequestService = new AccommodationReservationChangeRequestService(accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository);
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);

            _userService = new UserService(userRepository);
            _accommodationReservationChangeRequestDTO = accommodationReservationChangeRequestDTO;
            _messageDTO = messageDTO;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _confirmRejectRequestCommand = new RelayCommand(ConfirmRejectRequest);
        }

        public string RejectedMessage
        {
            get
            {
                return _rejectedMessage;
            }
            set
            {
                _rejectedMessage = value;
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

        public RelayCommand ConfirmRejectRequestCommand
        {
            get
            {
                return _confirmRejectRequestCommand;
            }
            set
            {
                _confirmRejectRequestCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
        private void ConfirmRejectRequest()
        {
            _accommodationReservationChangeRequestDTO.RejectedMessage = _rejectedMessage;
            _accommodationReservationChangeRequestDTO.Status = AccommodationChangeRequestStatus.Rejected;
            _accommodationReservationChangeRequestService.Update(_accommodationReservationChangeRequestDTO.ToAccommodationReservationChangeRequest());
            _messageService.Delete(_messageDTO.ToMessage());
            string sender = _userService.GetById(_messageDTO.RecieverId).Username;
            int receiverId = _userService.GetByUsername(_messageDTO.Sender).Id;
            Message _newRejectedMessage = new Message(0, _accommodationReservationChangeRequestDTO.Id, sender, receiverId, "Rejected Date Change Request", "rejected", MessageType.RejectedChangeRequest, false);
            _messageService.Save(_newRejectedMessage);
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner);
        }

    }
}
