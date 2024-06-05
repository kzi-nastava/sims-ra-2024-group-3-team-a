using BookingApp.Commands;
using BookingApp.View.Owner.AccommodationRenovationPages;
using BookingApp.View.Owner;
using BookingApp.View.Guest;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestSideMenuViewModel : ViewModel
    {
        private RelayCommand _closeSideMenuCommand;
        private RelayCommand _showAccommodationsPageCommand;
        private RelayCommand _showReviewsPageCommand;
        private RelayCommand _showReservationsPageCommand;
        private RelayCommand _showInboxPageCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _showProfileMenuPageCommand;
        private RelayCommand _showAnywhereAnytimePageCommand;
        private RelayCommand _showForumCommand;

        public GuestSideMenuViewModel()
        {
            _closeSideMenuCommand = new RelayCommand(CloseSideMenu);
            _showAccommodationsPageCommand = new RelayCommand(ShowAccommodationsPage);
            _showReviewsPageCommand = new RelayCommand(ShowReviewsPage);
            _showForumCommand = new RelayCommand(ShowForum);
            _showInboxPageCommand = new RelayCommand(ShowInboxPage);
            _logOutCommand = new RelayCommand(LogOut);
            _showProfileMenuPageCommand = new RelayCommand(ShowProfileMenuPage);
            _showReservationsPageCommand = new RelayCommand(ShowReservationsPage);
            _showAnywhereAnytimePageCommand = new RelayCommand(ShowAnywhereAnytimePage);
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
        public RelayCommand ShowForumCommand
        {
            get
            {
                return _showForumCommand;
            }
            set
            {
                _showForumCommand = value;
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
        public RelayCommand ShowReservationsPageCommand
        {
            get
            {
                return _showReservationsPageCommand;
            }
            set
            {
                _showReservationsPageCommand = value;
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
        public RelayCommand ShowAnywhereAnytimePageCommand
        {
            get
            {
                return _showAnywhereAnytimePageCommand;
            }
            set
            {
                _showAnywhereAnytimePageCommand = value;
                OnPropertyChanged();
            }
        }

        private void CloseSideMenu()
        {
            GuestMainViewWindow.SideMenuFrame.Content = null;
        }
        public void ShowAccommodationsPage()
        {
            GuestMainViewWindow.MainFrame.Content = new GuestHomePage(GuestMainViewWindow.LoggedInGuest);
        }
        public void ShowReviewsPage()
        {
            GuestMainViewWindow.MainFrame.Content = new ReviewsPage();
        }
        public void ShowForum()
        {
            GuestMainViewWindow.MainFrame.Content = new GuestForumPage(GuestMainViewWindow.LoggedInGuest);
        }
        public void ShowReservationsPage()
        {
            GuestMainViewWindow.MainFrame.Content = new MyReservationsPage(GuestMainViewWindow.LoggedInGuest);
        }
        public void ShowInboxPage()
        {
            GuestMainViewWindow.MainFrame.Content = new GuestInboxPage();
        }
        public void ShowAnywhereAnytimePage()
        {
            GuestMainViewWindow.MainFrame.Content = new AnywhereAnytimePage();
        }
        private void LogOut()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuestMainViewWindow.GetInstance().Close();
        }
        public void ShowProfileMenuPage()
        {
            //OwnerMainWindow.MainFrame.Content = new ProfileMenuPage(OwnerMainWindow.LoggedInOwner);
            GuestMainViewWindow.MainFrame.Content = new MyProfilePage();
        }
    }
}
