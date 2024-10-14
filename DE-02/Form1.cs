using DE_02.model_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DE_02
{
    public partial class frmsp : Form
    {
        private CONTEXTDB DB = new CONTEXTDB();
        private bool isNewProduct = false;
        private bool p;

        public frmsp()
        {
            InitializeComponent();
        }

        private void frmsp_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadLoaiSP();
            UpdateButtonState();
        }
        private void LoadProducts()
        {

            dataGridView1.Rows.Clear();
            List<SANPHAM> products = DB.SANPHAMs.Include(p => p.LOAISP).ToList();

            foreach (var product in products)
            {
                dataGridView1.Rows.Add(product.MaSP, product.TenSP, product.Ngaynhap.ToString(), product.LOAISP.TenLoai);
            }
        }
        private void LoadLoaiSP()
        {
            comboBox1.Items.Clear();

            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var loaiSPs = DB.LOAISPs.ToList();

            foreach (var loaiSP in loaiSPs)
            {
                comboBox1.Items.Add(new { Text = loaiSP.TenLoai, Value = loaiSP.MaLoai });
            }

            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)


        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[1].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells[2].Value.ToString());

                // Lấy loại sản phẩm và hiển thị lên ComboBox
                comboBox1.SelectedIndex = comboBox1.FindStringExact(selectedRow.Cells[3].Value.ToString());
            }
            UpdateButtonState();
        }
        private void UpdateButtonState()
        {
            button3.Enabled = dataGridView1.SelectedRows.Count > 0;
            button4.Enabled = dataGridView1.SelectedRows.Count > 0;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thoát chương trình", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Tạo đối tượng sản phẩm mới
                var product = new SANPHAM
                {
                    MaSP = textBox1.Text,
                    TenSP = textBox2.Text,
                    Ngaynhap = dateTimePicker1.Value,
                    MaLoai = ((dynamic)comboBox1.SelectedItem).Value
                };


                DB.SANPHAMs.Add(product);
                DB.SaveChanges();

                // Cập nhật DataGridView
                LoadProducts();
                ClearInputs();
            }

        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm.");
                return false;
            }
            return true;
        }
        private void ClearInputs()
        {
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var maSP = selectedRow.Cells[0].Value.ToString();

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var product = DB.SANPHAMs.Find(maSP);
                    if (product != null)
                    {
                        DB.SANPHAMs.Remove(product);
                        DB.SaveChanges();

                        LoadProducts();
                        ClearInputs();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && ValidateInput())
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var maSP = selectedRow.Cells[0].Value.ToString();

                var product = DB.SANPHAMs.Find(maSP);
                if (product != null)
                {
                    product.TenSP = textBox1.Text;
                    product.Ngaynhap = dateTimePicker1.Value;
                    product.MaLoai = ((dynamic)comboBox1.SelectedItem).Value;

                    DB.SaveChanges();

                    LoadProducts();
                    ClearInputs();
                }
            }
        }
    }
}
