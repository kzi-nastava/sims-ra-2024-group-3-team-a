using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class PostDTO : INotifyPropertyChanged
    {
        public PostDTO() 
        {
            ownersReported = new List<String>();
        }

        public PostDTO(Post post)
        {
            Id = post.Id;
            ForumId = post.ForumId;
            Username = post.Username;
            Text = post.Text;
            Reports = post.Reports;
            Type = post.Type;
            OwnersReported = post.OwnersReported;
        }

        public PostDTO(PostDTO postDTO)
        {
            Id = postDTO.Id;
            ForumId = postDTO.ForumId;
            Username = postDTO.Username;
            Text = postDTO.Text;
            Reports = postDTO.Reports;
            Type = postDTO.Type;
            OwnersReported = postDTO.OwnersReported;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private int forumId;
        public int ForumId
        {
            get { return forumId; }
            set
            {
                if (value != forumId)
                {
                    forumId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    OnPropertyChanged();
                }
            }
        }

        private int reports;
        public int Reports
        {
            get { return reports; }
            set
            {
                if (value != reports)
                {
                    reports = value;
                    OnPropertyChanged();
                }
            }
        }

        private PostType type;
        public PostType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<String> ownersReported;
        public List<String> OwnersReported
        {
            get { return ownersReported; }
            set
            {
                if (value != ownersReported)
                {
                    ownersReported = value;
                    OnPropertyChanged();
                }
            }
        }

        public Post ToPost()
        {
            return new Post(Id, ForumId, Username, Text, Reports, Type, OwnersReported);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
