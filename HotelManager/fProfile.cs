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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HotelManager
{
    public partial class fProfile : Form
    {
        int userName;
       
        String idCard, phone;
        public fProfile(int userName)
        {
            this.userName = userName;
            InitializeComponent();
            LoadProfile(userName);
           
        }
        string Password;
        public void LoadProfile(int username)
        {
            Account staff = AccountDAO.Instance.LoadStaffInforByUserName(username);
            lblUserName.Text = txbUserName.Text = staff.UserName;
            lblDisplayName.Text = txbDisplayName.Text = staff.Name;
            txbStaffType.Text = AccountTypeDAO.Instance.GetStaffTypeByUserName(username).Name;
            idCard=txbIDCard.Text = staff.IdCard.ToString();
            phone=txbPhoneNumber.Text = staff.PhoneNumber.ToString();
            txbAddress.Text = staff.Address;
            dpkDateOfBirth.Value = staff.DateOfBirth;
            cbSex.Text = staff.Sex;
            txbStartDay.Text = staff.StartDay.ToShortDateString();
            Password = staff.PassWord;
        }
        public void UpdateDisplayName(int username, string displayname)
        {
            AccountDAO.Instance.UpdateDisplayName(username, displayname);
        }
        public void UpdatePassword(string username, string password)
        {
            AccountDAO.Instance.UpdatePassword(username, password);
        }
        public void UpdateInfo(string username, string address, string phonenumber,string idCard, DateTime dateOfBirth, string sex)
        {
            AccountDAO.Instance.UpdateInfo(username, address, phonenumber,idCard,dateOfBirth,sex);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBookRoom_Click(object sender, EventArgs e)
        {
            if(txbDisplayName.Text!=String.Empty)
            {
                UpdateDisplayName(userName, txbDisplayName.Text);
                MessageBox.Show( "Cập nhật thông tin tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProfile(userName);
            }
            else
                MessageBox.Show( "Tên hiển thị không hợp lệ.\nVui lòng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private Account GetStaffNow()
        {
            Account account = new Account();
          
            account.UserName = txbUserName.Text.ToLower();
            account.IdStaffType = 1;
            account.Name = txbDisplayName.Text;
            account.IdCard = txbIDCard.Text;
            account.Sex = cbSex.Text;
            account.DateOfBirth = dpkDateOfBirth.Value;
            account.PhoneNumber = txbPhoneNumber.Text;
            account.Address = txbAddress.Text;
            account.StartDay = DateTime.Today;
            return account;
        }
        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (txbNewPass.Text != String.Empty && txbReNewPass.Text != String.Empty && txbPass.Text != String.Empty)
            {
                if (AccountDAO.Instance.HashPass(txbPass.Text) == Password)
                {
                
                    if (txbNewPass.Text == txbReNewPass.Text)
                    {
                        UpdatePassword(txbUserName.Text, txbNewPass.Text);
                        MessageBox.Show("Cập nhật mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txbPass.Text = txbNewPass.Text = txbReNewPass.Text = String.Empty;
                        LoadProfile(userName);
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu mới không hợp lệ.\nVui lòng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txbNewPass.Text = txbReNewPass.Text = String.Empty;
                    }
                
                }else
                 {
                MessageBox.Show( "Mật khẩu cũ không hợp lệ.\nVui lòng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPass.Text=txbNewPass.Text = txbReNewPass.Text = String.Empty;
                 }
            }else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
           

            Account accountnow = GetStaffNow();
           
            if (txbIDCard.Text!= String.Empty&& txbAddress.Text != String.Empty && txbPhoneNumber.Text!=String.Empty && cbSex.Text!=string.Empty && dpkDateOfBirth.Value<DateTime.Now.Date)
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
                if (AccountDAO.Instance.IsPhoneAccExists(accountnow.PhoneNumber) && AccountDAO.Instance.IsExistsAccByIdCard(accountnow.IdCard))
                {
                    if (idCard != accountnow.IdCard && phone!= accountnow.PhoneNumber)
                    {
                        MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (CustomerDAO.Instance.IsPhoneExists(accountnow.PhoneNumber))
                {
                    if (!AccountDAO.Instance.IsPhoneAccExists(accountnow.PhoneNumber))
                    {
                        MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (phone != accountnow.PhoneNumber)
                        {
                            MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                if (CustomerDAO.Instance.IsIdCardExists(accountnow.IdCard))
                {
                    if (!AccountDAO.Instance.IsExistsAccByIdCard(accountnow.IdCard))
                    {
                        MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (idCard != accountnow.IdCard)
                        {
                            MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }


                

                    UpdateInfo(txbUserName.Text, txbAddress.Text, txbPhoneNumber.Text, txbIDCard.Text, dpkDateOfBirth.Value, cbSex.Text);
                    MessageBox.Show("Cập nhật thông tin cơ bản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProfile(userName);
                
            }
            else
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void txbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

       
        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/' || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        private void TxbPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || 
                e.KeyChar == '\b'))
                e.Handled = true;
        }
        private void TxbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
    }
}
