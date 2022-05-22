using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bakery_Software
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.ForeColor = Color.Green;
            radioButton2.ForeColor = Color.Red;

            cmb_items.Items.Clear();
            cmb_items.Items.Add("Sweets Item 1");
            cmb_items.Items.Add("Sweets Item 2");
            cmb_items.Items.Add("Sweets Item 3");   
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.ForeColor = Color.Red;
            radioButton2.ForeColor = Color.Green;

            cmb_items.Items.Clear();
            cmb_items.Items.Add("Bakery Item 1");
            cmb_items.Items.Add("Bakery Item 2");
            cmb_items.Items.Add("Bakery Item 3");       
        }

        private void cmb_items_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_items.SelectedItem.ToString() == "Sweets Item 1")
            { txt_price.Text = "50"; }
            else if (cmb_items.SelectedItem.ToString() == "Sweets Item 2")
            { txt_price.Text = "100"; }
            else if (cmb_items.SelectedItem.ToString() == "Sweets Item 3")
            { txt_price.Text = "150"; }
            else if (cmb_items.SelectedItem.ToString() == "Bakery Item 1")
            { txt_price.Text = "200"; }
            else if (cmb_items.SelectedItem.ToString() == "Bakery Item 2")
            { txt_price.Text = "250"; }
            else if (cmb_items.SelectedItem.ToString() == "Bakery Item 3")
            { txt_price.Text = "300"; }
            else
            { txt_price.Text = "0"; }


            txt_total.Text = "";
            txt_qty.Text = "";
        }

        //Qty
        private void txt_qty_TextChanged(object sender, EventArgs e)
        {
            if (txt_qty.Text.Length>0)
            {
                txt_total.Text = (Convert.ToInt16(txt_qty.Text) * Convert.ToInt16(txt_price.Text)).ToString();
            }
        }

        //Add Item Button
        private void button1_Click(object sender, EventArgs e)
        {
            string[] arr = new string[4];
            arr[0] = cmb_items.SelectedItem.ToString();
            arr[1] = txt_price.Text;
            arr[2] = txt_qty.Text;
            arr[3] = txt_total.Text;

            ListViewItem lvi = new ListViewItem(arr);           
            listView1.Items.Add(lvi);

            txt_sub.Text = (Convert.ToInt16(txt_sub.Text) + Convert.ToInt16(txt_total.Text)).ToString();
        }

        //Delete Item Button
        private void button5_Click(object sender, EventArgs e)        
        {
          
            if (listView1.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count ; i++)
                {
                    if (listView1.Items[i].Selected)
                    {
                        txt_sub.Text = (Convert.ToInt16(txt_sub.Text) - Convert.ToInt16(listView1.Items[i].SubItems[3].Text)).ToString();
                        listView1.Items[i].Remove();
                    }
                }
            }
        }

        //Update Item Button
        private void button2_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].Selected = cmb_items.Enabled;
            listView1.SelectedItems[0].SubItems[1].Text = txt_price.Text;
            listView1.SelectedItems[0].SubItems[2].Text = txt_qty.Text;
            listView1.SelectedItems[0].SubItems[3].Text = txt_total.Text;

            double valorSum = 0;

            foreach (ListViewItem lstItem in listView1.Items) // listView has ListViewItem objects
            {
                valorSum += double.Parse(lstItem.SubItems[3].Text); // Columns 4
            }

            txt_sub.Text = valorSum.ToString();
            this.Update();   
        }

        // Discount
        private void txt_discount_TextChanged(object sender, EventArgs e)
        {
            if (txt_discount.Text.Length > 0)
            {
                txt_net.Text = (Convert.ToInt16(txt_sub.Text) - Convert.ToInt16(txt_discount.Text)).ToString();
            }
        }

        //Paid Amount
        private void txt_paid_TextChanged(object sender, EventArgs e)
        {
            if (txt_discount.Text.Length > 0)
            {
                txt_balance.Text = (Convert.ToInt16(txt_net.Text) - Convert.ToInt16(txt_paid.Text)).ToString();
            }
        }

        //Save (Insert) Button
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                try
                {
        string ConnectionString = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=AB_Inventory_DB; Data Source=DESKTOP-QOPJ822 ";

                    SqlConnection connection = new SqlConnection(ConnectionString); 
                    SqlCommand command = connection.CreateCommand();

            connection.Open();

         command.CommandText = "Insert into Test_Invoice_Master (InvoiceDate, Sub_Total, Discount, Net_Amount, Paid_Amount) values " +
                       " ( getdate() , " + txt_sub.Text + " ," + txt_discount.Text + " , " + txt_net.Text + ", " + txt_paid.Text + ")  select scope_identity() "; //scope_identity kthen vlerat e insertuara

                    string InvoiceID = command.ExecuteScalar().ToString(); //InvoiceID si primary key dhe eshte inner join me MasterID te tabela ne vazhdim

                    foreach (ListViewItem ListItem in listView1.Items) //objekti ListViewItem ne listView1
                    {
                     //kolonat ne listView
                    command.CommandText = "Insert into Test_Invoice_Detail (MasterID, ItemName, ItemPrice, ItemQtty, ItemTotal ) values   " +
               " ('" + InvoiceID + "', '" + ListItem.SubItems[0].Text + "', '" + ListItem.SubItems[1].Text + "', '" + ListItem.SubItems[2].Text + "' , " + ListItem.SubItems[3].Text + ")";
                   
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    MessageBox.Show("Sale Created Successfully, with Invoice # " + InvoiceID);
                    new Report.PrintInvoiceForm(InvoiceID).Show();
                }
                catch (Exception ee)
                {               
                    MessageBox.Show("Sale Not Created, Error!");
                }

            }
            else
            {
                MessageBox.Show("Must Add an Item in the List");
            }
        }

        //Print Invoice Button
        private void button4_Click(object sender, EventArgs e)
        {
            new Report.PrintInvoiceForm().Show();
        }

        //Clear All Button
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items.Clear();
                txt_sub.Text = (Convert.ToInt16(txt_sub.Text) - Convert.ToInt16(txt_sub.Text)).ToString();
            }
        }


        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            this.Hide();
            frm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAboutUs frm = new FrmAboutUs();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }
    }
}
