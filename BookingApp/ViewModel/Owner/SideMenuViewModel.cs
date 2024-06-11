using BookingApp.View.Owner;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Commands;
using BookingApp.View.Owner.AccommodationRenovationPages;
using BookingApp.View.Owner.ForumPages;
using GalaSoft.MvvmLight.Messaging;

namespace BookingApp.ViewModel.Owner
{
    public class SideMenuViewModel : ViewModel
    {
        private RelayCommand _closeSideMenuCommand;
        private RelayCommand _showAccommodationsPageCommand;
        private RelayCommand _showReviewsPageCommand;
        private RelayCommand _showInboxPageCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _showProfileMenuPageCommand;
        private RelayCommand _showRenovationsPageCommand;
        private RelayCommand _showForumsPageCommand;
        private RelayCommand _showSettingsPageCommand;

        public SideMenuViewModel()
        {
            _closeSideMenuCommand = new RelayCommand(CloseSideMenu);
            _showAccommodationsPageCommand = new RelayCommand(ShowAccommodationsPage);
            _showReviewsPageCommand = new RelayCommand(ShowReviewsPage);
            _showInboxPageCommand = new RelayCommand(ShowInboxPage);
            _logOutCommand = new RelayCommand(LogOut);
            _showProfileMenuPageCommand = new RelayCommand(ShowProfileMenuPage);
            _showRenovationsPageCommand = new RelayCommand(ShowRenovationsPage);
            _showForumsPageCommand = new RelayCommand(ShowForumsPage);
            _showSettingsPageCommand = new RelayCommand(ShowSettingsPage);
        }


        public RelayCommand CloseSideMenuCommand
        {
            get
            {
                return _closeSideMenuCommand;
            }
            set
            {
                _closeSideMenuCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowAccommodationsPageCommand
        {
            get
            {
                return _showAccommodationsPageCommand;
            }
            set
            {
                _showAccommodationsPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowReviewsPageCommand
        {
            get
            {
                return _showReviewsPageCommand;
            }
            set
            {
                _showReviewsPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowInboxPageCommand
        {
            get
            {
                return _showInboxPageCommand;
            }
            set
            {
                _showInboxPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand LogOutCommand
        {
            get
            {
                return _logOutCommand;
            }
            set
            {
                _logOutCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowProfileMenuPageCommand
        {
            get
            {
                return _showProfileMenuPageCommand;
            }
            set
            {
                _showProfileMenuPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowRenovationsPageCommand
        {
            get
            {
                return _showRenovationsPageCommand;
            }
            set
            {
                _showRenovationsPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowForumsPageCommand
        {
            get
            {
                return _showForumsPageCommand;
            }
            set
            {
                _showForumsPageCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowSettingsPageCommand
        {
            get
            {
                return _showSettingsPageCommand;
            }
            set
            {
                _showSettingsPageCommand = value;
                OnPropertyChanged();
            }
        }

        public void CloseSideMenu()
        {
            Messenger.Default.Send(new NotificationMessage("CloseSideMenu"));
        }
        public void ShowAccommodationsPage()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationsPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        public void ShowReviewsPage()
        {
            OwnerMainWindow.MainFrame.Content = new ReviewsPage();
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        public void ShowInboxPage()
        {
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        private void LogOut()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            OwnerMainWindow.GetInstance().Close();
        }
        public void ShowRenovationsPage()
        {
            OwnerMainWindow.MainFrame.Content = new RenovationsPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        public void ShowProfileMenuPage()
        {
            OwnerMainWindow.MainFrame.Content = new ProfileMenuPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }

        public void ShowForumsPage()
        {
            OwnerMainWindow.MainFrame.Content = new ForumsPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }

        public void ShowSettingsPage()
        {
            OwnerMainWindow.MainFrame.Content = new SettingsPage(OwnerMainWindow.LoggedInOwner);
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
    }
}
