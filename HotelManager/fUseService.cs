using HotelManager.DAO;
using HotelManager.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace HotelManager
{
    public partial class fUseService : Form
    {
        int staffSetUp;
        int IdServiceOld;
        int totalPrice = 0;
        public fUseService(int userName)
        {
            staffSetUp = userName;
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            LoadListServiceType();
            LoadListRoomType();
            LoadListFullRoom();
            ShowSurcharge();



        }
        public void Pay(int idBill, int discount)
        {
            BillDAO.Instance.UpdateRoomPrice(idBill);
            BillDAO.Instance.UpdateServicePrice(idBill);
            BillDAO.Instance.UpdateOther(idBill, discount);

        }
        public void LoadListRoomType()
        {
            List<RoomType> roomTypes = RoomTypeDAO.Instance.LoadListRoomType();
            switch (roomTypes.Count)
            {
                case 0:
                    {
                        color1.Visible = color2.Visible = color3.Visible = color4.Visible = color5.Visible = false;
                        lblRoomType1.Visible = lblRoomType2.Visible = lblRoomType3.Visible = lblRoomType4.Visible = lblRoomType5.Visible = false;
                        break;
                    }
                case 1:
                    {
                        lblRoomType1.Text = roomTypes[0].Name;
                        color2.Visible = color3.Visible = color4.Visible = color5.Visible = false;
                        lblRoomType2.Visible = lblRoomType3.Visible = lblRoomType4.Visible = lblRoomType5.Visible = false;
                        break;
                    }
                case 2:
                    {
                        lblRoomType1.Text = roomTypes[0].Name;
                        lblRoomType2.Text = roomTypes[1].Name;
                        color3.Visible = color4.Visible = color5.Visible = false;
                        lblRoomType3.Visible = lblRoomType4.Visible = lblRoomType5.Visible = false;
                        break;
                    }
                case 3:
                    {
                        lblRoomType1.Text = roomTypes[0].Name;
                        lblRoomType2.Text = roomTypes[1].Name;
                        lblRoomType3.Text = roomTypes[2].Name;
                        color4.Visible = color5.Visible = false;
                        lblRoomType4.Visible = lblRoomType5.Visible = false;
                        break;
                    }
                case 4:
                    {
                        lblRoomType1.Text = roomTypes[0].Name;
                        lblRoomType2.Text = roomTypes[1].Name;
                        lblRoomType3.Text = roomTypes[2].Name;
                        lblRoomType4.Text = roomTypes[3].Name;
                        color5.Visible = false;
                        lblRoomType5.Visible = false;
                        break;
                    }
            }
        }
        
        public void DrawControl(Room room, Bunifu.Framework.UI.BunifuTileButton button)
        {
            int idRoomTypeName = RoomTypeDAO.Instance.GetRoomTypeByIdRoom(room.Id).Id;
            switch (idRoomTypeName)
            {
                case 1:
                    {
                        button.BackColor = System.Drawing.Color.Tomato;
                        button.color = System.Drawing.Color.Tomato;
                        button.colorActive = System.Drawing.Color.LightSalmon;
                        break;
                    }
                case 2:
                    {
                        button.BackColor = System.Drawing.Color.Violet;
                        button.color = System.Drawing.Color.Violet;
                        button.colorActive = System.Drawing.Color.Thistle;
                        break;
                    }
                case 3:
                    {
                        button.BackColor = System.Drawing.Color.DeepSkyBlue;
                        button.color = System.Drawing.Color.DeepSkyBlue;
                        button.colorActive = System.Drawing.Color.LightSkyBlue;
                        break;
                    }
                case 4:
                    {
                        button.BackColor = System.Drawing.Color.LimeGreen;
                        button.color = System.Drawing.Color.LimeGreen;
                        button.colorActive = System.Drawing.Color.LightGreen;
                        break;
                    }
                default:
                    {
                        button.BackColor = System.Drawing.Color.Gray;
                        button.color = System.Drawing.Color.Gray;
                        button.colorActive = System.Drawing.Color.Silver;
                        break;
                    }
            }
        }
        public void LoadListFullRoom()
        {
            flowLayoutRooms.Controls.Clear();
            List<Room> rooms = RoomDAO.Instance.LoadListFullRoom(DateTime.Now.Date);
            foreach (Room item in rooms)
            {
                Bunifu.Framework.UI.BunifuTileButton button = new Bunifu.Framework.UI.BunifuTileButton();
                button.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.ForeColor = System.Drawing.Color.White;
                button.Image = global::HotelManager.Properties.Resources.Room;
                button.ImagePosition = 14;
                button.ImageZoom = 36;
                button.LabelPosition = 29;
                button.Size = new System.Drawing.Size(110, 95);
                button.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);

                button.Tag = item;
                button.LabelText = item.Name;
                button.Click += Button_Click;

                DrawControl(item, button);

                flowLayoutRooms.Controls.Add(button);

                //BillDAO.Instance.UpdateRoomPrice(item.Id);
            }
        }
   
        private void Button_Click(object sender, EventArgs e)
        {
            dataGridViewBookRoom.Rows.Clear();
            totalPrice = 0;
            Bunifu.Framework.UI.BunifuTileButton button = sender as Bunifu.Framework.UI.BunifuTileButton;
            flowLayoutRooms.Tag = button.Tag;
            button.BackColor = System.Drawing.Color.SeaGreen;
            button.color = System.Drawing.Color.SeaGreen;
            button.colorActive = System.Drawing.Color.MediumSeaGreen;
            foreach (var item in flowLayoutRooms.Controls)
            {
                if (item != button)
                    DrawControl((item as Bunifu.Framework.UI.BunifuTileButton).Tag as Room, item as Bunifu.Framework.UI.BunifuTileButton);
            }
            Room room = button.Tag as Room;
            if (!BillDAO.Instance.IsExistsBill(room.Id))
            {
                int idReceiveRoom = BookRoomDAO.Instance.GetIdReceiveRoomFromIdRoom(room.Id);
                InsertBill(idReceiveRoom, staffSetUp);
            }
            ShowBill(room.Id);
            if (dataGridViewBookRoom.Rows[0].Cells[0].Value.ToString().Equals(""))
            {
                LoadListServiceType();
            }
            
            
            BillDAO.Instance.UpdateRoomPrice(BillDAO.Instance.GetIdBillFromIdRoom(room.Id));
           
            ShowBillRoom(room.Id);
            txbTotalPrice.Text = totalPrice.ToString("c0", new CultureInfo("vi-vn"));

        }

        public bool IsExistsBill(int idRoom)
        {
            return BillDAO.Instance.IsExistsBill(idRoom);
        }
        public bool IsExistsBillDetails(int idRoom, int idService)
        {
            return BillDetailsDAO.Instance.IsExistsBillDetails(idRoom, idService);
        }
        public bool InsertBill(int idReceiveRoom, int staffSetUp)
        {
            return BillDAO.Instance.InsertBill(idReceiveRoom, staffSetUp);
        }
        public bool InsertBillDetails(int idBill, int idService, int count)
        {
            return BillDetailsDAO.Instance.InsertBillDetails(idBill, idService, count);
        }
        public bool UpdateBillDetails(int idBill, int idService, int _count)
        {
            return BillDetailsDAO.Instance.UpdateBillDetails(idBill, idService, _count);
        }
        public bool UpdateChangeBillDetails(int idBill, int idService, int IdServiceOld, int _count)
        {   
            return BillDetailsDAO.Instance.UpdateChangeBillDetails(idBill, idService, IdServiceOld, _count);
        }

        public void DeleteServiceBillDetail(int idRoom, int idService, int _count)
        {
            if (IsExistsBill(idRoom))
            {
                //Đã tồn tại Bill
                if (IsExistsBillDetails(idRoom, idService) && idService == ServiceDAO.Instance.FindIDService(cbService.Text.ToString()))
                {
        
                            int idBill = BillDAO.Instance.GetIdBillFromIdRoom(idRoom);
                   
                    if (BillDetailsDAO.Instance.DeleteServiceBillDetails(idBill, idService, _count) )
                    {
                        MessageBox.Show("Xóa dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xóa dịch vụ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Dịch vụ chưa được sử dụng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        public void UpdateBillDetail(int idRoom, int idService, int IdServiceOld, int count)
        {
            if (IsExistsBill(idRoom))
                
            {
                    if (count > 0)
                    {
                     int idBill = BillDAO.Instance.GetIdBillFromIdRoom(idRoom);

                    if(UpdateChangeBillDetails(idBill, idService, IdServiceOld, count)==false){
                        MessageBox.Show("Cập nhật dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }else{
                        MessageBox.Show("Cập nhật dịch vụ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
            }else
                    MessageBox.Show("Số lượng không hợp lệ.\nVui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        
        public void AddBill(int idRoom, int idService, int count)
        {
            if (IsExistsBill(idRoom))
            {
                
                if (!IsExistsBillDetails(idRoom, idService))
                {
                    //Chưa tồn tại BillDetails
                    if (count > 0)
                    {
                        int idBill = BillDAO.Instance.GetIdBillFromIdRoom(idRoom);
                        if (InsertBillDetails(idBill, idService, count))
                        {
                            MessageBox.Show("Thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Thêm dịch vụ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                        MessageBox.Show("Số lượng không hợp lệ.\nVui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //ĐÃ tồn tại BillDetails
                    int idBill = BillDAO.Instance.GetIdBillFromIdRoom(idRoom);
                    if (UpdateBillDetails(idBill, idService, count))
                    {
                        MessageBox.Show("Thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm dịch vụ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            else
            {
                //Chưa tồn tại Bill
                if (count > 0)
                {
                    int idReceiveRoom = BookRoomDAO.Instance.GetIdReceiveRoomFromIdRoom(idRoom);
                    InsertBill(idReceiveRoom, staffSetUp);
                    int idBill = BillDAO.Instance.GetIdBillMax();
                    if (InsertBillDetails(idBill, idService, count))
                    {
                        MessageBox.Show("Thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm dịch vụ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MetroFramework.MetroMessageBox.Show(this, "Số lượng không hợp lệ.\nVui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
       
       

        public void ShowBill(int idRoom)
        {   dataGridViewBookRoom.Rows.Clear();
            totalPrice = 0;
            CultureInfo cultureInfo = new CultureInfo("vi-vn");
            System.Data.DataTable table = BillDAO.Instance.ShowBill(idRoom);
            int _totalPrice = 0;
            int index = 0;
            foreach (DataRow item in table.Rows)
            {
                index++;
                dataGridViewBookRoom.Rows.Add(index,item["Tên dịch vụ"].ToString(), ((int)item["Đơn giá"]).ToString("c0", cultureInfo), ((int)item["Số lượng"]).ToString(), ((int)item["Thành tiền"]).ToString("c0", cultureInfo));
                _totalPrice += (int)item["Thành tiền"];
                
            }
            totalPrice += _totalPrice;
             dataGridViewBookRoom.Rows.Add("", "", "", "", totalPrice.ToString("c0", cultureInfo)); 
        }

        public void ShowBillRoom(int idRoom)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-vn");

            if (IsExistsBill(idRoom))
            {
                DataRow data = BillDAO.Instance.ShowBillRoom(idRoom);
                dataGridView1.Rows[0].Cells[0].Value = data["Tên phòng"].ToString();
                dataGridView1.Rows[0].Cells[1].Value = ((int)data["Đơn giá"]).ToString("c0", cultureInfo);
                dataGridView1.Rows[0].Cells[2].Value = ((DateTime)data["Ngày nhận"]).ToString().Split(' ')[0];
                dataGridView1.Rows[0].Cells[3].Value = ((DateTime)data["Ngày trả"]).ToString().Split(' ')[0];
                dataGridView1.Rows[0].Cells[4].Value = ((int)data["Tiền phòng"]).ToString("c0", cultureInfo);
                dataGridView1.Rows[0].Cells[5].Value = ((int)data["Phụ thu"]).ToString("c0", cultureInfo);
                int roomPrice = (int)data["Tiền phòng"] + (int)data["Phụ thu"];
                dataGridView1.Rows[0].Cells[6].Value = roomPrice.ToString("c0", cultureInfo);
                totalPrice += roomPrice;
            }
           
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void ShowSurcharge()
        {
            string query = "select * from Parameter";
            System.Data.DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                dataGridView2.Rows.Add(item["Name"].ToString(), item["Value"].ToString(), item["Describe"].ToString());
               
            }
        }
        int indexRow;
        
      
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thêm dịch vụ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                totalPrice = 0;
                Room room = flowLayoutRooms.Tag as Room;
                AddBill(room.Id, (cbService.SelectedItem as Service).Id, (int)numericUpDownCount.Value);
                ShowBill(room.Id);
                numericUpDownCount.Value = 1;
                BillDAO.Instance.UpdateRoomPrice(BillDAO.Instance.GetIdBillFromIdRoom(room.Id));
                ShowBillRoom(room.Id);
                txbTotalPrice.Text = totalPrice.ToString("c0", new CultureInfo("vi-vn"));
              

            }
        }


        private void btnClose__Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbIDRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            Room room = flowLayoutRooms.Tag as Room;
            if (MessageBox.Show("Bạn có chắc chắn thanh toán cho " + room.Name + " không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int idBill = BillDAO.Instance.GetIdBillFromIdRoom(room.Id);
                BillDAO.Instance.UpdateRoomPrice(BillDAO.Instance.GetIdBillFromIdRoom(room.Id));
                Pay(idBill, int.Parse(numericUpDown1.Value.ToString()));
                if (ReportDAO.Instance.InsertReport(idBill))
                {
                    MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    fPrintBill fPrintBill = new fPrintBill(room.Id, idBill);
                    fPrintBill.ShowDialog();
                    this.Show();
                    LoadData();
                    dataGridView1.Rows.Clear();
                    dataGridViewBookRoom.Rows.Clear();
                    txbTotalPrice.Text = string.Empty;
                    numericUpDown1.Value = 0;
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
      
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!dataGridViewBookRoom.Rows[0].Cells[0].Value.ToString().Equals(""))
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa dịch vụ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    if (numericUpDownCount.Value > int.Parse(dataGridViewBookRoom.SelectedRows[0].Cells[3].Value.ToString()))
                    {
                        MessageBox.Show("Số lượng không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        totalPrice = 0;
                        Room room = flowLayoutRooms.Tag as Room;
                        CultureInfo cultureInfo = new CultureInfo("vi-vn");
                        System.Data.DataTable table = BillDAO.Instance.ShowBill(room.Id);
                        int price = 0;
                        foreach (DataRow item in table.Rows)
                        {
                            if (txbPrice.Text.ToString().Equals(((int)item["Đơn giá"]).ToString("c0", cultureInfo)))
                                price = (int)item["Đơn giá"];
                        }
                        int amount = (int)numericUpDownCount.Value;
                        int total = amount * price;
                        totalPrice -= total;
                        DeleteServiceBillDetail(room.Id, (cbService.SelectedItem as Service).Id, amount);
                        ShowBill(room.Id);
                        BillDAO.Instance.UpdateRoomPrice(BillDAO.Instance.GetIdBillFromIdRoom(room.Id));
                        ShowBillRoom(room.Id);
                        if (dataGridViewBookRoom.Rows[0].Cells[0].Value.ToString().Equals(""))
                        {
                            LoadListServiceType();
                            numericUpDownCount.Value = 1;
                        }
                        txbTotalPrice.Text = totalPrice.ToString("c0", new CultureInfo("vi-vn"));


                    }



                }

            }
            else
            {
                MessageBox.Show("Chưa có dịch vụ được sử dụng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!dataGridViewBookRoom.Rows[0].Cells[0].Value.ToString().Equals(""))
            {
                if (MessageBox.Show("Bạn có muốn cập nhật dịch vụ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int tempt = 0;
                    Room room = flowLayoutRooms.Tag as Room;
                    CultureInfo cultureInfo = new CultureInfo("vi-vn");
                    System.Data.DataTable table = BillDAO.Instance.ShowBill(room.Id);

                    foreach (DataRow item in table.Rows)
                    {
                        if (txbPrice.Text.ToString().Equals(((int)item["Đơn giá"]).ToString("c0", cultureInfo)) &&
                            (cbService.SelectedItem as Service).Name.Equals(item["Tên dịch vụ"].ToString()) &&
                            numericUpDownCount.Value.ToString().Equals(item["Số lượng"].ToString()))
                        {
                            tempt++;
                        }
                    }
                    if (tempt > 0)
                    {
                        MessageBox.Show("Bạn chưa thay đổi dữ liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!fCustomer.CheckFillInText(new Control[] { cbServiceType, cbService, txbPrice, numericUpDownCount }))
                        {
                            MessageBox.Show("Không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            int index = dataGridViewBookRoom.SelectedRows[0].Index;
                           
                            IdServiceOld = ServiceDAO.Instance.FindIDService(dataGridViewBookRoom.SelectedRows[0].Cells[1].Value.ToString());
                            UpdateBillDetail(room.Id, (cbService.SelectedItem as Service).Id, IdServiceOld, int.Parse(numericUpDownCount.Value.ToString()));
                            ShowBill(room.Id);
                            dataGridViewBookRoom.SelectedRows[0].Selected = false;
                            dataGridViewBookRoom.Rows[index].Selected = true;
                            BillDAO.Instance.UpdateRoomPrice(BillDAO.Instance.GetIdBillFromIdRoom(room.Id));
                            txbTotalPrice.Text = totalPrice.ToString("c0", new CultureInfo("vi-vn"));
                            tempt = 0;



                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Dịch vụ chưa được sử dụng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        private void cbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServiceType.SelectedItem != null)
                LoadListService((cbServiceType.SelectedItem as ServiceType).Id);
        }
       
        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            CultureInfo cultureInfo = new CultureInfo("vi-vn");
            if (cbService.SelectedItem != null)
                txbPrice.Text = (cbService.SelectedItem as Service).Price.ToString("c0", cultureInfo);
        }
        public void LoadListServiceType()
        {
            cbServiceType.DataSource = ServiceTypeDAO.Instance.GetServiceTypes();
            cbServiceType.DisplayMember = "Name";
          
        }
        public void LoadListService(int idServiceType)
        {
            cbService.DataSource = ServiceDAO.Instance.GetServices(idServiceType);
            cbService.DisplayMember = "Name";
        }
        private void ChangeText(DataGridViewRow row)
        {
            ServiceDAO serviceDAO = new ServiceDAO();
            cbServiceType.Text = (string)serviceDAO.FindService(row.Cells["name"].Value.ToString());
            cbService.Text= row.Cells["name"].Value.ToString();
                
                txbPrice.Text = row.Cells["price"].Value.ToString();
                numericUpDownCount.Value= int.Parse(row.Cells["number"].Value.ToString());


        }
        
        

        private void DataGridViewBookRoom(object sender, EventArgs e)
        {
            if (dataGridViewBookRoom.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewBookRoom.SelectedRows[0];
                if (!row.Cells[0].Value.ToString().Equals(""))
                    ChangeText(row);
            }
        }
    }
}
