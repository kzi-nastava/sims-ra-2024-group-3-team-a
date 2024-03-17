﻿using BookingApp.DTO;
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
    public partial class ActiveTourWindow : Window
    {
        private TourDTO _tourDTO;
        private TouristDTO _touristDTO;
        private KeyPointsDTO _keypointsDTO;

        private readonly KeyPointsRepository _keyPointsRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourRepository _tourRepository;

   
        public static ObservableCollection<TouristDTO> Tourists { get; set; }

        public ActiveTourWindow(TourDTO tour)
        {
            InitializeComponent();
            _tourDTO = tour;
            Tourists = new ObservableCollection<TouristDTO>();
            this.DataContext = this;
            _keyPointsRepository = new KeyPointsRepository();
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            AddKeyPointsButtons();
            Update();
        }
        private void CancelTour(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tour has been canceled!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            _tourDTO.CurrentKeyPoint = "finished";
            _tourDTO.IsActive = false;
            _tourRepository.Update(_tourDTO.ToTourAllParam());
            this.Close();
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
                        }
                        else if (keyPointsNum != 2 && count <= _keypointsDTO.Middle.Count())
                        {
                            if (_keypointsDTO.Middle[count - 1] != "")
                            {
                                button.Content = _keypointsDTO.Middle[count - 1];

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
                        }
                        button.Name = "Button" + count.ToString();
                        AddButtonToGrid(button, i, j);
                        count++;
                    }
                }
            }
        }

        private Button CreateButton(int count)
        {
            Button button = new Button();
            button.Click += (sender, e) => ClickButton(button);
            return button;
        }

        private void SetInitialButtonProperties(Button button)
        {
            button.Content = _keypointsDTO.Begining;
            button.Background = new SolidColorBrush(Colors.IndianRed);
            _tourDTO.CurrentKeyPoint = _keypointsDTO.Begining;
            _tourDTO.IsActive = true;
        }

        private void AddButtonToGrid(Button button, int row, int column)
        {
            Grid.SetColumn(button, column);
            Grid.SetRow(button, row);
            gridMain.Children.Add(button);
        }

        public void ClickButton(Button button)
        {
            _tourDTO.CurrentKeyPoint = button.Content.ToString();
            button.Background = Brushes.IndianRed;
            if(button.Content == _keypointsDTO.Ending)
            {
                MessageBox.Show("Tour is finished!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                _tourDTO.CurrentKeyPoint = "finished";
                _tourDTO.IsActive=false;
                _tourRepository.Update(_tourDTO.ToTourAllParam());
                this.Close();
            }
        }

        private void TouristJoiningPoint(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TouristDTO selectedTourist = new TouristDTO();
            selectedTourist =   ((Button)sender).DataContext as TouristDTO;
            _touristDTO = selectedTourist;
            _touristDTO.JoiningKeyPoint = _tourDTO.CurrentKeyPoint;
            button.Background = Brushes.IndianRed;
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
