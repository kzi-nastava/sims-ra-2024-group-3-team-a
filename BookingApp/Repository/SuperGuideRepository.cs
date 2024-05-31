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
    class SuperGuideRepository : ISuperGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuides.csv";

        private readonly Serializer<SuperGuide> _serializer;

        private List<SuperGuide> _guides;

        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
            _guides = _serializer.FromCSV(FilePath);
        }

        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }


        public SuperGuide Save(SuperGuide superGuide)
        {
            superGuide.Id = NextId();
            _guides = _serializer.FromCSV(FilePath);
            foreach(var guide in _guides)
            {
                if (superGuide.Language == guide.Language && superGuide.GuideId == guide.Id) {
                    return superGuide;
                }
            }
            _guides.Add(
                superGuide);
            _serializer.ToCSV(FilePath, _guides);
            return superGuide;
        }

        public int NextId()
        {
            _guides = _serializer.FromCSV(FilePath);
            if (_guides.Count < 1)
            {
                return 1;
            }
            return _guides.Max(c => c.Id) + 1;
        }

        public void Delete(SuperGuide superGuide)
        {
            _guides = _serializer.FromCSV(FilePath);
            SuperGuide founded = _guides.Find(c => c.Id == superGuide.Id);
            _guides.Remove(founded);
            _serializer.ToCSV(FilePath, _guides);
        }

        public SuperGuide Update(SuperGuide superGuide)
        {
            _guides = _serializer.FromCSV(FilePath);
            SuperGuide current = _guides.Find(c => c.Id == superGuide.Id);
            int index = _guides.IndexOf(current);
            _guides.Remove(current);
            _guides.Insert(index, superGuide);
            _serializer.ToCSV(FilePath, _guides);
            return superGuide;
        }
     
    }
}
