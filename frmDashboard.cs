using System;
using System.Windows.Forms;

namespace Login_and_Register
{
    public partial class frmDashboard : Form
    {
        private readonly frmLogin loginForm;

        public frmDashboard(frmLogin loginForm)
        {
            InitializeComponent();
            this.loginForm = loginForm;
        }

        public frmDashboard() : this(new frmLogin())
        {
        }

        private void visitWeb_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "Website feature is not required for this lab.",
            "Information",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                loginForm.ClearLoginFields();
                loginForm.Show();
                this.Close();
            }
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
        }
    }
}
