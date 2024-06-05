using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Navigation;
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
        private IInputElement[] FocusChain;
        public TouristMainWindow(User user)
        {
            InitializeComponent();

            _tourMainViewModel = new TouristMainViewModel(new UserDTO(user));
            DataContext = _tourMainViewModel;

            if (Instance == null)
            {
                Instance = this;
            }

            FocusChain = new IInputElement[]
            {
                searchCountryTextBox,
                searchCityTextBox,
                searchDurationTextBox,
                languageComboBox,
                searcmaxTouristNumberTextBox,
              // buttonSearch,
                listViewTours


            };
          // searchCountryTextBox.Focus();
           this.PreviewKeyDown += TouristMainWindow_PreviewKeyDown;
           this.PreviewKeyDown += ListView_PreviewKeyDown;
      
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        public static TouristMainWindow GetInstance()
        {
            return Instance;
        }

      /*  private void Menu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            


            if (!FocusChain[5].IsKeyboardFocusWithin && e.Key == Key.Right)
            {
                    var next = FocusChain[0];
                    for (var i = 0; i < FocusChain.Length; i++)
                    {
                        if (FocusChain[i].IsKeyboardFocusWithin)
                        {
                            next = FocusChain[(i + 1) % FocusChain.Length];
                            break;
                        }
                    }

                    next.Focus();
                    Keyboard.Focus(next);
            }
            else if ((!FocusChain[5].IsKeyboardFocusWithin && e.Key == Key.Left ) || (FocusChain[5].IsKeyboardFocusWithin && e.Key == Key.Left && listViewTours.Items.IndexOf(listViewTours.SelectedItem)==0))
            {
                    var next = FocusChain[0];
                    for (var i = 0; i < FocusChain.Length; i++)
                    {
                        if (FocusChain[i].IsKeyboardFocusWithin && i != 0)
                        {
                            next = FocusChain[(i - 1) % FocusChain.Length];
                            break;
                        }
                    }
              

                    next.Focus();
                    
                    Keyboard.Focus(next);
            }
            
            else if (FocusChain[5].IsKeyboardFocusWithin)
            {
                base.OnPreviewKeyDown(e);

                if (e.Key == Key.Left)
                {
                    if (listViewTours.SelectedItem != null)
                    {
                        int index = listViewTours.Items.IndexOf(listViewTours.SelectedItem);
                        if (index > 0)
                        {
                            listViewTours.SelectedItem = listViewTours.Items[index - 1];
                            listViewTours.ScrollIntoView(listViewTours.SelectedItem);
                            e.Handled = true;
                        }
                    }
                }
                else if (e.Key == Key.Right)
                {
                    if (listViewTours.SelectedItem != null)
                    {
                        int index = listViewTours.Items.IndexOf(listViewTours.SelectedItem);
                        if (index < listViewTours.Items.Count - 1)
                        {
                            listViewTours.SelectedItem = listViewTours.Items[index + 1];
                            listViewTours.ScrollIntoView(listViewTours.SelectedItem);
                            e.Handled = true; 
                        }
                    }
                }
            }
            



        }*/

        private void TouristMainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9 && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
               
                More.IsSubmenuOpen = true;
                Settings.Focus();
                e.Handled = true; 
            }
        }
        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                // Check if the currently focused element is the Search button
                if (buttonSearch.IsFocused)
                {
                    // Focus the ListView
                    listViewTours.Focus();
                    e.Handled = true;
                }
              

            }
        }
       
        private void ListViewTours_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Execute the command associated with the ListView's MouseDoubleClick event
           var viewModel = DataContext as TouristMainViewModel;
            if (viewModel != null)
            {
                // Call the command and pass the selected item as parameter
                viewModel.ShowAppropriateWindow();
                return;
            }
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LightThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml",UriKind.Relative));
        }
        private void DarkThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
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
