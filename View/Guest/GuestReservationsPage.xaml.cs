using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guest;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestReservationsPage.xaml
    /// </summary>
    /// 

    public partial class GuestReservationsPage : Page
    {
        private UserDTO _userDTO;

        public GuestReservationsPage(UserDTO userDTO)
        {
            InitializeComponent();
            _userDTO = userDTO;
            DataContext = new GuestReservationsViewModel(_userDTO);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
