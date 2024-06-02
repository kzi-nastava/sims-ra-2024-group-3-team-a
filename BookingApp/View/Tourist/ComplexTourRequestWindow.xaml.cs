using BookingApp.DTO;
using BookingApp.ViewModel.Tourist;
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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for ComplexTourRequestWindow.xaml
    /// </summary>
    public partial class ComplexTourRequestWindow : Window
    {
        private ComplexTourRequestViewModel complexTourRequestViewModel;
        public ComplexTourRequestWindow(UserDTO loggedInUser)
        {
            complexTourRequestViewModel = new ComplexTourRequestViewModel(loggedInUser);
            InitializeComponent();
            DataContext = complexTourRequestViewModel;
            if (complexTourRequestViewModel.CloseAction == null)
                complexTourRequestViewModel.CloseAction = new Action(this.Close);
        }
    }
}
