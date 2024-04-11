using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.Model
{
    public class Message : ISerializable
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Sender { get; set; }
        public int RecieverId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public bool IsRead { get; set; }

        public Message() { }

        public Message(int id, int requestId, string sender, int recieverId, string header, string content, MessageType type, bool isRead)
        {
            Id = id;
            RequestId = requestId;
            Sender = sender;
            RecieverId = recieverId;
            Header = header;
            Content = content;
            Type = type;
            IsRead = isRead;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), RequestId.ToString(), Sender, RecieverId.ToString(), Header, Content, Type.ToString(), IsRead.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            RequestId = Convert.ToInt32(values[1]);
            Sender = values[2];
            RecieverId = Convert.ToInt32(values[3]);
            Header = values[4];
            Content = values[5];
            Type = (MessageType)Enum.Parse(typeof(MessageType), values[6]);
            IsRead = Convert.ToBoolean(values[7]);
        }
    }
}
