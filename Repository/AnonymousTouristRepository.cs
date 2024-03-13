using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AnonymousTouristRepository
    {
        private const string FilePath = "../../../Resources/Data/anonymousTourists.csv";

        private readonly Serializer<AnonymousTourist> _serializer;

        private List<AnonymousTourist> _anonymousTourists;

        public AnonymousTouristRepository()
        {
            _serializer = new Serializer<AnonymousTourist>();
            _anonymousTourists = _serializer.FromCSV(FilePath);
        }

        public List<AnonymousTourist> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AnonymousTourist Save(AnonymousTourist anonymous)
        {
            anonymous.Id = NextId();
            _anonymousTourists = _serializer.FromCSV(FilePath);
            _anonymousTourists.Add(anonymous);
            _serializer.ToCSV(FilePath, _anonymousTourists);
            return anonymous;
        }
       

        public int NextId()
        {
            _anonymousTourists = _serializer.FromCSV(FilePath);
            if (_anonymousTourists.Count < 1)
            {
                return 1;
            }
            return _anonymousTourists.Max(c => c.Id) + 1;
        }


        /*  public void Delete(Tour tour)
          {
              _tours = _serializer.FromCSV(FilePath);
              Tour founded = _tours.Find(c => c.Id == tour.Id);
              _tours.Remove(founded);
              _serializer.ToCSV(FilePath, _tours);
          }
        */
          public AnonymousTourist Update(AnonymousTourist anonymousTourist)
          {
              _anonymousTourists = _serializer.FromCSV(FilePath);
              AnonymousTourist current = _anonymousTourists.Find(c => c.Id == anonymousTourist.Id);
              int index = _anonymousTourists.IndexOf(current);
              _anonymousTourists.Remove(current);
              _anonymousTourists.Insert(index, anonymousTourist);
              _serializer.ToCSV(FilePath, _anonymousTourists);
              return anonymousTourist;
          }
      
    }
}
