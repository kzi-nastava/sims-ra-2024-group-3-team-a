using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();

        public Tour GetById(int id);

        public Tour Save(Tour tour);

        public int NextId();

        public void Delete(Tour tour);

        public Tour Update(Tour tour);

        public List<Tour> GetToursWithSameLocation(Tour tour);
        
        public Tour GetMostVisitedTour();
        
        public List<Tour> GetNotCancelled();
        public List<Tour> GetUpcoming();

        public Tour GetMostVisitedByYear(int year);
    }
}
