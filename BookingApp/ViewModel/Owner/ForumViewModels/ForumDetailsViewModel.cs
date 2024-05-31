using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.View.Owner.AnswerRequestPages;
using BookingApp.View.Owner.ForumPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner.ForumViewModels
{
    public class ForumDetailsViewModel : ViewModel
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _showSideMenuCommand;
        private RelayCommand _postCommand;

        private PostService _postService;

        private ObservableCollection<PostDTO> _postsDTO;
        private PostDTO _newPostDTO;
        private ForumDTO _forumDTO;
        private UserDTO _loggedInUser;

        public ForumDetailsViewModel(ForumDTO selectedForum, UserDTO loggedInUser)
        {
            _newPostDTO = new PostDTO();

            _goBackCommand = new RelayCommand(GoBack);
            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
            _postCommand = new RelayCommand(Post);

            _loggedInUser = loggedInUser;
            _forumDTO = selectedForum;

            IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
            IPostRepository postRepository = Injector.CreateInstance<IPostRepository>();
            _postService = new PostService(postRepository, forumRepository);

            List<PostDTO> postsList = _postService.GetPostsForForum(selectedForum.ToForum()).Select(post => new PostDTO(post)).ToList();
            _postsDTO = new ObservableCollection<PostDTO>(postsList);
        }
        
        private void Post()
        {
            if(_newPostDTO.Text != String.Empty)
            {
                _newPostDTO.ForumId = _forumDTO.Id;
                _newPostDTO.Username = _loggedInUser.Username;
                _newPostDTO.Type = PostType.SentByOwner;
                _newPostDTO.Reports = 0;

                _postsDTO.Add(new PostDTO(_postService.Save(_newPostDTO.ToPost())));
                _newPostDTO.Text = String.Empty;
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
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void GoBack()
        {
            OwnerMainWindow.MainFrame.GoBack();
        }
    }
}
