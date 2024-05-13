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
    /// Interaction logic for AcceptTourWindow.xaml
    /// </summary>
    public partial class AcceptTourWindow : Window
    {
        public static AcceptTourWindow Instance;
        public AcceptTourWindow(OrdinaryTourRequestDTO requestDTO, UserDTO user)
        {
            InitializeComponent();
            DataContext = new AcceptTourViewModel(requestDTO, user);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Instance = this;
           
        }
        public static AcceptTourWindow GetInstance()
        {
            return Instance;
        }
     
    }
}
