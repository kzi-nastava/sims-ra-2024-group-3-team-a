using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();

        public User Save(User user);

        public void Delete(User user);

        public User Update(User user);

        public User GetByUsername(string username);

        public User GetById(int id);
    }
}
