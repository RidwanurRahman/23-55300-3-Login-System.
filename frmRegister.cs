using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login_and_Register
{
    public partial class frmRegister : Form
    {
        private static readonly string myConn =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        private readonly frmLogin loginForm;

        public frmRegister(frmLogin loginForm)
        {
            InitializeComponent();
            this.loginForm = loginForm;
        }

        public frmRegister() : this(new frmLogin())
        {
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConPassword.Text;

            if (username == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Username and password fields cannot be empty.",
                    "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must contain at least 6 characters.",
                    "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match, please re-enter.",
                    "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtConPassword.Text = "";
                txtPassword.Focus();
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(myConn))
                {
                    con.Open();

                    using (SqlCommand check = new SqlCommand(
                        "SELECT COUNT(*) FROM tbl_users WHERE username = @username", con))
                    {
                        check.Parameters.AddWithValue("@username", username);

                        if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("That username is already taken.",
                                "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsername.Focus();
                            return;
                        }
                    }

                    string passwordHash = PasswordHelper.ComputeSha256(password);

                    string register =
                        "INSERT INTO tbl_users (username, password) VALUES (@username, @password)";

                    using (SqlCommand cmd = new SqlCommand(register, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", passwordHash);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Your account has been successfully created.",
                    "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                loginForm.ClearLoginFields();
                loginForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            char passwordChar = checkbxShowPas.Checked ? '\0' : '•';
            txtPassword.PasswordChar = passwordChar;
            txtConPassword.PasswordChar = passwordChar;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConPassword.Text = "";
            txtUsername.Focus();
        }

        private void clickLogin_Click(object sender, EventArgs e)
        {
            loginForm.ClearLoginFields();
            loginForm.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
