using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for TouristStatisticsWindow.xaml
    /// </summary>
    public partial class TouristStatisticsWindow : Window
    {
      /*  public TourStatisticsWindow(TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = new TourStatisticsViewModel(tourDTO);
        }

    */


        private TourDTO _tourDTO;
        private TourReservationRepository _tourReservationRepository;
        private TouristDTO _touristDTO;
        private int _underageCounter;
        private int _middleGroupCounter;
        private int _overFiftyCounter;

        public ObservableCollection<int> UnderageCollection { get; set; }
        public ObservableCollection<int> MiddleGroupCollection { get; set; }
        public ObservableCollection<int> OverFiftyCollection { get; set; }
        public ObservableCollection<TouristDTO> Tourists { get; set; }
        public TouristStatisticsWindow(TourDTO tour)
        {
            InitializeComponent();
            _tourDTO = tour;
            _tourReservationRepository = new TourReservationRepository();
            _touristDTO= new TouristDTO();
            Tourists = new ObservableCollection<TouristDTO>();
            UnderageCollection = new ObservableCollection<int>();
            MiddleGroupCollection = new ObservableCollection<int>();
            OverFiftyCollection = new ObservableCollection<int>();
            _underageCounter= 0;
            _middleGroupCounter= 0;
            _overFiftyCounter= 0;
            Update();
            DataContext = this;
        }
        private void Update()
        {
            foreach(TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                if(_tourDTO.Id == tourReservation.TourId)
                {
                    foreach (Model.Tourist tourist in tourReservation.Tourists)
                    {
                        if (tourist.JoiningKeyPoint != "")
                        {
                            _touristDTO= new TouristDTO(tourist);
                            CountAgeGroups(_touristDTO);
                            Tourists.Add(_touristDTO);
                        }
                    }
                }

            }
            UnderageCollection.Add(_underageCounter);
            MiddleGroupCollection.Add(_middleGroupCounter);
            OverFiftyCollection.Add(_overFiftyCounter);
        }
        private void CountAgeGroups(TouristDTO tourist)
        {
            if(tourist.Age < 18)
            {
                _underageCounter++;
            }else if (tourist.Age >=18 &&  tourist.Age <= 50)
            {
               _middleGroupCounter++;
            }else if (tourist.Age >50)
            {
                _overFiftyCounter++;
            }
        }

    }
}
