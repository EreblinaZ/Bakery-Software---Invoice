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


namespace Bakery_Software
{
    public partial class FrmLogin : Form
    {
        string ConStr = @"Data Source=DESKTOP-QOPJ822;Initial Catalog=SignUp;Integrated Security=true";
        SqlConnection con;
        SqlDataReader dr; //DataReader
        public FrmLogin()
        {
            InitializeComponent();
            con = new SqlConnection(ConStr);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string query = @"SELECT 
      [Username]
      ,[Password]
  FROM [dbo].[Tbl_SignUp] where Username='" + TbUsername.Text + "' AND Password='" + utils.hashPassword(TbPassword.Text) + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Form1 frm = new Form1();
                this.Hide();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password!");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmSignUp frm = new FrmSignUp();
            this.Hide();
            frm.Show();
        }
    }
}
