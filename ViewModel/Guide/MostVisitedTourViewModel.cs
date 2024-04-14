using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    class MostVisitedTourViewModel : ViewModel
    {
        private TourDTO _mostVisitedTourDTO;
        private TourService _tourService;
        public MostVisitedTourViewModel(int year)
        {
            _tourService = new TourService();
            Tour tour = _tourService.GetMostVisitedByYear(year);
            if (tour != null) 
            { 
            _mostVisitedTourDTO = new TourDTO(tour);
            }else
            {
                _mostVisitedTourDTO = null;
            }
        }
        public TourDTO MostVisitedTourDTO
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
            }
        }
    }

}
