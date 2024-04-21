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
        public TouristMainWindow(User user)
        {
            InitializeComponent();
         
            _tourMainViewModel = new TouristMainViewModel( new UserDTO(user));
            DataContext = _tourMainViewModel;

            if (Instance == null)
            {
                Instance = this;
            }
        }

        public static TouristMainWindow GetInstance()
        {
            return Instance;
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void ItemsControl_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
    public class JumpyTextBox : TextBox
    {
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Up || e.Key == Key.Down)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));

            if (e.Key == Key.Down || e.Key == Key.Right)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
