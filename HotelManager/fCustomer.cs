using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace HotelManager
{
    public partial class fCustomer : Form
    {
    
        #region Constructor
        internal fCustomer()
        {
            InitializeComponent();
            cbCustomerSearch.SelectedIndex = 3;
          
            LoadFullCustomer(GetFullCustomer());
            comboBoxSex.SelectedIndex = 0;
            SaveCustomer.OverwritePrompt = true;
            comboboxID.DisplayMember = "id";
            FormClosing += FCustomer_FormClosing;
            txbSearch.KeyPress += TxbSearch_KeyPress;
            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
        }

        #endregion

        #region Load
        private void LoadFullCustomer(DataTable table)
        {
            BindingSource source = new BindingSource();
            source.DataSource = table;
            dataGridViewCustomer.DataSource = source;
            bindingCustomer.BindingSource = source;
            comboboxID.DataSource = source;
        }
        
        #endregion

        #region Click

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {
            bool check;
            if (SaveCustomer.ShowDialog() == DialogResult.Cancel)
                return;
            switch (SaveCustomer.FilterIndex)
            {
                case 2:
                    check = ExportToExcel.Instance.Export(dataGridViewCustomer, SaveCustomer.FileName, ModeExportToExcel.XLSX);
                    break;
                case 3:
                    check = ExportToExcel.Instance.Export(dataGridViewCustomer, SaveCustomer.FileName, ModeExportToExcel.PDF);
                    break;
                default:
                    check = ExportToExcel.Instance.Export(dataGridViewCustomer, SaveCustomer.FileName, ModeExportToExcel.XLS);
                    break;
            }
            if (check)
                MessageBox.Show("Xuất file thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Lỗi (Cần cài đặt Office)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            new fAddCustomer().ShowDialog();
            if (btnCancel.Visible == false)
                LoadFullCustomer(GetFullCustomer());
            else
                BtnCancel_Click(null, null);
        }
        private void BindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            txbFullName.Text = string.Empty;
            txbAddress.Text = string.Empty;
            txbIDCard.Text = string.Empty;
            txbNationality.Text = string.Empty;
            txbPhoneNumber.Text = string.Empty;
        }
        private void BtnClose1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật khách hàng này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
                if (CheckDate())
                {
                    UpdateCustomer();
                    comboboxID.Focus();
                }
                else
                    MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            txbSearch.Text = txbSearch.Text.Trim();
            if (txbSearch.Text != string.Empty)
            {
                txbAddress.Text = string.Empty;
                txbFullName.Text = string.Empty;
                txbIDCard.Text = string.Empty;
                txbPhoneNumber.Text = string.Empty;
                txbNationality.Text = string.Empty;

                btnSearch.Visible = false;
                btnCancel.Visible = true;
                Search();
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LoadFullCustomer(GetFullCustomer());
            btnCancel.Visible = false;
            btnSearch.Visible = true;
        }
        #endregion

        #region Method
        public static bool CheckFillInText(Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control.Text == string.Empty)
                    return false;
            }
            return true;
        }
        public bool IsIdCardExists(string idCard)
        {
            return CustomerDAO.Instance.IsIdCardExists(idCard);
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private void UpdateCustomer()
        {
            Customer customerPre = groupCustomer.Tag as Customer;
            Customer customerNow = GetCustomerNow();
            if (comboboxID.Text == string.Empty)
            {
                MessageBox.Show("Khách hàng này chưa tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
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

                if (CustomerDAO.Instance.IsCusExistsByIdCard(customerNow.IdCard) && CustomerDAO.Instance.IsPhoneCusExists(customerNow.PhoneNumber))
                {
                    if (customerPre.IdCard != customerNow.IdCard && customerPre.PhoneNumber != customerNow.PhoneNumber)
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
                        if (customerPre.PhoneNumber != customerNow.PhoneNumber)
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
                        if (customerPre.IdCard != customerNow.IdCard)
                        {
                            MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                
                       
                        if (customerNow.Equals(customerPre))
                    {

                        MessageBox.Show("Bạn chưa thay đổi dữ liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                        else
                        {
                        
                        if (!IsIdCardExists(customerNow.IdCard) && !CustomerDAO.Instance.IsPhoneExists(customerNow.PhoneNumber))
                        {

                            bool check = CustomerDAO.Instance.UpdateCustomer(customerNow, customerPre);
                            if (check)
                            {
                                MessageBox.Show("Cập nhật thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                groupCustomer.Tag = customerNow;
                                int index = dataGridViewCustomer.SelectedRows[0].Index;
                                LoadFullCustomer(GetFullCustomer());
                                comboboxID.SelectedIndex = index;
                            }

                        }
                        else
                        {

                            bool check = CustomerDAO.Instance.UpdateCustomer(customerNow, customerPre);
                            if (check)
                            {
                                MessageBox.Show("Cập nhật thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                groupCustomer.Tag = customerNow;
                                int index = dataGridViewCustomer.SelectedRows[0].Index;
                                LoadFullCustomer(GetFullCustomer());
                                comboboxID.SelectedIndex = index;
                            }



                        }
                        
                    
                    
               
            }
            }
        }
        private void ChangeText(DataGridViewRow row)
        {
            if (row.IsNewRow)
            {
                txbFullName.Text = string.Empty;
                txbAddress.Text = string.Empty;
                txbIDCard.Text = string.Empty;
                txbNationality.Text = string.Empty;
                txbPhoneNumber.Text = string.Empty;
                bindingNavigatorMoveFirstItem.Enabled = false;
                bindingNavigatorMovePreviousItem.Enabled = false;
            }
            else
            {
                bindingNavigatorMoveFirstItem.Enabled = true;
                bindingNavigatorMovePreviousItem.Enabled = true;
                txbFullName.Text = row.Cells["colNameCustomer"].Value.ToString();
                txbAddress.Text = row.Cells["colAddress"].Value.ToString();
                txbIDCard.Text = row.Cells["colIDCard"].Value.ToString();
                txbNationality.Text = row.Cells["colNationality"].Value.ToString();
                txbPhoneNumber.Text = row.Cells["colPhone"].Value.ToString();
                comboBoxSex.SelectedItem = row.Cells["colSex"].Value;
                datepickerDateOfBirth.Value = (DateTime)row.Cells["colDateOfBirth"].Value;
                Customer customer = new Customer(((DataRowView)row.DataBoundItem).Row);
                groupCustomer.Tag = customer;
            }
        }
        private void Search()
        {
            string @string = txbSearch.Text;
            int mode = cbCustomerSearch.SelectedIndex;
            LoadFullCustomer(GetSearchCustomer(@string, mode));
        }

        #endregion

        #region GetData
        private Customer GetCustomerNow()
        {
            fStaff.Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { txbAddress, txbFullName, txbIDCard });
            Customer customer = new Customer();
            if (comboboxID.Text == string.Empty)
                customer.Id = 0;
            else
                customer.Id = int.Parse(comboboxID.Text);
            customer.IdCard = txbIDCard.Text;
            customer.Name = txbFullName.Text;
            customer.Sex = comboBoxSex.Text;
            customer.PhoneNumber = txbPhoneNumber.Text;
            customer.DateOfBirth = datepickerDateOfBirth.Value;
            customer.Nationality = txbNationality.Text;
            customer.Address = txbAddress.Text;
            return customer;
        }
        private DataTable GetFullCustomer()
        {
            return CustomerDAO.Instance.LoadFullCustomer();
        }
      
        /// <summary>
        /// --Mode is
        //--- 0 --- find along id
        //--- 1 --- find along name
        //--- 2 --- find along idCard
        //--- 3 --- find along NumberPhone
        /// </summary>
        /// <param name="string"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private DataTable GetSearchCustomer(string @string, int mode)
        {
            return CustomerDAO.Instance.Search(@string, mode);
        }

        #endregion

        #region Seclection Change
        private void DataGridViewCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCustomer.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewCustomer.SelectedRows[0];
                ChangeText(row);
            }
        }
        #endregion

        #region Check isDigit

        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        private bool CheckDate()
        {
            if (DateTime.Now.Subtract(datepickerDateOfBirth.Value).Days <= 0)
                return false;
            else return true;
        }

        #endregion

        #region Enter
        private void Txb_Enter(object sender, EventArgs e)
        {
            Bunifu.Framework.UI.BunifuMetroTextbox text = sender as Bunifu.Framework.UI.BunifuMetroTextbox;
            text.Tag = text.Text;
        }
        #endregion
        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;


            // ^ ([a - zA - Z0 - 9\.\-_ ?@] +)$

        }
        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/' || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        #region Leave
        private void Txb_Leave(object sender, EventArgs e)
        {
            Bunifu.Framework.UI.BunifuMetroTextbox text = sender as Bunifu.Framework.UI.BunifuMetroTextbox;
            if (text.Text == string.Empty)
                text.Text = text.Tag as string;
        }
        #endregion
        private void TxbCMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        #region Key
        private void TxbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                BtnSearch_Click(sender, null);
            else
                if (e.KeyChar == 27 && btnCancel.Visible == true)
                BtnCancel_Click(sender, null);
        }
        private void FCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 && btnCancel.Visible == true)
                BtnCancel_Click(sender, null);
        }
        #endregion

        #region Close
        private void FCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoadFullCustomer(GetFullCustomer());
        }
        #endregion

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Customer customerNow = GetCustomerNow();
            DialogResult result = MessageBox.Show("Bạn có muốn xóa thông tin khách hàng?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
                if (BillDAO.Instance.IsIDCustomerOtherBillExists(customerNow.Id)||BillDAO.Instance.IsIDCustomerBillExists(customerNow.Id) ||CustomerDAO.Instance.IsCustomerBookRoomExists(customerNow.Id.ToString()) || CustomerDAO.Instance.IsCustomerBookRoomDetailExists(customerNow.Id.ToString()))
                {

                    MessageBox.Show("Không thể xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    bool check = CustomerDAO.Instance.DeleteCustomer(customerNow.Id.ToString());
                    if (check)
                    {
                        MessageBox.Show("Xóa thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFullCustomer(GetFullCustomer());

                    }
                }
            }
        }
               
            
    }
}
