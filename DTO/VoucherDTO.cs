using BookingApp.Model;
using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BookingApp.DTO
{
    public class VoucherDTO: INotifyPropertyChanged
    {
        public VoucherDTO() { }
        public  VoucherDTO(Voucher voucher)
        {
            id = voucher.Id;
            type = voucher.Type;
            userId = voucher.UserId;
            isUsed = voucher.IsUsed;
            expireDate = voucher.ExpireDate;
            header = voucher.Header;
        }
        public VoucherDTO(VoucherDTO voucherDTO)
        {
            id = voucherDTO.Id;
            type = voucherDTO.Type;
            userId = voucherDTO.UserId;
            isUsed = voucherDTO.IsUsed;
            expireDate = voucherDTO.ExpireDate;
            header = voucherDTO.Header;
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
        private VoucherType type;
        public VoucherType Type
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
        private int userId;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged();
                }
            }
        }
        private Boolean isUsed;
        public Boolean IsUsed
        {
            get { return isUsed; }
            set
            {
                if (value != isUsed)
                {
                    isUsed = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime expireDate;
        public DateTime ExpireDate
        {
            get { return expireDate; }
            set
            {
                if (value != expireDate)
                {
                    expireDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                if (value != header)
                {
                   header = value;
                    OnPropertyChanged();
                }
            }
        }
        public Voucher ToVoucher()
        {
            return new Voucher(id, type, userId, isUsed, expireDate, header);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
