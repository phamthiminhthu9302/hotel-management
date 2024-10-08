﻿using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManager
{
    public partial class fRoom : Form
    {
        #region Properties
        private fRoomType _fRoomtType;
        #endregion

        #region Constructor
        public fRoom()
        {
            InitializeComponent();
            LoadFullRoomType();
            LoadFullRoom(GetFullRoom());
            dataGridViewRoom.SelectionChanged += DataGridViewRoom_SelectionChanged;
            comboboxID.DisplayMember = "id";
            dataGridViewRoom.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
        }

        #endregion
        private void TxbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) 
              || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        #region Kick
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnAddRoom_Click(object sender, EventArgs e)
        {
            new fAddRoom().ShowDialog();
            if (btnCancel.Visible == false)
                LoadFullRoom(GetFullRoom());
            else BtnCancel_Click(null, null);
        }
        private void BtnRoomType_Click(object sender, EventArgs e)
        {
            this.Hide();
            _fRoomtType.ShowDialog();
            LoadFullRoom(GetFullRoom());
            comboBoxRoomType.DataSource = _fRoomtType.TableRoomType;
            txbPrice.DataBindings.Clear();
            txbLimitPerson.DataBindings.Clear();
            txbPrice.DataBindings.Add(new Binding("Text", comboBoxRoomType.DataSource, "price_new"));
            txbLimitPerson.DataBindings.Add(new Binding("Text", comboBoxRoomType.DataSource, "limitPerson"));
            this.Show();
        }
        private void BtnCLose1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa phòng?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
                if (RoomDAO.Instance.IsRoomHaveBill(int.Parse(comboboxID.Text.ToString().Trim()))|| RoomDAO.Instance.IsRoomHaveBookRoom(int.Parse(comboboxID.Text.ToString().Trim())))
                {
                    MessageBox.Show("Không thể xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    RoomDAO.Instance.DeleteRoom(int.Parse(comboboxID.Text.ToString().Trim()));
                    MessageBox.Show("Xóa Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFullRoom(GetFullRoom());
                }
            }
        }
        private void BindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            txbNameRoom.Text = string.Empty;
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show( "Bạn có muốn cập nhật lại phòng?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
                UpdateRoom();
            comboboxID.Focus();
        }
        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {
            if (saveRoom.ShowDialog() == DialogResult.Cancel)
                return;
            else
            {
                bool check;
                try
                {
                    switch (saveRoom.FilterIndex)
                    {
                        case 2:
                            check = ExportToExcel.Instance.Export(dataGridViewRoom, saveRoom.FileName, ModeExportToExcel.XLSX);
                            break;
                        case 3:
                            check = ExportToExcel.Instance.Export(dataGridViewRoom, saveRoom.FileName, ModeExportToExcel.PDF);
                            break;
                        default:
                            check = ExportToExcel.Instance.Export(dataGridViewRoom, saveRoom.FileName, ModeExportToExcel.XLS);
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

        #endregion

        #region Load
        private void LoadFullRoom(DataTable table)
        {
            BindingSource source = new BindingSource();
            ChangePrice(table);
            source.DataSource = table;
            dataGridViewRoom.DataSource = source;
            bindingRoom.BindingSource = source;
            comboboxID.DataSource = source;
        }
       
        private void LoadFullRoomType()
        {
            DataTable table = GetFullRoomType();
            comboBoxRoomType.DataSource = table;
            comboBoxRoomType.DisplayMember = "Name";
            if (table.Rows.Count > 0)
                comboBoxRoomType.SelectedIndex = 0;
            _fRoomtType = new fRoomType(table);
            txbLimitPerson.DataBindings.Add(new Binding("Text", comboBoxRoomType.DataSource, "limitPerson"));
        }
        #endregion

        #region Method
        
        private void UpdateRoom()
        {
            if(comboboxID.Text == string.Empty)
            {
                MessageBox.Show( "Phòng này chưa tồn tại\n", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            if (!fCustomer.CheckFillInText(new Control[] { txbNameRoom, comboBoxStatusRoom, comboBoxRoomType }))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
              
                try
                {
                    int tempt = 0;
                    CultureInfo cultureInfo = new CultureInfo("vi-vn");
                    System.Data.DataTable table = GetFullRoom();
                    int i = comboBoxRoomType.SelectedIndex;
                            foreach (DataRow item in table.Rows)
                    {
                        if (comboboxID.Text.ToString().Equals(item["ID"].ToString()) &&
                        txbNameRoom.Text.ToString().Equals(item["Name"].ToString()) &&
                        txbLimitPerson.Text.ToString().Equals(((int)item["LimitPerson"]).ToString()) &&
                        txbPrice.Text.ToString().Equals(((int)item["Price"]).ToString("c0", cultureInfo)) &&
                       comboBoxStatusRoom.Text.ToString().Equals(Equals(item["nameStatusRoom"].ToString())&&
                       ((DataTable)comboBoxRoomType.DataSource).Rows[i]["Name"].Equals(item["nameRoomType"].ToString())))
                        {
                            tempt++; 
                        }
                    }
                    
                    Room roomNow = GetRoomNow();
                    if (tempt > 0)
                    {
                        MessageBox.Show( "Bạn chưa thay đổi dữ liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        bool check = RoomDAO.Instance.UpdateCustomer(roomNow);
                        if (check)
                        {
                            MessageBox.Show( "Cập Nhật Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            groupRoom.Tag = roomNow;
                            if (btnCancel.Visible == false)
                            {
                                int index = dataGridViewRoom.SelectedRows[0].Index;
                                LoadFullRoom(GetFullRoom());
                                comboboxID.SelectedIndex = index;
                                dataGridViewRoom.SelectedRows[0].Selected = false;
                                dataGridViewRoom.Rows[index].Selected = true;
                                tempt = 0;
                            }
                            else BtnCancel_Click(null, null);
                        }
                        else
                            MessageBox.Show( "Phòng này chưa tồn tại\n", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch
                {
                    MessageBox.Show( "Lỗi cập nhật phòng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ChangeText(DataGridViewRow row)
        {
            if(row.IsNewRow)
            {
                txbNameRoom.Text = string.Empty;
                bindingNavigatorMoveFirstItem.Enabled = false;
                bindingNavigatorMovePreviousItem.Enabled = false;
            }
            else
            {
                bindingNavigatorMoveFirstItem.Enabled = true;
                bindingNavigatorMovePreviousItem.Enabled = true;
                txbNameRoom.Text = row.Cells["colName"].Value.ToString();
                comboBoxStatusRoom.Text = row.Cells["colStatus"].Value.ToString();
                comboBoxRoomType.SelectedIndex = (int)row.Cells["colIdRoomType"].Value - 1;
               

                Room room = new Room(((DataRowView)row.DataBoundItem).Row);
                groupRoom.Tag = room;
            }
        }
        private void Search()
        {
            LoadFullRoom(GetSearchRoom());
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            txbSearch.Text = txbSearch.Text.Trim();
            if (txbSearch.Text != string.Empty)
            {   comboBoxStatusRoom.Text= string.Empty;
                txbNameRoom.Text = string.Empty;
                btnSearch.Visible = false;
                btnCancel.Visible = true;
                Search();
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LoadFullRoom(GetFullRoom());
            btnCancel.Visible = false;
            btnSearch.Visible = true;
        }
        #endregion

        #region Get Data
        private DataTable GetFullRoom()
        {
            return RoomDAO.Instance.LoadFullRoom();
        }
        private DataTable GetFullRoomType()
        {
            return RoomTypeDAO.Instance.LoadFullRoomType();
        }
       
        private Room GetRoomNow()
        {
            Room room = new Room();
            if (comboboxID.Text == string.Empty)
                room.Id = 0;
            else
                room.Id = int.Parse(comboboxID.Text);
            fStaff.Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { txbNameRoom });
            room.Name = txbNameRoom.Text;
            int index = comboBoxRoomType.SelectedIndex;
            room.IdRoomType = (int)((DataTable)comboBoxRoomType.DataSource).Rows[index]["id"];
            room.StatusRoom = comboBoxStatusRoom.Text;
         
            return room;
        }
        private DataTable GetSearchRoom()
        {
            if (int.TryParse(txbSearch.Text, out int id))
                return RoomDAO.Instance.Search(txbSearch.Text, id);
            else
                return RoomDAO.Instance.Search(txbSearch.Text, 0);
        }

        #endregion

        #region Change
        private void DataGridViewRoom_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridViewRoom.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewRoom.SelectedRows[0];
                ChangeText(row);
            }
        }
        private void ChangePrice(DataTable table)
        {
            table.Columns.Add("price_New", typeof(string));
            for (int i = 0; i < table.Rows.Count ; i++)
            {
                table.Rows[i]["price_New"] = ((int)table.Rows[i]["price"]).ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
            }
            table.Columns.Remove("price");
        }
        private void ComboBoxRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxRoomType.SelectedIndex;

            if (((DataTable)comboBoxRoomType.DataSource).Rows[index]["Price"].ToString().Contains("."))
                return;
            txbPrice.Text = ((int)((DataTable)comboBoxRoomType.DataSource).Rows[index]["Price"]).ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }
        #endregion

        #region Enter & leave
        private void TxbNameRoom_Enter(object sender, EventArgs e)
        {
            txbNameRoom.Tag = txbNameRoom.Text;
        }
        private void TxbNameRoom_Leave(object sender, EventArgs e)
        {
            if (txbNameRoom.Text == string.Empty)
                txbNameRoom.Text = txbNameRoom.Tag as string;
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
        private void FRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 && btnCancel.Visible == true)
                BtnCancel_Click(sender, null);
        }
        #endregion

        #region Close
        private void FRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnCancel_Click(sender, null);
        }
        #endregion

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
