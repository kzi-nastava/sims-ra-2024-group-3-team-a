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

namespace BookingApp.ViewModel.Tourist
{
    public class TourReviewViewModel : ViewModel
    { 
        private TourReviewService _tourReviewService { get; set; }

        private UserDTO _userDTO;

        private TourDTO _tourDTO;

        private List<string> _images;

        private TourReviewDTO _tourReviewDTO;

        private RelayCommand _rateTourCommand;

        private RelayCommand _cancelCommand;

        private RelayCommand _addImagesCommand;
        public TourReviewViewModel(TourDTO tourDTO, UserDTO loggedInUser)
        {
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            _tourReviewService = new TourReviewService(tourReviewRepository);
            _tourDTO = tourDTO;
            _userDTO = loggedInUser;
            _tourReviewDTO = new TourReviewDTO();
            _rateTourCommand =  new RelayCommand(RateTour);
            _addImagesCommand = new RelayCommand(AddImages);
            _cancelCommand = new RelayCommand(Cancel);
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
        public void RateTour()
        {
            _tourReviewDTO.TourId = _tourDTO.Id;
            _tourReviewDTO.TouristId = _userDTO.Id;
            _tourReviewDTO.Images = _images;
            _tourReviewService.Save(_tourReviewDTO.ToTourReview());
            MessageBox.Show("Tour is rated!");
            TourReviewWindow.GetInstance().Close();
        }

        public void AddImages()
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
        public void Cancel()
        {
            TourReviewWindow.GetInstance().Close();
        }
    }


}
