using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AnswerRequestPages;
using BookingApp.View.Owner.WizardAndHelp;
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
        private OwnerSettingsService _ownerSettingsService;
        private ObservableCollection<MessageDTO> _messagesDTO;

        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showInboxHelpCommand;

        private MessageDTO _selectedMessageDTO = null;

        private UserDTO _loggedInUser;
        private OwnerSettings _ownerSettings;

        public InboxViewModel(UserDTO loggedInUser)
        {
            _loggedInUser = loggedInUser;

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
            IOwnerSettingsRepository ownerSettingsRepository = Injector.CreateInstance<IOwnerSettingsRepository>();
            _messageService = new MessageService(messageRepository, accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository, tourRepository, tourReservationRepository, touristRepository, tourReviewRepository, voucherRepository);
            _ownerSettingsService = new OwnerSettingsService(ownerSettingsRepository);

            _ownerSettings = _ownerSettingsService.GetOwnerSettingsByOwner(loggedInUser.ToUser());

            List<Message> messages = _messageService.UpdateAndCreateMessages();
            _messageService.SetBestLocationMessage(messages, loggedInUser);
            List<MessageDTO> messagesList = _messageService.GetByOwner(loggedInUser.Id).Select(message => new MessageDTO(message)).ToList();
            _messagesDTO = new ObservableCollection<MessageDTO>(messagesList);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showInboxHelpCommand = new RelayCommand(ShowInboxHelp);
        }

        public OwnerSettings OwnerSettings
        {
            get
            {
                return _ownerSettings;
            }
            set
            {
                _ownerSettings = value;
                OnPropertyChanged();
            }
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
        public RelayCommand ShowInboxHelpCommand
        {
            get
            {
                return _showInboxHelpCommand;
            }
            set
            {
                _showInboxHelpCommand = value;
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
            else 
            {
                OwnerMainWindow.MainFrame.Content = new NewReviewDetailsPage(selectedItem);
                _selectedMessageDTO = null;
            }
        }

        private void ShowInboxHelp()
        {
            OwnerMainWindow.MainFrame.Content = new InboxHelpPage(_loggedInUser);
        }
    }
}
