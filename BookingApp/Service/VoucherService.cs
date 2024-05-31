using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.Interfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace BookingApp.Service
{
    public class VoucherService
    {
        private IVoucherRepository _voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        private readonly Serializer<Voucher> _serializer = new Serializer<Voucher>();
        public List<Voucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }
        public Voucher GetById(int id)
        {
            return _voucherRepository.GetById(id);
        }

        public Voucher Save(Voucher voucher)
        {
            return _voucherRepository.Save(voucher);
        }

        public void Delete(Voucher voucher)
        {
            List<Voucher> vouchers = new List<Voucher>();
            vouchers = GetAll().ToList();
            if (vouchers.Count > 0)
            {
                foreach (Voucher card in vouchers)
                {
                    if (voucher.Id == card.Id)
                    {
                        vouchers.Remove(card);
                        _serializer.ToCSV("../../../Resources/Data/vouchers.csv", vouchers);
                        break;
                    }

                }
            }
            
        }
        public List<Voucher> GetAllForUser(int userId) 
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach(Voucher v in GetAll())
            {
                if(v.UserId == userId)
                {
                    vouchers.Add(v);    
                }
            }
            return vouchers;
        }
        public Voucher Update(Voucher voucher)
        {
            return _voucherRepository.Update(voucher);
        }
        public void UpdateHeader()
        {
            List<Voucher> vouchers = GetAll();

            foreach (Voucher voucher in vouchers)
            {
                if (voucher.Type.Equals(Model.Enums.VoucherType.Gift))
                {
                    voucher.Header = "You went to five tours this year!";
                    Update(voucher);
                }
                else if (voucher.Type.Equals(Model.Enums.VoucherType.GuideQuitJob))
                {
                    voucher.Header = "Guide quit his job";
                    Update(voucher);
                }
                else
                {
                    voucher.Header = "Guide cancelled tour";
                    Update(voucher);
                }
            }
        }
        public void UpdateVouchers()
        {
            List<Voucher> vouchers = GetAll();

            foreach (Voucher voucher in vouchers)
            {
                voucher.TourId = -1;
                voucher.IsUsed = false;
                Update(voucher);
            }
        }

       
    }
}
