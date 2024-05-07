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
    public partial class fBookRoomDetails : Form
    {
        int idReceiveRoom;
        public fBookRoomDetails(int _idReceiRoom)
        {
            InitializeComponent();
            idReceiveRoom = _idReceiRoom;
            ShowReceiveRoom(_idReceiRoom);
            ShowCustomers(_idReceiRoom);
           
        }
        public void ShowReceiveRoom(int idReceiveRoom)
        {
            DataRow data = BookRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
            txbIDReceiveRoom.Text = ((int)data["Mã đặt phòng"]).ToString();
            txbRoomName.Text = data["Tên phòng"].ToString();
            dpkDateCheckIn.Value = (DateTime)data["Ngày nhận"];
            dpkDateCheckOut.Value = (DateTime)data["Ngày trả"];
            txbRoomTypeName.Text = data["Loại"].ToString() ;
    }
        public void ShowCustomers(int idReceiveRoom)
        {
            dataGridView.DataSource = BookRoomDAO.Instance.ShowCusomers(idReceiveRoom);
        }
        
        public void LoadDays()
        {
            txbDays.Text = (dpkDateCheckOut.Value.Date - dpkDateCheckIn.Value.Date).Days.ToString();
        }
        private void dpkDateCheckIn_onValueChanged(object sender, EventArgs e)
        {
            if (dpkDateCheckIn.Value <= DateTime.Now)
            {

                DataRow data = BookRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
                dpkDateCheckIn.Value = (DateTime)data["Ngày nhận"];
            }
               
            if (dpkDateCheckOut.Value <= dpkDateCheckIn.Value)
            {
                DataRow data = BookRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
                dpkDateCheckIn.Value = (DateTime)data["Ngày nhận"];
            }
            LoadDays();
        }

        private void dpkDateCheckOut_onValueChanged(object sender, EventArgs e)
        {
            if (dpkDateCheckOut.Value < DateTime.Now)
            {
                DataRow data = BookRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
                dpkDateCheckOut.Value = (DateTime)data["Ngày trả"];
            }
               
            if (dpkDateCheckOut.Value <= dpkDateCheckIn.Value)
            {
                DataRow data = BookRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
                dpkDateCheckOut.Value = (DateTime)data["Ngày trả"];
            }
                
            LoadDays();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            
            BookRoomDAO.Instance.UpdateBookRoom(idReceiveRoom, dpkDateCheckIn.Value, dpkDateCheckOut.Value);
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowReceiveRoom(idReceiveRoom);
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (BillDAO.Instance.IsExistBillPay(idReceiveRoom))
            {
                MessageBox.Show("Phòng này đã thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                fAddCustomerInfo f = new fAddCustomerInfo(idReceiveRoom);
                f.ShowDialog();
                Show();
                if (fAddCustomerInfo.ListIdCustomer.Count > 0)
                    foreach (var item in fAddCustomerInfo.ListIdCustomer)
                    {

                        BookRoomDAO.Instance.InsertReceiveRoomDetails(idReceiveRoom, item);


                    }
                ShowCustomers(idReceiveRoom);
            }
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (BillDAO.Instance.IsExistBillPay(idReceiveRoom))
            {
                MessageBox.Show("Phòng này đã thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string idCard = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
                int idCustomer = CustomerDAO.Instance.GetInfoByIdCard(idCard).Id;
                if (idCustomer != CustomerDAO.Instance.GetIDCustomerFromBookRoom(idReceiveRoom))
                {
                    BookRoomDAO.Instance.DeleteReceiveRoomDetails(idReceiveRoom, idCustomer);
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowCustomers(idReceiveRoom);
                }
                else
                    MessageBox.Show("Không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            if (BillDAO.Instance.IsExistBillPay(idReceiveRoom))
            {
                MessageBox.Show("Phòng này đã thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string idCard = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
                int idCustomer = CustomerDAO.Instance.GetInfoByIdCard(idCard).Id;
                fUpdateCustomerInfo f = new fUpdateCustomerInfo(idCard, idCustomer);
                f.ShowDialog();
                Show();
                ShowCustomers(idReceiveRoom);
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            fChangeRoom f = new fChangeRoom(idReceiveRoom);
            f.ShowDialog();
            Show();
            ShowReceiveRoom(idReceiveRoom);
        }
    }
}
