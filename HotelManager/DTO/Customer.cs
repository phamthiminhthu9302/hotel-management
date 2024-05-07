using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DTO
{
    public class Customer
    {
        private int id;
        private string idCard;
        private string name;
      
        private DateTime dateOfBirth;
        private string address;
        private string phoneNumber;
        private string sex;
        private string nationality;
        public Customer()
        {

        }
        public Customer(int id,string idCard, string name, string address, string phoneNumber, string sex, string nationality,DateTime dateOfBirth)
        {
            this.Id = id;
            this.IdCard = idCard;
            this.Name = name;
          
            this.DateOfBirth = dateOfBirth;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Sex = sex;
            this.Nationality = nationality;
        }
        public Customer(DataRow row)
        {
            this.Id= (int)row["id"];
            this.IdCard = row["idcard"].ToString();
            this.Name = row["Name"].ToString();
            this.DateOfBirth = (DateTime)row["DateOfBirth"];
            this.Address = row["address"].ToString();
            this.PhoneNumber = row["phoneNumber"].ToString(); ;
            this.Sex = row["sex"].ToString();
            this.Nationality = row["Nationality"].ToString();
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Customer);
        }
        public bool Equals(Customer customerPre)
        {
            if (customerPre == null)
                return false;
            if (this.idCard != customerPre.idCard) return false;
            if (this.Name != customerPre.Name) return false;
            if (this.dateOfBirth != customerPre.dateOfBirth) return false;
            if (this.address != customerPre.address) return false;
            if (this.phoneNumber != customerPre.phoneNumber) return false;
            if (this.sex != customerPre.sex) return false;
            if (this.nationality != customerPre.nationality) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Address { get => address; set => address = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Sex { get => sex; set => sex = value; }
        public string Nationality { get => nationality; set => nationality = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string IdCard { get => idCard; set => idCard = value; }
        public string Name { get => name; set => name = value; }
    
        public int Id { get => id; set => id = value; }
    }
}
