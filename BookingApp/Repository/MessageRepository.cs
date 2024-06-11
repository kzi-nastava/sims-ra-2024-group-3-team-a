using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace BookingApp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private const string FilePath = "../../../Resources/Data/messages.csv";

        private readonly Serializer<Message> _serializer;

        private List<Message> _messages;  

        public MessageRepository()
        {
            _serializer = new Serializer<Message>();
            _messages = _serializer.FromCSV(FilePath);
        }

        public List<Message> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Message Save(Message message)
        {
            message.Id = NextId();
            _messages = _serializer.FromCSV(FilePath);
            _messages.Add(message);
            _serializer.ToCSV(FilePath, _messages);
            return message;
        }

        private int NextId()
        {
            _messages = _serializer.FromCSV(FilePath);
            if (_messages.Count < 1)
            {
                return 1;
            }
            return _messages.Max(c => c.Id) + 1;
        }

        public void Delete(Message message)
        {
            _messages = _serializer.FromCSV(FilePath);
            Message founded = _messages.Find(c => c.Id == message.Id);
            _messages.Remove(founded);
            _serializer.ToCSV(FilePath, _messages);
        }

        public Message Update(Message message)
        {
            _messages = _serializer.FromCSV(FilePath);
            Message current = _messages.Find(c => c.Id == message.Id);
            current.RequestId = message.RequestId;
            current.Sender = message.Sender;
            current.Header = message.Header;
            current.Content = message.Content;
            current.Type = message.Type;
            current.IsRead = message.IsRead;
            _serializer.ToCSV(FilePath, _messages);
            return current;
        }
    }
}
