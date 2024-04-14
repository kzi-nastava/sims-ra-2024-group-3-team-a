using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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
using BookingApp.View.Owner;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }
        private UserDTO _userDTO;
        public static Frame MainFrame;
        public static Frame MyRequestsFrame;
        public static GuestMainWindow Instance;


        public GuestMainWindow(User user)
        {
            InitializeComponent();
            _userDTO = new UserDTO(user);
            DataContext = new GuestMainViewModel(_userDTO);
            
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();
            MainFrame = frameMain;
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public static GuestMainWindow GetInstance()
        {
            return Instance;
        }
        private void MyInbox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
