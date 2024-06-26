﻿using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    class GuideMainViewModel : ViewModel
    {
        private static ObservableCollection<TourDTO> _toursTodayDTO { get; set; }
       
        private Boolean _doesActiveTourExist = false;
        private readonly TourService _tourService;
        private readonly UserService _userService;
        private readonly SuperGuideService _superGuideService;
        private TourDTO _selectedTourDTO = null;
        private UserDTO _loggedInGuide;
        private TourDTO _mostVisitedTourDTO;
        private RelayCommand _showActiveTourCommand;
        private RelayCommand _showAllToursCommand;
        private RelayCommand _showTourStatisticsCommand;
        private RelayCommand _borderClickedCommand;
        private RelayCommand _showTourDetailsCommand;
        private RelayCommand _showTourRequestCommand;
        private RelayCommand _addNewTourCommand;
        private RelayCommand _shortcuts;
        private RelayCommand _logoutCommand;
        private RelayCommand _quitCommand;
        private RelayCommand _help;
        private RelayCommand _showLanguagesCommand;
        private RelayCommand _showtutCommand;
        public GuideMainViewModel(User guide)
        {
           
            _loggedInGuide = new UserDTO(guide);
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            ISuperGuideRepository superGuideRepository = Injector.CreateInstance<ISuperGuideRepository>();
            _superGuideService = new SuperGuideService(userRepository, superGuideRepository, tourRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);
            _userService = new UserService(userRepository);
            List<TourDTO> toursDTO = _tourService.GetTodayTours(guide).Select(tour => new TourDTO(tour)).ToList();
            _toursTodayDTO = new ObservableCollection<TourDTO>(toursDTO);
            _showActiveTourCommand = new RelayCommand(ShowActiveTour);
            _showAllToursCommand = new RelayCommand(ShowAllTours);
            _showTourStatisticsCommand = new RelayCommand(ShowTourStatistics);
            _showTourDetailsCommand = new RelayCommand(ShowTourDetails);
            _showTourRequestCommand = new RelayCommand(ShowTourRequest);
            _borderClickedCommand = new RelayCommand(ShowAllTours);
            _addNewTourCommand = new RelayCommand(AddNewTour);
            _logoutCommand = new RelayCommand(Logout);
            _quitCommand = new RelayCommand(Quit);
            _help = new RelayCommand(Help);
            _shortcuts = new RelayCommand(Shortcuts);
            _showLanguagesCommand = new RelayCommand(ShowLanguages);
            _showtutCommand = new RelayCommand(ShowTut);
            if (_tourService.GetMostVisitedTour() != null)
            {
                _mostVisitedTourDTO = new TourDTO(_tourService.GetMostVisitedTour());
            }
            else
            {
                _mostVisitedTourDTO = null;
            }
            ActiveTourExists();
        }
        public ObservableCollection<TourDTO> ToursTodayDTO
        {
            get { return _toursTodayDTO; }
            set
            {
                _toursTodayDTO = value;
                OnPropertyChanged();
            }
        }
       
        private void ActiveTourExists()
        {
            foreach ( TourDTO tour in _toursTodayDTO )
            {
                if (tour.IsActive)
                {
                    _doesActiveTourExist = true;
                    break;
                }
                {
                    _doesActiveTourExist = false;
                }
            }
        }
        public TourDTO SelectedTourDTO
        {
            get { return _selectedTourDTO; }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
                ShowActiveTour();
            }
        }
        public TourDTO MostVisitedTour
        {
            get { return _mostVisitedTourDTO; }
            set
            {
                _mostVisitedTourDTO = value;
                OnPropertyChanged();
                ShowActiveTour();
            }
        }
        public RelayCommand ShowActiveTourCommand
        {
            get { return _showActiveTourCommand; }
            set
            {
                _showActiveTourCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand AddNewTourCommand
        {
            get { return _addNewTourCommand; }
            set
            {
                _addNewTourCommand = value;
                OnPropertyChanged();
            }
        }
        private void AddNewTour()
        {
            AddTourWindow addTour = new AddTourWindow(_loggedInGuide);
            addTour.Show();
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
            MessageBox.Show("Ukoliko ste u toku prethodne godine ostvarili prosječnu ocjenu na turama veću od 4.0 na određenom jeziku i uz to održali bar 20 tura na tom jeziku- dobijate titulu supervodiča. Kada ova titula dobije boju, to je znak da posjedujete titulu. Ukoliko je siva i onemogućena za klik- nemate titulu supervodiča", "Uputstvo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public RelayCommand ShortcutsCommand
        {
            get { return _shortcuts; }
            set
            {
                _shortcuts = value;
                OnPropertyChanged();
            }
        }
        private void Shortcuts()
        {
            MessageBox.Show("CTRL + N    Dodaj novu turu\nCTRL + Z     Zahtjevi za ture\nCTRL + T     Prikaz svih tura\nCTRL + S     Statistika tura ", "Prečice", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public RelayCommand BorderClickedCommand
        {
            get { return _borderClickedCommand; }
            set
            {
                _borderClickedCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand QuitCommand
        {
            get { return _quitCommand; }
            set
            {
                _quitCommand = value;
                OnPropertyChanged();
            }
        }
        private void Quit()
        {
            _tourService.CancelUpcoming(_loggedInGuide.ToUser());
          
            if (_loggedInGuide.IsSuper == true)
            {
                foreach(SuperGuide guide in _superGuideService.GetAll())
                {
                    if(guide.GuideId == _loggedInGuide.Id)
                    {
                        _superGuideService.Delete(guide);
                    }
                }
            }
            _userService.Delete(_loggedInGuide.ToUser());
            MessageBox.Show("Uspjesno ste dali otkaz", "Obavjestenje", MessageBoxButton.OK, MessageBoxImage.Information);

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuideMainWindow.GetInstance().Close();
        }
        private void ShowActiveTour()
        {
            if (_selectedTourDTO.CurrentKeyPoint != "finished")
            {
                if (_doesActiveTourExist == true && _selectedTourDTO.IsActive != true)
                {
                    MessageBox.Show("Druga tura je već započela", "Obavještenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                ActiveTourWindow tourDetailsWindow = new ActiveTourWindow(_selectedTourDTO, _doesActiveTourExist, _loggedInGuide);
               
                tourDetailsWindow.Show();
                _tourService.Update(_selectedTourDTO.ToTourAllParam());
                GuideMainWindow.GetInstance().Close();          

            }
            else
            {
                MessageBox.Show("Ova tura je već završena", "Obavještenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public RelayCommand ShowAllToursCommand
        {
            get { return _showAllToursCommand; }
            set
            {
                _showAllToursCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowAllTours()
        {
            AllToursWindow allToursView = new AllToursWindow(_loggedInGuide);
            allToursView.Show();
            GuideMainWindow.GetInstance().Close();
        }
        public RelayCommand ShowTutCommand
        {
            get { return _showtutCommand; }
            set
            {
                _showtutCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTut()
        {
            Tutorijal tut = new Tutorijal();
            tut.Show();
        }
        public RelayCommand ShowTourStatisticsCommand
        {
            get { return _showTourStatisticsCommand; }
            set
            {
                _showTourStatisticsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourStatistics()
        {
            TourStatisticsWindow tourStatistics = new TourStatisticsWindow(_loggedInGuide);
            tourStatistics.Show();
            GuideMainWindow.GetInstance().Close();
        }
        public RelayCommand ShowTourDetailsCommand
        {
            get { return _showTourDetailsCommand; }
            set
            {
                _showTourDetailsCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourDetails(object parameter)
        {
            TourDTO selectedTourDTO = parameter as TourDTO;
            TourDetailsWindow details = new TourDetailsWindow(selectedTourDTO, _loggedInGuide);
            details.Show();

        }
        public RelayCommand ShowTourRequestCommand
        {
            get { return _showTourRequestCommand; }
            set
            {
                _showTourRequestCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowTourRequest()
        {
            TourRequestWindow requests = new TourRequestWindow(_loggedInGuide);
            requests.Show();
            GuideMainWindow.GetInstance().Close();

        }
        public RelayCommand ShowLanguagesCommand
        {
            get { return _showLanguagesCommand; }
            set
            {
                _showLanguagesCommand = value;
                OnPropertyChanged();
            }
        }
        private void ShowLanguages()
        {
            String languages = "";
            foreach (SuperGuide guide in _superGuideService.GetAll())
            {
                if (guide.GuideId == _loggedInGuide.Id)
                {
                    if(languages.Length < 2) {
                        languages += guide.Language.ToString();
                    }
                    else
                    {
                        languages += ", ";
                        languages += guide.Language.ToString();
                    }
                }
            }

            MessageBox.Show(languages, "", MessageBoxButton.OK, MessageBoxImage.None);

        }
        public UserDTO User
        {
            get { return _loggedInGuide; }
            set
            {
                _loggedInGuide = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set
            {
                _logoutCommand = value;
                OnPropertyChanged();
            }
        }
        private void Logout()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuideMainWindow.GetInstance().Close();
        }
    }
}
