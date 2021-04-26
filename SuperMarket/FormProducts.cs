using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class FormProducts : Form
    {
        public FormProducts()
        {
            InitializeComponent();
        }

        public List<string> Categories { get; set; }


        private void FormProducts_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetProducts();
            dataGridView1.Columns["Name"].Width = 150;
            dataGridView1.Columns["Category"].Width = 177;
            comboBoxCategory.Items.AddRange(DataBaseAccess.GetCategories().Select(c => c.Name).ToArray());
            comboBoxCategory2.Items.AddRange(DataBaseAccess.GetCategories().Select(c => c.Name).ToArray());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRows = dataGridView1.SelectedRows;
            var product = (DisplayType)selectedRows[0].DataBoundItem;
            txtProductName.Text = product.Name;
            textProductQuantity.Text = product.Quantity.ToString();
            textProductPrice.Text = product.Price.ToString();
            textBoxId.Text = product.Id.ToString();
            comboBoxCategory.Text = product.Category;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProductName.Text = "";
            textProductPrice.Text = "";
            textProductQuantity.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                bool result = DataBaseAccess.AddProduct(
                                            txtProductName.Text,
                                            Convert.ToInt32(textProductQuantity.Text),
                                            Convert.ToDouble(textProductPrice.Text),
                                            DataBaseAccess.GetCategoryByName(comboBoxCategory.Text)
                                          );
                if (result)
                {
                    MessageBox.Show("product added successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                    throw new Exception();

            }
            catch (Exception)
            {
                MessageBox.Show("Falid to add product please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

 

        private bool ValidateInputData()
        {
            if (!ValidateNameInput() || !ValidateQuantityInput() || !ValidatePriceInput() || !ValidateCategoryInput())
                return false;
            return true;           
        }

        private bool ValidateNameInput()
        {
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                MessageBox.Show("Name can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtProductName.Text.Length > 40)
            {
                MessageBox.Show("Name can not be more than 40 char", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(txtProductName.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Name can only contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateQuantityInput()
        {
            if (string.IsNullOrEmpty(textProductQuantity.Text))
            {
                MessageBox.Show("Quantity can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textProductQuantity.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Quantity can only contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToInt32(textProductQuantity.Text) < 0)
            {
                MessageBox.Show("Quantity can not be less than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidatePriceInput()
        {
            if (string.IsNullOrEmpty(textProductPrice.Text))
            {
                MessageBox.Show("Price can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textProductPrice.Text, @"^[0-9.]+$"))
            {
                MessageBox.Show("Price can only contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToDouble(textProductPrice.Text) < 0)
            {
                MessageBox.Show("Price can not be less than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateCategoryInput()
        {
            if (comboBoxCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select Category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (comboBoxCategory2.SelectedItem == null)
            {
                dataGridView1.DataSource = DataBaseAccess.GetProducts();
            }
            else
            {
                dataGridView1.DataSource = DataBaseAccess.GetProductsByCategory(comboBoxCategory2.SelectedItem.ToString());
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                bool result = DataBaseAccess.EditProduct(
                                            Convert.ToInt32(textBoxId.Text),
                                            txtProductName.Text,
                                            Convert.ToInt32(textProductQuantity.Text),
                                            Convert.ToDouble(textProductPrice.Text),
                                            DataBaseAccess.GetCategoryByName(comboBoxCategory.Text)
                                          );
                if (result)
                {
                    MessageBox.Show("Product edited successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                    throw new Exception();

            }
            catch (Exception)
            {
                MessageBox.Show("Falid to edit product please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DataBaseAccess.DeleteProduct(Convert.ToInt32(textBoxId.Text));

                if (result)
                {
                    MessageBox.Show("Product deleted successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                    throw new Exception();


            }
            catch (Exception)
            {
                MessageBox.Show("Falid to edit Product please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetProductsByCategory(comboBoxCategory2.SelectedItem.ToString());
        }
    }
}
