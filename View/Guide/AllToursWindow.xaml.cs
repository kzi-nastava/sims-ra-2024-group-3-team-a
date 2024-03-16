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
        private readonly TourRepository _TourRepository;
        private GuideMainWindow _guideMainWindow;

        public AllToursView(GuideMainWindow guideMainWindow)
        {
            InitializeComponent();
            DataContext = this;
            _TourRepository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            Update();
            _guideMainWindow = guideMainWindow;
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _TourRepository.GetAll()) Tours.Add(new TourDTO(tour));
        }

        private void ShowAddTourWindow(object sender, RoutedEventArgs e)
        {
            AddTourWindow addTourWindow = new AddTourWindow(this, _guideMainWindow);
            addTourWindow.Show();
        }
    }
}
