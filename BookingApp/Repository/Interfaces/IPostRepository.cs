using BookingApp.Model;
using System.Collections.Generic;

namespace BookingApp.Repository.Interfaces
{
    public interface IPostRepository
    {
        public List<Post> GetAll();
        public Post Save(Post post);
        public Post Update(Post post);
        public void Delete(Post post);
        public Post GetById(int id);
        public List<Post> GetPostsForForum(Forum forum);
    }
}
