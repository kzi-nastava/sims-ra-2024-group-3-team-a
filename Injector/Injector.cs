using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.InjectorNameSpace
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IAccommodationReservationChangeRequestRepository), new AccommodationReservationChangeRequestRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(IKeyPointsRepository), new KeyPointsRepository() },
            { typeof(IMessageRepository), new MessageRepository() },
            { typeof(ITouristRepository), new TouristRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(ITourReviewRepository), new TourReviewRepository() },
            { typeof(IVoucherRepository), new VoucherRepository() }
         };

        public static T CreateInstance<T>()
        {
            
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
