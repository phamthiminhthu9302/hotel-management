using HotelManager.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using DataTable = System.Data.DataTable;

namespace HotelManager.DAO
{
    public class ParameterDAO
    {
        #region Properties && constructor
        private ParameterDAO() { }

        private static ParameterDAO instance;
        internal static ParameterDAO Instance { get { if (instance == null) instance = new ParameterDAO(); return instance; } }
        #endregion

        #region Method
        internal bool UpdateParameter(string name, double value, string describe)
        {
            string query = "exec USP_UpdateParameter @name , @value , @describe";
            return DataProvider.Instance.ExecuteNoneQuery(query, new Object[] { name, value, describe }) > 0;
        }
        internal bool UpdateParameter(DTO.Parameter surcharge)
        {
            return UpdateParameter(surcharge.Name, surcharge.Value, surcharge.Describe);
        }
        internal DataTable LoadFullParameter()
        {
            return DataProvider.Instance.ExecuteQuery("USP_LoadFullParameter");
        }
        internal bool DeleteParameter(string surcharge)
        {
            string query = "exec USP_DeleteParameter @name";
            return DataProvider.Instance.ExecuteNoneQuery(query, new Object[] { surcharge }) > 0;
        }
        internal DataTable Search(string text)
        {
            string query = "USP_SearchParameter @string";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { text });
        }
        #endregion

    }
}
