using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace SuperMarket
{
    public partial class FrmLogin : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const string adminUserName = "admin";
        private const string adminPassword = "admin";
        public static Seller seller { get; set; }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();


        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(ValidInputData())
            {
                if (CheckIfAdmin())
                    LoginAdmin();
                else if (CheckIfSeller())
                    LoginSeller();
            }        
        }

        private bool ValidInputData()
        {
            if ((string)comboBoxRole.SelectedItem == null)
            {
                MessageBox.Show("Select A Role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Fields Cannot Be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } 
            else 
            {
                return true;
            }
        }

        private void LoginSeller()
        {
            if (ValidateSellerData())
            {
                new FormSelling().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect UserName or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginAdmin()
        {
            if(ValidateAdminData())
            {
                new FormManager().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Incorrect UserName or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private bool ValidateAdminData()
        {
            if (txtPassword.Text.TrimEnd() == adminUserName && txtUserName.Text.TrimEnd() == adminPassword)
                return true;
            return false;
        }

        private bool ValidateSellerData()
        {
            seller = DataBaseAccess.CheckIfSellerExists(txtUserName.Text, txtPassword.Text);
            if (seller == null)
                return false;
            else
                return true;             
        }

        private bool CheckIfSeller()
        {
            if ((string)comboBoxRole.SelectedItem == "Seller")
                return true;
            return false;
        }

        private bool CheckIfAdmin()
        {
            if ((string)comboBoxRole.SelectedItem == "Admin")
                return true;
            return false;
        }


        private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPassword.PasswordChar = '\0';
            } 
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
