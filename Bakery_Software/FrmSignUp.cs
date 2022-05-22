using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Bakery_Software
{
    public partial class FrmSignUp : Form
    {
        string ConStr = @"Data Source=DESKTOP-QOPJ822;Initial Catalog=SignUp;Integrated Security=true";
        SqlConnection con;
        public FrmSignUp()
        {
            InitializeComponent();
            con = new SqlConnection(ConStr);
        }

        string pattern = @"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";

        private void TbUsername_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(TbUsername.Text) == true)
            {
                TbUsername.Focus();
                errorProvider1.SetError(this.TbUsername, "Please enter username! ");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void TbPassword_Leave(object sender, EventArgs e)
        {
            if(Regex.IsMatch(TbPassword.Text, pattern) == false) 
            {
                TbPassword.Focus();
                errorProvider2.SetError(this.TbPassword, "UPPERCASE, LOWERCASE, NUMBERS, SPECIAL CHARACTERS");
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void TbConfirmPassword_Leave(object sender, EventArgs e)
        {
            if(TbPassword.Text != TbConfirmPassword.Text)
            {
                TbConfirmPassword.Focus();
                errorProvider3.SetError(this.TbConfirmPassword, "Password is not identical !");
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbUsername.Text) == true)
            {
                TbUsername.Focus();
                errorProvider1.SetError(this.TbUsername, "Please enter username! ");
            }
            else if (Regex.IsMatch(TbPassword.Text, pattern) == false)
            {
                TbPassword.Focus();
                errorProvider2.SetError(this.TbPassword, "UPPERCASE, LOWERCASE, NUMBERS, SPECIAL CHARACTERS");
            }
            else if (TbPassword.Text != TbConfirmPassword.Text)
            {
                TbConfirmPassword.Focus();
                errorProvider3.SetError(this.TbConfirmPassword, "Password is not identical !");
            }
            else
            {
                string query = @"INSERT INTO [dbo].[Tbl_SignUp]
           ([Username]
           ,[Password])
     VALUES
           ('" + TbUsername.Text + "','" + utils.hashPassword(TbPassword.Text) + "')";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Registered!");
                con.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            this.Hide();
            frm.Show();
        }


    }
}
