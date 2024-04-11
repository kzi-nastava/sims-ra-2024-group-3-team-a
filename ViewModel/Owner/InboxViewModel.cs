using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
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
        private MessageService _messageSerivce;
        private ObservableCollection<MessageDTO> _messagesDTO;

        private RelayCommand _showSideMenuCommand;

        private MessageDTO _selectedMessageDTO = null;

        public InboxViewModel(User loggedInUser)
        {
            _messageSerivce = new MessageService();
            _messageSerivce.UpdateAndCreateMessages();
            List<MessageDTO> messagesList = _messageSerivce.GetByOwner(loggedInUser.Id).Select(message => new MessageDTO(message)).ToList();
            _messagesDTO = new ObservableCollection<MessageDTO>(messagesList);

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
        }

        public ObservableCollection<MessageDTO> MessagesDTO
        {
            get
            {
                return _messagesDTO;
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
        }
    }
}
