using BookingApp.Repository.Interfaces;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Model.Enums;

namespace BookingApp.Service
{
    class SuperGuideService
    {
        private ISuperGuideRepository _superGuideRepository;
        private UserService _userService;
        private TourService _tourService;
        private TourReviewService _tourReviewService;

        public SuperGuideService( IUserRepository userRepository , ISuperGuideRepository superGuideRepository, ITourRepository tourRepository, ITouristRepository touristRepository, ITourReservationRepository tourReservationRepository, ITourReviewRepository tourReviewRepository, IVoucherRepository voucherRepository)
        {
            _superGuideRepository = superGuideRepository;
            _userService = new UserService(userRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _tourReviewService = new TourReviewService(tourReviewRepository);

        }
        public List<SuperGuide> GetAll()
        {
            return _superGuideRepository.GetAll();
        }
        public SuperGuide Get(int guideId, Languages language) {
             foreach(SuperGuide guide in GetAll())
            {
                if(guide.GuideId == guideId && guide.Language == language)
                {
                    return guide;
                }
            }
            return null;
        }
        public SuperGuide Save(SuperGuide superGuide)
        {
            return _superGuideRepository.Save(superGuide);
        }
        public void Delete(SuperGuide superGuide)
        {
            _superGuideRepository.Delete(superGuide);
        }
        public SuperGuide Update(SuperGuide superGuide)
        {
            return _superGuideRepository.Update(superGuide);
        }
        public void SuperGuideCheck(User user)
        {
            if (user.IsSuper == true)
            {
                SuperGuideRemoving(user);
                
            }
            else 
            {
                SuperGuideBecoming(user);
            }
        }
        public void SuperGuideBecoming(User user)
        {
            foreach (Languages language in _tourService.GetExistingLanguages()) {

              int count = FindCount(user, language);
              if (count > 20) 
              {
                    SuperGuide guide = new SuperGuide();
                    guide.GuideId = user.Id;
                    guide.Language = language;
                    guide.BeginingTime = DateTime.Now;
                    Save(guide);
                    user.IsSuper = true;
                    _userService.Update(user);
                }
            }
        }
        public void SuperGuideRemoving(User user)
        {
            foreach (SuperGuide super in GetAll())
            {
                if(super.GuideId == user.Id)
                {
                    if (super.BeginingTime.AddYears(1) <= DateTime.Now)
                    {
                        int count = FindCount(user, super.Language);
                        if (count < 20)
                        {
                            if (Get(user.Id, super.Language) != null)
                            {
                                Delete(Get(user.Id, super.Language));
                                SuperUserCheck(user);
                            }
                        }
                    }
                }
            }
              
        }

        public int FindCount(User user, Languages language)
        {
            int count = 0;
            foreach (Tour tour in _tourService.GetByGuideId(user.Id))
            {
                if (tour.Language == language)
                {
                    if (_tourReviewService.ScoreAboveFourCheck(tour)) { count++; }
                }
            }
            return count;
        }
        public void SuperUserCheck(User user)
        {
            foreach(SuperGuide guide in  GetAll())
            {
                if (guide.GuideId == user.Id)
                {
                    return;
                }
            }
            user.IsSuper = false;
            _userService.Update(user);
        }
    }

}
