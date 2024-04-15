using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IKeyPointsRepository
    {
        public List<KeyPoints> GetAll();

        public KeyPoints Save(KeyPoints keyPoints);

        public int NextId();

        public void Delete(KeyPoints keyPoints);

        public KeyPoints Update(KeyPoints keyPoints);

        public KeyPoints GetById(int id);
    }
}
