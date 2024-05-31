using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AnswerRequestPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class InboxViewModel : ViewModel
    {
        private MessageService _messageService;
        private ObservableCollection<MessageDTO> _messagesDTO;

        private RelayCommand _showSideMenuCommand;

        private MessageDTO _selectedMessageDTO = null;

        public InboxViewModel(UserDTO loggedInUser)
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

            List<Message> messages = _messageService.UpdateAndCreateMessages();
            _messageService.SetBestLocationMessage(messages, loggedInUser);
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
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void ShowMessageDetailsPage()
        {
            if (_selectedMessageDTO == null)
            {
                return;
            }

            var selectedItem = _selectedMessageDTO as MessageDTO;

            if(selectedItem.Type == MessageType.AccommodationChangeRequest)
            {
                OwnerMainWindow.MainFrame.Content = new RequestDetailsPage(selectedItem);
                _selectedMessageDTO = null;
            }
            else if(selectedItem.Type == MessageType.NewReviewNotification)
            {
                OwnerMainWindow.MainFrame.Content = new NewReviewDetailsPage(selectedItem);
                _selectedMessageDTO = null;
            }
            else if (selectedItem.Type == MessageType.ForumOpened)
            {
                OwnerMainWindow.MainFrame.Content = new NewReviewDetailsPage(selectedItem);
                _selectedMessageDTO = null;
            }
            else if (selectedItem.Type == MessageType.GoodLocationReccomendation || selectedItem.Type == MessageType.BadLocationReccomendation)
            {
                OwnerMainWindow.MainFrame.Content = new NewReviewDetailsPage(selectedItem);
                _selectedMessageDTO = null;
            }
        }
    }
}
