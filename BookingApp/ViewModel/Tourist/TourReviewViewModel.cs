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

namespace BookingApp.ViewModel.Tourist
{
    public class TourReviewViewModel : ViewModel
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
          
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showMyToursWindowCommand = new RelayCommand(ShowMyToursWindow);
            _showVoucherWindowCommand = new RelayCommand(ShowVoucherWindow);
            _showInboxWindowCommand = new RelayCommand(ShowinboxWindow);
            _removeImageCommand = new RelayCommand(RemoveImage);
            _closeWindowCommand = new RelayCommand(CloseWindow);
            _showTouristMainWindowCommand = new RelayCommand(ShowTouristMainWindow);
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

        public RelayCommand ShowMyToursWindowCommand
        {
            get
            {
                return _showMyToursWindowCommand;
            }
            set
            {
                _showMyToursWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowInboxWindowCommand
        {
            get
            {
                return _showInboxWindowCommand;
            }
            set
            {
                _showInboxWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowFinishedToursWindowCommand
        {
            get
            {
                return _showFinishedToursWindowCommand;
            }
            set
            {
                _showFinishedToursWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowVoucherWindowCommand
        {
            get
            {
                return _showVoucherWindowCommand;
            }
            set
            {
                _showVoucherWindowCommand = value;
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
        public RelayCommand ShowTouristMainWindowCommand
        {
            get
            {
                return _showTouristMainWindowCommand;
            }
            set
            {
                _showTouristMainWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public void RateTour()
        {
            _tourReviewDTO.TourId = _tourDTO.Id;
            _tourReviewDTO.TouristId = _userDTO.Id;
            _tourReviewDTO.Images = _images;
            _tourReviewService.Save(_tourReviewDTO.ToTourReview());
            MessageBox.Show("Tour is rated!");
            CloseAction();
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
        public void ShowFinishedToursWindow()
        {
            FinishedToursWindow finishedToursWindow = new FinishedToursWindow(_userDTO);
            finishedToursWindow.Owner = Application.Current.MainWindow;
           
            finishedToursWindow.ShowDialog();
        }

        public void ShowMyToursWindow()
        {
            MyToursWindow myToursWindow = new MyToursWindow();
            myToursWindow.ShowDialog();
        }

        public void ShowVoucherWindow()
        {
            VoucherWindow voucherWindow = new VoucherWindow(_userDTO);
            voucherWindow.ShowDialog();
        }

        public void ShowinboxWindow()
        {
            InboxWindow inboxWindow = new InboxWindow(_userDTO);
            inboxWindow.ShowDialog();
        }
        public void ShowTouristMainWindow()
        {


            TouristMainWindow touristMainWindow = new TouristMainWindow(_userDTO.ToUser());
            touristMainWindow.ShowDialog();


        }

        public void CloseWindow()
        {
            CloseAction();
        }
    }


}
