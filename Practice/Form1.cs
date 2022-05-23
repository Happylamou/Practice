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
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

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
            DisplayData2();

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
        private void DisplayData2()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from Results", con);
            adapt.Fill(dt);
            dataGridView3.DataSource = dt;
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

        private void expBtn_Click(object sender, EventArgs e)
        {



            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            app.Visible = false;
            worksheet = workbook.Sheets[1];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridview";
            //header values to xlsx
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            //column, row values to xlsx
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            //https://stackoverflow.com/questions/41283098/export-from-datagridview-to-excel-border-around-data

            // saving the file  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel file|*.xlsx";
            saveFileDialog1.Title = "Save Excel file";
            saveFileDialog1.ShowDialog();

            workbook.SaveAs(this.Text = saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            app.Quit();
        }

        private void excSearch_Click(object sender, EventArgs e)
        {

            string file = ""; //variable for the Excel File Location
            DataTable dtex = new DataTable();
            //DataRow row;
            dataGridView1.Columns.Add("Name", "Age");
            DialogResult result = openFileDialog1.ShowDialog();

            string[] column0Array = new string[dataGridView1.RowCount]; // gets 3 records from DGV as well as the empty line

            int i = 0; /*
            foreach (DataGridViewRow DTrow in dataGridView1.Rows)
            {
                column0Array[i] = DTrow.Cells[1].Value != null ? DTrow.Cells[1].Value.ToString().Trim() : string.Empty;
                i++;
            }
            */
            for (int p = 0; p < dataGridView1.RowCount; p++)
            {
                column0Array[p] = dataGridView1.Rows[p].Cells[1].Value != null ? dataGridView1.Rows[p].Cells[1].Value.ToString().Trim() : String.Empty;
                i++;
            }
            string toDisplay = string.Join("+", column0Array);
            //MessageBox.Show(toDisplay);


            // -------------------------------------------------------------------------------- After 3rd result gets gets empty value, which i assume is the empty line at the bottom of DGV
            // Ran into this while trying to fix System.IndexOutOfRangeException: 'Index was outside the bounds of the array.'


            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
                try
                {
                    Excel.Application excApp = new Excel.Application();
                    Excel.Workbook wb = excApp.Workbooks.Open(file);
                    Excel._Worksheet ws = wb.Sheets[1];
                    Excel.Range excRange = ws.UsedRange;

                    int rowCount = excRange.Rows.Count;
                    int colCount = excRange.Columns.Count;
                    int found = 0;
                    int notFound = 0;

                    string[] MissingEntries = new string[10];
                    var SrcColumn = 0;
                    var SrcRow = 0;

                    for (int j = 0; j < column0Array.Length; j++) // counting incorectly in some cases, unsure of what causes it
                    {
                        string Arraytxt = column0Array[j];
                        var results = excRange.Find(Arraytxt, LookAt: Excel.XlLookAt.xlWhole); 
                        if (results != null) 
                        { 
                            SrcColumn = results.Column;
                            SrcRow = results.Row;
                            //found++;
                        }
                        else
                        {
                            MissingEntries[notFound] = Arraytxt;
                            notFound++;
                            continue;
                        }
                        

                        //check if cell empty

                        if (excRange.Cells[SrcRow, SrcColumn + 1] != null && excRange.Cells[SrcRow, SrcColumn + 1].Value2 != null && excRange.Cells[SrcRow, SrcColumn].Value.ToString() == Arraytxt)
                        {
                                cmd = new SqlCommand("insert into Results(name,age,date) values(@Name,@Age,@Date)", con);
                                con.Open();
                                cmd.Parameters.AddWithValue("@Name", excRange.Cells[SrcRow, SrcColumn].Value2.ToString());
                                cmd.Parameters.AddWithValue("@Age", excRange.Cells[SrcRow, SrcColumn + 1].Value2.ToString());
                                cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToString());
                                cmd.ExecuteNonQuery();
                                con.Close();
                                DisplayData2();
                                found++;
                                
                        }
                        else
                        {
                            notFound++;
                            continue;
                        }
                        //running into Microsoft.CSharp.RuntimeBinder.RuntimeBinderException with new file         

                        //MessageBox.Show("row :" + SrcRow.ToString() + " Column:" + SrcColumn.ToString());   //coordinates display
                    }
                    // breaks here due to index out of range
                    MessageBox.Show(found + " records were found and " + notFound + " were/was not");
                    string DisplayMissing = string.Join("\n", MissingEntries);
                    MessageBox.Show("Missing records in xlsx: \n" + DisplayMissing);



                    wb.Close();
                    excApp.Quit();
                }
                catch { }
                                
            }

        }
    }
}
/*
 for (int j = 0; j <= column0Array.Length; j++)
                    {
                        string Arraytxt = column0Array[j];
                        var results = excRange.Find(Arraytxt, LookAt: Excel.XlLookAt.xlWhole);
                        var SrcColumn = results.Column;
                        var SrcRow = results.Row;

                        //int rowCounter;
                        
                            row = dtex.NewRow();
                            rowCounter = 0;
                            
                                //check if cell empty
                                
                                if (excRange.Cells[SrcRow, SrcColumn+1] != null && excRange.Cells[SrcRow, SrcColumn+1].Value2 != null)
                                {
                                    row[rowCounter] = excRange.Cells[SrcRow, SrcColumn].Value.ToString();

                                }
                                else
                                {
                                    row[rowCounter] = "";
                                }
                               rowCounter++;
                            
                            dtex.Rows.Add(row); //add row to DataTable                     

                        MessageBox.Show("row :" + SrcRow.ToString() + " Column:"+ SrcColumn.ToString());
                    }
                    MessageBox.Show(found + " records were found and " + notFound + " were not");


                    dataGridView3.DataSource = dtex; */