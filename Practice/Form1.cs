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

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'praktika_dataDataSet.names_con' table. You can move, or remove it, as needed.
            // done with datasource button on form
            this.names_conTableAdapter.Fill(this.praktika_dataDataSet.names_con);

        }
        // found on web, unsure if this is the way to insert data for practice
        // https://www.c-sharpcorner.com/article/display-data-in-a-datagridview-C-Sharp-6/
        /* private void Form1_Load(object sender, EventArgs e)
         {
             SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Student", "server = MCNDESKTOP33; database = Avinash; UID = sa; password = *******");
             DataSet ds = new DataSet();
             da.Fill(ds, "Student");
             dataGridView1.DataSource = ds.Tables["Student"].DefaultView;
         }
        */
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
