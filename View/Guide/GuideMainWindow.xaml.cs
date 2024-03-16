using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{

    public partial class GuideMainWindow : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }
        private readonly TourRepository _tourRepository;
        private TourDTO _tourDTO; 

        public GuideMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            Update();
        }

        public void Update()
        {
            Tours.Clear();
           
            foreach (Tour tour in _tourRepository.GetAll())
            {
                TourDTO tourDTO = new TourDTO(tour);
                if (tourDTO.BeginingTime.Date == DateTime.Today)
                {
                    Tours.Add(tourDTO);
                }
            }
        }

        private void ShowActiveTourWindow(object sender, RoutedEventArgs e)
        {
            TourDTO selectedTour = ((Button)sender).DataContext as TourDTO;
            if (selectedTour.CurrentKeyPoint != "finished")
            {

                if (_tourDTO == null || _tourDTO != selectedTour)
                {
                    _tourDTO = selectedTour;

                    ActiveTourWindow tourDetailsWindow = new ActiveTourWindow(_tourDTO);
                    tourDetailsWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("This tour has already finished", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowAllToursWindow (object sender, RoutedEventArgs e)
        {
            AllToursView allToursView = new AllToursView(this);
            allToursView.Show();
        }
    }
}
