﻿using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
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
        private ITouristProfileRepository _touristProfileRepository = Injector.CreateInstance<ITouristProfileRepository>();
        private IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private readonly Serializer<User> _serializer = new Serializer<User>();
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
            List<User> users = new List<User>();
            users = GetAll().ToList();
            if (users.Count > 0)
            {
                foreach (User card in users)
                {
                    if (user.Id == card.Id)
                    {
                        users.Remove(card);
                        _serializer.ToCSV("../../../Resources/Data/users.csv", users);
                        break;
                    }

                }
            }

        }
        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        public List<User> GetUsersWithAccommodationOnLocation(Location location)
        {
            List<User> users = new List<User>();
            List<Accommodation> accommodations = _accommodationRepository.GetAll().Where(a => a.Place.Country == location.Country && a.Place.City == location.City).ToList();

            foreach (var accommodation in accommodations)
            {
                users.Add(_userRepository.GetById(accommodation.OwnerId));
            }

            List<User> uniqueUsers = users.GroupBy(user => user.Id).Select(group => group.First()).ToList();
            return uniqueUsers;
        }
        public TouristProfile GetTouristProfileById(int id)
        {
        
            TouristProfile tourist = new TouristProfile();
            foreach(TouristProfile touristProfile in _touristProfileRepository.GetAll())
            {
                if(touristProfile.UserId == id)
                {
                    return touristProfile;
                }
            }
            return null;
        }
        
    }
}
