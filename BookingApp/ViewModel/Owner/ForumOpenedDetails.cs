using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class ForumOpenedDetails : ViewModel
    {
        MessageService _messageService;

        MessageDTO _messageDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;

        public ForumOpenedDetails(MessageDTO messageDTO)
        {
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

            messageDTO.IsRead = true;
            _messageService.Update(messageDTO.ToMessage());

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _messageDTO = messageDTO;
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
