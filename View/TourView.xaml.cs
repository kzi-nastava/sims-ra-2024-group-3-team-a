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
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TourView : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }

        private readonly TourRepository _repository;

        public TourView()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            Update();
        }
        private void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _repository.GetAll()) Tours.Add(new TourDTO(tour));
        }
    }
}
