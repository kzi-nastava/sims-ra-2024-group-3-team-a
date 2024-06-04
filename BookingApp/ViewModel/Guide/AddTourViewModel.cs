using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.ViewModel.Guide
{
    public class AddTourViewModel : ViewModel
    {
        private KeyPointService _keyPointService;
        private TourService _tourService;
        private Languages _selectedLanguage;
        private RelayCommand _addImagesCommand;
        private RelayCommand _removeImageCommand;
        private List<BitmapImage> _imagePreviews;
        public ObservableCollection<BitmapImage> imagesCollection;
        private BitmapImage _selectedImage;
        private DateTime _selectedDate;
        private RelayCommand _addDateCommand;
        private RelayCommand _submitCommand;
        private RelayCommand _help;
        private string _keyPointString;
        private TourDTO _tourDTO;
        private List<string> _images;
        private ObservableCollection<DateTime> _dates;
        private UserDTO _loggedGuide;
        public event EventHandler TourAdded;
        public AddTourViewModel(UserDTO guide)
        {
            _loggedGuide = guide;
            
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            IKeyPointRepository keyPointsRepository = Injector.CreateInstance<IKeyPointRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _keyPointService = new KeyPointService(keyPointsRepository);
            _imagePreviews = new List<BitmapImage>();
            imagesCollection = new ObservableCollection<BitmapImage>(_imagePreviews);
            _tourDTO = new TourDTO();
            _submitCommand = new RelayCommand(Submit);
            _dates = new ObservableCollection<DateTime> ();
            _addDateCommand = new RelayCommand(AddDate);
            _help = new RelayCommand(Help);
            _submitCommand = new RelayCommand(Submit);
            _addImagesCommand = new RelayCommand(AddImages);
            _removeImageCommand = new RelayCommand(RemoveImage);
            countries = new List<string> { "Austrija","BiH", "Crna Gora","Francuska","Hrvatska","Italija", "Makedonija","Madjarska","Njemacka", "Srbija", "Slovenija", "Spanija" };
        }
        public RelayCommand HelpCommand
        {
            get { return _help; }
            set
            {
                _help = value;
                OnPropertyChanged();
            }
        }
        private void Help()
        {
            MessageBox.Show("Željene ključne tačke navodite putem zareza", "Uputstvo", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private List<string> countries;
        public List<string> Countries
        {
            get { return countries; }
            set
            {
                countries = value; 
                OnPropertyChanged();
            }
        }

        public void RemoveImage()
        {
            if (SelectedImage == null)
            {
                return;
            }

            int index = ImagePreviews.IndexOf(SelectedImage);
            if (index >= 0)
            {
                ImagePreviews.RemoveAt(index);
            }
            if (_images != null && _images.Count > index)
            {
                _images.RemoveAt(index);
            }
        }
        public RelayCommand RemoveImageCommand
         {
             get
             {
                 return _removeImageCommand;
             }
             set
             {
                 _removeImageCommand = value;
                 OnPropertyChanged();
             }
         }
        public ObservableCollection<BitmapImage> ImagePreviews
        {
            get
            {
                return imagesCollection;
            }
            set
            {
                imagesCollection = value;
                OnPropertyChanged();
            }
        }
        public BitmapImage SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
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
        public RelayCommand AddImagesCommand
        {
            get { return _addImagesCommand; }
            set
            {
                _addImagesCommand = value;
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
                var strings = openFileDialog.FileNames;
                _images = strings.ToList();

                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i] = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, _images[i]).ToString();
                    strings[i] = Path.GetFullPath(_images[i]);
                    BitmapImage imageSource = new BitmapImage(new Uri(strings[i]));
                    ImagePreviews.Add(imageSource);
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

