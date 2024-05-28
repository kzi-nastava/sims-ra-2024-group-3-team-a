using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IMessageRepository
    {
        public List<Message> GetAll();

        public Message Save(Message message);

        public void Delete(Message message);

        public Message Update(Message message);
    }
}
