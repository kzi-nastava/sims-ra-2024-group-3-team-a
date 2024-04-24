using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.ViewModel.Guide
{
    public class AddTourViewModel : ViewModel
    {
        private KeyPointService _keyPointService;
        private TourService _tourService;
        private Languages _selectedLanguage;
        private DateTime _selectedDate;
        private RelayCommand _addDateCommand;
        private RelayCommand _submitCommand;
        private string _keyPointString;
        private TourDTO _tourDTO;
        private List<string> _images;
        private ObservableCollection<DateTime> _dates;
        private UserDTO _loggedGuide;
        public event EventHandler TourAdded;
        public AddTourViewModel(UserDTO guide)
        {
            _loggedGuide = guide;
            _keyPointService = new KeyPointService();
            _tourService = new TourService();
            _tourDTO = new TourDTO();
            _submitCommand = new RelayCommand(Submit);
            _dates = new ObservableCollection<DateTime> ();
            _addDateCommand = new RelayCommand(AddDate);
            _submitCommand = new RelayCommand(Submit);
        }
        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }
        public string KeyPointsString
        {
            get
            {
                return _keyPointString;
            }
            set
            {
                _keyPointString = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DateTime> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AddDateCommand
        {
            get { return _addDateCommand; }
            set
            {
                _addDateCommand = value;
                OnPropertyChanged();
            }
        }
        private void AddDate()
        {
            DateTime date = _selectedDate;
            Dates.Add(date);
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        private void AddImages()
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
        public RelayCommand SubmitCommand
        {
            get { return _submitCommand; }
            set
            {
                _submitCommand = value;
                OnPropertyChanged();
            }
        }
        private void Submit()
        {
            string[] tourKeyPoints = _keyPointString.Split(',');

            if (tourKeyPoints.Length < 2)
            {
                MessageBox.Show("At least two key points needed (beginning and ending)");
                return;
            }

           
            _tourDTO.Images = _images;
            _tourDTO.GuideId = _loggedGuide.Id;

            foreach (var date in _dates)
            {
                TourDTO tourDTO = new TourDTO(_tourDTO);
                tourDTO.BeginingTime = date;
                tourDTO.Images = _images;
                _tourDTO=new TourDTO(_tourService.Save(tourDTO.ToTourAllParam()));
                SetKeyPoints(tourKeyPoints);
            }
            AddTourWindow.GetInstance().Close();
        }
        public IEnumerable<Languages> Languages
        {
            get
            {
                return Enum.GetValues(typeof(Languages)).Cast<Languages>();
            }
            set { }
        }
        public Languages SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }
        private void SetKeyPoints(string[] keyPoints)
        {
            foreach (var keyPoint in keyPoints)
            {
                KeyPointDTO newPoint = new KeyPointDTO();
                newPoint.Name = keyPoint;
                newPoint.TourId = _tourDTO.Id;
                newPoint.IsCurrent = false;
                newPoint.HasPassed = false;
                _keyPointService.Save(newPoint.ToKeyPoint());
            }
        }
    }
}

