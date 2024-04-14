using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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
    public partial class AllToursWindow : Window
    {
        public static ObservableCollection<TourDTO> Tours { get; set; }
        private readonly TourRepository _tourRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly VoucherRepository _voucherRepository;
        private GuideMainWindow _guideMainWindow;
        private UserDTO _loggedGuide;

        public AllToursWindow(GuideMainWindow guideMainWindow, UserDTO guide)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _tourReservationRepository = new TourReservationRepository();
            _voucherRepository = new VoucherRepository();
            Tours = new ObservableCollection<TourDTO>();
            _loggedGuide = guide;
            Update();
            _guideMainWindow = guideMainWindow;
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour tour in _tourRepository.GetAll())
            {
                if(tour.GuideId == _loggedGuide.Id)
                    Tours.Add(new TourDTO(tour));
            }          
        }

        private void ShowAddTourWindow(object sender, RoutedEventArgs e)
        {
            AddTourWindow addTourWindow = new AddTourWindow(this, _guideMainWindow, _loggedGuide);
            addTourWindow.Show();
        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {
            TourDTO selectedTour = new TourDTO();
            selectedTour = dataGridTour.SelectedItem as TourDTO;
            if (selectedTour != null)
            {
                DateTime currentTimePlus48Hours = DateTime.Now.AddHours(48);
                if (selectedTour.BeginingTime > currentTimePlus48Hours)
                {
                    foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
                    {
                        if (tourReservation.TourId == selectedTour.Id)
                        {
                            Voucher voucher = new Voucher();
                            voucher.UserId = tourReservation.UserId;
                            voucher.ExpireDate = DateTime.Parse("12/4/2030");
                            voucher.Type = VoucherType.GuideCanceledTour;
                            _voucherRepository.Save(voucher);

                        }
                    }
                    selectedTour.CurrentKeyPoint = "canceled";
                    _tourRepository.Update(selectedTour.ToTourAllParam());
                }
                else
                {
                    MessageBox.Show("The tour cannot be canceled as it is less than 48 hours away from its beginning time.");
                }
            }
        }
    }
}
