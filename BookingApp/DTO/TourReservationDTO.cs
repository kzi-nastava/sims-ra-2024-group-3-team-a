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
     public class TourReservationDTO : INotifyPropertyChanged
    {
        public TourReservationDTO() { }

        public TourReservationDTO(int id, int userId, int tourId, string userName)
        { 
            this.id = id;
            this.userId = userId;
            this.tourId = tourId;
            this.userName = userName;
        }

        public TourReservationDTO(TourReservation tourReservation)
        {
            id = tourReservation.Id;
            userId = tourReservation.UserId;
            tourId = tourReservation.TourId;
            userName = tourReservation.UserName;
            numberOfTourists = tourReservation.NumberOfTourists;
        }

        public TourReservationDTO(TourDTO tourDTO, UserDTO userDTO)
        {

            userId = userDTO.Id;
            tourId = tourDTO.Id;
            userName = userDTO.Username;
        }

        public TourReservationDTO(TourReservationDTO tourReservationDTO, List<TouristDTO> anonymousTouristDTO)
        {
            id = tourReservationDTO.Id;
            userId = tourReservationDTO.UserId;
            tourId = tourReservationDTO.TourId;
            userName = tourReservationDTO.UserName;
            touristsDTO = anonymousTouristDTO;
            numberOfTourists = tourReservationDTO.NumberOfTourists;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private int userId;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int tourId;
        public int TourId
        {
            get { return tourId; }
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<TouristDTO> touristsDTO;
        public List<TouristDTO>  TouristsDTO
        {
            get { return touristsDTO; }
            set
            {
                if (value != touristsDTO)
                {
                    touristsDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        private int numberOfTourists;
        public int NumberOfTourists
        {
            get { return numberOfTourists; }
            set
            {
                if (value != numberOfTourists)
                {
                    numberOfTourists = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourReservation ToTourReservation()
        {
            List<Tourist> tourists = new List<Tourist>();

            foreach(TouristDTO touristDTO in touristsDTO)
            {
                tourists.Add(touristDTO.ToTourist());
            }

            return new TourReservation(id, userId, tourId, userName, tourists, numberOfTourists);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
     }
}
