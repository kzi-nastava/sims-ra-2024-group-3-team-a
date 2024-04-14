using BookingApp.Commands;
using BookingApp.DTO;
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
    class TouristStatisticsViewModel : ViewModel
    {

        private TourDTO _tourDTO;
        private TourReservationService _tourReservationService;
        private TouristDTO _touristDTO;
        private int _underageCounter = 0;
        private int _middleGroupCounter = 0;
        private int _overFiftyCounter = 0;
        public ObservableCollection<TouristDTO> _touristsDTO { get; set; }

        public TouristStatisticsViewModel(TourDTO tour)
        {
            _tourDTO = tour;
            _tourReservationService = new TourReservationService();
            List<TouristDTO> touristsDTO = _tourReservationService.GetJoinedTourists(_tourDTO.ToTourAllParam()).Select(tourist => new TouristDTO(tourist)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(touristsDTO);
            CountAgeGroups(touristsDTO);
        }
        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get { return _touristsDTO; }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }
        public int UnderageCounter
        {
            get { return _underageCounter; }
            set
            {
                _underageCounter = value;
                OnPropertyChanged();
            }
        }
        public int OverFiftyCounter
        {
            get { return _overFiftyCounter; }
            set
            {
                _overFiftyCounter = value;
                OnPropertyChanged();
            }
        }
        public int MiddleGroupCounter
        {
            get { return _middleGroupCounter; }
            set
            {
                _middleGroupCounter = value;
                OnPropertyChanged();
            }
        }

        private void CountAgeGroups(List<TouristDTO> tourists)
        {
            foreach (TouristDTO tourist in tourists)
            {
                if (tourist.Age < 18)
                {
                    _underageCounter++;
                }
                else if (tourist.Age >= 18 && tourist.Age <= 50)
                {
                    _middleGroupCounter++;
                }
                else if (tourist.Age > 50)
                {
                    _overFiftyCounter++;
                }
            }
        }

    }
}

