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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TrackTourWindow.xaml
    /// </summary>
    public partial class TrackTourWindow : Window
    {
        private TourDTO _tourDTO;
        private TouristDTO _touristDTO;
        private KeyPointsDTO _keypointsDTO;

        private readonly KeyPointsRepository _keyPointsRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourRepository _tourRepository;
        public static ObservableCollection<TouristDTO> Tourists { get; set; }
        public TrackTourWindow(TourDTO tourDTO)
        {
            InitializeComponent();
            _tourDTO = tourDTO;
            
            _keyPointsRepository = new KeyPointsRepository();
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            Tourists = new ObservableCollection<TouristDTO>();
            DataContext = new { Tour = _tourDTO, Tourist = Tourists };
            AddKeyPointsButtons();
            Update();
        }

        public void AddKeyPointsButtons()
        {
            KeyPoints keypoint = _keyPointsRepository.GetById(_tourDTO.KeyPointsDTO.Id);
            _keypointsDTO = new KeyPointsDTO(keypoint);
            int keyPointsNum = _keypointsDTO.Middle.Count() + 2;
            int count = 0;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (count < keyPointsNum)
                    {
                        Button button = CreateButton(count);

                        if (count == 0)
                        {
                            SetInitialButtonProperties(button);
                            if (IsButtonLastKeyPoint(button))
                            {
                                button.Background = new SolidColorBrush(Colors.IndianRed);
                            }

                        }
                        else if (keyPointsNum != 2 && count <= _keypointsDTO.Middle.Count())
                        {
                            if (_keypointsDTO.Middle[count - 1] != "")
                            {
                                button.Content = _keypointsDTO.Middle[count - 1];
                                if (IsButtonLastKeyPoint(button))
                                {
                                    button.Background = new SolidColorBrush(Colors.IndianRed);
                                }

                            }
                            else
                            {
                                count++;
                                continue;
                            }
                        }
                        else
                        {
                            button.Content = _keypointsDTO.Ending;
                            if (IsButtonLastKeyPoint(button))
                            {
                                button.Background = new SolidColorBrush(Colors.IndianRed);
                            }
                        }

                        button.Name = "Button" + count.ToString();
                        AddButtonToGrid(button, i, j);
                        count++;
                    }
                }
            }
        }
        private Boolean IsButtonLastKeyPoint(Button b)
        {
            if (b.Content.ToString() == _tourDTO.CurrentKeyPoint)
            {
                return true;
            }
            else return false;
        }

        private Button CreateButton(int count)
        {
            Button button = new Button();
            return button;
        }

        private void SetInitialButtonProperties(Button button)
        {
            button.Content = _keypointsDTO.Begining;  
        }

        private void AddButtonToGrid(Button button, int row, int column)
        {
            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);
            gridMain.Children.Add(button);
        }
        private void Update()
        {
            Tourists.Clear();

            foreach (TourReservation reservation in _tourReservationRepository.GetAll())
            {
                if (reservation.TourId == _tourDTO.Id)
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
