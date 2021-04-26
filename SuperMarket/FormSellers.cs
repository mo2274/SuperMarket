using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace SuperMarket
{
    public partial class FormSellers : Form
    {
        
        public FormSellers()
        {
            InitializeComponent();
        }

        private void FormSellers_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetSellers();
            dataGridView1.Columns["Id"].Width = 30;
            dataGridView1.Columns["EmailAddress"].Width = 195;
            dataGridView1.Columns["Age"].Width = 30;
            dataGridView1.Columns["Phone"].Width = 100;
            dataGridView1.Columns["Name"].Width = 170;
            dataGridView1.Columns["Password"].Width = 100;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                textSellerPassword.PasswordChar = '\0';
            }
            else
            {
                textSellerPassword.PasswordChar = '*';
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBaseAccess.GetSellers();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                bool result = DataBaseAccess.AddSeller(
                                            textSellerName.Text,
                                            Convert.ToInt32(textSellerAge.Text),
                                            textSellerPhone.Text,
                                            textSellerPassword.Text,
                                            textSellerEmailAddress.Text
                                          );
                if (result)
                {
                    MessageBox.Show("Seller added successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                    throw new Exception();

            }
            catch (Exception)
            {
                MessageBox.Show("Falid to add seller please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private bool ValidateInputData()
        {
            if (!ValidateNameInput() || !ValidateAgeInput() || !ValidatePhoneInput() || !ValidateEmailAddressInput() || !ValidatePasswordInput())
                return false;
            return true;
        }

        private bool ValidateNameInput()
        {
            if (string.IsNullOrEmpty(textSellerName.Text))
            {
                MessageBox.Show("Name can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (textSellerName.Text.Length > 40)
            {
                MessageBox.Show("Name can not be more than 40 char", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(textSellerName.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Name can only contain letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ValidateAgeInput()
        {
            if (string.IsNullOrEmpty(textSellerAge.Text))
            {
                MessageBox.Show("Age can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textSellerAge.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Age can only contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToInt32(textSellerAge.Text) > 100)
            {
                MessageBox.Show("Age can not be more than 100", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToInt32(textSellerAge.Text) < 18)
            {
                MessageBox.Show("Age can not be less than 18", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidatePhoneInput()
        {
            if (string.IsNullOrEmpty(textSellerPhone.Text))
            {
                MessageBox.Show("Phone can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textSellerPhone.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Phone can only contain numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textSellerPhone.Text.TrimEnd().Length != 11)
            {
                MessageBox.Show("Phone can not be other than 11 numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidatePasswordInput()
        {
            if (string.IsNullOrEmpty(textSellerPassword.Text))
            {
                MessageBox.Show("Password can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textSellerPassword.Text, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"))
            {
                MessageBox.Show("Password must contain Minimum eight characters, at least one letter, " +
                    "one number and one special character:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        
        private bool ValidateEmailAddressInput()
        {
            if (string.IsNullOrEmpty(textSellerEmailAddress.Text))
            {
                MessageBox.Show("EmailAddress can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var m = new System.Net.Mail.MailAddress(textSellerEmailAddress.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("EmailAddress is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                throw;
            }


            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textSellerName.Text = "";
            textSellerPassword.Text = "";
            textSellerEmailAddress.Text = "";
            textSellerPhone.Text = "";
            textSellerAge.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRows = dataGridView1.SelectedRows;
            var seller = (Seller)selectedRows[0].DataBoundItem;
            textSellerName.Text = seller.Name;
            textSellerPhone.Text = seller.Phone;
            textSellerAge.Text = seller.Age.ToString();
            textSellerPassword.Text = seller.Password;
            textSellerEmailAddress.Text = seller.EmailAddress;
            textBoxId.Text = seller.Id.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputData())
                return;
            try
            {
                bool result = DataBaseAccess.EditSeller(
                                            Convert.ToInt32(textBoxId.Text),
                                            textSellerName.Text,
                                            Convert.ToInt32(textSellerAge.Text),
                                            textSellerPhone.Text,
                                            textSellerPassword.Text,
                                            textSellerEmailAddress.Text
                                          );
                if (result)
                {
                    MessageBox.Show("Seller edited successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }                 
                else
                    throw new Exception();

            }
            catch (Exception)
            {
                MessageBox.Show("Falid to edit seller please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DataBaseAccess.DeleteSeller(Convert.ToInt32(textBoxId.Text));

                if (result)
                {
                    MessageBox.Show("Seller deleted successfully", "Success", MessageBoxButtons.OK);
                    btnRefresh_Click(sender, e);
                }
                else
                    throw new Exception();


            }
            catch (Exception)
            {
                MessageBox.Show("Falid to edit seller please try again", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
