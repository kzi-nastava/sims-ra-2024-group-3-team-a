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
    /// <summary>
    /// Interaction logic for MostWantedLocation.xaml
    /// </summary>
    public partial class MostWantedLocation : Window
    {
        private Brush _defaultBrushBorder;
        public MostWantedLocation( UserDTO user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            DataContext = new MostWantedLocationViewModel(user);
        }
    
}
}
