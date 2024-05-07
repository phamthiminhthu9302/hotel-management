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
    public partial class fAddStaff : Form
    {
       
        public fAddStaff()
        {
            
            InitializeComponent();
            LoadFullStaffType();
            comboBoxSex.SelectedIndex = 0;
        }
      
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thêm nhân viên mới không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {

                if (CheckDate())
                {
                    InsertStaff();
                }
            }

        }
        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '\b'  
                ))
                e.Handled = true;
            // ^ ([a - zA - Z0 - 9\.\-_ ?@] +)$

        }
        private void TxbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        private void LoadFullStaffType()
        {
            comboBoxSex.SelectedIndex = 0;
            DataTable table = GetFullStaffType();
            comboBoxStaffType.DataSource = table;
            comboBoxStaffType.DisplayMember = "Name";
            if (table.Rows.Count > 0)
                comboBoxStaffType.SelectedIndex = 0;
        }
        private DataTable GetFullStaffType()
        {
            return AccountTypeDAO.Instance.LoadFullStaffType();
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void InsertStaff()
        {
            Account accountNow = GetStaffNow();
            accountNow.PassWord = fStaff.HassPass;
            bool isFill = fCustomer.CheckFillInText(new Control[] { txbName, comboBoxStaffType, txbName ,
                                                            txbIDcard , comboBoxSex , txbPhoneNumber, txbAddress});
            if (isFill)
            {
                if (txbIDcard.Text.Length != 9)
                {
                    MessageBox.Show("Thông tin CMND không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txbPhoneNumber.Text.Length != 10 || IsPhoneNumberValid(txbPhoneNumber.Text) == false) 
                {
                    MessageBox.Show("Thông tin số điện thoại không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (AccountDAO.Instance.IsPhoneAccExists(accountNow.PhoneNumber) && AccountDAO.Instance.IsExistsAccByIdCard(accountNow.IdCard))
                {
                    
                        MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    
                }
                if (CustomerDAO.Instance.IsPhoneExists(accountNow.PhoneNumber))
                {
                    
                        MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    
                    
                }
                if (CustomerDAO.Instance.IsIdCardExists(accountNow.IdCard))
                {
                   
                        MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    
                }
                if (AccountDAO.Instance.IsAccExistByUserName(accountNow.UserName))
                {
                    MessageBox.Show("Trùng tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
  
                        if (!AccountDAO.Instance.IsAccExistByUserName(accountNow.UserName) && !CustomerDAO.Instance.IsIdCardExists(accountNow.IdCard)&& !CustomerDAO.Instance.IsPhoneExists(accountNow.PhoneNumber)) {
                            AccountDAO.Instance.InsertAccount(accountNow);
                            MessageBox.Show("Thêm Thành Công\n Mật khẩu mặc đinh cho tài khoản " + txbName.Text +
                                ": 123456", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txbFullName.Text = txbName.Text = comboBoxStaffType.Text = txbName.Text = txbIDcard.Text = comboBoxSex.Text = txbPhoneNumber.Text = txbAddress.Text = String.Empty;

                        }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            }
            
        }
        private Account GetStaffNow()
        {
            Account account = new Account();

            #region Format
            fStaff.Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { txbName, txbIDcard, txbAddress, txbFullName });
            #endregion

            account.UserName = txbName.Text.ToLower();
            int index = comboBoxStaffType.SelectedIndex;
            account.IdStaffType = (int)((DataTable)comboBoxStaffType.DataSource).Rows[index]["id"];
            account.Name = txbFullName.Text;
            account.IdCard = txbIDcard.Text;
            account.Sex = comboBoxSex.Text;
            account.DateOfBirth = datepickerDateOfBirth.Value;
            account.PhoneNumber = txbPhoneNumber.Text;
            account.Address = txbAddress.Text;
            account.StartDay = datePickerStartDay.Value;
            return account;
        }

        private bool CheckDate()
        {
            if (!CheckTrueDate(datepickerDateOfBirth.Value, DateTime.Now))
            {
                MessageBox.Show("Ngày sinh không hợp lệ (Tuổi phải lớn hơn 18)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                if (!CheckTrueDate(datepickerDateOfBirth.Value, datePickerStartDay.Value))
            {
                MessageBox.Show("Ngày vào làm không hợp lệ (Lớn hơn 18 tuổi)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckTrueDate(DateTime date1, DateTime date2)
        {
            if (date2.Subtract(date1).Days < 6574)
                return false;
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
       
        private void fAddStaff_Load(object sender, EventArgs e)
        {
            datePickerStartDay.Value = DateTime.Now;
            comboBoxSex.SelectedIndex = 1;
        }

        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/'|| char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
    }

}
