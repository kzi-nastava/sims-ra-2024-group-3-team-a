using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class NewReviewDetailsViewModel : ViewModel
    {
        MessageService _messageService;

        MessageDTO _messageDTO;

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;

        public NewReviewDetailsViewModel(MessageDTO messageDTO)
        {
            _messageService = new MessageService();
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
