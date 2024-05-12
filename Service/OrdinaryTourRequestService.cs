using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class OrdinaryTourRequestService
    {

        private IOrdinaryTourRequestRepository _ordinaryTourRequestRepository;

        public OrdinaryTourRequestService(IOrdinaryTourRequestRepository ordinaryTourRequestRepository)
        {
            _ordinaryTourRequestRepository = ordinaryTourRequestRepository;
        }

        public List<OrdinaryTourRequest> GetAll()
        {
            return _ordinaryTourRequestRepository.GetAll();
        }

        public OrdinaryTourRequest Save(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            return _ordinaryTourRequestRepository.Save(ordinaryTourRequestRepository);
        }

        public void Delete(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            _ordinaryTourRequestRepository.Delete(ordinaryTourRequestRepository);
        }

        public OrdinaryTourRequest Update(OrdinaryTourRequest ordinaryTourRequestRepository)
        {
            return _ordinaryTourRequestRepository.Update(ordinaryTourRequestRepository);
        }
        public OrdinaryTourRequest GetById(int id)
        {
            return _ordinaryTourRequestRepository.GetById(id);
        }
        public List<OrdinaryTourRequest> GetAllForUser(int userId) 
        {
            return _ordinaryTourRequestRepository.GetAll().Where(ordinaryTourRequest => ordinaryTourRequest.UserId == userId).ToList();
        }
        public int CountAcceptedOrdinaryTourRequests(int userId)
        {
            return GetAllForUser(userId).Count(request => request.Status == TourRequestStatus.Accepted);
        }
        public int CountRejectedOrdinaryTourRequests(int userId)
        {
            return GetAllForUser(userId).Count(request => request.Status == TourRequestStatus.Rejected);
        }
        public int CountAcceptedOrdinaryTourRequestsForSpecificYear(int userId, int year)
        {
            return GetAllForUser(userId)
          .Count(request => request.Status == TourRequestStatus.Accepted &&
                            request.BeginDate.Year == year);
        }
        public int CountRejectedOrdinaryTourRequestsForSpecificYear(int userId, int year)
        {
            return GetAllForUser(userId)
          .Count(request => request.Status == TourRequestStatus.Accepted &&
                            request.BeginDate.Year == year);
        }
        public int CountOrdinaryTourRequestsbyLanguage(int userId,string language)
        {
            return GetAllForUser(userId)
          .Count(request => request.Language.ToString().Equals(language));
        }
        public int CountOrdinaryTourRequestsByLocation(int userId, Location location)
        {
            return GetAllForUser(userId)
                .Count(request => request.Place.City.Equals(location.City) && request.Place.Country.Equals(location.Country));
        }
        public List<string> GetLanguages(int userId)
        {
            List<string> languageList = new List<string>();
           foreach(OrdinaryTourRequest ordinaryTourRequest in GetAllForUser(userId)) 
           {

                languageList.Add(ordinaryTourRequest.Language.ToString());
            
           }
            languageList = languageList.Distinct().ToList();
            return languageList;
        }
        public List<Location> GetLocations(int userId)
        {
            List<Location> locationList = new List<Location>();
            foreach (OrdinaryTourRequest ordinaryTourRequest in GetAllForUser(userId))
            {

                locationList.Add(ordinaryTourRequest.Place);

            }
            locationList = locationList.Distinct(new LocationEqualityComparer()).ToList();
            return locationList;
        }
        public class LocationEqualityComparer : IEqualityComparer<Location>
        {
            public bool Equals(Location x, Location y)
            {
                return x.City.Equals(y.City) && x.Country.Equals(y.Country);
            }

            public int GetHashCode(Location obj)
            {
                return obj.City.GetHashCode() ^ obj.Country.GetHashCode();
            }
        }
        public double CalculateAverageTouristNumber(int userId, int selectedYear)
        {
            if(selectedYear == 0)
            {
                double acceptedRequestsCount = CountAcceptedOrdinaryTourRequests(userId);
                double numberOfTourists =  GetAllForUser(userId).Where(request => request.Status == TourRequestStatus.Accepted).Sum(request => request.NumberOfTourists);
                if(acceptedRequestsCount == 0) { return 0; }
                return numberOfTourists / acceptedRequestsCount;
            }
            else
            {
                double acceptedRequestsCount = CountAcceptedOrdinaryTourRequestsForSpecificYear(userId, selectedYear);
                double numberOfTourists = GetAllForUser(userId).Where(request => request.Status == TourRequestStatus.Accepted && request.BeginDate.Year == selectedYear).Sum(request => request.NumberOfTourists);
                if (acceptedRequestsCount == 0) { return 0; }
                return numberOfTourists / acceptedRequestsCount;
            } 
        }

    }
}
