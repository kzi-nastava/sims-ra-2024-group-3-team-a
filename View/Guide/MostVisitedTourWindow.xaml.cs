using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
    /// Interaction logic for MostVisitedTourWindow.xaml
    /// </summary>
    public partial class MostVisitedTourWindow : Window
    {
        private int _year;
        private TourDTO _tourDTO;
        public static ObservableCollection<TourDTO> Tours { get; set; }
        private TourRepository _tourRepository;
        private int _maxTouristsCounter;
        public MostVisitedTourWindow(int year)
        {
            InitializeComponent();
            _tourDTO = new TourDTO();
            Tours = new ObservableCollection<TourDTO>();
            _tourRepository = new TourRepository();
            _year= year;
            Update();
            DataContext = this;
        }
        private void Update()
        {
            _maxTouristsCounter = -1;
            foreach (Tour tour in _tourRepository.GetAll())
            {
                if (tour.TouristsPresent > _maxTouristsCounter && tour.BeginingTime.Year == _year && tour.CurrentKeyPoint=="finished")
                {
                    _maxTouristsCounter = tour.TouristsPresent;
                    _tourDTO = new TourDTO(tour);

                }
            }
            if (_maxTouristsCounter != -1)
            {
                Tours.Add(_tourDTO);
            }
        }
    }
}
