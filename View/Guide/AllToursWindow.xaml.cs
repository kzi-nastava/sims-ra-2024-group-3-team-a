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

namespace BookingApp.View
{
    public partial class AllToursView : Window
    {
        public static ObservableCollection<TourDTO> Tours { get; set; }
        private static TourDTO _tour { get; set; }

        private readonly TourRepository _repository;

        public AllToursView()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            _tour = new TourDTO();
            Update();
        }
        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _repository.GetAll()) Tours.Add(new TourDTO(tour));
        }
        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            AddTourWindow addTourWindow = new AddTourWindow(this);
            addTourWindow.Show();
        }

    }
}
