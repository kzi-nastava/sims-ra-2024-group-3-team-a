using BookingApp.DTO;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// Interaction logic for TourInformationWindow.xaml
    /// </summary>
    public partial class TourInformationWindow : Window
    {
        public static TourInformationViewModel _tourInformationView;
        public static TourInformationWindow Instance;
        
        public TourInformationWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _tourInformationView = new TourInformationViewModel(tourDTO, userDTO);
            DataContext = _tourInformationView;
           
            
                Instance = this;
            
            if (_tourInformationView.CloseAction == null)
                _tourInformationView.CloseAction = new Action(this.Close);

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           
        }
        public static TourInformationWindow GetInstance()
        {
            return Instance;
        }



        
        private void LightThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));
        }
        private void DarkThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
        }
        

        private void ImageListView_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is TourInformationViewModel viewModel)
            {
                viewModel.SetScrollViewer(scrollViewer);
            }
        }
    }

   
    

}
