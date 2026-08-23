using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login_and_Register
{
    public partial class frmLogin : Form
    {
        private static readonly string myConn =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        public frmLogin()
        {
            InitializeComponent();
        }

        public void ClearLoginFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        // Optional connection test required by the original lab specification.
        public void TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(myConn))
                {
                    con.Open();
                    MessageBox.Show("SQL Server connection successful.",
                        "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to SQL Server.\n\n" + ex.Message,
                    "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter both username and password.",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string login = "SELECT password FROM tbl_users WHERE username = @username";

                using (SqlConnection con = new SqlConnection(myConn))
                using (SqlCommand cmd = new SqlCommand(login, con))
                {
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string storedHash = result.ToString();
                        string enteredHash = PasswordHelper.ComputeSha256(txtPassword.Text);

                        if (string.Equals(storedHash, enteredHash, StringComparison.OrdinalIgnoreCase))
                        {
                            frmDashboard dashboard = new frmDashboard(this);
                            dashboard.Show();
                            this.Hide();
                            return;
                        }
                    }

                    MessageBox.Show("Wrong username or password, please try again.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearLoginFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = checkbxShowPas.Checked ? '\0' : '•';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearLoginFields();
        }

        private void clickRegister_Click(object sender, EventArgs e)
        {
            new frmRegister(this).Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
