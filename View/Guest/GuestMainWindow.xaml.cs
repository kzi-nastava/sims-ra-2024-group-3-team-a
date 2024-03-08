using BookingApp.DTO;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        private readonly AccommodationRepository _repository;


        public GuestMainWindow()
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
    }
}
