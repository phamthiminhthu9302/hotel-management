using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Internal;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fBookRoom : Form
    {
        int ma;
        Customer  Customer;
        string Card, phone;
        public fBookRoom(int manv)
        {
            ma = manv;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            LoadRoomType();
            LoadDate();
            LoadDays();
            LoadListBookRoom();
            cbSex.SelectedIndex = 0;
            cbNationality.SelectedIndex = 0;
        }
        public void LoadRoomType()
        {
            cbRoomType.DataSource = RoomTypeDAO.Instance.LoadListRoomType();
            cbRoomType.DisplayMember = "Name";
           
        }
        public void LoadRoomTypeInfo(int id)
        {
          
            List<Room> rooms = RoomDAO.Instance.LoadEmptyRoom(id);
            txbRoomTypeID.DataSource = rooms;
            txbRoomTypeID.DisplayMember = "ID";
            txbRoomTypeID.ValueMember = "ID";
            CultureInfo cultureInfo = new CultureInfo("vi-vn");
            if (txbRoomTypeID.Text != "")
            {
                txbRoomTypeID.Enabled = true;
                txbRoomTypeID.BackColor = Color.White;
                txbRoomTypeName.BackColor = Color.White;

                RoomType type = RoomTypeDAO.Instance.GetRoomTypeByIdRoom(int.Parse(txbRoomTypeID.Text));
                txbPrice.Text = type.Price.ToString("c0", cultureInfo);
            
                txbAmountPeople.Text = type.LimitPerson.ToString();
            }
            


        }
        public void LoadDate()
        {
            dpkDateOfBirth.Value = new DateTime(1998, 4, 6);
            dpkDateCheckIn.Value = DateTime.Now;
            dpkDateCheckOut.Value = DateTime.Now.AddDays(1);
        }
        public void LoadDays()
        {
            txbDays.Text = (dpkDateCheckOut.Value.Date - dpkDateCheckIn.Value.Date).Days.ToString();
        }

        public bool IsIdCardExists(string idCard)
        {
            return CustomerDAO.Instance.IsIdCardExists(idCard);
        }
        public void InsertCustomer(string idCard, string name, DateTime dateofBirth, string address, string phonenumber, string sex, string nationality)
        {
            CustomerDAO.Instance.InsertCustomer(idCard, name, dateofBirth, address, phonenumber, sex, nationality);
        }
        public void GetInfoByIdCard(string idCard)
        {
            Customer customer = CustomerDAO.Instance.GetInfoByIdCard(idCard);
            Card = txbIDCard.Text = customer.IdCard.ToString();
            txbFullName.Text = customer.Name;
            txbAddress.Text = customer.Address;
            dpkDateOfBirth.Value = customer.DateOfBirth;
            cbSex.Text = customer.Sex;
            phone = txbPhoneNumber.Text = customer.PhoneNumber.ToString();
            cbNationality.Text = customer.Nationality;
           

        }
        public void InsertBookRoom(int idCustomer, int idRoom, DateTime datecheckin, DateTime datecheckout, DateTime datebookroom, int ma)
        {
            BookRoomDAO.Instance.InsertBookRoom(idCustomer, idRoom, datecheckin, datecheckout, datebookroom, ma);
        }
        public int GetCurrentIDBookRoom(DateTime dateTime)
        {
            return BookRoomDAO.Instance.GetCurrentIDBookRoom(dateTime);
        }
        public void LoadListBookRoom()
        {
            dataGridViewBookRoom.DataSource = BookRoomDAO.Instance.LoadListBookRoom(DateTime.Now.Date);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void cbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomTypeInfo((cbRoomType.SelectedItem as RoomType).Id);
            List<RoomType> roomTypes = new List<RoomType>();
            roomTypes = RoomTypeDAO.Instance.LoadListRoomType();
            CultureInfo cultureInfo = new CultureInfo("vi-vn");
            foreach (RoomType dr in roomTypes)
            {
                if (txbRoomTypeID.Text == "" && dr.Name.ToString().Equals((cbRoomType.SelectedItem as RoomType).Name))
                {
                    txbRoomTypeID.Enabled = false;
                    txbRoomTypeID.BackColor=Color.WhiteSmoke;
                    txbRoomTypeName.BackColor = Color.WhiteSmoke;

                    txbPrice.Text = dr.Price.ToString("c0", cultureInfo);
                    txbAmountPeople.Text = dr.LimitPerson.ToString();
                    txbRoomTypeName.Text = "";
                }


            }
        }

        private void dpkDateCheckOut_onValueChanged(object sender, EventArgs e)
        {
            if (dpkDateCheckOut.Value < DateTime.Now)
                LoadDate();
            if (dpkDateCheckOut.Value <= dpkDateCheckIn.Value)
                LoadDate();
            LoadDays();
        }

        private void dpkDateCheckIn_onValueChanged(object sender, EventArgs e)
        {
            if (dpkDateCheckIn.Value <= DateTime.Now)
                LoadDate();
            if (dpkDateCheckOut.Value <= dpkDateCheckIn.Value)
                LoadDate();
            LoadDays();
        }

        private void txbIDCardSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == 13)
                btnSearch_Click(sender, null);
        }

        private void txbIDCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txbIDCardSearch.Text != String.Empty)
            {
                if (CustomerDAO.Instance.IsCusExistsByIdCard(txbIDCardSearch.Text))
                {
                    GetInfoByIdCard(txbIDCardSearch.Text);
                    
                }
                    
                    

                else
                    MessageBox.Show("CMND không tồn tại.\nVui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ClearData()
        {
            Card=phone=txbIDCardSearch.Text = txbIDCard.Text = txbFullName.Text = txbAddress.Text = txbPhoneNumber.Text = cbNationality.Text = String.Empty;
            LoadDate();
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại Việt Nam
            string regexPattern = @"^0[0-9]{9}$";

            // Kiểm tra sự khớp của số điện thoại với biểu thức chính quy
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch(phoneNumber);
        }
        private Customer GetCustomerNow()
        {

            Customer customer = new Customer();


            customer.Id = 0;

            customer.IdCard = txbIDCard.Text;
            customer.Name = txbFullName.Text;
            customer.Sex = cbSex.Text;
            customer.PhoneNumber = txbPhoneNumber.Text;
            customer.DateOfBirth = dpkDateOfBirth.Value;
            customer.Nationality = cbNationality.Text;
            customer.Address = txbAddress.Text;
            return customer;
        }
        private void btnBookRoom_Click(object sender, EventArgs e)
        {
           
            Customer customerNow = GetCustomerNow();
            if (MessageBox.Show("Bạn có muốn đặt phòng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { if (txbRoomTypeID.Text != String.Empty)
                {
                    if (txbIDCard.Text != String.Empty && txbFullName.Text != String.Empty && txbAddress.Text != String.Empty && txbPhoneNumber.Text != String.Empty && cbNationality.Text != String.Empty)
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
                            if (phone == String.Empty && Card == String.Empty)
                            {
                                MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                if (Card != customerNow.IdCard && phone != customerNow.PhoneNumber)
                                {
                                    MessageBox.Show("Trùng số chứng minh nhân dân và số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
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
                                if (phone == String.Empty)
                                {
                                    MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;

                                }
                                else
                                {
                                    if (phone != customerNow.PhoneNumber)
                                    {
                                        MessageBox.Show("Trùng số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
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
                                if (Card == String.Empty)
                                {

                                    MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;

                                }
                                else
                                {
                                    if (Card != customerNow.IdCard)
                                    {
                                        MessageBox.Show("Trùng số chứng minh nhân dân", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                }
                            }
                        }
                                int i = 0;

                                if (!IsIdCardExists(customerNow.IdCard) && !CustomerDAO.Instance.IsPhoneExists(customerNow.PhoneNumber))
                                {

                                    InsertCustomer(txbIDCard.Text, txbFullName.Text, dpkDateOfBirth.Value, txbAddress.Text, txbPhoneNumber.Text, cbSex.Text, cbNationality.Text);
                                    int id = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard).Id;
                                    InsertBookRoom(id, int.Parse(txbRoomTypeID.Text), dpkDateCheckIn.Value, dpkDateCheckOut.Value, DateTime.Now, ma);
                                    RoomDAO.Instance.UpdateStatusRoom(int.Parse(txbRoomTypeID.Text));
                                    MessageBox.Show("Đặt phòng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearData();
                                    LoadListBookRoom();
                                    cbRoomType_SelectedIndexChanged(sender, EventArgs.Empty);

                                    i++;




                                }
                                if (i == 0)
                                {
                                    Customer = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard);
                                    int id = CustomerDAO.Instance.GetInfoByIdCard(customerNow.IdCard).Id;

                                    CustomerDAO.Instance.UpdateCustomer(Customer.Id, txbFullName.Text, txbIDCard.Text, txbPhoneNumber.Text.ToString(), dpkDateOfBirth.Value,
                                                    txbAddress.Text, cbSex.Text, cbNationality.Text);
                                    InsertBookRoom(id, int.Parse(txbRoomTypeID.Text), dpkDateCheckIn.Value, dpkDateCheckOut.Value, DateTime.Now, ma);
                                    RoomDAO.Instance.UpdateStatusRoom(int.Parse(txbRoomTypeID.Text));
                                    MessageBox.Show("Đặt phòng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearData();
                                    LoadListBookRoom();
                                    cbRoomType_SelectedIndexChanged(sender, EventArgs.Empty);


                                }


                            }
                            else
                                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn phòng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
        private void TxbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-' ||
               e.KeyChar == ',' || e.KeyChar == '/' || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
        }
       
        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (dataGridViewBookRoom.Rows.Count > 0)
            {
                int idBookRoom = (int)dataGridViewBookRoom.SelectedRows[0].Cells[0].Value;
               
                fBookRoomDetails f = new fBookRoomDetails(idBookRoom);
                f.ShowDialog();
                Show();
                LoadListBookRoom();
                LoadRoomType();
            }

        }

        private void txbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
                e.Handled = true;
            
        }

        private void LoadMaPhong(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = RoomDAO.Instance.LoadFullRoom();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString().Equals(txbRoomTypeID.Text))
                {
                    txbRoomTypeName.Text = dr["Name"].ToString();
                }
            }
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa đặt phòng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dataGridViewBookRoom.Rows.Count > 0)

                {
                    int idBookRoom = (int)dataGridViewBookRoom.SelectedRows[0].Cells[0].Value;
                    if (BookRoomDAO.Instance.IsBookRoomHaveBill(idBookRoom))
                    {
                        MessageBox.Show("Không thể xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        RoomDAO.Instance.UpdateStatusRoomOld(idBookRoom);

                        BookRoomDAO.Instance.DeleteBookRoom(idBookRoom);
                          
                        LoadListBookRoom();
                        cbRoomType_SelectedIndexChanged(sender, EventArgs.Empty);
                        MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                   
                    
                }
            }
        }
    }
}
