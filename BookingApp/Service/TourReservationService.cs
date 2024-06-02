using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private ITourReservationRepository _tourReservationRepository;

        private TourReviewService _tourReviewService;
        private TouristService _touristService;
        private UserService _userService;
        private VoucherService _voucherService;
        private TourService _tourService;

        public TourReservationService(ITourReservationRepository tourReservationRepository, IUserRepository userRepository, ITouristRepository touristRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository)
        {
            _tourReservationRepository = tourReservationRepository;
            _userService = new UserService(userRepository);
            _touristService = new TouristService(touristRepository);
            _tourReviewService = new TourReviewService(tourReviewRepository);
            _voucherService = new VoucherService(voucherRepository);
            
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            return _tourReservationRepository.Save(tourReservation);
        }

        public void Delete(TourReservation tourReservation)
        {
            _tourReservationRepository.Delete(tourReservation);
        }

        public TourReservation Update(TourReservation tourReservation) 
        {
            return _tourReservationRepository.Update(tourReservation);
        }

        public TourReservation GetById(int id)
        { 
            return _tourReservationRepository.GetById(id);
        }
        public List<TourReservation> GetAllForUser(int userId)

        {
            List<TourReservation> tourReservations = new List<TourReservation>();
            foreach(TourReservation tourReservation in GetAll())
            {
                if(tourReservation.UserId == userId)
                {
                    tourReservations.Add(tourReservation);  
                }
            
            }
            return tourReservations;
        }

        public List<Tourist> GetJoinedTourists(Tour tour) 
        {
            List<Tourist> turisti = new List<Tourist>();
            foreach (TourReservation tourReservation in GetAll())
            {
                if (tour.Id == tourReservation.TourId)
                {
                    foreach (Model.Tourist tourist in tourReservation.Tourists)
                    {
                        if (tourist.JoiningKeyPoint != "")
                        {
                            AddReview(tourReservation, tourist);
                            _touristService.Save(tourist);
                            turisti.Add(tourist);
                        }
                    }
                }

            }
            return turisti;
        }
       
        public List<Tourist> GetReservationTourists(Tour tour)
        {
            List<Tourist> turisti = new List<Tourist>();
            foreach (TourReservation tourReservation in GetAll())
            {
                if (tour.Id == tourReservation.TourId)
                {
                    foreach (Model.Tourist tourist in tourReservation.Tourists)
                    {
                        _touristService.Save(tourist);
                        turisti.Add(tourist);  
                    }
                }

            }
            return turisti;
        }
      
        private void AddReview(TourReservation tourReservation, Model.Tourist tourist)
        {
            foreach (TourReview tourReview in _tourReviewService.GetAll())
            {
                if (tourReview.TouristId == tourReservation.UserId && tourist.Name == _userService.GetById(tourReview.TouristId).Username && tourReservation.TourId == tourReview.TourId)
                {
                    tourist.Review = tourReview;
                }
            }
        }
        public void MakeTourReservationVoucher(Tour tour)
        {
            foreach (TourReservation tourReservation in GetAll())
            {
                if (tourReservation.TourId == tour.Id)
                {
                    Voucher voucher = new Voucher();
                    voucher.UserId = tourReservation.UserId;
                    voucher.ExpireDate =DateTime.Now.AddYears(1);
                    voucher.Type = VoucherType.GuideCanceledTour;
                    voucher.Header = "Canceled tour";
                    voucher.TourId = -1;
                    _voucherService.Save(voucher);
                }
            }
        }

        public bool IsVoucherUsed(Tour tour)
        {
            foreach(Voucher voucher in _voucherService.GetAll())
            {
                if(voucher.TourId == tour.Id)
                {
                    return true;
                }
            }

            return false;
        }
       

       /* public void FindCandidatesForVoucher(int userId)
        {
           
            Tour tour = new Tour();
            DateTime tourDate;
            int i;
            List<Tourist> tourists = new List<Tourist> ();
            List<TourReservation> tourReservations = new List<TourReservation>();
            for (i = 0; i < GetAllForUser(userId).Count;i++ )
            {
                tourists = tourReservations[i].Tourists;
                if (tourists[0].JoiningKeyPoint!="")
                {
                    tour = FindTourForReservation(tourReservations[i]);
                    tourDate = tour.BeginingTime;
                    i=FindOtherCandidatesForVoucher(i, tourDate, userId);
                   


                }
            }
           

        }*/
        /*public int FindOtherCandidatesForVoucher(int tourReservationIndex, DateTime beginingTime, int userId)
        {
            Tour tour = new Tour();
            DateTime endDate = beginingTime.AddYears(1);
            DateTime tourDate;
            int count = 0;
            List<Tourist> tourists = new List<Tourist>();
            List<TourReservation> tourReservations = new List<TourReservation>();
           
            
                for (int i = tourReservationIndex; i < GetAllForUser(userId).Count; i++)
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

            return 0;

        }
        public void MakeVoucher(int tourId, int userId)
        {
            Voucher voucher = new Voucher();
            voucher.Type = Model.Enums.VoucherType.Gift;
            voucher.TourId = tourId;
            voucher.UserId = userId;
            voucher.IsUsed = false;
            voucher.ExpireDate = DateTime.Now.AddMonths(6);
            _voucherService.Save(voucher);
            _voucherService.UpdateHeader();
        }
        public Tour FindTourForReservation(TourReservation tourReservation)
        {
            Tour tour = new Tour();
            foreach (Tour t in _tourService.GetAll())
            {
                if (tour.Id == tourReservation.TourId)
                {
                    tour = t;
                    break;

                }
            }
            return tour;
        }*/
    }
}
