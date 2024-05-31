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
    public class ComplexTourRequestRepository:IComplexTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/complexTourRequests.csv";

        private readonly Serializer<ComplexTourRequest> _serializer;

        private List<ComplexTourRequest> _complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequest>();
            _complexTourRequests = _serializer.FromCSV(FilePath);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourRequest GetById(int id)
        {

            _complexTourRequests = _serializer.FromCSV(FilePath);
            return _complexTourRequests.FirstOrDefault(u => u.Id == id);

        }

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
        {
            complexTourRequest.Id = NextId();
            _complexTourRequests = _serializer.FromCSV(FilePath);
            _complexTourRequests.Add(complexTourRequest);
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return complexTourRequest;
        }

        public int NextId()
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            if (_complexTourRequests.Count < 1)
            {
                return 1;
            }
            return _complexTourRequests.Max(c => c.Id) + 1;
        }

        public void Delete(ComplexTourRequest complexTourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest founded = _complexTourRequests.Find(c => c.Id == complexTourRequest.Id);
            _complexTourRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _complexTourRequests);
        }

        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest current = _complexTourRequests.Find(c => c.Id == complexTourRequest.Id);
            int index = _complexTourRequests.IndexOf(current);
            _complexTourRequests.Remove(current);
            _complexTourRequests.Insert(index, complexTourRequest);
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return complexTourRequest;
        }
    }
}
