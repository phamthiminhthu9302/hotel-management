using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fAddCustomerInfo : Form
    {
        private static List<int> listIdCustomer;
       
        Customer Customer;
        public static List<int> ListIdCustomer { get => listIdCustomer; set => listIdCustomer = value; }
        int idBookRoom;
        string Card, phone;
        public fAddCustomerInfo(int idBRoom)
        {
            InitializeComponent();
            idBookRoom = idBRoom;
            ListIdCustomer = new List<int>();
            cbSex.SelectedIndex = 0;
            cbNationality.SelectedIndex = 0;
        }
        
        public bool IsIdCardExists(string idCard)
        {
            return CustomerDAO.Instance.IsIdCardExists(idCard);
        }
        public void InsertCustomer(string idCard, string name, DateTime dateofBirth, string address, string phonenumber, string sex, string nationality)
        {
            CustomerDAO.Instance.InsertCustomer(idCard, name, dateofBirth, address, phonenumber, sex, nationality);
        }
        public void GetInfoByIdCard(string idCard)
        {
            Customer customer = CustomerDAO.Instance.GetInfoByIdCard(idCard);
            Card = txbIDCard.Text = customer.IdCard.ToString();
            txbFullName.Text = customer.Name;
            txbAddress.Text = customer.Address;
            dpkDateOfBirth.Value = customer.DateOfBirth;
            cbSex.Text = customer.Sex;
           phone= txbPhoneNumber.Text = customer.PhoneNumber.ToString();
            cbNationality.Text = customer.Nationality;
            
        }
        public void ClearData()
        {
            Card= phone =txbIDCardSearch.Text = txbIDCard.Text = txbFullName.Text = txbAddress.Text = txbPhoneNumber.Text = cbNationality.Text = String.Empty;
        }
        public void AddIdCustomer(int idCustomer)
        {
            if(ListIdCustomer.Count!=0)
            {
                bool check = false;
                foreach (int item in ListIdCustomer)
                {
                    if (item == idCustomer)
                    {
                        check = true;
                        break;
                    }
                }
                if(!check) ListIdCustomer.Add(idCustomer);
            }
            else
            ListIdCustomer.Add(idCustomer);
        }
        private Customer GetCustomerNow()
        {
           
            Customer customer = new Customer();

           
                customer.Id = 0;
 
            customer.IdCard = txbIDCard.Text;
            customer.Name = txbFullName.Text;
            customer.Sex = cbSex.Text;
            customer.PhoneNumber = txbPhoneNumber.Text;
            customer.DateOfBirth = dpkDateOfBirth.Value;
            customer.Nationality = cbNationality.Text;
            customer.Address = txbAddress.Text;
            return customer;
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
         
            Customer customerNow = GetCustomerNow();
            if (txbFullName.Text != string.Empty && txbIDCard.Text != string.Empty && txbAddress.Text != string.Empty && cbNationality.Text != string.Empty && txbPhoneNumber.Text != string.Empty)
            {
                if (txbIDCard.Text.Length != 9)
                {
                    MessageBox.Show("Thông tin CMND không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txbPhoneNumber.Text.Length != 10 || IsPhoneNumberValid(txbPhoneNumber.Text)==false)
                {
                    MessageBox.Show("Thông tin số điện thoại không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (listIdCustomer.Count==0)
                {
                    if (BookRoomDAO.Instance.IsCusBookRoomExistsByIdBookRoom(idBookRoom) == customerNow.IdCard)
                    {
                        MessageBox.Show("Khách hàng đã đặt phòng này", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    List<int> list = new List<int>();
                    list = BookRoomDAO.Instance.IsCusBookRoomDetailExistsByIdBookRoom(idBookRoom);
                    Customer customer = null;
                    
                    foreach (int a in list)
                    {
                        customer = CustomerDAO.Instance.GetInfoCustomerById(a);
                        if(customer.IdCard== customerNow.IdCard)
                        {
                            MessageBox.Show("Khách hàng đã đặt phòng này", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    List<int> list = new List<int>();
                    list = BookRoomDAO.Instance.IsCusBookRoomDetailExistsByIdBookRoom(idBookRoom);
                    Customer customer = null;
                    customer = CustomerDAO.Instance.GetInfoByIdCard(BookRoomDAO.Instance.IsCusBookRoomExistsByIdBookRoom(idBookRoom));
                    foreach (var item in ListIdCustomer)
                    {
                        foreach (int a in list)
                            if (customer.Id == item|| a == item)
                            {
                                MessageBox.Show("Khách hàng đã đặt phòng này", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                           

                    }




                }

                if (CustomerDAO.Instance.IsCusExistsByIdCard(customerNow.IdCard) && CustomerDAO.Instance.IsPhoneCusExists(customerNow.PhoneNumber))
                    {
                 
                    if(phone== String.Empty && Card == String.Empty)
                    {
                        MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (Card != customerNow.IdCard && phone != customerNow.PhoneNumber)
                        {
                            MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    
                       
                    
                }

                if (CustomerDAO.Instance.IsPhoneExists(customerNow.PhoneNumber))
                {
                    if (!CustomerDAO.Instance.IsPhoneCusExists(customerNow.PhoneNumber))
                    {
                        MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (phone == String.Empty ) { 
                                MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            
                        }
                        else
                        {
                            if(phone != customerNow.PhoneNumber)
                            {
                                MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                if (IsIdCardExists(customerNow.IdCard))
                {
                    if (!CustomerDAO.Instance.IsCusExistsByIdCard(customerNow.IdCard))
                    {
                        MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (Card== String.Empty)
                        {
                            
                                MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            
                        }
                        else
                        {
                            if (Card != customerNow.IdCard)
                            {
                                MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }


                int i = 0;

                    if (!IsIdCardExists(customerNow.IdCard) && !CustomerDAO.Instance.IsPhoneExists(customerNow.PhoneNumber))
                    {
                  
                    InsertCustomer(txbIDCard.Text, txbFullName.Text, dpkDateOfBirth.Value, txbAddress.Text, txbPhoneNumber.Text, cbSex.Text, cbNationality.Text);
                    int id = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard).Id;
                    MessageBox.Show("Thêm khách hàng thành công.", "Thông báo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddIdCustomer(id);
                    ClearData();

                    i++;




                }
                if (i == 0)
                {

                    Customer = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard);
                    int id = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard).Id;
                    CustomerDAO.Instance.UpdateCustomer(Customer.Id, txbFullName.Text, txbIDCard.Text, txbPhoneNumber.Text, dpkDateOfBirth.Value,
                                          txbAddress.Text, cbSex.Text, cbNationality.Text);
                                MessageBox.Show("Thêm khách hàng thành công.", "Thông báo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AddIdCustomer(id);

                    
                    ClearData();



                        }
                    

                
            }
            else
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txbIDCardSearch.Text != String.Empty)
            {
                if (IsIdCardExists(txbIDCardSearch.Text))
                {
                    GetInfoByIdCard(txbIDCardSearch.Text);
                   
                }
                    
                else
                    MessageBox.Show("Thẻ CMND không tồn tại.\nVui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txbIDCardSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txbIDCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/' || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }

        private void TxbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
    }
}
