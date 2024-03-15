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
        private TourRepository _repository;
        private KeyPointRepository _keyPointRepository;
        private TourDTO _tourDTO;
        private string _tourKeyPoints;
        private List<DateTime> _dates;
        public event EventHandler TourAdded;
        private AllToursView _allToursView;
        private GuideMainWindow _guideMainWindow;
        private Brush _defaultBrushBorder;

        public AddTourWindow(AllToursView allToursView, GuideMainWindow guideMainWindow)
        {
            InitializeComponent();
            _repository = new TourRepository();
            _guideMainWindow = guideMainWindow;
            _defaultBrushBorder = textBoxName.BorderBrush.Clone();
            _keyPointRepository = new KeyPointRepository();
            _tourDTO = new TourDTO();
            _dates = new List<DateTime>();
            _allToursView = allToursView;
            DataContext = _tourDTO;
        }

        private int _datesNum = 0;
        private void Add_Click(object sender, RoutedEventArgs e)
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

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _tourKeyPoints = textBoxKeyPoints.Text;
            string[] tourKeyPoints = _tourKeyPoints.Split(',');

            if (tourKeyPoints.Length < 2)
            {
                System.Windows.MessageBox.Show("At least two key points needed (begining i ending)");
                return;
            }


            _tourDTO.KeyPointsDTO.Begining = tourKeyPoints[0];
            if (tourKeyPoints.Length != 1)
            {
                for (int i = 1; i < tourKeyPoints.Length - 1; i++)
                {
                    _tourDTO.KeyPointsDTO.Middle.Add(tourKeyPoints[i]);
                }
            }
            _tourDTO.KeyPointsDTO.Ending = tourKeyPoints[tourKeyPoints.Length - 1];
            if (comboBoxType.SelectedItem == comboBoxItemSerbian)
                _tourDTO.Language = Languages.Serbian;
            else if (comboBoxType.SelectedItem == comboBoxItemEnglish)
                _tourDTO.Language = Languages.English;
            else if (comboBoxType.SelectedItem == comboBoxItemGerman)
                _tourDTO.Language = Languages.German;
            else
                _tourDTO.Language = Languages.French;

            int id = (_keyPointRepository.Save(_tourDTO.KeyPointsDTO.ToKeyPoint())).Id;
            _tourDTO.Images = _images;
            _tourDTO.KeyPointsDTO.Id = id;

            foreach (var date in _dates)
            {
                TourDTO tourDTO = new TourDTO(_tourDTO);
                tourDTO.BeginingTime = date;
                tourDTO.Images = _images;
                _repository.Save(tourDTO.ToTourAllParam());
            }


            _allToursView.Update();
            _guideMainWindow.Update();
            Close();
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


        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

  
    }
}
