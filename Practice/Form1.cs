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
using System.Runtime.InteropServices;
using System.Diagnostics;

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


        private void textId_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("Row: {0}, Column: {1}",
                dataGridView1.CurrentCell.RowIndex,
                dataGridView1.CurrentCell.ColumnIndex);

            txtName.Text = dataGridView1[1, e.RowIndex].Value.ToString();
            txtAge.Text = dataGridView1[2, e.RowIndex].Value.ToString();
            textId.Text = dataGridView1[0, e.RowIndex].Value.ToString();
        }

        private void textId_TextChanged(object sender, EventArgs e)
        {
            String searchValue = textId.Text;
            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[0].Value.ToString().Equals(searchValue))
                    {

                        rowIndex = row.Index;
                        txtName.Text = dataGridView1[1, row.Index].Value.ToString();
                        txtAge.Text = dataGridView1[2, row.Index].Value.ToString();
                        textId.Text = dataGridView1[0, row.Index].Value.ToString();
                        break;

                    }
                }
            }
        }
        //https://www.freecodespot.com/blog/csharp-import-excel/
        
        private void btn_ExcelUp_Click(object sender, EventArgs e)
        {
            string file = ""; //variable for the Excel File Location
            DataTable dtex = new DataTable(); 
            DataRow row;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                file = openFileDialog1.FileName; 
                try
                {
                    Microsoft.Office.Interop.Excel.Application excApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook wb = excApp.Workbooks.Open(file);
                    Microsoft.Office.Interop.Excel._Worksheet ws = wb.Sheets[1];
                    Microsoft.Office.Interop.Excel.Range excRange = ws.UsedRange;

                    int rowCount = excRange.Rows.Count; 
                    int colCount = 3; 
                    // get column name
                    for (int i = 1; i <= rowCount; i++)
                    {
                        for (int j = 1; j <= colCount; j++)
                        {
                            dtex.Columns.Add(excRange.Cells[i, j].Value2.ToString());
                        }
                        break;
                    }

                    //get row data

                    int rowCounter; 
                    for (int i = 2; i <= rowCount; i++) 
                    {
                        row = dtex.NewRow(); 
                        rowCounter = 0;
                        for (int j = 1; j <= colCount; j++) 
                        {
                            //check if cell empty
                            if (excRange.Cells[i, j] != null && excRange.Cells[i, j].Value2 != null)
                            {
                                row[rowCounter] = excRange.Cells[i, j].Value2.ToString();
                            }
                            else
                            {
                                row[rowCounter] = "";
                            }
                            rowCounter++;
                        }
                        dtex.Rows.Add(row); //add row to DataTable
                    }

                    dataGridView2.DataSource = dtex; 

                    //close and clean excel process
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Marshal.ReleaseComObject(excRange);
                    Marshal.ReleaseComObject(ws);
                    //quit apps
                    wb.Close();
                    Marshal.ReleaseComObject(wb);
                    excApp.Quit();
                    Marshal.ReleaseComObject(excApp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

       
    }

}
