using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class KeyPointDTO : INotifyPropertyChanged
    {
        public KeyPointDTO(){}

        public KeyPointDTO(KeyPoint keyPoint)
        {
            id = keyPoint.Id;
            name = keyPoint.Name;
            type = keyPoint.Type;
            tourId= keyPoint.TourId;
            isCurrent = keyPoint.IsCurrent;
            hasPassed= keyPoint.HasPassed;
        }

        public KeyPointDTO(KeyPointDTO keyPoint)
        {
            id = keyPoint.Id;
            name = keyPoint.Name;
            tourId = keyPoint.TourId;
            type = keyPoint.Type;
            isCurrent = keyPoint.IsCurrent;
            hasPassed= keyPoint.HasPassed;
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private KeyPointsType type;
        public KeyPointsType Type
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

        private Boolean isCurrent;
        public Boolean IsCurrent
        {
            get { return isCurrent; }
            set
            {
                if (value != isCurrent)
                {
                    isCurrent = value;
                    OnPropertyChanged();
                }
            }
        }
        private Boolean hasPassed;
        public Boolean HasPassed
        {
            get { return hasPassed; }
            set
            {
                if (value != hasPassed)
                {
                    hasPassed = value;
                    OnPropertyChanged();
                }
            }
        }
        public KeyPoint ToKeyPoint()
        {
            return new KeyPoint(id, name,tourId, type, isCurrent, hasPassed);
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
