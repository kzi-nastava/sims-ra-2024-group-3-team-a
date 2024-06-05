using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class ForumService
    {
        private IForumRepository _forumRepository;  
        private IPostRepository _postRepository;
        private IAccommodationRepository _accommodationRepository;

        public ForumService(IForumRepository forumRepository, IPostRepository postRepository, IAccommodationRepository accommodationRepository)
        {
            _forumRepository = forumRepository;
            _postRepository = postRepository;
            _accommodationRepository = accommodationRepository;
        }

        public List<Forum> GetAll()
        {
            List<Forum> forums = _forumRepository.GetAll();

            for (int i = 0; i < forums.Count; i++)
            {
                if (IsImportant(forums[i]))
                {
                    forums[i].IsVeryImportant = true;
                }
            }

            return forums;
        }

        private bool IsImportant(Forum forum)
        {
            List<Post> posts = _postRepository.GetPostsForForum(forum);

            if (posts.Count(post => post.Type == PostType.SentByOwner) >= 10 || posts.Count(post => post.Type == PostType.SentByGuestWhoWasOnLocation) >= 20)
            {
                return true;
            }
            return false;
        }

        public List<Forum> GetForUser(User user)
        {
            List<Location> locations = _accommodationRepository.GetByOwner(user.Id).Select(a => a.Place).ToList(); 
            return GetAll().Where(forum => locations.Any(loc => loc.Country == forum.Location.Country && loc.City == forum.Location.City)).ToList(); 
        }

        public Forum Save(Forum forum)
        {
            return _forumRepository.Save(forum);
        }

        public void Delete(Forum forum)
        {
            _forumRepository.Delete(forum);
        }

        public Forum Update(Forum forum)
        {
            return _forumRepository.Update(forum);
        }

        public Forum GetById(int id)
        {
            return _forumRepository.GetById(id);
        }
    }
}
