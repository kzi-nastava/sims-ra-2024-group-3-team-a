using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class PostService
    {
        /*private IForumRepository _forumRepository;
        private IPostRepository _postRepository;
        private IAccommodationRepository _accommodationRepository;

        public ForumService(IForumRepository forumRepository, IPostRepository postRepository, IAccommodationRepository accommodationRepository)
        {
            _forumRepository = forumRepository;
            _postRepository = postRepository;
        }*/

        private IPostRepository _postRepository;
        private IForumRepository _forumRepository;

        public PostService(IPostRepository postRepository, IForumRepository forumRepository)
        {
            _postRepository = postRepository;
            _forumRepository = forumRepository;
        }

        public List<Post> GetAll()
        {
            return _postRepository.GetAll();
        }

        public Post Save(Post post)
        {
            return _postRepository.Save(post);
        }

        public Post Update(Post post)
        {
            return _postRepository.Update(post);
        }

        public void Delete(Post post)
        {
            _postRepository.Delete(post);
        }

        public List<Post> GetPostsForForum(Forum forum)
        {
            return _postRepository.GetPostsForForum(forum);
        }
    }
}
