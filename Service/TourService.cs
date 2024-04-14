using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourService
    {
        private TourRepository _tourRepository = new TourRepository();

        private TourReservationService _tourReservationService = new TourReservationService();

        public List<Tour> GetAll() 
        {
            return _tourRepository.GetAll();
        }

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        public Tour Save(Tour tour) 
        {
            return _tourRepository.Save(tour);
        }

        public void Delete(Tour tour)
        {
             _tourRepository.Delete(tour);
        }

        public Tour Update(Tour tour)
        {
            return _tourRepository.Update(tour);
        }

     
        public List<Tour> GetActiveTours()
        {
            List<Tour> activeTours = new List<Tour>();
                foreach (Tour t in GetAll())
                {
                    foreach (TourReservation tr in _tourReservationService.GetAll())
                    {
                        if (t.Id == tr.TourId && t.IsActive)
                        {
                            activeTours.Add(t);
                        }
                    }
                }
           return activeTours;
        }

        public List<Tour> GetUnactiveTours()
        {
            List<Tour> unactiveTours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                foreach (TourReservation tr in _tourReservationService.GetAll())
                {
                    if (t.Id == tr.TourId && !t.IsActive && !t.CurrentKeyPoint.Equals("finished"))
                    {
                        unactiveTours.Add(t);
                    }
                }
            }
            return unactiveTours;
        }
        public List<Tour> GetFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour t in GetAll())
            {
                foreach (TourReservation tr in _tourReservationService.GetAll())
                {
                    if (t.Id == tr.TourId && t.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(t);
                    }
                }
            }
            return finishedTours.Distinct().ToList();
        }
    
    public List<Tour> GetAllFinishedTours()
        {
            List<Tour> finishedTours = new List<Tour>();
            foreach (Tour tour in GetAll())
            {
                    if (tour.CurrentKeyPoint.Equals("finished"))
                    {
                        finishedTours.Add(tour);
                    }

            }
            return finishedTours;
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
        public Tour GetMostVisitedByYear(int year)
        {
            int maxTourists = 0;
            Tour mostVisited = null;
            foreach (Tour tour in GetAll())
            {
                if (tour.TouristsPresent > maxTourists && tour.CurrentKeyPoint == "finished" && tour.BeginingTime.Year==year)
                {
                    maxTourists = tour.TouristsPresent;
                    mostVisited = tour;
                }

            }
            return mostVisited;
        }
    }
}
