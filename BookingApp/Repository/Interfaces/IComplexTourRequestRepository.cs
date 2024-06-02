using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IComplexTourRequestRepository
    {
        public List<ComplexTourRequest> GetAll();

        public ComplexTourRequest GetById(int id);

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest);

        public void Delete(ComplexTourRequest complexTourRequest);

        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest);
    }
}
