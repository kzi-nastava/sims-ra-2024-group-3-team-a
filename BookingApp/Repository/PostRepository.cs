using BookingApp.Model;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class PostRepository : IPostRepository
    {
        private const string FilePath = "../../../Resources/Data/posts.csv";
        private readonly Serializer<Post> _serializer;
        private List<Post> _posts;

        public PostRepository()
        {
            _serializer = new Serializer<Post>();
            _posts = _serializer.FromCSV(FilePath);
        }

        public List<Post> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Post Save(Post post)
        {
            post.Id = NextId();
            _posts = _serializer.FromCSV(FilePath);
            _posts.Add(post);
            _serializer.ToCSV(FilePath, _posts);
            return post;
        }

        private int NextId()
        {
            _posts = _serializer.FromCSV(FilePath);
            if (_posts.Count < 1)
            {
                return 1;
            }
            return _posts.Max(p => p.Id) + 1;
        }

        public Post Update(Post post)
        {
            _posts = _serializer.FromCSV(FilePath);
            Post current = _posts.Find(p => p.Id == post.Id);
            int index = _posts.IndexOf(current);
            _posts.Remove(current);
            _posts.Insert(index, post);
            _serializer.ToCSV(FilePath, _posts);
            return post;
        }

        public void Delete(Post post)
        {
            _posts = _serializer.FromCSV(FilePath);
            Post found = _posts.Find(p => p.Id == post.Id);
            _posts.Remove(found);
            _serializer.ToCSV(FilePath, _posts);
        }

        public Post GetById(int id)
        {
            _posts = _serializer.FromCSV(FilePath);
            return _posts.Find(p => p.Id == id);
        }

        public List<Post> GetPostsForForum(Forum forum)
        {
            List<Post> posts = GetAll();
            return posts.FindAll(p => p.ForumId == forum.Id);
        }
    }
}
