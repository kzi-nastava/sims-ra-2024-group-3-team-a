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

        public SettingsViewModel()
        {

            _lightThemeCommand = new RelayCommand(LightThemeClick);
            _darkThemeCommand = new RelayCommand(DarkThemeClick);

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

        private void LightThemeClick(object parameter)
        {
            AppTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));
        }

        private void DarkThemeClick(object parameter)
        {
            AppTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
        }
    }
}
