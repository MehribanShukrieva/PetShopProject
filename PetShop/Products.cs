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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            DisplayProducts();
            EmpNameLbl.Text = Login.Employee;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=PetShopDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        private void DisplayProducts()
        {
            Con.Open();
            string Query = "Select * from ProductTbl ";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            PrNameTb.Text = "";
            PrPriceTb.Text = "";
            PrQntTb.Text = "";
            PrCatTb.SelectedIndex = 0;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PrCatTb.SelectedIndex == -1  || PrQntTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into ProductTbl (PrName, PrCat, PrQty, PrPrice) values(@PN, @PC, @PQ, @PP)", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", PrCatTb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PQ", PrQntTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Added");
                    Con.Close();
                    DisplayProducts();
                    Clear();
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
        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PrCatTb.SelectedIndex == -1 || PrQntTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update ProductTbl set PrName=@PN, PrCat=@PC, PrQty=@PQ, PrPrice=@PP where PrId=@PKey", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", PrCatTb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PQ", PrQntTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);
                    cmd.Parameters.AddWithValue("@PKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Updated!");
                    Con.Close();
                    DisplayProducts();
                    Clear();
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
        }
        int Key = 0;
        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            PrCatTb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
            PrQntTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            
            if (PrNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
               
            }
        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Product");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from  ProductTbl where PrId = @PKey", Con);
                    cmd.Parameters.AddWithValue("@PKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted Successfully!");
                    Con.Close();
                    DisplayProducts();
                    Clear();
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
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employee obj = new Employee();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Billings obj = new Billings();
            obj.Show();
            this.Hide();
        }
    }
}
