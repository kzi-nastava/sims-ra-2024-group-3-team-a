using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class SettingsViewModel : ViewModel
    {
        private UserDTO _loggedInUser;
        private OwnerSettingsService _ownerSettingsService;
        private OwnerSettings _ownerSettings;

        private RelayCommand _showSideMenuCommand;

        private bool _isSwitchOn;

        public SettingsViewModel(UserDTO loggedInUser)
        {
            _loggedInUser = loggedInUser;

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);

            IOwnerSettingsRepository ownerSettingsRepository = Injector.CreateInstance<IOwnerSettingsRepository>();
            _ownerSettingsService = new OwnerSettingsService(ownerSettingsRepository);

            _ownerSettings = _ownerSettingsService.GetOwnerSettingsByOwner(loggedInUser.ToUser());
            _isSwitchOn = _ownerSettings.HelpOn;
        }

        public bool IsSwitchOn
        {
            get => _isSwitchOn;
            set
            {
                _isSwitchOn = value;
                _ownerSettings.HelpOn = value;
                _ownerSettingsService.Update(_ownerSettings);
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

        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
    }
}
