using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class SettingsViewModel:ViewModel
    {

        private RelayCommand _lightThemeCommand;
        private RelayCommand _darkThemeCommand;
        private RelayCommand _serbianLanguageCommand;
        private RelayCommand _englishLanguageCommand;
        private App app;
        public Action CloseAction { get; set; }
        private RelayCommand _closeWindowCommand;

        private const string SRB = "sr-RS";

        private const string ENG = "en-US";
        public SettingsViewModel()
        {

            _lightThemeCommand = new RelayCommand(LightThemeClick);
            _darkThemeCommand = new RelayCommand(DarkThemeClick);
            _serbianLanguageCommand = new RelayCommand(SerbianClick);
            _englishLanguageCommand = new RelayCommand(EnglishClick);
            _closeWindowCommand = new RelayCommand(CloseWindow);
        }

        private bool _isDarkModeChecked;
        public bool IsDarkModeChecked
        {
            get { return _isDarkModeChecked; }
            set
            {
                if (_isDarkModeChecked != value)
                {
                    _isDarkModeChecked = value;
                    OnPropertyChanged(nameof(IsDarkModeChecked));
                    if (_isDarkModeChecked)
                    {
                      
                    }
                }
            }
        }
        private bool _isSerbianChecked;
        public bool IsSerbianChecked
        {
            get { return _isSerbianChecked; }
            set
            {
                if (_isSerbianChecked != value)
                {
                    _isSerbianChecked = value;
                    OnPropertyChanged(nameof(IsSerbianChecked));
                    if (_isSerbianChecked)
                    {

                    }
                }
            }
        }
        private bool _isEnglishChecked;
        public bool IsEnglishChecked
        {
            get { return _isEnglishChecked; }
            set
            {
                if (_isEnglishChecked != value)
                {
                    _isEnglishChecked = value;
                    OnPropertyChanged(nameof(IsEnglishChecked));
                    if (_isEnglishChecked)
                    {

                    }
                }
            }
        }
        public RelayCommand LightThemeCommmand
        {
            get
            {
                return _lightThemeCommand;
            }
            set
            {
                _lightThemeCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand DarkThemeCommmand
        {
            get
            {
                return _darkThemeCommand;
            }
            set
            {
                _darkThemeCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand EnglishCommand
        {
            get
            {
                return _englishLanguageCommand;
            }
            set
            {
                _englishLanguageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SerbianCommand
        {
            get
            {
                return _serbianLanguageCommand;
            }
            set
            {
                _serbianLanguageCommand = value;
                OnPropertyChanged();
            }
        }

        private bool _isLightModeChecked;
        public bool IsLightModeChecked
        {
            get { return _isLightModeChecked; }
            set
            {
                if (_isLightModeChecked != value)
                {
                    _isLightModeChecked = value;
                    OnPropertyChanged(nameof(IsLightModeChecked));
                    if (_isLightModeChecked)
                    {
                       
                    }
                }
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
        private void LightThemeClick(object parameter)
        {
            AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));
        }

        private void DarkThemeClick(object parameter)
        {
            AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
        }
        private void EnglishClick(object parameter)
        {
            app = (App)System.Windows.Application.Current;
            app.ChangeLanguage(ENG);
            

        }
        private void SerbianClick(object parameter)
        {
            app = (App)System.Windows.Application.Current;
            app.ChangeLanguage(SRB);
        }
        public void CloseWindow()
        {


            CloseAction();
        }
    }
}
