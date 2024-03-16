using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class KeyPointsDTO : INotifyPropertyChanged
    {
        public KeyPointsDTO() 
        { 
            middle = new List<string>(); 
        }

        public KeyPointsDTO(KeyPoints keyPoint)
        {
            id = keyPoint.Id;
            begining = keyPoint.Begining;
            ending = keyPoint.Ending;
            middle = keyPoint.Middle;
        }
          
        public KeyPointsDTO(KeyPointsDTO keyPoint)
        {
            id = keyPoint.Id;
            begining = keyPoint.Begining;
            ending = keyPoint.Ending;
            middle = keyPoint.Middle;
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

        private string begining;
        public string Begining
        {
            get { return begining; }
            set
            {
                if (value != begining)
                {
                    begining = value;
                    OnPropertyChanged();
                }
            }
        }

        private string ending;
        public string Ending
        {
            get { return ending; }
            set
            {
                if (value != ending)
                {
                    ending = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> middle;
        public List<string> Middle
        {
            get { return middle; }
            set
            {
                if (value != middle)
                {
                    middle = value;
                    OnPropertyChanged();
                }
            }
        }

        public KeyPoints ToKeyPoint()
        {
            return new KeyPoints (id, begining,middle, ending);
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
