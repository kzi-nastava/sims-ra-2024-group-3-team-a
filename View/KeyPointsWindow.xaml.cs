﻿using BookingApp.DTO;
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

        public KeyPointsWindow(TourDTO tour)
        {
            InitializeComponent();
            _tour = tour;
            UpdateUI();
        }

        private void UpdateUI()
        {
           
        }
    }
}
