using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationDTO : INotifyPropertyChanged
    {
        public AccommodationDTO()
        {
            locationDTO = new LocationDTO();
            images = new List<string>();
        }

        public AccommodationDTO(Accommodation accommodation)
        {
            id = accommodation.Id;
            name = accommodation.Name;
            locationDTO = new LocationDTO(accommodation.Place);
            type = accommodation.Type;
            capacity = accommodation.Capacity;
            minDaysReservation = accommodation.MinDaysReservation;
            cancellationPeriod = accommodation.CancellationPeriod;
            images = accommodation.Images;
        }

        public AccommodationDTO(AccommodationDTO accommodationDTO)
        {
            id = accommodationDTO.Id;
            name = accommodationDTO.Name;
            locationDTO = new LocationDTO(accommodationDTO.PlaceDTO);
            type = accommodationDTO.Type;
            capacity = accommodationDTO.Capacity;
            minDaysReservation = accommodationDTO.MinDaysReservation;
            cancellationPeriod = accommodationDTO.CancellationPeriod;
            images = accommodationDTO.Images;
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private LocationDTO locationDTO;
        public LocationDTO PlaceDTO
        {
            get { return locationDTO; }
            set
            {
                if (value != locationDTO)
                {
                    locationDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccomodationType type;
        public AccomodationType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private int minDaysReservation;
        public int MinDaysReservation
        {
            get { return minDaysReservation; }
            set
            {
                if (value != minDaysReservation)
                {
                    minDaysReservation = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cancellationPeriod;
        public int CancellationPeriod
        {
            get { return cancellationPeriod; }
            set
            {
                if (value != cancellationPeriod)
                {
                    cancellationPeriod = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> images;
        public List<string> Images
        {
            get { return images; }
            set
            {
                if (value != images)
                {
                    images = value;
                    OnPropertyChanged();
                }
            }
        }
        public Accommodation ToAccommodation()
        {
            return new Accommodation(name, locationDTO.ToLocation(), type, capacity, minDaysReservation, cancellationPeriod, images);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
    }
}
