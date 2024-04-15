using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourReservationService
    {
        private TourReservationRepository _tourReservationRepository = new TourReservationRepository();

        private TourReviewService _tourReviewService = new TourReviewService();
        private TouristService _touristService = new TouristService();
        private UserService _userService = new UserService();
        private VoucherService _voucherService = new VoucherService();
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

    }
}
