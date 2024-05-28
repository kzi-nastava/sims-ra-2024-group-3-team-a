using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class ProfileMenuViewModel : ViewModel
    {
        private RelayCommand _showSideMenuCommand;
        private UserDTO _ownerDTO;

        public ProfileMenuViewModel(UserDTO loggedInUser)
        {
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _ownerDTO = loggedInUser;
        }

        public UserDTO OwnerDTO
        {
            get
            {
                return _ownerDTO;
            }
            set
            {
                _ownerDTO = value;
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

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }
    }
}
