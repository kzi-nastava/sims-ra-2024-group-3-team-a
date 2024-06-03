using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationPage.xaml
    /// </summary>
    public partial class AddAccommodationPage : Page
    {
        private Brush _defaultBrushBorder;
        private AddAccommodationViewModel _addAccommodationViewModel;

        public AddAccommodationPage(UserDTO loggedInOwner)
        {
            InitializeComponent();

            _addAccommodationViewModel = new AddAccommodationViewModel(loggedInOwner);
            DataContext = _addAccommodationViewModel;
        }
    }
}
