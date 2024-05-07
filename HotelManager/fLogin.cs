using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        public bool Login()
        {
            return AccountDAO.Instance.Login(txbUserName.Text, txbPassWord.Text);
        }
        public Account IDLogin()
        {
            return AccountDAO.Instance.IDLogin(txbUserName.Text, txbPassWord.Text);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '\b'
                ))
                e.Handled = true;
            // ^ ([a - zA - Z0 - 9\.\-_ ?@] +)$

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
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (!CheckFillInText(new Control[] { txbUserName, txbPassWord }))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {

                if (Login())
                {
                    this.Hide();
                    Account account = IDLogin();

                    fManagement f = new fManagement(account.ID);
                    f.ShowDialog();


                }
                else
                {
                    MessageBox.Show("Tên Đăng Nhập không tồn tại hoặc mật Khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        
        }

        private void btnExit__Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txbPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnLogin_Click_1(sender, null);
        }
    }
}
