using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestPage.xaml
    /// </summary>
    public partial class MyReviewNotRatedPage : Page
    {
        private Brush _defaultBrushBorder;

        private MyReviewNotRatedViewModel _myReviewNotRatedViewModel;

        public MyReviewNotRatedPage(AccommodationReservationDTO reservation)
        {
            InitializeComponent();

            _myReviewNotRatedViewModel = new MyReviewNotRatedViewModel(reservation);
            DataContext = _myReviewNotRatedViewModel;
        }
    }
}
