using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Owner
{
    public class AddAccommodationViewModel : ViewModel
    {
        private AccommodationDTO _accommodationDTO;
        private AccommodationService _accommodationService;  

        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _addCommand;
        private RelayCommand _addImagesCommand;

        private UserDTO _loggedInOwner;
        private List<string> _images; 
        public AddAccommodationViewModel(UserDTO loggedInOwner)
        {
            _loggedInOwner = loggedInOwner;

            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>(); 
            _accommodationService = new AccommodationService(accommodationRepository);

            _images = new List<string>();

            _accommodationDTO = new AccommodationDTO();
            _accommodationDTO.OwnerId = _loggedInOwner.Id;
            _accommodationDTO.CancellationPeriod = 1;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _addCommand = new RelayCommand(Add);
            _addImagesCommand = new RelayCommand(AddImages);
        }

        public AccommodationDTO AccommodationDTO
        {
            get
            {
                return _accommodationDTO;
            }
            set
            {
                _accommodationDTO = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<AccommodationType> AccommodationTypes
        {
            get
            {
                return Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>(); 
            }
            set { }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand;
            }
            set
            {
                _showSideMenuCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand;
            }
            set
            {
                _addCommand = value;
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

        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
        private void Add()
        {
            _accommodationDTO.Images = _images;

            _accommodationService.Save(_accommodationDTO.ToAccommodation());

            OwnerMainWindow.MainFrame.Content = new AccommodationsPage(_loggedInOwner);
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
    }
}
