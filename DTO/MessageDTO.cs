using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class MessageDTO : INotifyPropertyChanged
    {
        public MessageDTO() { }

        public MessageDTO(MessageDTO messageDTO)
        {
            id = messageDTO.Id;
            requestId = messageDTO.RequestId;
            sender = messageDTO.Sender;
            header = messageDTO.Header;
            content = messageDTO.Content;
            type = messageDTO.Type;
            isRead = messageDTO.IsRead;
        }

        public MessageDTO(Message message)
        {
            id = message.Id;
            requestId = message.RequestId;
            sender = message.Sender;
            header = message.Header;
            content = message.Content;
            type = message.Type;
            isRead = message.IsRead;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private int requestId;
        public int RequestId
        {
            get { return requestId; }
            set
            {
                if (value != requestId)
                {
                    requestId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string sender;
        public string Sender
        {
            get { return sender; }
            set
            {
                if (value != sender)
                {
                    sender = value;
                    OnPropertyChanged();
                }
            }
        }

        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                if (value != header)
                {
                    header = value;
                    OnPropertyChanged();
                }
            }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    content = value;
                    OnPropertyChanged();
                }
            }
        }

        private MessageType type;
        public MessageType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isRead;
        public bool IsRead
        {
            get { return isRead; }
            set
            {
                if (value != isRead)
                {
                    isRead = value;
                    OnPropertyChanged();
                }
            }
        }

        public Message ToMessage()
        {
            return new Message(id, requestId, sender, header, content, type, isRead);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
