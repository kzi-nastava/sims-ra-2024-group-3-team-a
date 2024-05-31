using BookingApp.Model;
using System.Collections.Generic;

namespace BookingApp.Repository.Interfaces
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public Forum Save(Forum forum);
        public Forum Update(Forum forum);
        public void Delete(Forum forum);
        public Forum GetById(int id);
    }
}