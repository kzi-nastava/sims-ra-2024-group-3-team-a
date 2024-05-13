﻿using BookingApp.DTO;
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
        private IInputElement[] FocusChain;
        public TourInformationWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _tourInformationView = new TourInformationViewModel(tourDTO, userDTO);
            DataContext = _tourInformationView;
            if (Instance == null)
            {
                Instance = this;
            }
            if (_tourInformationView.CloseAction == null)
                _tourInformationView.CloseAction = new Action(this.Close);

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.PreviewKeyDown += TouristInformationWindow_PreviewKeyDown;
        }
        public static TourInformationWindow GetInstance()
        {
            return Instance;
        }



        /* private void Scroll_PreviewKeyDown(object sender, KeyEventArgs e)
          {





                if ( e.Key == Key.Left || e.Key == Key.Right)
                {
                    // Get the ScrollViewer
                    ScrollViewer scrollViewer = sender as ScrollViewer;

                    if (scrollViewer != null)
                    {
                        double verticalOffset = scrollViewer.VerticalOffset;
                        double horizontalOffset = scrollViewer.HorizontalOffset;

                        switch (e.Key)
                        {
                            case Key.Up:
                                verticalOffset -= 10; // Adjust the scroll amount as needed
                                scrollViewer.ScrollToVerticalOffset(verticalOffset);
                                break;
                            case Key.Down:
                                verticalOffset += 10; // Adjust the scroll amount as needed
                                scrollViewer.ScrollToVerticalOffset(verticalOffset);
                                break;
                            case Key.Left:
                                horizontalOffset -= 10; // Adjust the scroll amount as needed
                                scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                                break;
                            case Key.Right:
                                horizontalOffset += 10; // Adjust the scroll amount as needed
                                scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
                                break;
                        }

                        e.Handled = true; // Mark the event as handled
                    }
                }




          */
        private void TouristInformationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9 && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {

                More.IsSubmenuOpen = true;
                Settings.Focus();
                e.Handled = true;
            }
        }
    }
   

    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed; // Default to Collapsed if the value is not an integer
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}