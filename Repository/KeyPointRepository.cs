using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{

    public class KeyPointRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<KeyPoints> _serializer;

        private List<KeyPoints> _keypoints;

        public KeyPointRepository()
        {
            _serializer = new Serializer<KeyPoints>();
            _keypoints = _serializer.FromCSV(FilePath);
        }

        public List<KeyPoints> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public KeyPoints Save(KeyPoints keyPoints)
        {
            keyPoints.Id = NextId();
            _keypoints = _serializer.FromCSV(FilePath);
            _keypoints.Add(keyPoints);
            _serializer.ToCSV(FilePath, _keypoints);
            return keyPoints;
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

        public void Delete(KeyPoints keyPoints)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            KeyPoints founded =  _keypoints.Find(c => c.Id == keyPoints.Id);
            _keypoints.Remove(founded);
            _serializer.ToCSV(FilePath, _keypoints);
        }

        public KeyPoints Update(KeyPoints keyPoints)
        {
            _keypoints = _serializer.FromCSV(FilePath);
            KeyPoints current = _keypoints.Find(c => c.Id == keyPoints.Id);
            int index = _keypoints.IndexOf(current);
            _keypoints.Remove(current);
            _keypoints.Insert(index, keyPoints);
            _serializer.ToCSV(FilePath, _keypoints);
            return keyPoints;
        }
    }
}
