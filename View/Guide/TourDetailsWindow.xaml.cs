using BookingApp.DTO;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
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

    public partial class TourDetailsWindow : Window
    {
        public static TourDetailsWindow Instance;
        public TourDetailsWindow(TourDTO tour, UserDTO guide)
        {
            InitializeComponent();
            DataContext = new TourDetailsViewModel(tour, guide);
            if (Instance == null)
            {
                Instance = this;
            }

        }
        public static TourDetailsWindow GetInstance()
        {
            return Instance;
        }


    }
}
