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
    public partial class fUpdateCustomerInfo : Form
    {
        string idCard;
        int idCustomer;
        string Card, phone;
        public fUpdateCustomerInfo(string _idCard, int _idCustomer)
        {
            InitializeComponent();
            idCard = _idCard;
            idCustomer = _idCustomer;
            LoadCustomerInfo(idCustomer);
        }

        public void LoadCustomerInfo(int idCustomer)
        {
            Customer customer = CustomerDAO.Instance.GetInfoCustomerById(idCustomer);
            Card=txbIDCard.Text = customer.IdCard.ToString();
            txbFullName.Text = customer.Name;
            txbAddress.Text = customer.Address;
            dpkDateOfBirth.Value = customer.DateOfBirth;
            cbSex.Text = customer.Sex;
            phone= txbPhoneNumber.Text = customer.PhoneNumber.ToString();
            cbNationality.Text = customer.Nationality;
           
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
        public void UpdateCustomer()
        {

            CustomerDAO.Instance.UpdateCustomer(idCustomer, txbFullName.Text, txbIDCard.Text, txbPhoneNumber.Text, dpkDateOfBirth.Value, txbAddress.Text, cbSex.Text, cbNationality.Text);
        }
        public void ClearData()
        {
            txbIDCard.Text = txbFullName.Text = txbAddress.Text = txbPhoneNumber.Text = cbNationality.Text = String.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void txbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        public bool IsIdCardExists(string idCard)
        {
            return CustomerDAO.Instance.IsIdCardExists(idCard);
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
                if (txbPhoneNumber.Text.Length != 10 || IsPhoneNumberValid(txbPhoneNumber.Text) == false)
                {
                    MessageBox.Show("Thông tin số điện thoại không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
                    if (CustomerDAO.Instance.IsCusExistsByIdCard(customerNow.IdCard) && CustomerDAO.Instance.IsPhoneCusExists(customerNow.PhoneNumber))
                    {
                    if (Card != customerNow.IdCard && phone != customerNow.PhoneNumber)
                    {
                        MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
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
                    if(phone != customerNow.PhoneNumber)
                                    {
                        MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
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
                        if (Card != customerNow.IdCard)
                        {
                            MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                }
               

                        if (!IsIdCardExists(customerNow.IdCard) && !CustomerDAO.Instance.IsPhoneExists(customerNow.PhoneNumber))
                        {

                            UpdateCustomer();
                            MessageBox.Show("Cập nhật thông tin khách hàng thành công.", "Thông báo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearData();
                            LoadCustomerInfo(idCustomer);
                        }
                        else
                        {

                            UpdateCustomer();
                            MessageBox.Show("Cập nhật thông tin khách hàng thành công.", "Thông báo.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearData();
                            LoadCustomerInfo(idCustomer);



                        }

                    
                
               
            }

            else
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
     
    }
