using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AnswerRequestPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Owner.AnswerRequestViewModels
{
    public class RequestDetailsViewModel : ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _answerRequest;

        private MessageService _messageService;

        private MessageDTO _messageDTO;

        public RequestDetailsViewModel(MessageDTO messageDTO)
        {
            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _answerRequest = new RelayCommand(AnswerRequest);
            _messageDTO = messageDTO;

            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);

            _messageDTO.IsRead = true;
            _messageService.Update(messageDTO.ToMessage());     
        }

        public MessageDTO MessageDTO
        {
            get
            {
                return _messageDTO;
            }
            set
            {
                _messageDTO = value;
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

        public RelayCommand AnswerRequestCommand
        {
            get
            {
                return _answerRequest;
            }
            set
            {
                _answerRequest = value;
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

        private void AnswerRequest()
        {
            OwnerMainWindow.MainFrame.Content = new AnswerRequestPage(_messageDTO);
        }
    }
}
