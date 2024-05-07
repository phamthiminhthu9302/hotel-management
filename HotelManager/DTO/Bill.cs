using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DTO
{
    public class Bill
    {
        private int id;
        private int idRoom;
        private int staffSetUp;
        private DateTime dateOfCreate;
        private int roomPrice;
        private int servicePrice;
        private int totalPrice;
        private int discount;
        private string idStatusBill;
        public Bill(DataRow data)
        {
            Id = (int)data["ID"];
            IdRoom = (int)data["IDBookRoom"];
            StaffSetUp = (int)data["IDStaff"];
            DateOfCreate = (DateTime)data["dateOfCreate"];
            RoomPrice = (int)data["RoomPrice"];
            ServicePrice = (int)data["ServicePrice"];
            TotalPrice = (int)data["TotalPrice"];
            Discount = (int)data["discount"];
            IdStatusBill = data["StatusBill"].ToString();
        }
       
        public int Id { get => id; set => id = value; }
        public int IdRoom { get => idRoom; set => idRoom = value; }
        public int StaffSetUp { get => staffSetUp; set => staffSetUp = value; }
        public DateTime DateOfCreate { get => dateOfCreate; set => dateOfCreate = value; }
        public int RoomPrice { get => roomPrice; set => roomPrice = value; }
        public int ServicePrice { get => servicePrice; set => servicePrice = value; }
        public int TotalPrice { get => totalPrice; set => totalPrice = value; }
        public int Discount { get => discount; set => discount = value; }
        public string IdStatusBill { get => idStatusBill; set => idStatusBill = value; }
    }
}
