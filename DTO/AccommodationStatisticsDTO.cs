using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationStatisticsDTO : INotifyPropertyChanged
    {
        public AccommodationStatisticsDTO() { }
        public AccommodationStatisticsDTO(AccommodationStatisticsDTO accommodationStatisticsDTO)
        {
            reservations = accommodationStatisticsDTO.Reservations;
            cancellations = accommodationStatisticsDTO.Cancellations;
            accommodationReservationChanges = accommodationStatisticsDTO.AccommodationReservationChanges;
            accommodationRenovationReccommendations = accommodationStatisticsDTO.AccommodationRenovationRecommendations;
            isMostOccupied = accommodationStatisticsDTO.IsMostOccupied;
        }

        public AccommodationStatisticsDTO(AccommodationStatistics accommodationStatistics)
        {
            reservations = accommodationStatistics.Reservations;
            cancellations = accommodationStatistics.AccommodationReservationCancellations;
            accommodationReservationChanges = accommodationStatistics.AccommodationReservationChangeRequests;
            accommodationRenovationReccommendations = accommodationStatistics.AccommodationRenovationRecommendations;
            isMostOccupied = accommodationStatistics.IsMostOccupied;
        }

        private int reservations;
        public int Reservations
        {
            get { return reservations; }
            set
            {
                if (value != reservations)
                {
                    reservations = value;
                    OnPropertyChanged();
                }
            }
        }
      
        private int cancellations;
        public int Cancellations
        {
            get { return cancellations; }
            set
            {
                if (value != cancellations)
                {
                    cancellations = value;
                    OnPropertyChanged();
                }
            }
        }

        private int accommodationReservationChanges;
        public int AccommodationReservationChanges
        {
            get { return accommodationReservationChanges; }
            set
            {
                if (value != accommodationReservationChanges)
                {
                    accommodationReservationChanges = value;
                    OnPropertyChanged();
                }
            }
        }

        private int accommodationRenovationReccommendations;
        public int AccommodationRenovationRecommendations
        {
            get { return accommodationRenovationReccommendations; }
            set
            {
                if (value != accommodationRenovationReccommendations)
                {
                    accommodationRenovationReccommendations = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isMostOccupied;
        public bool IsMostOccupied
        {
            get { return isMostOccupied; }
            set
            {
                if (value != isMostOccupied)
                {
                    isMostOccupied = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationStatistics ToAccommodationStatistics()
        {
            return new AccommodationStatistics(reservations, cancellations, accommodationReservationChanges, accommodationRenovationReccommendations, isMostOccupied);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
