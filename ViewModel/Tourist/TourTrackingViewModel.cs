using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class TourTrackingViewModel : ViewModel
    {
      
      
        private TourDTO _tourDTO;
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private ObservableCollection<TouristDTO> _touristsDTO;
        public TourTrackingViewModel(TourDTO tourDTO)
        { 
            _tourDTO = tourDTO;
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            List<TouristDTO> tourists = _tourService.GetTourists(tourDTO.ToTourAllParam()).Select(tourists => new TouristDTO(tourists)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(tourists);


        }

        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get
            {
                return _touristsDTO;
            }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }





    }
}
