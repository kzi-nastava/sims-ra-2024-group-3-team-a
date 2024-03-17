using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
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
        private TourRepository _tourRepository;
        private KeyPointsRepository _keyPointRepository;

        private TourDTO _tourDTO;
        private string _tourKeyPoints;
        private List<DateTime> _dates;

        private UserDTO _loggedGuide;

        public event EventHandler TourAdded;
        private AllToursWindow _allToursView;
        private GuideMainWindow _guideMainWindow;
        private Brush _defaultBrushBorder;


        public AddTourWindow(AllToursWindow allToursView, GuideMainWindow guideMainWindow, UserDTO guide)
        {
            InitializeComponent();
            _tourRepository = new TourRepository();
            _guideMainWindow = guideMainWindow;
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            _keyPointRepository = new KeyPointsRepository();
            _tourDTO = new TourDTO();
            _dates = new List<DateTime>();
            _allToursView = allToursView;
            _loggedGuide = guide;
            DataContext = _tourDTO;
        }

        private int _datesNum = 0;
        private void AddDates(object sender, RoutedEventArgs e)
        {
            _datesNum++;
            DateTime parsedDateTime;
            if (DateTime.TryParse(datePicker.Value.ToString(), out parsedDateTime))
            {
                _dates.Add(parsedDateTime);
            }
            DateTimePicker date = new DateTimePicker();
            date.Text = datePicker.Text;
            date.IsEnabled = false;
            Grid.SetColumn(date, 0);
            Grid.SetRow(date, _datesNum);
            GridDates.Children.Add(date);
        }

        private List<string> _images;
        private void AddImages(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                _images = openFileDialog.FileNames.ToList();

                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, _images[i]).ToString();
                }
            }
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            _tourKeyPoints = textBoxKeyPoints.Text;
            string[] tourKeyPoints = _tourKeyPoints.Split(',');

            if (tourKeyPoints.Length < 2)
            {
                System.Windows.MessageBox.Show("At least two key points needed (begining i ending)");
                return;
            }

            SetKeyPoints(tourKeyPoints);
            SetLanguage();

            int id = (_keyPointRepository.Save(_tourDTO.KeyPointsDTO.ToKeyPoint())).Id;
            _tourDTO.KeyPointsDTO.Id = id;
            _tourDTO.Images = _images;
            _tourDTO.GuideId = _loggedGuide.Id;

            foreach (var date in _dates)
            {
                TourDTO tourDTO = new TourDTO(_tourDTO);
                tourDTO.BeginingTime = date;
                tourDTO.Images = _images;
                _tourRepository.Save(tourDTO.ToTourAllParam());
            }

            _allToursView.Update();
            _guideMainWindow.Update();
            Close();
        }
        
        private void SetLanguage()
        {
            if (comboBoxType.SelectedItem == comboBoxItemSerbian)
                _tourDTO.Language = Languages.Serbian;
            else if (comboBoxType.SelectedItem == comboBoxItemEnglish)
                _tourDTO.Language = Languages.English;
            else if (comboBoxType.SelectedItem == comboBoxItemGerman)
                _tourDTO.Language = Languages.German;
            else
                _tourDTO.Language = Languages.French;
        }

        private void SetKeyPoints(string[] keyPoints)
        {
            _tourDTO.KeyPointsDTO.Begining = keyPoints[0];
            if (keyPoints.Length != 1)
            {
                for (int i = 1; i < keyPoints.Length - 1; i++)
                {
                    _tourDTO.KeyPointsDTO.Middle.Add(keyPoints[i]);
                }
            }
            _tourDTO.KeyPointsDTO.Ending = keyPoints[keyPoints.Length - 1];
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

        private void LeaveButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
