using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public VoucherType Type { get; set; }
        public int UserId { get; set; }
        public Boolean IsUsed { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Header { get; set; }
        public Voucher() { }
        public Voucher(int id, VoucherType type, int userId, bool isUsed, DateTime expireDate, string header)
        {
            Id = id;
            Type = type;
            UserId = userId;
            IsUsed = isUsed;
            ExpireDate = expireDate;
            Header = header;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Type.ToString(), UserId.ToString(), IsUsed.ToString(), ExpireDate.ToString(), Header.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Type = (VoucherType)Enum.Parse(typeof(VoucherType), values[1]);
            UserId = Convert.ToInt32(values[2]);
            IsUsed = Convert.ToBoolean(values[3]);
            ExpireDate = DateTime.Parse(values[4]);
            Header = values[5];
        }
    }
}
