using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fChangeRoom : Form
    {
        int idRoom;
        public fChangeRoom(int _idBRoom)
        {
            InitializeComponent();
            idRoom = _idBRoom;
            LoadListRoomType();
           
        }

        public void LoadListRoomType()
        {
            List<RoomType> rooms = RoomTypeDAO.Instance.LoadListRoomType();
            cbRoomType.DataSource = rooms;
            cbRoomType.DisplayMember = "Name";
           

           
        }
        public void LoadEmptyRoom(int idRoomType)
        {
            List<Room> rooms = RoomDAO.Instance.LoadEmptyRoom(idRoomType);
            cbRoom.DataSource = rooms;
            cbRoom.DisplayMember = "Name";
           
        }
        public void LoadRoomTypeInfo(int idRoom)
        {
          
            RoomType roomType = RoomTypeDAO.Instance.GetRoomTypeByIdRoom(idRoom);
            txbRoomTypeName.Text = roomType.Name;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbRoomTypeName.Text = (cbRoomType.SelectedItem as RoomType).Name;
            LoadEmptyRoom((cbRoomType.SelectedItem as RoomType).Id);
            CultureInfo cultureInfo = new CultureInfo("vi-vn");
            txbLimitPerson.Text = (cbRoomType.SelectedItem as RoomType).LimitPerson.ToString();
            txbPrice.Text = (cbRoomType.SelectedItem as RoomType).Price.ToString("c", cultureInfo);
            if (cbRoom.Text == "")
            {
                cbRoom.Enabled = false;
                txbRoomName.Text = "";
                cbRoom.BackColor = Color.WhiteSmoke;
                txbRoomName.BackColor = Color.WhiteSmoke;
            }
            else
            {
                cbRoom.Enabled = true;
                LoadRoomTypeInfo((cbRoom.SelectedItem as Room).Id);
                cbRoom.BackColor = Color.White;
                txbRoomName.BackColor = Color.White;
            }

        }

        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbRoomName.Text = (cbRoom.SelectedItem as Room).Name;
           
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //Phải cập nhật trạng thái của phòng cũ
            RoomDAO.Instance.UpdateStatusRoomOld(idRoom);
            BookRoomDAO.Instance.UpdateReceiveRoom(idRoom, (cbRoom.SelectedItem as Room).Id);
            MessageBox.Show("Đổi phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
