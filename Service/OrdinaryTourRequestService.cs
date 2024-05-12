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
                return numberOfTourists / acceptedRequestsCount;
            } 
        }

        public List<OrdinaryTourRequest> GetOnWait()
        {
            List<OrdinaryTourRequest> notRejectedTours = new List<OrdinaryTourRequest>();
            foreach (OrdinaryTourRequest request in GetAll())
            {
                if(request.Status == TourRequestStatus.OnWait)
                {
                    notRejectedTours.Add(request);
                }
            }
            return notRejectedTours;
        }
        public int CountByLanguage(string language, List<OrdinaryTourRequest> requests)
        {
            int count = 0;
            foreach(OrdinaryTourRequest request in requests)
            {
                if(request.Language.ToString().ToLower().Contains(language.ToString().ToLower())) { count++; }
            }
            return count;
        }
        public int CountByLocation(string location, List<OrdinaryTourRequest> requests)
        {
            int count = 0;
            foreach (OrdinaryTourRequest request in requests)
            {
                if (request.Place.City == location) { count++; }
            }
            return count;
        }
        public List<OrdinaryTourRequest> GetByYear(int year)
        {
            List<OrdinaryTourRequest> requests = new List<OrdinaryTourRequest>();
            foreach (OrdinaryTourRequest request in GetAll())
            {
                if (request.BeginDate.Year == year)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<OrdinaryTourRequest> GetByMonth(int year, int month)
        {
            List<OrdinaryTourRequest> requests = new List<OrdinaryTourRequest>();
            foreach (OrdinaryTourRequest request in GetByYear(year))
            {
                if (request.BeginDate.Month == month || request.EndDate.Month == month)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }

    }
}
