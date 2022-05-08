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
            var c = new SqlConnection("server = DESKTOP-GOUEP53\\SQLEXPRESS; database = Praktika; User Id = potato; password = 123"); // Your Connection String here
            var dataAdapter = new SqlDataAdapter(select, c);

            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];

        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
