using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner.ForumPages;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest;
using BookingApp.Model;

namespace BookingApp.ViewModel.Guest
{
    public class GuestForumViewModel: ViewModel
    { 
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _newForumPageCommand;
        private RelayCommand _makeNewForumCommand;
        private UserDTO _loggedInUser;

        private ForumService _forumService;
        private PostService _postService;
        private AccommodationReservationService _accommodationReservationService;
        private UserService _userService;
        private AccommodationService _accommodationService;

        private List<AccommodationDTO> _myVisitedAccommodations;

        private ObservableCollection<ForumDTO> _forumsDTO;
        private ForumDTO _selectedForumDTO;

    public GuestForumViewModel(UserDTO loggedInUser)
    {
        IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
        IPostRepository postRepository = Injector.CreateInstance<IPostRepository>();
        IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
        _forumService = new ForumService(forumRepository, postRepository, accommodationRepository);
        _postService = new PostService(postRepository, forumRepository);
        _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
        _accommodationService = new AccommodationService(accommodationRepository);
        
        List<ForumDTO> forumsList = _forumService.GetAll().Select(forum => new ForumDTO(forum)).ToList();
        _forumsDTO = new ObservableCollection<ForumDTO>(forumsList);

        List<AccommodationReservationDTO> myReservations = _accommodationReservationService.GetAllRatedByGuestId(loggedInUser.Id).Select(r => new AccommodationReservationDTO(r)).ToList();
        List<AccommodationDTO> allAccommodations = _accommodationService.GetAll().Select(r => new AccommodationDTO(r)).ToList();

            List<AccommodationDTO> myVisitedAccommodations = new List<AccommodationDTO>();
            foreach (var accommodation in allAccommodations)
            {
                if(myReservations.Any(r => r.AccommodationId == accommodation.Id))
                {
                    myVisitedAccommodations.Add(accommodation);
                }
            }
            _myVisitedAccommodations = myVisitedAccommodations;
        _loggedInUser = loggedInUser;
        _showSideMenuCommand = new RelayCommand(ShowSideMenu);
        _newForumPageCommand = new RelayCommand(ShowNewForumPage);
        _makeNewForumCommand = new RelayCommand(MakeNewForum);
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
    public RelayCommand NewForumPageCommand
    {
        get
        {
            return _newForumPageCommand;
        }
        set
        {
            _newForumPageCommand = value;
        }
    }
    public RelayCommand MakeNewForumCommand
    {
        get
        {
            return _makeNewForumCommand;
        }
        set
        {
            _makeNewForumCommand = value;
        }
    }
    private void ShowNewForumPage()
    {
        GuestMainViewWindow.MainFrame.Content = new NewForumPage();
    }
        private void MakeNewForum()
        {
            Location location = new Location(_forumCity, _forumCountry);
            Forum forum = new Forum(0, location, _loggedInUser.Id, false, false, false);
            _forumService.Save(forum);
            List<string> listOwnersReported = new List<string>();
            if (_myVisitedAccommodations.Any(a => a.PlaceDTO.City == _forumCity && a.PlaceDTO.Country == _forumCountry))
            {
                Post post = new Post(0, forum.Id, _loggedInUser.Username, _forumFirstPost, 0, Model.Enums.PostType.SentByGuestWhoWasOnLocation, listOwnersReported);
                _postService.Save(post);
            }
            else
            {
                Post post = new Post(0, forum.Id, _loggedInUser.Username, _forumFirstPost, 0, Model.Enums.PostType.SentByGuestWhoWasNotOnLocation, listOwnersReported);
                _postService.Save(post);
            }
            
        }
    private void ShowSideMenu()
    {
        GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
    }

    private void ShowForumDetailsPage()
    {
        GuestMainViewWindow.MainFrame.Content = new GuestForumDetailsPage(SelectedForumDTO, _loggedInUser);
    }
    private string _forumCity;
    public string ForumCity
    {
        get
        {
            return _forumCity;
        }
        set
        {
            _forumCity = value;
        }
    }
    private string _forumCountry;
    public string ForumCountry
    {
            get
            {
                return _forumCountry;
            }
            set
            {
                _forumCountry = value;
            }
    }
        private string _forumFirstPost;
        public string ForumFirstPost
        {
            get
            {
                return _forumFirstPost;
            }
            set
            {
                _forumFirstPost = value;
            }
        }





    }

}
