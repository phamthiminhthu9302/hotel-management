using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DTO
{
    public class BookRoom
    {
        private int id;
        private int idCustomer;
        private int idStaff;
        private int idRoom;
        private DateTime dateCheckIn;
        private DateTime dateCheckOut;
        private DateTime dateBookRoom;

        public BookRoom(int id, int idCustomer,int idStaff, int idRoom, DateTime dateCheckIn, DateTime dateCheckOut,DateTime dateBookRoom)
        {
            Id = id;
            IdCustomer = idCustomer;
            IdStaff= idStaff;
            IdRoom = idRoom;
            DateCheckIn = dateCheckIn;
            DateCheckOut = dateCheckOut;
            DateBookRoom = dateBookRoom;
        }
        public BookRoom(DataRow row)
        {
            Id = (int)row["id"];
            IdCustomer = (int)row["idCustomer"];
          /*  IdRoom = (int)row["idRoom"];*/
            DateCheckIn = (DateTime)row["DateCheckIn"];
            DateCheckOut = (DateTime)row["DateCheckOut"];
            DateBookRoom = (DateTime)row["DateBookRoom"];
        }

        public int Id { get => id; set => id = value; }
        public int IdCustomer { get => idCustomer; set => idCustomer = value; }
        public int IdStaff { get => idStaff; set => idStaff = value; }
        public int IdRoom { get => idRoom; set => idRoom = value; }
        public DateTime DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public DateTime DateBookRoom { get => dateBookRoom; set => dateBookRoom = value; }
    }
}
