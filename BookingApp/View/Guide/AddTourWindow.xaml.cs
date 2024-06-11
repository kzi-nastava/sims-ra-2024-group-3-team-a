using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.View.Owner;
using BookingApp.ViewModel.Guide;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;


namespace BookingApp.View
{

    public partial class AddTourWindow : Window
    {
        private Brush _defaultBrushBorder;
        public static AddTourWindow Instance;
        public AddTourWindow(UserDTO guide)
        {
            InitializeComponent();
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            DataContext = new AddTourViewModel(guide); 
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public static AddTourWindow GetInstance()
        {
            return Instance;
        }
    
    
    }

}
