using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class TourDTO : INotifyPropertyChanged
    {
        public TourDTO() 
        {
            locationDTO = new LocationDTO();
           // keyPointsDTO = new KeyPointsDTO();
            images = new List<string>();
        }
        public TourDTO(Tour tour)
        {
            id = tour.Id;
            guideId = tour.GuideId;
            name = tour.Name;
            description = tour.Description;
            locationDTO = new LocationDTO(tour.Place);
            language = tour.Language;
            maxTouristNumber = tour.MaxTouristNumber;
          //  keyPointsDTO = new KeyPointsDTO(tour.KeyPoints);
            beginingTime = tour.BeginingTime;
            duration = tour.Duration;
            currentKeyPoint = tour.CurrentKeyPoint;
            images = tour.Images;
            isActive = tour.IsActive;
            currentCapacity = tour.CurrentCapacity;
            touristsPresent = tour.TouristsPresent;
        }
        public TourDTO(TourDTO tourDTO)
        {
            id = tourDTO.Id;
            guideId = tourDTO.GuideId;
            name = tourDTO.Name;
            description = tourDTO.Description;
            locationDTO = new LocationDTO(tourDTO.LocationDTO);
            language = tourDTO.Language;
            maxTouristNumber = tourDTO.MaxTouristNumber;
           // keyPointsDTO = new KeyPointsDTO(tourDTO.KeyPointsDTO); ;
            beginingTime = tourDTO.BeginingTime;
            duration = tourDTO.Duration;
            currentKeyPoint = tourDTO.CurrentKeyPoint;
            images = tourDTO.Images;
            currentCapacity= tourDTO.CurrentCapacity;
            touristsPresent= tourDTO.TouristsPresent;
        }

        private LocationDTO locationDTO;
        public LocationDTO LocationDTO
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

        private int guideId;
        public int GuideId
        {
            get { return guideId; }
            set
            {
                if (value != guideId)
                {
                    guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string currentKeyPoint;
        public string CurrentKeyPoint
        {
            get { return currentKeyPoint; }
            set
            {
                if (value != currentKeyPoint)
                {
                    currentKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LocationDTOString
        {
            get { return locationDTO.ToString(); }

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

        private Languages language;
        public Languages Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }

        private int maxTouristNumber;
        public int MaxTouristNumber
        {
            get { return maxTouristNumber; }
            set
            {
                if (value != maxTouristNumber)
                {
                    maxTouristNumber = value;
                    OnPropertyChanged();
                }
            }
        }

       /* private KeyPointsDTO keyPointsDTO;
        public KeyPointsDTO KeyPointsDTO
        {
            get { return keyPointsDTO; } 
            set
            {
                if (value != keyPointsDTO)
                {
                    keyPointsDTO = value;
                    OnPropertyChanged();
                }

            }
        }*/

        private DateTime beginingTime;
        public DateTime BeginingTime
        {
            get { return  beginingTime; }
            set
            {
                if (value != beginingTime)
                {
                    beginingTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private double duration;
        public double Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
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

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        private int currentCapacity;
        public int CurrentCapacity
        {
            get { return currentCapacity; }
            set
            {
                if (value != currentCapacity)
                {
                    currentCapacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private int touristsPresent;
        public int TouristsPresent
        {
            get { return touristsPresent; }
            set
            {
                if (value != touristsPresent)
                {
                    touristsPresent = value;
                    OnPropertyChanged();
                }
            }
        }
        public Tour ToTour()
        {
           return new Tour(name, locationDTO.ToLocation(), language,maxTouristNumber, beginingTime);
        }
       
        public Tour ToTourAllParam()
        {
            
            return new Tour(id,guideId, name, locationDTO.ToLocation(), description, language, maxTouristNumber, /*keyPointsDTO.ToKeyPoint()*/ beginingTime, duration, images, currentKeyPoint, isActive, touristsPresent);
        }

        public Tour ToTourWithCapacity()
        {

            return new Tour(id,guideId,name, locationDTO.ToLocation(), description, language, maxTouristNumber, /*keyPointsDTO.ToKeyPoint()*/beginingTime, duration, images, currentKeyPoint, isActive, currentCapacity, touristsPresent);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
