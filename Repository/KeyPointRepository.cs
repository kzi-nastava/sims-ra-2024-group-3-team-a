using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{

    public class KeyPointRepository : IKeyPointRepository
    {
        private const string FilePath = "../../../Resources/Data/keypoint.csv";

        private readonly Serializer<KeyPoint> _serializer;

        private List<KeyPoint> _keypoints;

        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoint>();
            _keypoints = _serializer.FromCSV(FilePath);
        }

        public List<KeyPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public KeyPoint Save(KeyPoint keyPoint)
        {
            keyPoint.Id = NextId();
            _keypoints = _serializer.FromCSV(FilePath);
            _keypoints.Add(keyPoint);
            _serializer.ToCSV(FilePath, _keypoints);
            return keyPoint;
        }

        public int NextId()
        {
            _keypoints = _serializer.FromCSV(FilePath);
            if (_keypoints.Count < 1)
            {
                return 1;
            }
            return _keypoints.Max(c => c.Id) + 1;
        }

        public void Delete(KeyPoint keyPoint)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            KeyPoint founded = _keypoints.Find(c => c.Id == keyPoint.Id);
            _keypoints.Remove(founded);
            _serializer.ToCSV(FilePath, _keypoints);
        }

        public KeyPoint Update(KeyPoint keyPoint)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            KeyPoint current = _keypoints.Find(c => c.Id == keyPoint.Id);
            int index = _keypoints.IndexOf(current);
            _keypoints.Remove(current);
            _keypoints.Insert(index, keyPoint);
            _serializer.ToCSV(FilePath, _keypoints);
            return keyPoint;
        }

        public KeyPoint GetById(int id)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            return _keypoints.FirstOrDefault(c => c.Id == id);
        }
        public KeyPoint GetByName(string name)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            return _keypoints.FirstOrDefault(c => c.Name == name);
        }
    }
}
