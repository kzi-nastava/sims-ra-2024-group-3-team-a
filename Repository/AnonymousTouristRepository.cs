﻿using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AnonymousTouristRepository
    {
        private const string FilePath = "../../../Resources/Data/anonymousTourists.csv";

        private readonly Serializer<AnonymousTourist> _serializer;

        private List<AnonymousTourist> _anonymousTourists;

        public AnonymousTouristRepository()
        {
            _serializer = new Serializer<AnonymousTourist>();
            _anonymousTourists = _serializer.FromCSV(FilePath);
        }

        public List<AnonymousTourist> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AnonymousTourist Save(AnonymousTourist anonymous)
        {
            
            _anonymousTourists = _serializer.FromCSV(FilePath);
            _anonymousTourists.Add(anonymous);
            _serializer.ToCSV(FilePath, _anonymousTourists);
            return anonymous;
        }

      /*  public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
    }*/
}
}