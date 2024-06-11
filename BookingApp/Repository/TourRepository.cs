using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BookingApp.Service;
using BookingApp.Repository.Interfaces;
using BookingApp.Model.Enums;

namespace BookingApp.Repository
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;
     
        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour GetById(int id) 
        {

            _tours = _serializer.FromCSV(FilePath);
            return _tours.FirstOrDefault(u => u.Id == id  );

        }
        public List<Tour> GetByGuideId(int id)
        {

            _tours = _serializer.FromCSV(FilePath);
            DateTime tomorrow = DateTime.Today.AddDays(1);
            DateTime today = DateTime.Today;
            DateTime oneYearAgo = today.AddYears(-1);
            return _tours.FindAll(u => u.GuideId == id && u.BeginingTime >=oneYearAgo && u.BeginingTime < tomorrow);

        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);      
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
        public List<Tour> GetToursWithSameLocation(Tour tour)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour oneTour in GetAll())
            {
                if (oneTour.Place.Country == tour.Place.Country && oneTour.Place.City == tour.Place.City && oneTour.CurrentCapacity != 0)
                {
                    tours.Add(oneTour);
                }
            }
            return tours;
        }
        public Tour GetMostVisitedTour()
        {
            int maxTourists = 0;
            Tour mostVisited = null;
            foreach (Tour tour in GetAll())
            {
                if (tour.TouristsPresent > maxTourists && tour.CurrentKeyPoint == "finished")
                {
                    maxTourists = tour.TouristsPresent;
                    mostVisited = tour;
                }
            }
            return mostVisited;
        }
        public List<Tour> GetNotCancelled()
        {
            List<Tour> tours = new List<Tour>();
            foreach(Tour tour in GetAll())
            {
                if (tour.CurrentKeyPoint != "canceled")
                    tours.Add(tour);
            }
            return tours;
        }
        public Tour GetMostVisitedByYear(int year)
        {
            int maxTourists = 0;
            Tour mostVisited = null;
            foreach (Tour tour in GetAll())
            {
                if (tour.TouristsPresent > maxTourists && tour.CurrentKeyPoint == "finished" && tour.BeginingTime.Year == year)
                {
                    maxTourists = tour.TouristsPresent;
                    mostVisited = tour;
                }
            }
            return mostVisited;
        }
        public List<Tour> GetUpcoming(User guide)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in GetNotCancelled())
            {
                if (tour.BeginingTime >= DateTime.Now && tour.CurrentKeyPoint != "finished" && tour.GuideId == guide.Id)
                    tours.Add(tour);
            }
            return tours;
        }
      
        public List<Languages> GetExistingLanguages()
        {
            List <Languages> languages = new List<Languages> ();
            foreach (Tour tour in GetAll())
            {
                if ( !languages.Contains( tour.Language))
                {
                    languages.Add(tour.Language);
                }
            }
            return languages;
        }
    }
}
