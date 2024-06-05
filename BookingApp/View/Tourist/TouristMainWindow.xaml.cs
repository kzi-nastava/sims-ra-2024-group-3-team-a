using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public static TouristMainWindow Instance;
        private TouristMainViewModel _tourMainViewModel { get; set; }
      
       
        public TouristMainWindow(User user)
        {
            InitializeComponent();

            _tourMainViewModel = new TouristMainViewModel(new UserDTO(user));
            DataContext = _tourMainViewModel;

            if (Instance == null)
            {
                Instance = this;
            }
            if (_tourMainViewModel.CloseAction == null)
                _tourMainViewModel.CloseAction = new Action(this.Close);


            this.PreviewKeyDown += ListView_PreviewKeyDown;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           
        }

        public static TouristMainWindow GetInstance()
        {
            return Instance;
        }

   

        private void LightThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml",UriKind.Relative));
        }
        private void DarkThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
        }
        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                
                if (buttonSearch.IsFocused)
                {
                   
                    listViewTours.Focus();
                    e.Handled = true;
                }


            }
        }
    }
    
}
public class GuideIdToSuperGuideConverter : IValueConverter
{
    private TouristMainViewModel _viewModel;

    public GuideIdToSuperGuideConverter(TouristMainViewModel viewModel)
    {
        _viewModel = viewModel;
    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int guideId = (int)value;
        bool isSuperGuide = _viewModel.isSuperGuide(guideId);
        return isSuperGuide;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
