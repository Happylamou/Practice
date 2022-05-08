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

namespace Practice
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"server = DESKTOP-GOUEP53\SQLEXPRESS; database = Praktika; User Id = potato; password = 123");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        public Form1()
        {
            InitializeComponent();
            DisplayData();
            
        }

        private void Form1_Load(object sender, EventArgs e)
       {
        var select = "SELECT * FROM names_con";
        var c = new SqlConnection(@"server = DESKTOP-GOUEP53\SQLEXPRESS; database = Praktika; User Id = potato; password = 123"); // Your Connection String here
        var dataAdapter = new SqlDataAdapter(select, c);

        var commandBuilder = new SqlCommandBuilder(dataAdapter);
        var ds = new DataSet();
        dataAdapter.Fill(ds);
        dataGridView1.AllowUserToAddRows = true;
        dataGridView1.DataSource = ds.Tables[0];

        }
        //https://www.codeproject.com/Questions/493429/Saveplusdataplusfromplustextboxplustoplusdatabase
 
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtAge.Text != "")
            {
                cmd = new SqlCommand("insert into names_con(name,age) values(@Name,@Age)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Inserted Successfully");
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }
 
        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from names_con", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void ClearData()
        {
            txtName.Text = "";
            txtAge.Text = "";
        }

        private void btn_Update_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtAge.Text != "")
            {
                cmd = new SqlCommand("update names_con set name ='" + txtName.Text + "',age ='" + txtAge.Text + "' where id = @Id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@Id", textId.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Record Updated Successfully");
            con.Close();
            DisplayData();
            ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }

        private void btn_Delete_Click_1(object sender, EventArgs e)
        {
            if (int.Parse(textId.Text) != 0)
            {

                cmd = new SqlCommand("delete from names_con where id = @id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@id", textId.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Deleted Successfully!");
            DisplayData();
            ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


    }
}
