using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class BookRoomDAO
    {
        private static BookRoomDAO instance;
        private BookRoomDAO() { }
        public bool InsertBookRoom(int idCustomer, int idRoom,DateTime datecheckin,DateTime datecheckout, DateTime datebookroom, int manv)
        {
            string query = "USP_InsertBookRoom @idCustomer , @idRoom , @datecheckin , @datecheckout , @datebookroom , @manv";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idCustomer, idRoom, datecheckin, datecheckout, datebookroom, manv }) > 0;
        }
        public DataTable LoadListBookRoom(DateTime dateTime)
        {
            string query = "USP_LoadBookRoomsByDate @date";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { dateTime });

        }
        public int GetCurrentIDBookRoom(DateTime dateTime)
        {
            string query = "USP_LoadBookRoomsByDate @date";
            DataRow dataRow= DataProvider.Instance.ExecuteQuery(query, new object[] { dateTime }).Rows[0];
            return (int)dataRow["Mã đặt phòng"];
        }
        public string IsCusBookRoomExistsByIdBookRoom(int idBookRoom)
        {
            string query = "USP_IsCusBookRoomExistsByIdBookRoom @idBookRoom";
           return (string)DataProvider.Instance.ExecuteScalar(query, new object[] { idBookRoom });
        }
        
         public List<int> IsCusBookRoomDetailExistsByIdBookRoom(int idBookRoom)
        {
            string query = "USP_IsCusBookRoomDetailExistsByIdBookRoom @idBookRoom";
            List<int> list = new List<int>();

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { idBookRoom });
            foreach (DataRow item in data.Rows)
            {
                int value = int.Parse(item[0].ToString());

                list.Add(value);
            }
            return list;
        }
        public bool IsBookRoomHaveBill(int id)
        {
            string query = "USP_IsBookRoomHaveBill @id";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { id});
            return dataTable.Rows.Count > 0;
        }


        public DataRow ShowBookRoomInfo(int idBookRoom)
        {
            string query = "ShowBookRoomInfo @idBookRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { idBookRoom }).Rows[0];
        }
        public bool UpdateBookRoom(int id,DateTime datecheckin,DateTime datecheckout)
        {
            string query = "USP_UpdateBookRoom @id , @dateCheckIn , @datecheckOut";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id, datecheckin, datecheckout }) > 0;
        }
        public bool IsIDCustomerOtherBookRoomDetailExists(int id)
        {
            string query = "IsIDCustomerOtherBookRoomDetailExists @id";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { id});
            return dataTable.Rows.Count > 0;
        }
       
           public bool DeleteBookRoom(int id)
        {
            string query = "USP_DeleteBookRoom @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id }) > 0;
        }
        public bool InsertReceiveRoomDetails(int idReceiveRoom, int idCustomerOther)
        {
            string query = "InsertReceiveRoomDetails @idReceiveRoom , @idCustomer";
            int count = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idReceiveRoom, idCustomerOther });
            return count > 0;
        }
        public bool DeleteReceiveRoomDetails(int idReceiveRoom, int idCustomerOther)
        {
            string query = "USP_DeleteReceiveRoomDetails @idReceiveRoom , @idCustomer";
            int count = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idReceiveRoom, idCustomerOther });
            return count > 0;
        }
        public bool InsertReceiveRoom(int idBookRoom, int idRoom)
        {
            string query = "InsertReceiveRoom @idBookRoom , @idRoom";
            int count = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idBookRoom, idRoom });
            return count > 0;
        }
        public int GetIDCurrent()
        {
            string query = "GetIDReceiveRoomCurrent";
            return (int)DataProvider.Instance.ExecuteScalar(query);
        }
        public DataTable LoadReceiveRoomInfo()
        {
            string query = "USP_LoadReceiveRoomsByDate @date";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { DateTime.Now.Date });
        }
        public int GetIdReceiveRoomFromIdRoom(int idRoom)
        {
            string query = "USP_GetIdReceiRoomFromIdRoom @idRoom";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { idRoom });
            BookRoom receiveRoom = new BookRoom(dataTable.Rows[0]);
            return receiveRoom.Id;
        }
        public DataTable ShowReceiveRoom(int id)
        {
            string query = "USP_ShowReceiveRoom @idReceiveRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }
        public DataTable ShowCusomers(int id)
        {
            string query = "USP_ShowCustomerFromReceiveRoom @idReceiveRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }
        public bool UpdateReceiveRoom(int idRoom,int id)
        {
            string query = "USP_UpdateReceiveRoom @idRoom , @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idRoom,id  }) > 0;
        }
        public static BookRoomDAO Instance { get { if (instance == null) instance = new BookRoomDAO();return instance; }
            private set => instance = value; }
    }
}
