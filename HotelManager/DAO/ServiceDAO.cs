﻿using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class ServiceDAO
    {
        private static ServiceDAO instance;
        public List<Service> GetServices(int idServiceType)
        {
            List<Service> services = new List<Service>();
            string query = "USP_LoadServiceByServiceType @idServiceType";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { idServiceType });
            foreach (DataRow item in dataTable.Rows)
            {
                Service service = new Service(item);
                services.Add(service);
            }
            return services;
        }

        internal bool InsertService(string name, int idtype, int price)
        {
            string query = "USP_InsertService @name , @idServiceType , @price";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[]
            {
                name, idtype, price
            }) > 0;
        }
        internal bool InsertService(Service serviceNow)
        {
            return InsertService(serviceNow.Name, serviceNow.IdServiceType, serviceNow.Price);
        }
        internal bool UpdateService(int id, string name, int idServiceType, int price)
        {
            string query = "USP_UpdateService @id , @name , @idServiceType , @price";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id, name, idServiceType, price }) > 0;
        }
        internal bool UpdateService(Service serviceNow, Service servicePre)
        {
            return UpdateService(serviceNow.Id, serviceNow.Name, serviceNow.IdServiceType, serviceNow.Price);
        }
        
        internal DataTable LoadFullService()
        {
            string query = "USP_LoadFullService";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        internal DataTable Search(string name, int id)
        {
            string query = "USP_SearchService @string , @int";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { name, id });
        }
        internal bool IsIDServiceExist(string id)
        {
            string query = "USP_IsIDServiceExist @id";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { id }).Rows.Count>0;
        }
        internal int FindIDService(string name)
        {   
            string query = "USP_FindIDService @name";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { name });
        }
        internal string FindService(string name)
        {
            string query = "USP_FindService @name";
            return (string)DataProvider.Instance.ExecuteScalar(query, new object[] { name});
        }
        
        public bool DeleteService(int id)
        {

            string query = "USP_DeleteService @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id }) > 0;
        }
        public bool DeleteServiceByIdServiceType(int id)
        {

            string query = "USP_DeleteServiceByIdServiceType @id";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { id }) > 0;
        }
        public static ServiceDAO Instance { get { if (instance == null) instance = new ServiceDAO();return instance; }
            private set => instance = value; }
    }
}
