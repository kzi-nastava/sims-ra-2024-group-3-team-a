using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        private readonly AccommodationRepository _repository;

        public OwnerMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<AccommodationDTO>();

            Update();
        }

        private void Update()
        {
            Accommodations.Clear();
            foreach (var accommodation in _repository.GetAll())
            {
                Accommodations.Add(new AccommodationDTO(accommodation));
            }
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            Update();
        }

        private void ShowAddAccommodationWindow(object sender, RoutedEventArgs e)
        {
            AddAccommodationWindow addAccommodationWindow = new AddAccommodationWindow();
            addAccommodationWindow.AccommodationAdded += UpdateEvent;
            addAccommodationWindow.ShowDialog();
        }

        private void RateGuestWindow(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rate guest");
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem == tabItemAccommodations)
            {
                buttonFunction.Click -= RateGuestWindow;
                buttonFunction.Click += ShowAddAccommodationWindow;
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\add.png", UriKind.Relative));
                textBlockFunction.Text = "Add";
            }
            else
            {
                buttonFunction.Click -= ShowAddAccommodationWindow;
                buttonFunction.Click += RateGuestWindow;
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\edit.png", UriKind.Relative));
                textBlockFunction.Text = "Rate";
            }
        }
    }
}
