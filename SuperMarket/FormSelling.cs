using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class FormSelling : Form
    {
        public FormSelling()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormSelling_MouseDown(object sender, MouseEventArgs e)
        {
            FrmLogin.ReleaseCapture();
            FrmLogin.SendMessage(this.Handle, FrmLogin.WM_NCLBUTTONDOWN, FrmLogin.HT_CAPTION, 0);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            if (DataBaseAccess.AddItem(Convert.ToInt32(txtPillId.Text), txtProductName.Text, Convert.ToInt32(txtProductQuantity.Text), FrmLogin.seller))
            {
                MessageBox.Show("Item Added Successfully", "Success", MessageBoxButtons.OK);
                AddDataSourceToItemsGrid();
                AddDataSourceToBillsGrid();
                AddDataSourceToProductsGrid();

            }        
            else
                MessageBox.Show("Faild to add item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool ValidateInputData()
        {
            if(string.IsNullOrEmpty(txtPillId.Text) || string.IsNullOrEmpty(txtProductName.Text) || string.IsNullOrEmpty(txtProductQuantity.Text))
            {
                MessageBox.Show("Fields can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var product = DataBaseAccess.GetProductByName(txtProductName.Text);
            if (product == null)
            {
                MessageBox.Show("Product Does not Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if (product.Quantity < Convert.ToInt32(txtProductQuantity.Text))
            {
                MessageBox.Show("No enough quantity from this product, Available quantity is : " + $"{product.Quantity}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void FormSelling_Load(object sender, EventArgs e)
        {
            lblSellerName.Text = FrmLogin.seller.Name;
            lblDate.Text = DateTime.Now.Date.ToShortDateString();
            comboBoxCategory.Items.AddRange(DataBaseAccess.GetCategories().Select(c => c.Name).ToArray());
            AddDataSourceToItemsGrid();
            AddDataSourceToProductsGrid();
            AddDataSourceToBillsGrid();
        }

        private void AddDataSourceToProductsGrid()
        {
            dataGridViewProducts.DataSource = DataBaseAccess.GetProducts().Select(p => new ProductData {Product = p.Name, Price = p.Price, Quantity = p.Quantity }).ToList();
            dataGridViewProducts.Columns["Product"].Width = 115;
            dataGridViewProducts.Columns["Price"].Width = 50;
            dataGridViewProducts.Columns["Quantity"].Width = 50;
        }

        private void AddDataSourceToBillsGrid()
        {
            dataGridViewBills.DataSource = DataBaseAccess.GetBills();
            dataGridViewBills.Columns["BillDate"].Width = 200;

        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewProducts.DataSource = DataBaseAccess.GetProductsByCategory(comboBoxCategory.SelectedItem.ToString())
                .Select(p => new { Product = p.Name, Quantity = p.Quantity })
                .ToList();
        }

        private void AddDataSourceToItemsGrid()
        {
            if (txtPillId.Text != "")
            {
                dataGridViewItems.DataSource = DataBaseAccess.GetItems(Convert.ToInt32(txtPillId.Text))
                .Select(i => new {BillId = i.BillId, Product = i.Product.Name, Price = i.Product.Price, Quantity = i.ProductQuantity, Total = i.ProductQuantity  * i.Product.Price })
                .ToList();
                lblTotal.Text = DataBaseAccess.CalculateTotal(Convert.ToInt32(txtPillId.Text)).ToString();
            }

            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            AddDataSourceToItemsGrid();
        }

        private void dataGridViewProducts_Click(object sender, EventArgs e)
        {
            var selectedRows = dataGridViewProducts.SelectedRows;
            var product = (ProductData)selectedRows[0].DataBoundItem;
            txtProductName.Text = product.Product;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateBillData())
                return;
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        Bitmap bmp;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var bill = DataBaseAccess.GetBill(Convert.ToInt32(txtPillId.Text));
            e.Graphics.DrawString(FrmLogin.seller.Name, new Font("Centuey Gothic", 16, FontStyle.Bold), Brushes.Black, new PointF(60, 60));
            e.Graphics.DrawString($"{bill.Date:d}", new Font("Centuey Gothic", 16, FontStyle.Bold), Brushes.Black, new PointF(630, 60));
            e.Graphics.DrawLine(new Pen(Brushes.Black), new PointF(60, 90), new PointF(790, 90));
            PrintItems(e);
            
            /*
            var hieght = dataGridViewItems.Height;
            dataGridViewItems.Height = dataGridViewItems.RowCount * dataGridViewItems.RowTemplate.Height + 100;
            bmp = new Bitmap(dataGridViewItems.Width, dataGridViewItems.Height);
            dataGridViewItems.DrawToBitmap(bmp, new Rectangle(0, 0, dataGridViewItems.Width, dataGridViewItems.Height));
            dataGridViewItems.Height = hieght;
            e.Graphics.DrawImage(bmp, 60, 100);
            */
        }

        private void PrintItems(System.Drawing.Printing.PrintPageEventArgs e)
        {
            int index = 120;
            var items = DataBaseAccess.GetItems(Convert.ToInt32(txtPillId.Text));
            string line = string.Format("{0,-30} {1,-30} {2,-30} {3,-30}", "Product", "Price", "Quantity", "Total");
            e.Graphics.DrawString(line, new Font("Centuey Gothic", 14, FontStyle.Regular), Brushes.Blue, new PointF(60, index));
            index += dataGridViewItems.RowTemplate.Height + 10;
            e.Graphics.DrawLine(new Pen(Brushes.Black), new PointF(60, index), new PointF(790, index));
            foreach (var item in items)
            {
                index += dataGridViewItems.RowTemplate.Height + 5;
                string line2 = string.Format("{0,-30} {1,-30} {2,-30} {3,-30}",
                    item.Product.Name, item.Product.Price, item.ProductQuantity, item.Product.Price * item.ProductQuantity);
                e.Graphics.DrawString(line2, new Font("Centuey Gothic", 13, FontStyle.Regular), Brushes.Blue, new PointF(60, index));
                index += dataGridViewItems.RowTemplate.Height + 5;
                e.Graphics.DrawLine(new Pen(Brushes.Black), new PointF(60, index), new PointF(790, index));
            }
            e.Graphics.DrawString($"Total: {lblTotal.Text:c}", new Font("Centuey Gothic", 16, FontStyle.Bold), Brushes.Black, new PointF(600, index + 20));
        }

        private bool ValidateBillData()
        {
            if (string.IsNullOrEmpty(txtPillId.Text))
            {
                MessageBox.Show("bill can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (DataBaseAccess.GetBill(Convert.ToInt32(txtPillId.Text)) == null)
            {
                MessageBox.Show("bill does not exist, choose existing bill", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
