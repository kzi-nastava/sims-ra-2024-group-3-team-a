using BookingApp.DTO;
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
    /// Interaction logic for ReviewDetailsWindow.xaml
    /// </summary>
    public partial class ReviewDetailsWindow : Window
    {
        private readonly TouristDTO _touristDTO;
        private readonly TouristRepository _touristRepository;
        public static ObservableCollection<TourReviewDTO> Reviews { get; set; }
        public ReviewDetailsWindow(TouristDTO touristDTO)
        {
            InitializeComponent();
            _touristDTO = touristDTO;
            Reviews = new ObservableCollection<TourReviewDTO>();
            _touristRepository = new TouristRepository();
            Update();
            DataContext = Reviews;
        }
        private void Update()
        {
            Reviews.Add(_touristDTO.Review);
        }
        private void MarkAsInvalid(object sender, RoutedEventArgs e)
        {
            _touristDTO.Review.IsNotValid = "nije validna";
            _touristRepository.Update(_touristDTO.ToTourist());
        }
    }
}
