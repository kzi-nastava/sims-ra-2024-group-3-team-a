using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for TourReviewsWindow.xaml
    /// </summary>
    public partial class TourReviewsWindow : Window
    {
        public TourReviewsWindow(TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = new TourReviewsViewModel(tourDTO);
        }
    }
}
