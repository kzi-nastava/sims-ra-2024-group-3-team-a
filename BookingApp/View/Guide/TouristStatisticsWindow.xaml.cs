using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TouristStatisticsWindow(TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = new TouristStatisticsViewModel(tourDTO);
        }
    }
}
