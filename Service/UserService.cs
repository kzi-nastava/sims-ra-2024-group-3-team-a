using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class UserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User Save(User user)
        {
            return _userRepository.Save(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }
    }
}
