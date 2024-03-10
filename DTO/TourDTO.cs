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
    public class TourDTO
    {
        
        public TourDTO() { }

       public TourDTO( Location place, string language, int maxTouristNumber, DateTime beginingTime)
        {
          
          this.locationDto= new LocationDTO(place); 
          this.language = language;
          this.maxTouristNumber = maxTouristNumber;
          this.beginingTime = beginingTime;
        }

        public TourDTO(string name, Location place, string description,   string language, int maxTouristNumber, KeyPoints keyPoints, DateTime beginingTime, TimeSpan duration, List<string> images)
        {
            this.name = name;
            this.locationDto = new LocationDTO(place);
            this.description = description;
            this.language = language;
            this.maxTouristNumber = maxTouristNumber;
            this.keyPointsDTO = new KeyPointsDTO(keyPoints);
            this.beginingTime = beginingTime;
            this.duration = duration;
            this.images = images;

        }
        public TourDTO(Tour tour)
        {
            id = tour.Id;
            name = tour.Name;
            description = tour.Description;
            locationDto = new LocationDTO(tour.Place);
            language = tour.Language;
            maxTouristNumber = tour.MaxTouristNumber;
            keyPointsDTO = new KeyPointsDTO(tour.KeyPoint);
            beginingTime = tour.BeginingTime;
            duration = tour.Duration;
            images = tour.Images;

        }
        public TourDTO(TourDTO tour)
        {
            id = tour.Id;
            name = tour.Name;
            description = tour.Description;
            locationDto = new LocationDTO(tour.LocationDTO);
            language = tour.Language;
            maxTouristNumber = tour.MaxTouristNumber;
            keyPointsDTO = new KeyPointsDTO(tour.KeyPointsDTO); ;
            beginingTime = tour.BeginingTime;
            duration = tour.Duration;
            images = tour.Images;

        }


        private LocationDTO locationDto;

        public LocationDTO LocationDTO
        {
            get { return locationDto; }
            set
            {
                if (value != locationDto)
                {
                    locationDto = value;
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
        private string language;

        public string Language
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
        private KeyPointsDTO keyPointsDTO;
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
        }

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

        private TimeSpan duration;
        public TimeSpan Duration
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
        public Tour ToTour()
        {
           return new Tour(locationDto.ToLocation(), language,maxTouristNumber, beginingTime);
        }
        public Tour ToTourAllParam()
        {
            return new Tour(name, locationDto.ToLocation(), description, language, maxTouristNumber, keyPointsDTO.ToKeyPoint(), beginingTime, duration, images);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
