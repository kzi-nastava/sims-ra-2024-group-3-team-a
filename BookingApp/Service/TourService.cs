using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Service
{
    public class TourService
    {
        private ITourRepository _tourRepository;
        private TourReservationService _tourReservationService;
        private MessageRepository _messageRepository;
        private VoucherService _voucherService;

        public TourService(ITourRepository tourRepository, IUserRepository userRepository, ITouristRepository touristRepository, ITourReservationRepository tourReservationRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository) 
        {
            _tourRepository = tourRepository;
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _voucherService = new VoucherService(voucherRepository);
        }

        public List<Tour> GetAll() 
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }
        public List<Tour> GetByGuideId(int id)
        {
            return _tourRepository.GetByGuideId(id);
        }
        public Tour Save(Tour tour) 
        {
            return _tourRepository.Save(tour);
        }
        public void Delete(Tour tour)
        {
             _tourRepository.Delete(tour);
        }
        public Tour Update(Tour tour)
        {
            return _tourRepository.Update(tour);
        }
        public List<Tour> GetActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
                foreach (Tour tour in GetAll())
                {
                    foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                    {
                        if (tour.Id == tourReservation.TourId && tour.IsActive)
                        {
                            activeTours.Add(tour);
                        }
                    }
                }
           return activeTours.Distinct().ToList();
        }
        public List<Tour> GetAllActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.IsActive)
                {
                    activeTours.Add(tour);
                }
            }
            return activeTours;
        }
        public List<Tour> GetUnactiveTours()
        {
            List<Tour> unactiveTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && !tour.IsActive && !tour.CurrentKeyPoint.Equals("finished"))
                    {
                        unactiveTours.Add(tour);
                    }
                }
            }
            return unactiveTours.Distinct().ToList();
        }
        public List<Tour> GetFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                foreach (TourReservation tourReservation in _tourReservationService.GetAll())
                {
                    if (tour.Id == tourReservation.TourId && tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }
                }
            }
            return finishedTours.Distinct().ToList();
        }
        public List<Tour> GetAllFinishedTours(User guide)
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                    if (tour.CurrentKeyPoint.Equals("finished") && tour.GuideId == guide.Id)
                    {
                        finishedTours.Add(tour);
                    }

            }
            return finishedTours;
        }
        public Tour GetMostVisitedTour()
        {
            return _tourRepository.GetMostVisitedTour();
        }
        public Tour GetMostVisitedByYear(int year)
        {
            return _tourRepository.GetMostVisitedByYear(year);
        }
        public List<Tour> GetNotCancelled()
        {
            return _tourRepository.GetNotCancelled();
        }
        public List<Tour> GetUpcoming(User user)
        {
            return _tourRepository.GetUpcoming(user);
        }
        public List<Tour> GetTodayTours(User user)
        {
            List<Tour> toursToday = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                if (tour.BeginingTime.Date == DateTime.Today && tour.GuideId == user.Id)
                {
                    toursToday.Add(tour);
                }
            }
            return toursToday;
        }
        public List<Tour> GetToursWithSameLocation(Tour tour)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                if (t.Place.Country == tour.Place.Country && t.Place.City == tour.Place.City && t.CurrentCapacity != 0)
                {
                    tours.Add(t);
                }
            }
            return tours;
        }

        public List<Tourist> GetTourists(Tour tour)
        {
            List<Tourist> tourists = new List<Tourist>();

            foreach (TourReservation reservation in _tourReservationService.GetAll())
            {
                if (reservation.TourId == tour.Id)
                {
                    foreach (Tourist tourist in reservation.Tourists)
                    {
                        
                        tourists.Add(tourist);
                    }
                }
            }
            return tourists;
        }


         public void FindCandidatesForVoucher(int userId)
         {

             Tour tour = new Tour();
             DateTime tourDate;
             int i;
             List<Tourist> tourists = new List<Tourist> ();
             List<TourReservation> tourReservations = new List<TourReservation>();
             tourReservations = _tourReservationService.GetAllForUser(userId);
             for (i = 0; i < tourReservations.Count;i++ )
             {
                 tourists = tourReservations[i].Tourists;
                 if (tourists[0].JoiningKeyPoint!="")
                 {
                     tour = FindTourForReservation(tourReservations[i]);
                     tourDate = tour.BeginingTime;
                     i=FindOtherCandidatesForVoucher(i, tourDate, userId);



                 }
             }


         }
        public int FindOtherCandidatesForVoucher(int tourReservationIndex, DateTime beginingTime, int userId)
        {
            Tour tour = new Tour();
            DateTime endDate = beginingTime.AddYears(1);
            DateTime tourDate;
            int count = 0;
            List<Tourist> tourists = new List<Tourist>();
            List<TourReservation> tourReservations = new List<TourReservation>();
            tourReservations = _tourReservationService.GetAllForUser(userId);
            int i=0;
            for ( i = tourReservationIndex; i < tourReservations.Count; i++)
                {
                    if(count!=4)
                    {
                        tourists = tourReservations[i].Tourists;
                        if (tourists[0].JoiningKeyPoint != "")
                        {


                            tour = FindTourForReservation(tourReservations[i]);
                            tourDate = tour.BeginingTime;
                            if (tourDate < endDate)
                            {
                                count++;
                            }
                            else
                            {
                                return i;
                            }

                        }
                    }
                    else
                    {
                        MakeVoucher(tour.Id, userId);
                        return i++;
                    }
                }
            MakeVoucher(tour.Id, userId);
            return i++;

        }
        public void MakeVoucher(int tourId, int userId)
        {
            Voucher voucher = new Voucher();
            voucher.Type = Model.Enums.VoucherType.Gift;
            voucher.TourId = tourId;
            voucher.UserId = userId;
            voucher.IsUsed = false;
            voucher.ExpireDate = DateTime.Now.AddMonths(6);
            voucher.Header = "You went to five tours this year!";
            if (!DoesVoucherAllreadyExists(voucher, userId))
            {
                _voucherService.Save(voucher);

            }
          
        }
        public bool DoesVoucherAllreadyExists(Voucher voucher, int userId)
        {
            foreach(Voucher v in _voucherService.GetAllForUser(userId))
            {
                if (v.ExpireDate.Day == voucher.ExpireDate.Day && v.ExpireDate.Month == voucher.ExpireDate.Month && v.ExpireDate.Year==voucher.ExpireDate.Year)
                    return true;
            }
            return false;
        }
        public Tour FindTourForReservation(TourReservation tourReservation)
        {
            Tour tour = new Tour();
            foreach (Tour t in GetAll())
            {
                if (t.Id == tourReservation.TourId)
                {
                    tour = t;
                    break;

                }
            }
            return tour;
        }

        public List<Languages> GetExistingLanguages()
        {
            return _tourRepository.GetExistingLanguages();
        }


    }
}
