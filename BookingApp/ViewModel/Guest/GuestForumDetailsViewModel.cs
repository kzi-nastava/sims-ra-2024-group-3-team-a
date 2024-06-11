using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Guest;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class GuestForumDetailsViewModel: ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _postCommand;
        private RelayCommand _lockForumCommand;

        private PostService _postService;

        private ObservableCollection<PostDTO> _postsDTO;
        private PostDTO _newPostDTO;
        private ForumDTO _forumDTO;
        private UserDTO _loggedInUser;
        private ForumDTO _selectedForumDTO;

        private List<AccommodationDTO> _myVisitedAccommodations;
        private ForumService _forumService;
        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;

        public GuestForumDetailsViewModel(ForumDTO selectedForum, UserDTO loggedInUser)
        {
            _newPostDTO = new PostDTO();
            _selectedForumDTO = selectedForum;

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _postCommand = new RelayCommand(Post);
            _lockForumCommand = new RelayCommand(LockForum);

            _loggedInUser = loggedInUser;
            _forumDTO = selectedForum;

            IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
            IPostRepository postRepository = Injector.CreateInstance<IPostRepository>();
            _postService = new PostService(postRepository, forumRepository);
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            _forumService = new ForumService(forumRepository, postRepository, accommodationRepository);
            _accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository, accommodationRepository, userRepository);
            _accommodationService = new AccommodationService(accommodationRepository);

            List<PostDTO> postsList = _postService.GetPostsForForum(selectedForum.ToForum()).Select(post => new PostDTO(post)).ToList();
            _postsDTO = new ObservableCollection<PostDTO>(postsList);

            List<AccommodationReservationDTO> myReservations = _accommodationReservationService.GetAllRatedByGuestId(loggedInUser.Id).Select(r => new AccommodationReservationDTO(r)).ToList();
            List<AccommodationDTO> allAccommodations = _accommodationService.GetAll().Select(r => new AccommodationDTO(r)).ToList();

            List<AccommodationDTO> myVisitedAccommodations = new List<AccommodationDTO>();
            foreach (var accommodation in allAccommodations)
            {
                if (myReservations.Any(r => r.AccommodationId == accommodation.Id))
                {
                    myVisitedAccommodations.Add(accommodation);
                }
            }
            _myVisitedAccommodations = myVisitedAccommodations;
        }

        private void Post()
        {
            if(_forumDTO.IsClosed == true)
            {
                MessageBox.Show("This forum is locked");
                return;
            }
            if (_newPostDTO.Text != String.Empty)
            {
                _newPostDTO.ForumId = _forumDTO.Id;
                _newPostDTO.Username = _loggedInUser.Username;

                if (_myVisitedAccommodations.Any(a => a.PlaceDTO.Country == _forumDTO.LocationDTO.Country && a.PlaceDTO.City == _forumDTO.LocationDTO.City))
                {
                    _newPostDTO.Type = PostType.SentByGuestWhoWasOnLocation;
                } 
                else
                {
                    _newPostDTO.Type = PostType.SentByGuestWhoWasNotOnLocation;
                }
                _newPostDTO.Reports = 0;

                _postsDTO.Add(new PostDTO(_postService.Save(_newPostDTO.ToPost())));
                _newPostDTO.Text = String.Empty;

                List<PostDTO> postsList = _postService.GetPostsForForum(_selectedForumDTO.ToForum()).Select(post => new PostDTO(post)).ToList();
                _postsDTO = new ObservableCollection<PostDTO>(postsList);
            }
        }
        private void LockForum()
        {
            if(_loggedInUser.Id == _forumDTO.ForumCreatorId)
            {
                _forumDTO.IsClosed = true;
                _forumService.Update(_forumDTO.ToForum());
                MessageBox.Show("Successfully locked forum");
            }
            else
            {
                MessageBox.Show("You are not creator of this forum!");
            }
        }

        public PostDTO NewPostDTO
        {
            get
            {
                return _newPostDTO;
            }
            set
            {
                _newPostDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PostDTO> PostsDTO
        {
            get
            {
                return _postsDTO;
            }
            set
            {
                _postsDTO = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PostCommand
        {
            get
            {
                return _postCommand;
            }
            set
            {
                _postCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand LockForumCommand
        {
            get
            {
                return _lockForumCommand;
            }
            set
            {
                _lockForumCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
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
            GuestMainViewWindow.SideMenuFrame.Content = new GuestSideMenuPage();
        }

        private void GoBack()
        {
            //OwnerMainWindow.MainFrame.GoBack();
        }
    }
}
