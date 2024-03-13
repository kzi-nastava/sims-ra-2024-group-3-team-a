﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
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
    public partial class KeyPointsWindow : Window
    {
        private TourDTO _tour;
        private KeyPointsDTO _keypoints;
        private readonly KeyPointRepository _repository;

        public KeyPointsWindow(TourDTO tour)
        {
            InitializeComponent();
            _tour = tour;
            DataContext = this;
            _repository = new KeyPointRepository();
            Buttons();
           
        }

        public void Buttons()
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
                        if (count == 0)
                        {
                            MyControl1.Content = _keypoints.Begining;
                            MyControl1.Background = new SolidColorBrush(Colors.IndianRed);
                        }
                        else if (count <= _keypoints.Middle.Count())
                        {
                            MyControl1.Content = _keypoints.Middle[count - 1];
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            b.Background = Brushes.IndianRed;
            if (gridMain.Children.IndexOf(b) == gridMain.Children.Count - 1)
            {
                MessageBox.Show("Tura je završena!", "Obavještenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateUI()
        {
           
        }
    }
}