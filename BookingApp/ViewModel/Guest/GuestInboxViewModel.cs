using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner.AnswerRequestPages;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest;

namespace BookingApp.ViewModel.Guest
{
    public class GuestInboxViewModel: ViewModel
    {
        private MessageService _messageService;
        private ObservableCollection<MessageDTO> _messagesDTO;

        private RelayCommand _showSideMenuCommand;

        private MessageDTO _selectedMessageDTO = null;

        public GuestInboxViewModel(UserDTO loggedInUser)
        {
            IMessageRepository messageRepository = Injector.CreateInstance<IMessageRepository>();
            IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository = Injector.CreateInstance<IAccommodationReservationChangeRequestRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);

            _messageService.UpdateAndCreateMessages();
            List<MessageDTO> messagesList = _messageService.GetByOwner(loggedInUser.Id).Select(message => new MessageDTO(message)).ToList();
            _messagesDTO = new ObservableCollection<MessageDTO>(messagesList);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
        }

        public ObservableCollection<MessageDTO> MessagesDTO
        {
            get
            {
                return new ObservableCollection<MessageDTO>(_messagesDTO.ToList().OrderBy(c => c.IsRead));
            }
            set
            {
                _messagesDTO = value;
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

        public MessageDTO SelectedMessageDTO
        {
            get
            {
                return _selectedMessageDTO;
            }
            set
            {
                _selectedMessageDTO = value;
                OnPropertyChanged();
                ShowMessageDetailsPage();
            }
        }

        public void ShowSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
        }
        public void CloseSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = null;
        }
        private void ShowMessageDetailsPage()
        {
            
        }
    }
}
