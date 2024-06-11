using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.ViewModel.Owner;
using BookingApp.ViewModel.Tourist;
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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TrackTourWindow.xaml
    /// </summary>
    public partial class TrackTourWindow : Window
    {
        public TrackTourWindow(TourDTO tourDTO)
        {
            InitializeComponent();
         
            DataContext = new TourTrackingViewModel(tourDTO);
            if (new TourTrackingViewModel(tourDTO).CloseAction == null)
                new TourTrackingViewModel(tourDTO).CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
       

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TourTrackingViewModel viewModel)
            {
                viewModel.SetScrollViewer(scrollViewer);
            }
        }
    }
    public class ReachedStatusToBrushConverter : IValueConverter
    {
        SolidColorBrush brush3;
        SolidColorBrush brush4;
        SolidColorBrush brush5;
        public ReachedStatusToBrushConverter()
        {
            Color color1 = (Color)ColorConverter.ConvertFromString("#ffe2f1");
            Color color4 = (Color)ColorConverter.ConvertFromString("#ffaad7");
            Color color5 = (Color)ColorConverter.ConvertFromString("#ffffd8");

            brush3 = new SolidColorBrush(color1);
            brush4 = new SolidColorBrush(color4);
            brush5 = new SolidColorBrush(color5);
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool current = (bool)value;
            return current ? brush4 : brush5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
