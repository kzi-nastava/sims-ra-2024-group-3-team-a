using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BookingApp.DTO
{
    public class ForumDTO : INotifyPropertyChanged
    {

        public ForumDTO()
        {
            LocationDTO = new LocationDTO();
        }

        public ForumDTO(Forum forum)
        {
            Id = forum.Id;
            LocationDTO = new LocationDTO(forum.Location);
            ForumCreatorId = forum.ForumCreatorId;
            SeenByOwner = forum.SeenByOwner;
            IsVeryImportant = forum.IsVeryImportant;
            IsClosed = forum.IsClosed;
        }

        public ForumDTO(ForumDTO forumDTO)
        {
            Id = forumDTO.Id;
            LocationDTO = new LocationDTO(forumDTO.LocationDTO);
            ForumCreatorId = forumDTO.ForumCreatorId;
            SeenByOwner = forumDTO.SeenByOwner;
            IsVeryImportant = forumDTO.IsVeryImportant;
            IsClosed = forumDTO.IsClosed;
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
        private int forumCreatorId;
        public int ForumCreatorId
        {
            get { return forumCreatorId; }
            set
            {
                if (value != forumCreatorId)
                {
                    forumCreatorId = value;
                    OnPropertyChanged();
                }
            }
        }
        private LocationDTO locationDTO;
        public LocationDTO LocationDTO
        {
            get { return locationDTO; }
            set
            {
                if (value != locationDTO)
                {
                    locationDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool seenByOwner;
        public bool SeenByOwner
        {
            get { return seenByOwner; }
            set
            {
                if (value != seenByOwner)
                {
                    seenByOwner = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isVeryImportant;
        public bool IsVeryImportant
        {
            get { return isVeryImportant; }
            set
            {
                if (value != isVeryImportant)
                {
                    isVeryImportant = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isClosed;
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (value != isClosed)
                {
                    isClosed = value;
                    OnPropertyChanged();
                }
            }
        }

        public String LocationDTOHeader
        {
            get
            {
                return LocationDTO.City + ", " + LocationDTO.Country;
            }
        }

        private PostRepository _postRepository = new PostRepository();
        public String LastPostString
        {
            get
            {
                Post lastPost = _postRepository.GetPostsForForum(this.ToForum()).LastOrDefault();
                return lastPost.Username + ": " + lastPost.Text; 
            }
        }

        public Forum ToForum()
        {
            return new Forum(Id, LocationDTO.ToLocation(),ForumCreatorId, SeenByOwner, IsVeryImportant, IsClosed);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
