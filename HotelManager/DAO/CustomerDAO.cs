using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class CustomerDAO
    {
        private static CustomerDAO instance;
        
        private CustomerDAO() { }
        #region Method
        public bool IsIdCardExists(string idCard)
        {
            string query = "USP_IsIdCardExists @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        public bool IsPhoneExists(string idCard)
        {
            string query = "USP_IsPhoneExists @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        
            
        public bool IsPhoneCusExists(string idCard)
        {
            string query = "USP_IsPhoneCusExists @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        public bool IsCusExistsByIdCard(string idCard)
        {
            string query = "USP_IsCusExistsByIdCard @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        public bool IsCustomerBookRoomExists(string idCard)
        {
            string query = "USP_IsCustomerBookRoomExists @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        public bool IsCustomerBookRoomDetailExists(string idCard)
        {
            string query = "USP_IsCustomerBookRoomDetailExists @idCard";
            int count = DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count;
            return count > 0;
        }
        public bool InsertCustomer(string idCard, string name,DateTime dateofBirth,string address,string phonenumber,string sex,string nationality)
        {
            string query = "USP_InsertCustomer_ @idCard , @name , @dateOfBirth , @address , @phoneNumber , @sex , @nationality";
            return DataProvider.Instance.ExecuteNoneQuery(query,new object[] { idCard, name, dateofBirth, address, phonenumber, sex, nationality })>0;
        }
        public bool DeleteCustomer(string id)
        {
            string query = "USP_DeleteCustomer @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id }) > 0;
        }
        public Customer GetInfoByIdCard(string idCard)
        {
            
            string query = "USP_IsCusExistsByIdCard @idCard";
            Customer customer =new Customer(DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows[0]);
            return customer;

        }
        public Customer GetInfoCustomerById(int id)
        {
            string query = "USP_GetInfoCustomerById @id";
            Customer customer = new Customer(DataProvider.Instance.ExecuteQuery(query, new object[] { id }).Rows[0]);
            return customer;

        }
        internal bool InsertCustomer(string customerName, string idCard, string address, DateTime dateOfBirth, string phoneNumber, string sex, string nationality)
        {
            
            string query = "exec USP_InsertCustomer @customerName , @idCard , @address , @dateOfBirth , @phoneNumber , @sex , @nationality";
            int count = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { customerName, idCard, address, dateOfBirth, phoneNumber, sex, nationality });
            return count > 0;
        }
        internal bool InsertCustomer(Customer customer)
        {
            return InsertCustomer(customer.Name, customer.IdCard, customer.Address,
                customer.DateOfBirth, customer.PhoneNumber, customer.Sex, customer.Nationality);
        }


        internal bool UpdateCustomer(Customer customerNow, Customer customerPre)
        {
            string query = "USP_UpdateCustomer @id , @customerName ," +
                            " @idCardNow , @address , @dateOfBirth , " +
                            "@phoneNumber , @sex , @nationality , @idCardPre";
            object[] parameter = new object[] {customerNow.Id, customerNow.Name, customerNow.IdCard,
                                    customerNow.Address, customerNow.DateOfBirth, customerNow.PhoneNumber,
                                    customerNow.Sex, customerNow.Nationality, customerPre.IdCard};
            return DataProvider.Instance.ExecuteNoneQuery(query, parameter) > 0;
        }
        public bool UpdateCustomer(int id,string name,string idCard, string phoneNumber, DateTime dateOfBirth,string address,string sex,string nationality)
        {
            string query = "USP_UpdateCustomer_ @id , @name , @idCard , @phoneNumber , @dateOfBirth , @address , @sex , @nationality";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id,name,idCard,phoneNumber,dateOfBirth,address,sex,nationality})>0;
        }

        internal DataTable LoadFullCustomer()
        {
            string query = "USP_LoadFullCustomer";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        internal DataTable Search(string text, int phoneNumber)
        {
            string query = "USP_SearchCustomer @string , @int";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { text, phoneNumber });
        }
        public int GetIDCustomerFromBookRoom(int idReceiveRoom)
        {
            string query = "USP_GetIDCustomerFromBookRoom @idReceiveRoom";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { idReceiveRoom });
        }
        public int GetInforCustomerFromBookRoom(int idReceiveRoom)
        {
            string query = "USP_GetInforCustomerFromBookRoom @idReceiveRoom";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { idReceiveRoom });
        }


        #endregion
        public static CustomerDAO Instance { get { if (instance == null) instance = new CustomerDAO();return instance; }
            private set => instance = value; }
        
    }
}
