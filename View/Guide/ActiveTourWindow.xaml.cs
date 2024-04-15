using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.View.Guide;
using BookingApp.ViewModel.Guide;
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
        public static ActiveTourWindow Instance;
        public ActiveTourWindow(TourDTO tour, Boolean activeTourExists)
        {
            InitializeComponent();
            DataContext = new ActiveTourViewModel(tour, activeTourExists);
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public static ActiveTourWindow GetInstance()
        {
            return Instance;
        }

    }
}
