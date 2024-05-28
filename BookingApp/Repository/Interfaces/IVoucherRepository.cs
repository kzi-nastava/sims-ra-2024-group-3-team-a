using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.Interfaces
{
    public interface IVoucherRepository
    {
        public List<Voucher> GetAll();

        public Voucher GetById(int id);

        public Voucher Save(Voucher voucher);

        public void Delete(Voucher voucher);

        public Voucher Update(Voucher voucher);
    }
}
