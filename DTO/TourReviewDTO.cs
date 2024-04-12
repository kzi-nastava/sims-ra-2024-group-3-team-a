﻿using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class TourReviewDTO : INotifyPropertyChanged
    {
        public TourReviewDTO() 
        {
            images = new List<string>();
        }

        public TourReviewDTO(TourReview tourReview)
        {
            id = tourReview.Id;
            touristId = tourReview.TouristId;
            guideKnowledgeRating = tourReview.GuideKnowledgeRating;
            guideLanguageRating = tourReview.GuideLanguageRating;
            tourEntertainmentRating = tourReview.TourEntertainmentRating;
            comment = tourReview.Comment;
            isValid = tourReview.IsValid;
            images= tourReview.Images;
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

        private int touristId;
        public int TouristId
        {
            get { return touristId; }
            set
            {
                if (value != touristId)
                {
                    touristId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int tourId;
        public int TourId
        {
            get { return tourId; }
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged();
                }
            }
        }
        //int id, int touristId, int tourId, int guideKnowledgeRating, int guideLanguageRating, int tourEntertainmentRating, string comment, List<string> images)

        private int guideKnowledgeRating;
        public int GuideKnowledgeRating
        {
            get { return guideKnowledgeRating; }
            set
            {
                if (value != guideKnowledgeRating)
                {
                    guideKnowledgeRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private int guideLanguageRating;
        public int GuideLanguageRating
        {
            get { return guideLanguageRating; }
            set
            {
                if (value != guideLanguageRating)
                {
                    guideLanguageRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private int tourEntertainmentRating;
        public int TourEntertainmentRating
        {
            get { return tourEntertainmentRating; }
            set
            {
                if (value != tourEntertainmentRating)
                {
                    tourEntertainmentRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private Boolean isValid;
        public Boolean IsValid
        {
            get { return isValid; }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> images;
        public List<string> Images
        {
            get { return images; }
            set
            {
                if (value != images)
                {
                    images = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourReview ToTourReview()
        {
            return new TourReview(id, touristId, tourId, guideKnowledgeRating, guideLanguageRating, tourEntertainmentRating, comment,isValid, images);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
