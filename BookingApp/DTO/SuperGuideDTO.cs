using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    class SuperGuideDTO : INotifyPropertyChanged
    {
        public SuperGuideDTO()
        {
        }
        public SuperGuideDTO(SuperGuide superGuide)
        {
            id = superGuide.Id;
            guideId = superGuide.GuideId;
            beginingTime = superGuide.BeginingTime;
            language = superGuide.Language;
        }
        public  SuperGuideDTO(SuperGuideDTO superGuideDTO)
        {
            id = superGuideDTO.Id;
            guideId = superGuideDTO.GuideId;
            beginingTime = superGuideDTO.BeginingTime;
            language = superGuideDTO.Language;
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

        private int guideId;

        public int GuideId
        {
            get { return guideId; }
            set
            {
                if (value != guideId)
                {
                    guideId = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime beginingTime;
        public DateTime BeginingTime
        {
            get { return beginingTime; }
            set
            {
                if (value != beginingTime)
                {
                    beginingTime = value;
                    OnPropertyChanged();
                }
            }
        }
        private Languages language;
        public Languages Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        public SuperGuide ToSuperGuide()
        {
            return new SuperGuide(id, guideId, beginingTime, language);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}