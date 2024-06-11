using BookingApp.DTO;
using BookingApp.ViewModel.Guide;
using System.Windows;

namespace BookingApp.View.Guide
{
    public partial class ReviewDetailsWindow : Window
    {
        public ReviewDetailsWindow(TouristDTO touristDTO)
        {
            InitializeComponent();
            DataContext = new ReviewDetailsViewModel(touristDTO);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen; 
        }
    }
}