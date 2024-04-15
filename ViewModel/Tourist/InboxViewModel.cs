using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class InboxViewModel: ViewModel
    {
        private MessageService _messageService;
        private TourService _tourService;
        private ObservableCollection<MessageDTO> _messageDTO;
        private MessageDTO _selectedMessageDTO = null;
        public InboxViewModel(UserDTO loggedInUser)
        {
            
            _messageService = new MessageService();
            _tourService = new TourService();
            List<MessageDTO> messages = _messageService.GetByOwner(loggedInUser.Id).Select(message => new MessageDTO(message)).ToList();
            _messageDTO = new ObservableCollection<MessageDTO>(messages);

        }

        public ObservableCollection<MessageDTO> MessagesDTO 
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

        public MessageDTO SelectedMessageDTO
        {
            get
            {
                return _selectedMessageDTO;
            }
            set
            {
                _selectedMessageDTO= value; 
                OnPropertyChanged();
                ShowMessageDetailsWindow();
            }
        }
        private void ShowMessageDetailsWindow()
        {
            if (_selectedMessageDTO == null)
            {
                return;
            }

            var selectedItem = _selectedMessageDTO as MessageDTO;
            selectedItem.IsRead = true;
            _messageService.Update(selectedItem.ToMessage());

            Tour tour = _tourService.GetById(selectedItem.RequestId);
            TrackTourWindow trackTourWindow = new TrackTourWindow(new TourDTO(tour));

            if (selectedItem.Type.Equals(Model.Enums.MessageType.TourAttendance)) 
            {
                trackTourWindow.ShowDialog();   
            }
        }
    }
}
