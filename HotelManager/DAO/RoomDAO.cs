using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class RoomDAO
    {
        private static RoomDAO instance;
        #region Method

        internal DataTable LoadFullRoom()
        {
            return DataProvider.Instance.ExecuteQuery("USP_LoadFullRoom");
        }
        public bool IsRoomHaveBill(int id)
        {
            string query = "USP_IsRoomHaveBill @id";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { id });
            return dataTable.Rows.Count > 0;
        }
        public bool IsRoomHaveBookRoom(int id)
        {
            string query = "USP_IsRoomHaveBookRoom @id";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { id });
            return dataTable.Rows.Count > 0;
        }
        internal bool InsertRoom(Room roomNow)
        {
            return InsertRoom(roomNow.Name, roomNow.IdRoomType, roomNow.StatusRoom);
        }
        internal bool InsertRoom(string roomName, int idRoomType, string StatusRoom)
        {
            string query = "USP_InsertRoom @nameRoom , @idRoomType , @StatusRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomName, idRoomType, StatusRoom }) > 0;
        }
        internal DataTable LoadRoomTypeInfo(int id)
        {
            string query = "USP_LoadRoomTypeInfo @id";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id });
          
            return data;
        }
        public bool DeleteRoom(int id)
        {
            string query = "USP_DeleteRoom @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id }) > 0;
        }
        internal bool UpdateCustomer(Room roomNow)
        {
            string query = "USP_UpdateRoom  @id , @nameRoom , @idRoomType , @StatusRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomNow.Id, roomNow.Name, roomNow.IdRoomType, roomNow.StatusRoom }) > 0;
        }
        internal DataTable Search(string text, int id)
        {
            string query = "USP_SearchRoom @string , @id";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { text, id });
        }
        public List<Room> LoadEmptyRoom(int idRoomType)
        {
            List<Room> rooms = new List<Room>();
            string query = "USP_LoadEmptyRoom @idRoomType";
            DataTable data = DataProvider.Instance.ExecuteQuery(query,new object[] { idRoomType });
            foreach (DataRow item in data.Rows)
            {
                Room room = new Room(item);
                rooms.Add(room);
            }
            return rooms;
        }
        public List<Room> LoadListFullRoom(DateTime dateTime)
        {
            string query = "USP_LoadListFullRoom @date";
            List<Room> rooms = new List<Room>();
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { dateTime });
            foreach (DataRow item in data.Rows)
            {
                Room room = new Room(item);
                rooms.Add(room);
            }
            return rooms;
        }
        public int GetPeoples(int idBill)
        {
            string query = "USP_GetPeoples @idBill";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { idBill })+1;
        }
        public int GetIdRoomFromReceiveRoom(int idReceiveRoom)
        {
            string query = "USP_GetIDRoomFromReceiveRoom @idReceiveRoom";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { idReceiveRoom });
        }
        public String GetNameRoomFromIDRoom(int idReceiveRoom)
        {
            string query = "USP_GetNameRoomFromIDRoom @idReceiveRoom";
            return (String)DataProvider.Instance.ExecuteScalar(query, new object[] { idReceiveRoom });
        }
        public bool UpdateStatusRoom(int idRoom)
        {
            string query = "USP_UpdateStatusRoom @idRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idRoom }) > 0;
        }

        public bool UpdateStatusRoomOld(int idRoom)
        {
            string query = "USP_UpdateStatusRoomOld @idRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idRoom }) > 0;
        }
        #endregion
      
       
       
        public static RoomDAO Instance { get {if(instance==null) instance=new RoomDAO();return instance; }
            private set => instance = value; }
        private RoomDAO() { }
    }
}
