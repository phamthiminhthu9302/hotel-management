﻿using HotelManager.DAO;
using HotelManager.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManager
{
    public partial class fStaff : Form
    {
        #region Properties
        public static readonly string HassPass = "e10adc3949ba59abbe56e057f20f883e"; // password default
        #endregion

        #region Constructor
        internal fStaff()
        {
            InitializeComponent();
            LoadFullStaffType();
            LoadFullStaff(GetFullStaff());
            txbSearch.KeyPress += TxbSearch_KeyPress;
            KeyPress += FStaff_KeyPress;
            dataGridStaff.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
        }


        #endregion

        #region Click
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            new fAddStaff().ShowDialog();
            if (btnCancel.Visible == false)
                LoadFullStaff(GetFullStaff());
            else
                BtnCancel_Click(null, null);
           
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show( "Bạn có muốn cập nhật nhân viên này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
                if (CheckDate())
                {
                    UpdateStaff();
                }
            }
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (fCustomer.CheckFillInText(new Control[] { txbUserName }))
            {
                DialogResult result = MessageBox.Show( "Bạn có muốn đặt lại mật khẩu với tên đăng nhập " + txbUserName.Text + " không?", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.OK)
                {
                    ResetPassword();
                }
            }
            else
            {
                MessageBox.Show( "Không được để trống tên đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {
            if (saveStaff.ShowDialog() == DialogResult.Cancel)
                return;
            else
            {
                bool check;
                try
                {
                    switch (saveStaff.FilterIndex)
                    {
                        case 2:
                            check = ExportToExcel.Instance.Export(dataGridStaff, saveStaff.FileName, ModeExportToExcel.XLSX);
                            break;
                        case 3:
                            check = ExportToExcel.Instance.Export(dataGridStaff, saveStaff.FileName, ModeExportToExcel.PDF);
                            break;
                        default:
                            check = ExportToExcel.Instance.Export(dataGridStaff, saveStaff.FileName, ModeExportToExcel.XLS);
                            break;
                    }
                    if (check)
                        MessageBox.Show( "Xuất thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show( "Lỗi xuất thất bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    MessageBox.Show( "Lỗi (Cần cài đặt Office)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            txbUserName.Text = string.Empty;
            txbName.Text = string.Empty;
            txbIDcard.Text = string.Empty;
            txbPhoneNumber.Text = string.Empty;
            txbAddress.Text = string.Empty;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            txbSearch.Text = txbSearch.Text.Trim();
            if (txbSearch.Text != string.Empty)
            {
                txbUserName.Text = string.Empty;
                txbName.Text = string.Empty;
                txbIDcard.Text = string.Empty;
                txbPhoneNumber.Text = string.Empty;
                txbAddress.Text = string.Empty;

                btnSearch.Visible = false;
                btnCancel.Visible = true;
                Search();
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LoadFullStaff(GetFullStaff());
            btnCancel.Visible = false;
            btnSearch.Visible = true;
        }
        #endregion

        #region Method
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void UpdateStaff()
        {
            Account accountPre = groupStaff.Tag as Account;

            Account accountnow = GetStaffNow();
            bool isFill = fCustomer.CheckFillInText(new Control[] { txbUserName, comboBoxStaffType, txbName ,
                                                            txbIDcard , comboBoxSex , txbPhoneNumber, txbAddress});
            if (!isFill)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if ( txbIDcard.Text.Length != 9)
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
                    if (accountPre.IdCard != accountnow.IdCard && accountPre.PhoneNumber != accountnow.PhoneNumber)
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
                        if (accountPre.PhoneNumber != accountnow.PhoneNumber)
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
                        if (accountPre.IdCard != accountnow.IdCard)
                        {
                            MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
               
                   
                        if (accountnow.Equals(accountPre))
                        {
                            MessageBox.Show("Bạn chưa thay đổi dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (!CustomerDAO.Instance.IsIdCardExists(accountnow.IdCard) && !CustomerDAO.Instance.IsPhoneExists(accountnow.PhoneNumber))
                            {
                             
                                if (AccountDAO.Instance.UpdateAccount(accountnow))
                                {
                                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    groupStaff.Tag = accountnow;
                                    if (btnCancel.Visible == false)
                                    {
                                        int index = dataGridStaff.SelectedRows[0].Index;
                                        LoadFullStaff(GetFullStaff());
                                        dataGridStaff.SelectedRows[0].Selected = false;
                                        dataGridStaff.Rows[index].Selected = true;
                                    }
                                }
                            }
                            else{
                           
                                    if (AccountDAO.Instance.UpdateAccount(accountnow))
                                    {
                                        MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        groupStaff.Tag = accountnow;
                                        if (btnCancel.Visible == false)
                                        {
                                            int index = dataGridStaff.SelectedRows[0].Index;
                                            LoadFullStaff(GetFullStaff());
                                            dataGridStaff.SelectedRows[0].Selected = false;
                                            dataGridStaff.Rows[index].Selected = true;
                                        }
                                    }
                               
                            
                               
                               
                            }
                            
                               
                            
                        
                   
                }
               
                
            }

        }
        private void ResetPassword()
        {
            try
            {
                bool check = AccountDAO.Instance.ResetPassword(txbUserName.Text, HassPass );
                if (check)
                {
                    MessageBox.Show( "Đặt lại mật khẩu thành công\nMật khẩu mặt định là: 123456", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show( "Không thể đặt lại mật khẩu(Tên đăng nhập chưa tồn tại)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch
            {
                MessageBox.Show( "Lỗi không xác định", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/' || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
    
    private void Search()
        {
            LoadFullStaff(GetSearchStaff());
        }
        private void ChangeText(DataGridViewRow row)
        {
            if (row.IsNewRow)
            {
                txbUserName.Text = string.Empty;
                txbName.Text = string.Empty;
                txbIDcard.Text = string.Empty;
                txbPhoneNumber.Text = string.Empty;
                txbAddress.Text = string.Empty;
            }
            else
            {
                txbUserName.Text = row.Cells[colUserName.Name].Value as string;
                txbAddress.Text = row.Cells[colAddress.Name].Value as string;
                txbName.Text = row.Cells[colname.Name].Value as string;
                txbPhoneNumber.Text = row.Cells[colPhone.Name].Value.ToString();
                txbIDcard.Text = row.Cells[colIDCard.Name].Value as string;
                datepickerDateOfBirth.Text = row.Cells[colDateOfBirth.Name].Value as string;
                datePickerStartDay.Text = row.Cells[colStartDay.Name].Value as string;
                comboBoxSex.Text = row.Cells[colSex.Name].Value as string;
                comboBoxStaffType.Text= row.Cells[colNameStaffType.Name].Value.ToString() ;


                Account staff = new Account();
                staff.UserName = txbUserName.Text;
                staff.Address = txbAddress.Text;
                staff.Name = txbName.Text;
                staff.PhoneNumber = txbPhoneNumber.Text;
                staff.IdCard = txbIDcard.Text;
                staff.DateOfBirth = datepickerDateOfBirth.Value;
                staff.StartDay = datePickerStartDay.Value;
                staff.Sex = comboBoxSex.Text;
                staff.IdStaffType = (int)row.Cells[colIDStaffType.Name].Value;
                groupStaff.Tag = staff;
                //bindingNavigatorMoveFirstItem.Enabled = true;
                //bindingNavigatorMovePreviousItem.Enabled = true;
            }
        }
        internal static void Trim(Bunifu.Framework.UI.BunifuMetroTextbox[] textboxes)
        {
            for (int i = 0; i < textboxes.Length; i++)
            {
                textboxes[i].Text = textboxes[i].Text.Trim();
            }
        }
        #endregion

        #region Load data
        private void LoadFullStaff(System.Data.DataTable table)
        {
            BindingSource source = new BindingSource();
            source.DataSource = table;
            dataGridStaff.DataSource = source;
            bindingStaff.BindingSource = source;
        }
        private void LoadFullStaffType()
        {
            comboBoxSex.SelectedIndex = 0;
            System.Data.DataTable table = GetFullStaffType();
            comboBoxStaffType.DataSource = table;
            comboBoxStaffType.DisplayMember = "Name";
            if (table.Rows.Count > 0)
                comboBoxStaffType.SelectedIndex = 0;
        }
        #endregion

        #region GetData
        private System.Data.DataTable GetFullStaff()
        {
            return AccountDAO.Instance.LoadFullStaff();
        }
        private System.Data.DataTable GetFullStaffType()
        {
            return AccountTypeDAO.Instance.LoadFullStaffType();
        }
        private Account GetStaffNow()
        {
            Account account = new Account();

            #region Format
            Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { txbName, txbIDcard, txbAddress });
            #endregion
          
            int index = comboBoxStaffType.SelectedIndex;
            account.UserName = txbUserName.Text.ToLower();
            account.IdStaffType = (int)((System.Data.DataTable)comboBoxStaffType.DataSource).Rows[index]["id"];
            account.Name = txbName.Text;
            account.IdCard = txbIDcard.Text;
            account.Sex = comboBoxSex.Text;
            account.DateOfBirth = datepickerDateOfBirth.Value;
            account.PhoneNumber = txbPhoneNumber.Text;
            account.Address = txbAddress.Text;
            account.StartDay = datePickerStartDay.Value;
            return account;
        }
        private System.Data.DataTable GetSearchStaff()
        {
            if (int.TryParse(txbSearch.Text, out int phoneNumber))
                return AccountDAO.Instance.Search(txbSearch.Text, phoneNumber);
            else
                return AccountDAO.Instance.Search(txbSearch.Text, 0);
        }
        #endregion

        #region Check isDigit
        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private bool CheckTrueDate(DateTime date1, DateTime date2)
        {
            if (date2.Subtract(date1).Days < 6574)
                return false;
            return true;
        }
        private bool CheckDate()
        {
            if (!CheckTrueDate(datepickerDateOfBirth.Value, DateTime.Now))
            {
                MessageBox.Show( "Ngày sinh không hợp lệ (Tuổi phải lớn hơn 18)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                if (!CheckTrueDate(datepickerDateOfBirth.Value, datePickerStartDay.Value))
            {
                MessageBox.Show( "Ngày vào làm không hợp lệ (Lớn hơn 18 tuổi)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        
            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        
             
            // ^ ([a - zA - Z0 - 9\.\-_ ?@] +)$

        }
        #endregion

        #region ChangeText
        private void DataGridStaffType_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridStaff.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridStaff.SelectedRows[0];
                ChangeText(row);
            }
        }
        #endregion

        #region Key
        private void TxbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                BtnSearch_Click(sender, null);
            else
                if (e.KeyChar == 27 && btnCancel.Visible == true)
                BtnCancel_Click(sender, null);
        }
        private void FStaff_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 && btnCancel.Visible == true)
                BtnCancel_Click(sender, null);
        }
        #endregion

        #region Enter & Leave
        private void Txb_Enter(object sender, EventArgs e)
        {
            Bunifu.Framework.UI.BunifuMetroTextbox textbox = sender as Bunifu.Framework.UI.BunifuMetroTextbox;
            textbox.Tag = textbox.Text;
        }
        private void Txb_Leave(object sender, EventArgs e)
        {
            Bunifu.Framework.UI.BunifuMetroTextbox textbox = sender as Bunifu.Framework.UI.BunifuMetroTextbox;
            if(textbox.Text == string.Empty)
            {
                textbox.Text = textbox.Tag as string;
            }
        }
        #endregion

        #region Close
        private void FStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnCancel_Click(null, null);
        }
        #endregion

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            fAccess f = new fAccess();
            f.ShowDialog();
            LoadFullStaffType();
            if (btnCancel.Visible == false)
                LoadFullStaff(GetFullStaff());
            else
                BtnCancel_Click(null, null);
        }

        private void btnDeleteClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa thông tin nhân viên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
                DeleteStaff();
            txbUserName.Focus();
        }
        private void DeleteStaff()
        {
           
                    Account StaffNow = GetStaffNow();
                    int id=AccountDAO.Instance.GetIdAccByUserName(StaffNow.UserName.ToString());
                    if (AccountDAO.Instance.IsBookRoomExistsAcc(id) || AccountDAO.Instance.USP_IsBillExistsAcc(id))
                    {
                MessageBox.Show("Không thể xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
                    else
                    {
                        bool check = AccountDAO.Instance.DeleteAccount(txbUserName.Text.ToLower());
                        if (check)
                        {

                            MessageBox.Show("Xóa thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadFullStaff(GetFullStaff());

                        }
                        else
                            MessageBox.Show("Xóa thông tin nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
                   
            
           

        }

        private void TxbCMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
