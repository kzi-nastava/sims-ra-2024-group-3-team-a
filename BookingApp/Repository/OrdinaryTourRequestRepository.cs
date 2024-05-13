using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class OrdinaryTourRequestRepository : IOrdinaryTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/ordinaryTourRequests.csv";

        private readonly Serializer<OrdinaryTourRequest> _serializer;

        private List<OrdinaryTourRequest> _ordinaryTourRequests;

        public OrdinaryTourRequestRepository()
        {
            _serializer = new Serializer<OrdinaryTourRequest>();
            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
        }

        public List<OrdinaryTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OrdinaryTourRequest GetById(int id)
        {

            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
            return _ordinaryTourRequests.FirstOrDefault(u => u.Id == id);

        }

        public OrdinaryTourRequest Save(OrdinaryTourRequest ordinaryTourRequest)
        {
            ordinaryTourRequest.Id = NextId();
            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
            _ordinaryTourRequests.Add(ordinaryTourRequest);
            _serializer.ToCSV(FilePath, _ordinaryTourRequests);
            return ordinaryTourRequest;
        }

        public int NextId()
        {
            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
            if (_ordinaryTourRequests.Count < 1)
            {
                return 1;
            }
            return _ordinaryTourRequests.Max(c => c.Id) + 1;
        }

        public void Delete(OrdinaryTourRequest ordinaryTourRequest)
        {
            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
            OrdinaryTourRequest founded = _ordinaryTourRequests.Find(c => c.Id == ordinaryTourRequest.Id);
            _ordinaryTourRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _ordinaryTourRequests);
        }

        public OrdinaryTourRequest Update(OrdinaryTourRequest ordinaryTourRequest)
        {
            _ordinaryTourRequests = _serializer.FromCSV(FilePath);
            OrdinaryTourRequest current = _ordinaryTourRequests.Find(c => c.Id == ordinaryTourRequest.Id);
            int index = _ordinaryTourRequests.IndexOf(current);
            _ordinaryTourRequests.Remove(current);
            _ordinaryTourRequests.Insert(index, ordinaryTourRequest);
            _serializer.ToCSV(FilePath, _ordinaryTourRequests);
            return ordinaryTourRequest;
        }
    }
}
