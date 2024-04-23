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
                listViewTours


            };
        }

        public static TouristMainWindow GetInstance()
        {
            return Instance;
        }
        private void Menu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && listViewTours.SelectedItem != null)
            {
                var viewModel = DataContext as TouristMainViewModel; // Replace YourViewModelType with your actual ViewModel type
                if (viewModel != null)
                {
                    viewModel.ShowAppropriateWindowCommand.Execute(listViewTours.SelectedItem);
                }
            }
            else if (e.Key == Key.Right)
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

                if (next == listViewTours)
                {
                    listViewTours.Focus();
                    return;
                }

                next.Focus();
                Keyboard.Focus(next);
            }
        }   }
}
