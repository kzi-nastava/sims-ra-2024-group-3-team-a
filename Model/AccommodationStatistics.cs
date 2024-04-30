using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class AccommodationStatistics
    {
        public int Reservations { get; set; }
        public int AccommodationReservationCancellations { get; set; }
        public int AccommodationReservationChangeRequests { get; set; }
        public int AccommodationRenovationReccommendations { get; set; }
        public bool IsMostOccupied { get; set; }

        public AccommodationStatistics()
        {
            Reservations = 0;
            AccommodationReservationCancellations = 0;
            AccommodationReservationChangeRequests = 0;
            AccommodationRenovationReccommendations = 0;
        }

        public AccommodationStatistics(int reservations, int accommodationReservationCancellations, int accommodationReservationChangeRequests, int accommodationRenovationReccommendations, bool isMostOccupied)
        {
            Reservations = reservations;
            AccommodationReservationCancellations = accommodationReservationCancellations;
            AccommodationReservationChangeRequests = accommodationReservationChangeRequests;
            AccommodationRenovationReccommendations = accommodationRenovationReccommendations;
            IsMostOccupied = isMostOccupied;
        }
    }
}
