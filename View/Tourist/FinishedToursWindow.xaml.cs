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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for FinishedToursWindow.xaml
    /// </summary>
    public partial class FinishedToursWindow : Window
    {
        private TourDTO _tourDTO { get; set; }
        public static ObservableCollection<TourDTO> FinishedTours { get; set; }

        private readonly TourRepository _tourRepository;

        private UserDTO _userDTO { get; set; }
        public FinishedToursWindow(UserDTO userDTO)
        {
            InitializeComponent();
            _tourDTO = new TourDTO();
            _userDTO = userDTO;
            _tourRepository = new TourRepository(); 
            FinishedTours = new ObservableCollection<TourDTO>();

            DataContext = this;
            Update();
        }
        private void Update()
        {
            FinishedTours.Clear();
            foreach (Tour tour in _tourRepository.GetFinishedTours())
                FinishedTours.Add(new TourDTO(tour));
        }
        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            _tourDTO = listBoxFinishedTours.SelectedItem as TourDTO;
            TourReviewWindow tourReviewWindow = new TourReviewWindow(_tourDTO, _userDTO);
            tourReviewWindow.ShowDialog();
        }
    }
}
