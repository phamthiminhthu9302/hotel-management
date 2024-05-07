using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DTO
{
    public class Room
    {
        private int id;
        private string name;
        private int idRoomType;
        private string statusRoom;
        public Room() { }
        public Room(int id, string name, int idRoomType, string statusRoom)
        {
            this.Id = id;
            this.Name = name;
            this.IdRoomType = idRoomType;
            this.StatusRoom = statusRoom;
        }
        public Room(DataRow row)
        {
            this.Id = (int)row["ID"];
            this.Name =row["Name"].ToString();
            this.IdRoomType =(int)row["idRoomType"];
            this.StatusRoom = row["nameStatusRoom"].ToString();
        }
        public bool Equals(Room roomPre)
        {
            if (roomPre == null) return false;
            if (roomPre.id != this.id) return false;
            if (roomPre.Name != this.Name) return false;
            if (roomPre.idRoomType != this.idRoomType) return false;
            if (roomPre.StatusRoom != this.StatusRoom) return false;
            return true;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Room);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int IdRoomType { get => idRoomType; set => idRoomType = value; }
        public string StatusRoom { get => statusRoom; set => statusRoom = value; }
    }
}
