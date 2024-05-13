using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class SuperGuestRepository: ISuperGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuests.csv";

        private readonly Serializer<SuperGuest> _serializer;

        private List<SuperGuest> _superGuests;

        public List<SuperGuest> GetAll()
        {
            _superGuests = _serializer.FromCSV(FilePath);
            return _superGuests;
        }

        public SuperGuest Save(SuperGuest user)
        {
            user.Id = NextId();
            _superGuests = _serializer.FromCSV(FilePath);
            _superGuests.Add(user);
            _serializer.ToCSV(FilePath, _superGuests);
            return user;
        }
        private int NextId()
        {
            _superGuests = _serializer.FromCSV(FilePath);
            if (_superGuests.Count < 1)
            {
                return 1;
            }
            return _superGuests.Max(c => c.Id) + 1;
        }

        public void Delete(SuperGuest user)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            _superGuests.Remove(user);
            _serializer.ToCSV(FilePath, _superGuests);
        }

        public SuperGuest Update(SuperGuest user)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuest current = _superGuests.Find(c => c.Id == user.Id);
            int index = _superGuests.IndexOf(current);
            _superGuests.Remove(current);
            _superGuests.Insert(index, user);
            _serializer.ToCSV(FilePath, _superGuests);
            return user;
        }

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
            _superGuests = _serializer.FromCSV(FilePath);
        }

        public SuperGuest GetById(int id)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            return _superGuests.FirstOrDefault(u => u.Id == id);
        }
        public SuperGuest GetByUserId(int userId)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            return _superGuests.FirstOrDefault(u => u.UserId == userId);
        }
    }
}

