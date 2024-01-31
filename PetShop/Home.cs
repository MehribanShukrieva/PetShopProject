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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            CountDogs();
            CountBirds();
            CountCats();
            Finance();
            EmpNameLbl.Text = Login.Employee;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=PetShopDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        private void Finance()
        {
            try
            {
                
                    Con.Open();

                    using (SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl", Con))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                        {
                            FinanceLbl.Text = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            FinanceLbl.Text = "No data found";
                        }
                    
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display error message, etc.)
                Console.WriteLine("An error occurred: " + ex.Message);
            } finally { Con.Close(); }
        }

        private void CountDogs()
        {
            Con.Open();
            string Cat = "Dog";
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTbl where PrCat='"+ Cat +"'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            DogsLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountBirds()
        {
            Con.Open();
            string Cat = "Bird";
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTbl where PrCat='" + Cat + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            BirdsLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountCats()
        {
            Con.Open();
            string Cat = "Cat";
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTbl where PrCat='" + Cat + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            CatsLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products obj = new Products();
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

        private void BirdsLbl_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Billings obj = new Billings();
            obj.Show();
            this.Hide();
        }

        private void FinanceLbl_Click(object sender, EventArgs e)
        {

        }
    }
}
