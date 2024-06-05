using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;
using BookingApp.View.Tourist;
using System.Windows;
using BookingApp.Repository.Interfaces;
using BookingApp.InjectorNameSpace;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReviewViewModel : Validation.ValidationBase
    { 
        private TourReviewService _tourReviewService { get; set; }

        private UserDTO _userDTO;

        private TourDTO _tourDTO;

        private List<string> _images;

        private TourReviewDTO _tourReviewDTO;

        private BitmapImage _selectedImage;

        private RelayCommand _rateTourCommand;

        private RelayCommand _cancelCommand;

        private RelayCommand _addImagesCommand;
        private RelayCommand _showMyToursWindowCommand;
        private RelayCommand _showInboxWindowCommand;
        private RelayCommand _showFinishedToursWindowCommand;
        private RelayCommand _showVoucherWindowCommand;
        private RelayCommand _removeImageCommand;
        private RelayCommand _closeWindowCommand;
        private RelayCommand _showTouristMainWindowCommand;
        private App app;
        private string _currentLanguage;
        public Action CloseAction { get; set; }
        private List<BitmapImage> _imagePreviews;
        public ObservableCollection<BitmapImage> imagesCollection;
        public TourReviewViewModel(TourDTO tourDTO, UserDTO loggedInUser)
        {
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            _imagePreviews = new List<BitmapImage>();
            imagesCollection = new ObservableCollection<BitmapImage>(_imagePreviews);
            _tourReviewService = new TourReviewService(tourReviewRepository);
            _tourDTO = tourDTO;
            _userDTO = loggedInUser;
            _tourReviewDTO = new TourReviewDTO();
            _rateTourCommand =  new RelayCommand(RateTour);
            _addImagesCommand = new RelayCommand(AddImages);
            _closeWindowCommand = new RelayCommand(CloseWindow);
            var currentLanguage = App.Instance.CurrentLanguage.Name;
            _currentLanguage = currentLanguage;


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

        public TourReviewDTO TourReviewDTO
        {
            get
            {
                return _tourReviewDTO;
            }
            set
            {
                _tourReviewDTO = value;
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


        public RelayCommand RateTourCommand
        {
            get
            {
                return _rateTourCommand;
            }
            set
            {
                _rateTourCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddImagesCommand
        {
            get
            {
                return _addImagesCommand;
            }
            set
            {
                _addImagesCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
                OnPropertyChanged();
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
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand;
            }
            set
            {
                _closeWindowCommand = value;
                OnPropertyChanged();
            }
        }
       


        protected override void ValidateSelf1()
        {

            if (!int.TryParse(_tourReviewDTO.GuideKnowledgeRating.ToString(), out int GuideKnowledgeRating) || GuideKnowledgeRating <= 0)
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["GuideKnowledgeRating"] = "You didn't rate Guide's knowledge";
                }
                else
                {
                    ValidationErrors["GuideKnowledgeRating"] = "Niste ocijenili vodicevo znanje";
                }
                
            }

            if (!int.TryParse(_tourReviewDTO.GuideLanguageRating.ToString(), out int GuideLanguageRating) || GuideLanguageRating <= 0)
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["GuideLanguageRating"] = "You didn't rate Guide's language knowledge";
                }
                else
                {
                    ValidationErrors["GuideLanguageRating"] = "Niste ocijenili vodicev jezik";
                }
                   
            }
            if (!int.TryParse(_tourReviewDTO.TourEntertainmentRating.ToString(), out int TourEntertainmentRating) || TourEntertainmentRating <= 0)
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["TourEntertainmentRating"] = "You didn't rate Tour's entertainment";
                }
                else
                {
                    ValidationErrors["TourEntertainmentRating"] = "Niste ocijenili zanimljivost ture";
                }
                   
            }
            if(string.IsNullOrWhiteSpace(_tourReviewDTO.Comment))
            {
                if (_currentLanguage.Equals("en-US"))
                {
                    ValidationErrors["Comment"] = "Please leave a comment";
                }
                else
                {
                    ValidationErrors["Comment"] = "Molimo vas unesite komentar";
                }
                   
            }






            OnPropertyChanged(nameof(ValidationErrors));
        }

        protected override void ValidateSelf2()
        {
            OnPropertyChanged(nameof(ValidationErrors));
        }

        public void RateTour()
        {
            Validate1();
            if(IsValid)
            {
                _tourReviewDTO.TourId = _tourDTO.Id;
                _tourReviewDTO.TouristId = _userDTO.Id;
                _tourReviewDTO.Images = _images;
                _tourReviewService.Save(_tourReviewDTO.ToTourReview());
                if (_currentLanguage.Equals("en-US"))
                {
                    MessageBox.Show("Tour is rated!");
                    CloseAction();
                }
                else
                {
                    MessageBox.Show("Tura je ocijenjena!");
                    CloseAction();
                }
                    
            }
            
        }

        public void AddImages()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
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
        public void RemoveImage()
        {
            if (_selectedImage == null)
            {
                return;
            }
            var selectedItem = _selectedImage as BitmapImage;
            ImagePreviews?.Remove(selectedItem);

        }
      

        
       

        public void CloseWindow()
        {
            CloseAction();
        }
    }


}
