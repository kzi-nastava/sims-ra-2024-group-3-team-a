using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class UserService
    {
        private UserRepository _userRepository = new UserRepository();

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}
