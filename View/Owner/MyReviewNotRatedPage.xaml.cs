using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestPage.xaml
    /// </summary>
    public partial class MyReviewNotRatedPage : Page
    {
        private Brush _defaultBrushBorder;

        private MyReviewNotRatedViewModel _myReviewNotRatedViewModel;

        public MyReviewNotRatedPage(AccommodationReservationDTO reservation)
        {
            InitializeComponent();

            _myReviewNotRatedViewModel = new MyReviewNotRatedViewModel(reservation);
            DataContext = _myReviewNotRatedViewModel;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (CheckInput())
                buttonAdd.IsEnabled = true;
            else
                buttonAdd.IsEnabled = false;
        }
        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach (var grid in stackPanel.Children.OfType<Grid>())
            {
                foreach (var control in grid.Children)
                {
                    if (control is TextBox)
                    {
                        TextBox textBox = (TextBox)control;
                        if (textBox.Text == string.Empty)
                        {
                            BorderBrushToRed(textBox);
                            validInput = false;
                        }
                        else
                        {
                            BorderBrushToDefault(textBox);       
                        }
                    }
                }
            }

            return validInput;
        }
        private bool CheckInput()
        {
            bool validInput = EmptyTextBoxCheck();

            if(!int.TryParse(textBoxCleannessRating.Text, out int guest))
            {
                BorderBrushToRed(textBoxCleannessRating);
                validInput = false;
            }
            else
            {
                if (int.Parse(textBoxCleannessRating.Text) < 1 || int.Parse(textBoxCleannessRating.Text) > 5)
                {
                    BorderBrushToRed(textBoxCleannessRating);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxCleannessRating);
                }
            }

            if (!int.TryParse(textBoxRulesRespectRating.Text, out int guestRating))
            {
                BorderBrushToRed(textBoxRulesRespectRating);
                validInput = false;
            }
            else
            {
                if (int.Parse(textBoxRulesRespectRating.Text) < 1 || int.Parse(textBoxRulesRespectRating.Text) > 5)
                {
                    BorderBrushToRed(textBoxRulesRespectRating);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxRulesRespectRating);
                }
            }

            return validInput;
        }
        private void BorderBrushToRed(TextBox textBox)
        {
            textBox.BorderBrush = Brushes.Red;
            textBox.BorderThickness = new Thickness(2);
        }
        private void BorderBrushToDefault(TextBox textBox)
        {
            textBox.BorderBrush = _defaultBrushBorder;
            textBox.BorderThickness = new Thickness(2);
        }
    }
}
