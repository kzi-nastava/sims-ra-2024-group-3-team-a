using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.ForumPages;
using BookingApp.View.Owner.WizardAndHelp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace BookingApp.ViewModel.Owner.ForumViewModels
{
    public class ForumsViewModel : ViewModel
    {
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _showForumHelpCommand;

        private UserDTO _loggedInUser;
        private ForumService _forumService;
        private OwnerSettingsService _ownerSettingsService;
        
        private ObservableCollection<ForumDTO> _forumsDTO;
        private ForumDTO _selectedForumDTO;
        private OwnerSettings _ownerSettings;

        public ForumsViewModel(UserDTO loggedInUser)
        {
            IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
            IPostRepository postRepository = Injector.CreateInstance<IPostRepository>();
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IOwnerSettingsRepository ownerSettingsRepository = Injector.CreateInstance<IOwnerSettingsRepository>();
            _forumService = new ForumService(forumRepository, postRepository, accommodationRepository);
            _ownerSettingsService = new OwnerSettingsService(ownerSettingsRepository);

            List<ForumDTO> forumsList = _forumService.GetForUser(loggedInUser.ToUser()).Select(forum => new ForumDTO(forum)).ToList();
            _forumsDTO = new ObservableCollection<ForumDTO>(forumsList);

            _loggedInUser = loggedInUser;
            _ownerSettings = _ownerSettingsService.GetOwnerSettingsByOwner(loggedInUser.ToUser());


            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _showForumHelpCommand = new RelayCommand(ShowForumHelp);
        }

        public OwnerSettings OwnerSettings
        {
            get
            {
                return _ownerSettings;
            }
            set
            {
                _ownerSettings = value;
            }
        }
        public ForumDTO SelectedForumDTO
        {
            get
            {
                return _selectedForumDTO;
            }
            set
            {
                _selectedForumDTO = value;
                OnPropertyChanged();
                ShowForumDetailsPage();
            }
        }
        public ObservableCollection<ForumDTO> ForumsDTO
        {
            get
            {
                return _forumsDTO;
            }
            set
            {
                _forumsDTO = value;
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
            }
        }
        public RelayCommand ShowForumHelpCommand
        {
            get
            {
                return _showForumHelpCommand;
            }
            set
            {
                _showForumHelpCommand = value;
            }
        }

        private void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void ShowForumDetailsPage()
        {
            OwnerMainWindow.MainFrame.Content = new ForumDetailsPage(SelectedForumDTO, _loggedInUser);
            _selectedForumDTO = null;
        }

        private void ShowForumHelp()
        {
            OwnerMainWindow.MainFrame.Content = new ForumHelpPage(_loggedInUser);
        }
    }
}
