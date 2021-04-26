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

namespace SuperMarket
{
    public partial class FormManager : Form
    {
        public readonly Color defaultColor = Color.FromArgb(24, 30, 54);
        public readonly Color selectionColor = Color.FromArgb(46, 51, 73);

        public FormManager()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSellers_Click(object sender, EventArgs e)
        {
            PnlNav.Top = btnSellers.Top;
            PnlNav.Height = btnSellers.Height;
            btnSellers.BackColor = selectionColor;
            btnProducts.BackColor = defaultColor;
            btnCategories.BackColor = defaultColor;
            btnLogOut.BackColor = defaultColor;
            ShowSelectedForm(new FormSellers());
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnProducts_Click(object sender, EventArgs e)
        {
            PnlNav.Top = btnProducts.Top;
            PnlNav.Height = btnProducts.Height;
            btnProducts.BackColor = selectionColor;
            btnSellers.BackColor = defaultColor;
            btnCategories.BackColor = defaultColor;
            btnCategories.BackColor = defaultColor;
            btnLogOut.BackColor = defaultColor;
            ShowSelectedForm(new FormProducts());
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            PnlNav.Top = btnCategories.Top;
            PnlNav.Height = btnCategories.Height;
            btnCategories.BackColor = selectionColor;
            btnSellers.BackColor = defaultColor;
            btnProducts.BackColor = defaultColor;
            btnLogOut.BackColor = defaultColor;
            ShowSelectedForm(new FormCategories());
        }

        private void ShowSelectedForm(Form selectedForm)
        {
            pnlContainer.Controls.Clear();
            selectedForm.Dock = DockStyle.Fill;
            selectedForm.TopLevel = false;
            selectedForm.TopMost = true;
            pnlContainer.Controls.Add(selectedForm);
            selectedForm.Show();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            PnlNav.Top = btnLogOut.Top;
            PnlNav.Height = btnLogOut.Height;
            btnLogOut.BackColor = selectionColor;
            btnSellers.BackColor = defaultColor;
            btnProducts.BackColor = defaultColor;
            btnCategories.BackColor = defaultColor;
            new FrmLogin().Show();
            this.Close();
        }

        private void Manager_MouseDown(object sender, MouseEventArgs e)
        {
            FrmLogin.ReleaseCapture();
            FrmLogin.SendMessage(this.Handle, FrmLogin.WM_NCLBUTTONDOWN, FrmLogin.HT_CAPTION, 0);
        }

        private void pnlContainer_Paint(object sender, PaintEventArgs e)
        {
  
        }
    }
}
