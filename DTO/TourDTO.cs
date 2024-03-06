﻿using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public TourDTO(Tour tour)
        {
            locationDto = new LocationDTO(tour.Place);
            language = tour.Language;
            maxTouristNumber = tour.MaxTouristNumber;
            beginingTime = tour.BeginingTime;
        }

        public TourDTO(TourDTO tour)
        {
            locationDto = tour.LocationDTO;
            language = tour.Language;
            maxTouristNumber = tour.MaxTouristNumber;
            beginingTime = tour.BeginingTime;
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

        public String Location
        {
            get { return locationDto.ToString(); }
        }

        public Tour ToTour()
        {
           return new Tour(locationDto.ToLocation(), language, maxTouristNumber, beginingTime);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}