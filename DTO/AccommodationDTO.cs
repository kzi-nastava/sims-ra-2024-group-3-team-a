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
        private LocationDTO _location;

        public AccommodationDTO()
        {
            _location = new LocationDTO();
            images = new List<string>();
        }

        public AccommodationDTO(LocationDTO location)
        {
            _location = location;
            images = new List<string>();
        }

        public AccommodationDTO(Accommodation accommodation)
        {
            id = accommodation.Id;
            name = accommodation.Name;
            _location = new LocationDTO(accommodation.Place);
            type = accommodation.Type;
            capacity = accommodation.Capacity;
            minDaysReservation = accommodation.MinDaysReservation;
            cancellationPeriod = accommodation.CancellationPeriod;
            images = accommodation.Images;
        }

        public AccommodationDTO(AccommodationDTO accommodation)
        {
            id = accommodation.Id;
            name = accommodation.Name;
            _location = new LocationDTO(accommodation.PlaceDTO);
            type = accommodation.Type;
            capacity = accommodation.Capacity;
            minDaysReservation = accommodation.MinDaysReservation;
            cancellationPeriod = accommodation.CancellationPeriod;
            images = accommodation.Images;
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

        public LocationDTO PlaceDTO
        {
            get { return _location; }
            set
            {
                if (value != _location)
                {
                    _location = value;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public  Accommodation ToAccommodation()
        {
            return new Accommodation(name, _location.ToLocation(), type, capacity, minDaysReservation, cancellationPeriod, images);
        }   
    }
}
