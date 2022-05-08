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
        public Form1()
        {
            InitializeComponent();
        }

        /*
                private void Form1_Load(object sender, EventArgs e)
                {
                    // TODO: This line of code loads data into the 'praktikaDataSet.names_con' table. You can move, or remove it, as needed.
                    this.names_conTableAdapter1.Fill(this.praktikaDataSet.names_con);

                }
        */
       private void Form1_Load(object sender, EventArgs e)
       {
        var select = "SELECT * FROM names_con";
        var c = new SqlConnection(@"server = DESKTOP-GOUEP53\SQLEXPRESS; database = Praktika; User Id = potato; password = 123"); // Your Connection String here
        var dataAdapter = new SqlDataAdapter(select, c);

        var commandBuilder = new SqlCommandBuilder(dataAdapter);
        var ds = new DataSet();
        dataAdapter.Fill(ds);
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.DataSource = ds.Tables[0];

       }
        //https://www.codeproject.com/Questions/493429/Saveplusdataplusfromplustextboxplustoplusdatabase

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"server = DESKTOP-GOUEP53\SQLEXPRESS; database = Praktika; User Id = potato; password = 123");
            SqlCommand cmd = new SqlCommand("Insert Into names_con (name, age) Values (@Name, @Age)", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static void main(string[] args)
        {

            Application.Run(new Form1());

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
    }
}
