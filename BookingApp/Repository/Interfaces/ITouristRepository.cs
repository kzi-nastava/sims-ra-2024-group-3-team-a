using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ITouristRepository
    {
        public List<Tourist> GetAll();

        public Tourist Save(Tourist tourist);

        public int NextId();

        public void Delete(Tourist tourist);

        public Tourist Update(Tourist tourist);

        public Tourist GetById(int id);
    }
}
