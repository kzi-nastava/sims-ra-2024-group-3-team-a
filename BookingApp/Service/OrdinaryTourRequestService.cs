using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Core;

namespace BookingApp.Service
{
    public class OrdinaryTourRequestService
    {

        private IOrdinaryTourRequestRepository _ordinaryTourRequestRepository;
        private ITourRepository _tourRepository;
        private IMessageRepository _messageRepository;
        private ITouristRepository _touristRepository;
        private IUserRepository _userRepository;
        private ITourReservationRepository _tourReservationRepository;
        private ITourReviewRepository _tourReviewRepository;
        private IVoucherRepository _voucherRepository;
        private IAccommodationReservationChangeRequestRepository _accommodationReservationChangeRequestRepository;
        private IAccommodationReservationRepository _accommodationReservationRepository;
        private IAccommodationRepository _accommodationRepository;
        private MessageService _messageService;

        public OrdinaryTourRequestService(IAccommodationReservationChangeRequestRepository accommodationReservationChangeRequestRepository, IAccommodationReservationRepository accommodationReservationRepository, IAccommodationRepository accommodationRepository, IOrdinaryTourRequestRepository ordinaryTourRequestRepository, ITourRepository tourRepository, IMessageRepository messageRepository, ITouristRepository touristRepository, IUserRepository userRepository, ITourReservationRepository tourReservationRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository)
        {
            _ordinaryTourRequestRepository = ordinaryTourRequestRepository;
            _tourRepository = tourRepository;
            _touristRepository = touristRepository;
            _userRepository = userRepository;
            _tourReservationRepository = tourReservationRepository;
            _tourReviewRepository = tourReviewRepository;
            _voucherRepository = voucherRepository;
            _accommodationReservationChangeRequestRepository = accommodationReservationChangeRequestRepository;
            _accommodationRepository= accommodationRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _messageService = new MessageService(messageRepository,accommodationReservationChangeRequestRepository, accommodationReservationRepository, accommodationRepository, userRepository,tourRepository, tourReservationRepository, touristRepository,  tourReviewRepository, voucherRepository);
            
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
        public int CountOrdinaryTourRequestsByLocation(int userId, Model.Location location)
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
        public List<Model.Location> GetLocations(int userId)
        {
            List<Model.Location> locationList = new List<Model.Location>();
            foreach (OrdinaryTourRequest ordinaryTourRequest in GetAllForUser(userId))
            {

                locationList.Add(ordinaryTourRequest.Place);

            }
            locationList = locationList.Distinct(new LocationEqualityComparer()).ToList();
            return locationList;
        }
        public class LocationEqualityComparer : IEqualityComparer<Model.Location>
        {
            public bool Equals(Model.Location x, Model.Location y)
            {
                return x.City.Equals(y.City) && x.Country.Equals(y.Country);
            }

            public int GetHashCode(Model.Location obj)
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
        public List<OrdinaryTourRequest> GetForOneYearTime()
        {
            List<OrdinaryTourRequest> requests = new List<OrdinaryTourRequest>();
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            DateTime today = DateTime.Now;
            foreach (OrdinaryTourRequest request in GetAll())
            {
                if (request.RequestSentDate >= oneYearAgo && request.RequestSentDate <= today)
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
        public Languages GetMostWantedLanguage()
        {
            int count = 0;
            Languages mostWanted = Languages.Afrikaans;
            foreach (OrdinaryTourRequest request in GetForOneYearTime())
            {
                if (CountByLanguage(request.Language.ToString(), GetForOneYearTime()) > count)
                {
                    count = CountByLanguage(request.Language.ToString(), GetForOneYearTime());
                    mostWanted = request.Language;
                }
            }
            return mostWanted;
        }
        public Model.Location GetMostWantedLocation()
        {
            int count = 0;
            Model.Location mostWanted = null;
            foreach (OrdinaryTourRequest request in GetForOneYearTime())
            {
                if (CountByLocation(request.Place.City.ToString(), GetForOneYearTime()) > count)
                {
                    count = CountByLocation(request.Place.City.ToString(), GetForOneYearTime());
                    mostWanted = request.Place;
                }
            }
            return mostWanted;
        }
        public void GetCandidatesForMessages(UserDTO userDTO)
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {

                foreach (OrdinaryTourRequest ordinaryTourRequest in GetAllForUser(userDTO.Id))
                {
                    if (ordinaryTourRequest.Status.Equals(TourRequestStatus.Rejected) || ordinaryTourRequest.Status.Equals(TourRequestStatus.Invalid))
                    {
                        if ((ordinaryTourRequest.Language.Equals(tour.Language) || (tour.Place.City == ordinaryTourRequest.Place.City && tour.Place.Country == ordinaryTourRequest.Place.Country)) && tour.MadeFromStatistics)
                        {
                            bool sameLanguage = ordinaryTourRequest.Language.Equals(tour.Language);
                            bool sameLocation = ordinaryTourRequest.Place.City == tour.Place.City && ordinaryTourRequest.Place.Country == tour.Place.Country;
                            _messageService.CreateSystemMessage(userDTO.ToUser(), tour, sameLanguage, sameLocation);
                        }
                    }
                }
            }
        }
        public void FindInvalidOrdinaryTourRequests(UserDTO userDTO)
        {
            DateTime currentTimePlus48Hours = DateTime.Now.AddHours(48);
            foreach(OrdinaryTourRequest ordinaryTour in GetAllForUser(userDTO.Id))
            {
                if (ordinaryTour.BeginDate < currentTimePlus48Hours)
                {
                    ordinaryTour.Status = TourRequestStatus.Invalid;
                    Update(ordinaryTour);
                }
      
            }


            
        }    
    }
}
