using BookingApp.DTO;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Interaction logic for CreateTourRequestWindow.xaml
    /// </summary>
    public partial class CreateTourRequestWindow : Window
    {
        private CreateTourRequestViewModel _createTourRequestViewModel;
        public CreateTourRequestWindow(UserDTO loggedInUser)
        {

            InitializeComponent();
            _createTourRequestViewModel = new CreateTourRequestViewModel(loggedInUser);
            DataContext = _createTourRequestViewModel;
        }
    }
}
