using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    public partial class KeyPointsWindow : Window
    {
        private TourDTO _tour;
        private TouristDTO _selectedTourist;
        private KeyPointsDTO _keypoints;
        private readonly KeyPointRepository _repository;
        private readonly TourReservationRepository _tourReservationRepository;

   
        public static ObservableCollection<TouristDTO> Tourists { get; set; }

        public KeyPointsWindow(TourDTO tour)
        {
            InitializeComponent();
            _tour = tour;
            Tourists = new ObservableCollection<TouristDTO>();
            this.DataContext = this;
            _repository = new KeyPointRepository();
            _tourReservationRepository = new TourReservationRepository();
            AddButton();
            Update();
           
           
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tour has been canceled!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            _tour.CurrentKeyPoint = "finished";
            _tour.IsActive = false;
            this.Close();
        }

        public void AddButton()
        {
            KeyPoints keypoint = _repository.GetById(_tour.KeyPointsDTO.Id);
            _keypoints = new KeyPointsDTO(keypoint);
            int keyPointsNum = _keypoints.Middle.Count() + 2;
            int count = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (count < keyPointsNum)
                    {
                        Button MyControl1 = new Button();
                        MyControl1.Click += (sender, e) =>
                        {
                            ClickButton(MyControl1);
                        };
                        if (count == 0)
                        {
                            MyControl1.Content = _keypoints.Begining;
                            MyControl1.Background = new SolidColorBrush(Colors.IndianRed);
                            _tour.CurrentKeyPoint = _keypoints.Begining;
                            _tour.IsActive = true;
                        }
                        else if (keyPointsNum !=2 && count <= _keypoints.Middle.Count())
                        {
                            if (_keypoints.Middle[count - 1] != "")
                            {
                                MyControl1.Content = _keypoints.Middle[count - 1];
                               
                            }
                            else
                            {
                                count++;
                                continue;
                            }
                        }
                        else
                        {
                            MyControl1.Content = _keypoints.Ending;
                        }
                        MyControl1.Name = "Button" + count.ToString();
                        Grid.SetColumn(MyControl1, j);
                        Grid.SetRow(MyControl1, i);
                        gridMain.Children.Add(MyControl1);

                        count++;
                    }
                }

            }
        }

       public void ClickButton(Button button)
        {
            _tour.CurrentKeyPoint = button.Content.ToString();
            button.Background = Brushes.IndianRed;
            if(button.Content == _keypoints.Ending)
            {
                MessageBox.Show("Tour is finished!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                _tour.CurrentKeyPoint = "finished";
                _tour.IsActive=false;
                 this.Close();
            }
        }

        private void JoinTourist_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TouristDTO selectedTourist = new TouristDTO();
            selectedTourist =   ((Button)sender).DataContext as TouristDTO;
            _selectedTourist = selectedTourist;
            _selectedTourist.JoiningKeyPoint = _tour.CurrentKeyPoint;
            button.Background = Brushes.IndianRed;
    
        }
        private void Update()
        {
            Tourists.Clear();

            foreach (TourReservation reservation in _tourReservationRepository.GetAll())
            {
                if (reservation.TourId == _tour.Id)
                {
                    foreach (Model.Tourist tourist in reservation.Tourists)
                    {
                        TouristDTO anonymousTourist = new TouristDTO(tourist);
                        Tourists.Add(anonymousTourist);
                    }
                }
            }
          
        }
    }
}
