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
    public partial class FormCategories : Form
    {
        public FormCategories()
        {
            InitializeComponent();
        }

        private void FormCategories_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetCategories();
            dataGridView1.Columns["Name"].Width = 100;
            dataGridView1.Columns["Description"].Width = 430;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                var result = DataBaseAccess.AddCategory(txtName.Text, textDescription.Text);
                if (result)
                {
                    MessageBox.Show("Category added successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falid to add category please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                var result = DataBaseAccess.EditCategory(Convert.ToInt32(textBoxId.Text), txtName.Text, textDescription.Text);
                if (result)
                {
                    MessageBox.Show("Category Edited successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falid to Edited category please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool ValidateInputData()
        {
            if (!ValidateNameInput() || !ValidateDescriptionInput())
                return false;
            return true;
        }

        private bool ValidateNameInput()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtName.Text.Length > 40)
            {
                MessageBox.Show("Name can not be more than 40 char", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(txtName.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Name can only contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateDescriptionInput()
        {
            if (string.IsNullOrEmpty(textDescription.Text))
            {
                MessageBox.Show("textDescription can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (textDescription.Text.Length > 200)
            {
                MessageBox.Show("textDescription can not be more than 200 char", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(textDescription.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("textDescription can only contain letters and white spaces", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DataBaseAccess.DeleteCategory(Convert.ToInt32(textBoxId.Text));
                if (result)
                {
                    MessageBox.Show("Category Deleted successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            { 
                MessageBox.Show("Falid to delete category please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            textDescription.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRows = dataGridView1.SelectedRows;
            var category = (CategoryDisplay)selectedRows[0].DataBoundItem;
            txtName.Text = category.Name;
            textDescription.Text = category.Description;
            textBoxId.Text = category.Id.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetCategories();
        }
    }
}
