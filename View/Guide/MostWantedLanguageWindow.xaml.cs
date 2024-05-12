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
    /// Interaction logic for MostWantedLanguage.xaml
    /// </summary>
    public partial class MostWantedLanguageWindow  : Window
    {
        private Brush _defaultBrushBorder;
        public MostWantedLanguageWindow(UserDTO user)
        {
            InitializeComponent();
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            DataContext = new MostWantedLanguageViewModel(user);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (InputCheck())
                SubmitButton.IsEnabled = true;
            else
                SubmitButton.IsEnabled = false;
        }

        private bool EmptyTextBoxCheck()
        {
            bool validInput = true;

            foreach (var control in gridInput.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    if (textBox.Text == string.Empty)
                    {
                        textBox.BorderBrush = Brushes.Red;
                        validInput = false;
                    }
                    else
                    {
                        textBox.BorderBrush = _defaultBrushBorder;
                    }
                }
            }
            return validInput;
        }

        private bool InputCheck()
        {
            bool validInput = EmptyTextBoxCheck();

            if (!int.TryParse(MaxTouristTextBox.Text, out int maxTourist))
            {
                BorderBrushToRed(MaxTouristTextBox);
                validInput = false;
            }
            else
            {
                if (int.Parse(MaxTouristTextBox.Text) < 1)
                {
                    BorderBrushToRed(MaxTouristTextBox);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(MaxTouristTextBox);
                }
            }

            if (!double.TryParse(textBoxDuration.Text, out double duration))
            {
                BorderBrushToRed(textBoxDuration);
                validInput = false;
            }
            else
            {
                if (double.Parse(textBoxDuration.Text) < 1)
                {
                    BorderBrushToRed(textBoxDuration);
                    validInput = false;
                }
                else
                {
                    BorderBrushToDefault(textBoxDuration);
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

