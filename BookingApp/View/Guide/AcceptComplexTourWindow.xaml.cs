using BookingApp.DTO;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for AcceptComplexTourWindow.xaml
    /// </summary>
    public partial class AcceptComplexTourWindow : Window
    {
        public static AcceptComplexTourWindow Instance;
        public AcceptComplexTourWindow(OrdinaryTourRequestDTO requestDTO, ComplexTourRequestDTO complexDTO, UserDTO user)
        {
            InitializeComponent();
            DataContext = new AcceptComplexTourViewModel(requestDTO, complexDTO, user);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Instance = this;

        }
        public static AcceptComplexTourWindow GetInstance()
        {
            return Instance;
        }

    }
}
