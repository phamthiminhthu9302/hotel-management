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
    public partial class fAddCustomer : Form
    {
        public fAddCustomer()
        {
            InitializeComponent();
            comboBoxSex.SelectedIndex = 0;
            txbNationality.SelectedIndex = 0;
        }
        
        
        private void Clean()
        {
            txbFullName.Text = string.Empty;
            txbAddress.Text = string.Empty;
            txbIDCard.Text = string.Empty;
            txbNationality.Text = string.Empty;
            txbPhoneNumber.Text = string.Empty;
        }
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thêm khách hàng mới?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
                if (CheckDate())
                {
                    InsertCustomer();
                }
                else
                    MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private bool CheckDate()
        {
            if (DateTime.Now.Subtract(datepickerDateOfBirth.Value).Days <= 0)
                return false;
            else return true;
        }
        public bool IsIdCardExists(string idCard)
        {
            return CustomerDAO.Instance.IsIdCardExists(idCard);
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
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void InsertCustomer()
        {
            Customer customer = Getcustomer();
            if (!CheckFillInText(new Control[] { txbPhoneNumber, txbFullName, txbIDCard, txbNationality, txbAddress, comboBoxSex }))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
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
                if (IsIdCardExists(customer.IdCard) && CustomerDAO.Instance.IsPhoneExists(customer.PhoneNumber))
                {

                    MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (CustomerDAO.Instance.IsPhoneExists(customer.PhoneNumber))
                {
                    MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (IsIdCardExists(customer.IdCard))
                {
                    MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }


                if (!IsIdCardExists(txbIDCard.Text) && !CustomerDAO.Instance.IsPhoneExists(customer.PhoneNumber))
                {

                    if (CustomerDAO.Instance.InsertCustomer(customer))
                    {
                        MessageBox.Show("Thêm thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clean();
                    }

                }
            
                    
                

            }
        }
        public static bool CheckFillInText(Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control.Text == string.Empty)
                    return false;
            }
            return true;
        }
        private Customer Getcustomer()
        {
            fStaff.Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { txbAddress, txbFullName, txbIDCard });
            Customer customer = new Customer();
            customer.IdCard = txbIDCard.Text;
           
            customer.Name = txbFullName.Text;
            customer.Sex = comboBoxSex.Text;
            customer.PhoneNumber = txbPhoneNumber.Text;
            customer.DateOfBirth = datepickerDateOfBirth.Value;
            customer.Nationality = txbNationality.Text;
            customer.Address = txbAddress.Text;
            return customer;
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            Close();
        }
        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
