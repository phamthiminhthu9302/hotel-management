using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace HotelManager.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        internal int GetIdBillMax()
        {
            string query = "USP_GetIdBillMax";
            return (int)DataProvider.Instance.ExecuteScalar(query);
        }
     
             internal bool IsIDCustomerBillExists(int id)
        {
            string query = "IsIDCustomerBillExists @id";

            return DataProvider.Instance.ExecuteQuery(query, new object[] { id }).Rows.Count > 0;
           
            
        }
        internal bool IsIDCustomerOtherBillExists(int id)
        {
            string query = "IsIDCustomerOtherBillExists @id";

            return DataProvider.Instance.ExecuteQuery(query, new object[] { id }).Rows.Count > 0;


        }
        
            internal DataTable LoadTotalBill()
        {
            string query = "USP_LoadTotalBill";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        internal int GetIdBillFromIdRoom(int idRoom)
        {
            string query = "USP_GetIdBillFromIdRoom @idRoom";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { idRoom });
            
           
        }
        internal bool IsExistsBill(int idRoom)// > 0 Tồn tại Bill
        {
            string query = "USP_IsExistBillOfRoom @idRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { idRoom }).Rows.Count > 0;
        }
        internal bool IsExistBillPay(int id)
        {
            string query = "USP_IsExistBillPay @id";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { id }).Rows.Count > 0;
        }

        internal bool InsertBill(int idRoom, int staffSetUp)
        {
            string query = "USP_InsertBill @idRoom , @staffSetUp";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idRoom, staffSetUp }) > 0;
        }
        internal DataTable ShowBill(int idRoom)
        {
            string query = "USP_ShowBill @idRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { idRoom });
        }
        
        internal DataTable ShowBillPreView(int idBill)
        {
            string query = "USP_ShowBillPreView @idRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { idBill });
        }
        internal DataRow ShowBillRoom(int idRoom)
        {
            string query = "USP_ShowBillRoom @idRoom";
            return DataProvider.Instance.ExecuteQuery(query, new object[] {  idRoom }).Rows[0];
        }
        internal bool UpdateRoomPrice(int idBill)
        {
            string query = "USP_UpdateBill_RoomPrice @idBill";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idBill }) > 0;
        }
        internal bool UpdateServicePrice(int idBill)
        {
            string query = "USP_UpdateBill_ServicePrice @idBill";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idBill }) > 0;
        }
        internal bool UpdateOther(int idBill, int discount)
        {
            string query = "USP_UpdateBill_Other @idBill , @discount";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idBill, discount }) > 0;
        }
        internal bool UpdateBillDetails(int idBill, int idService, int _count)
        {
            string query = "USP_UpdateBillDetails @idBill , idService , @discount";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idBill, idService, _count }) > 0;
        }
        internal DataTable LoaddFullBill()
        {
            string query = "USP_LoadFUllBill";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        internal DataTable SearchBill(string text, int mode)
        {
            string query = "USP_SearchBill @string , @mode";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { text, mode });
        }

        internal static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set => instance = value;
        }
    }
}
