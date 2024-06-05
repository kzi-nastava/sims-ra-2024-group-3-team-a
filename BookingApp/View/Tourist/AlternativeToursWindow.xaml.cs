using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Tourist;
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
    /// Interaction logic for AlternativeToursWindow.xaml
    /// </summary>
    public partial class AlternativeToursWindow : Window
    {
        private AlternativeToursViewModel _alternativeToursViewModel;
        public AlternativeToursWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();

            _alternativeToursViewModel = new AlternativeToursViewModel(tourDTO, userDTO);   
            DataContext = _alternativeToursViewModel;
            if (_alternativeToursViewModel.CloseAction == null)
                _alternativeToursViewModel.CloseAction = new Action(this.Close);
            this.PreviewKeyDown += ListView_PreviewKeyDown;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           
        }
        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {

                if (buttonSearch.IsFocused)
                {

                    listViewTours.Focus();
                    e.Handled = true;
                }


            }
        }

    }
}
