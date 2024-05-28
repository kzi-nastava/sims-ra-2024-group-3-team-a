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

        public SideMenuViewModel()
        {
            _closeSideMenuCommand = new RelayCommand(CloseSideMenu);
            _showAccommodationsPageCommand = new RelayCommand(ShowAccommodationsPage);
            _showReviewsPageCommand = new RelayCommand(ShowReviewsPage);
            _showInboxPageCommand = new RelayCommand(ShowInboxPage);
            _logOutCommand = new RelayCommand(LogOut);
            _showProfileMenuPageCommand = new RelayCommand(ShowProfileMenuPage);
            _showRenovationsPageCommand = new RelayCommand(ShowRenovationsPage);
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

        public void CloseSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = null;
        }
        public void ShowAccommodationsPage()
        {
            OwnerMainWindow.MainFrame.Content = new AccommodationsPage(OwnerMainWindow.LoggedInOwner);
        }
        public void ShowReviewsPage()
        {
            OwnerMainWindow.MainFrame.Content = new ReviewsPage();
        }
        public void ShowInboxPage()
        {
            OwnerMainWindow.MainFrame.Content = new InboxPage(OwnerMainWindow.LoggedInOwner);
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
        }
        public void ShowProfileMenuPage()
        {
            OwnerMainWindow.MainFrame.Content = new ProfileMenuPage(OwnerMainWindow.LoggedInOwner);
        }
    }
}
