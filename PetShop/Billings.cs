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

namespace PetShop
{
    public partial class Billings : Form
    {

        public Billings()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.Employee;
            GetCustomers();
            DisplayProduct();
            DisplayTransactions();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=PetShopDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        private void GetCustomers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTbl", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(Rdr);
            CustIdCb.ValueMember = "CustId";
            CustIdCb.DataSource = dt;
            Con.Close();
        }
        private void DisplayProduct()
        {
            Con.Open();
            string Query = "Select * from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void DisplayTransactions()
        {
            Con.Open();
            string Query = "Select * from BillTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BillDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void GetCustName()
        {
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                string Query = "Select * from CustomerTbl where CustId=@CustId";
                SqlCommand cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@CustId", CustIdCb.SelectedValue.ToString());
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    CustNameTb.Text = dr["CustName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void UpdateStock()
        {
            try
            {
                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update ProductTbl set PrQty=@PQ where PrId=@PKey", Con);
                cmd.Parameters.AddWithValue("PQ", NewQty);
                cmd.Parameters.AddWithValue("PKey", key);
                cmd.ExecuteNonQuery();
                Con.Close();
                DisplayProduct();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int n = 0, GrdTotal = 0;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Curious Tails Pet Shop", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID PRODUCT PRICE QUANTITY TOTAL", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));

            int pos = 60; // Adjust the starting position as needed

            foreach (DataGridViewRow row in BillsDGV.Rows)
            {
                if (row != null)
                {
                    int prodid = row.Cells["ID"].Value != null ? Convert.ToInt32(row.Cells["ID"].Value) : 0;
                    string prodname = row.Cells["ProductName"].Value?.ToString() ?? "";
                    int prodprice = row.Cells["Price"].Value != null ? Convert.ToInt32(row.Cells["Price"].Value) : 0;
                    int prodqty = row.Cells["Quantity"].Value != null ? Convert.ToInt32(row.Cells["Quantity"].Value) : 0;
                    int tottal = row.Cells["Total"].Value != null ? Convert.ToInt32(row.Cells["Total"].Value) : 0;

                    e.Graphics.DrawString("" + prodid, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                    e.Graphics.DrawString("" + prodname, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                    e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                    e.Graphics.DrawString("" + prodqty, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                    e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(235, pos));

                    pos += 20; // Increment the position for the next row
                }
            }

            e.Graphics.DrawString("Grand Total is: Dollars" + GrdTotal, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
            e.Graphics.DrawString("**********************PetShop**********************" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Crimson, new Point(10, pos + 85));

            // Clear and refresh the DataGridView
            BillsDGV.Rows.Clear();
            BillsDGV.Refresh();

            // Reset variables
            pos = 100;
            GrdTotal = 0;
            n = 0;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > Stock)
            {
                MessageBox.Show("Not Enough Pets in House!");
            }
            else if (QtyTb.Text == "" || key == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);

                // Check if the column count is less than 5, add columns if needed
                if (BillsDGV.ColumnCount < 5)
                {
                    BillsDGV.ColumnCount = 5;
                    // Set column names or headers as needed
                    BillsDGV.Columns[0].Name = "ID";
                    BillsDGV.Columns[1].Name = "ProductName";
                    BillsDGV.Columns[2].Name = "Quantity";
                    BillsDGV.Columns[3].Name = "Price";
                    BillsDGV.Columns[4].Name = "Total";
                }

                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillsDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = PrNameTb.Text;
                newRow.Cells[2].Value = QtyTb.Text;
                newRow.Cells[3].Value = PrPriceTb.Text;
                newRow.Cells[4].Value = total;

                GrdTotal = GrdTotal + total;
                BillsDGV.Rows.Add(newRow);
                n++;
                TotalLbl.Text = "Money" + GrdTotal;
                UpdateStock();
                Reset();
            }
        }
        private void CustNameTb_TextChanged(object sender, EventArgs e)
        {

        }
        int key = 0, Stock = 0;

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
            PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (PrNameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
        private void InsertBill()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into BillTbl (BDate, CustId, CustName, EmpName, Amt) values(@BD, @CI,@CN, @EN, @Am)", Con);
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@CI", CustIdCb.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameLbl.Text);
                cmd.Parameters.AddWithValue("@Am", GrdTotal);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bill Saved");
                Con.Close();
                DisplayTransactions();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }
        string prodname;
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            InsertBill();
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprrnm", 285, 600);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        int prodid, prodqty, prodprice, tottal, pos = 60;
        private void label3_Click(object sender, EventArgs e)
        {
            Employee obj = new Employee();
            obj.Show();
            this.Hide();
        }

        private void Billings_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products obj = new Products();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void EmpNameLbl_Click(object sender, EventArgs e)
        {

        }

        private void CustIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustName();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void BillsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void Reset()
        {
            PrNameTb.Text = "";
            QtyTb.Text = "";
            Stock = 0;
            key = 0;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
