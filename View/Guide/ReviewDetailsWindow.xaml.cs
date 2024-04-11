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
        //private readonly TourReviewRepository _tourReviewRepository;
        private readonly TouristDTO _touristDTO;
        public static ObservableCollection<TourReviewDTO> Reviews { get; set; }
        public ReviewDetailsWindow(TouristDTO touristDTO)
        {
            InitializeComponent();
            _touristDTO = touristDTO;
           // _tourReviewRepository = new TourReviewRepository();
            Reviews = new ObservableCollection<TourReviewDTO>();
            Update();
        }
        private void Update()
        {
            
        }
    }
}
