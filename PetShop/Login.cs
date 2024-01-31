using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            
        }
        SqlConnection con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=PetShopDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        public static string Employee;
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void EmployeeLbl_TextChanged(object sender, EventArgs e)
        {
            
        }
        //Functions Con;
        /*private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (UNameLbl.Text =="" || PasswordLbl.Text == "")
            {
                MessageBox.Show("MissingInformation!");
            }
            else
            {
                con.Open();
                string Query = "select count(*) from EmployeeTbl where EmpName = '" + UNameLbl.Text + "'EmpPass ='" + PasswordLbl.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")

                {
                    Employee=UNameLbl.Text;
                    Employee Obj= new Employee();
                    Obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Wrong User Name or Password!");
                    UNameLbl.Text = "";
                    PasswordLbl.Text = "";
                }
                con.Close();
            }
        }*/
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (UNameLbl.Text == "" || PasswordLbl.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                {
                    con.Open();
                    string Query = "select count(*) from EmployeeTbl where EmpName = @UserName and EmpPass = @Password";
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", UNameLbl.Text);
                        cmd.Parameters.AddWithValue("@Password", PasswordLbl.Text);

                        int count = (int)cmd.ExecuteScalar();

                        if (count == 1)
                        {
                            Employee = UNameLbl.Text;
                            Employee Obj = new Employee();
                            Obj.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Wrong User Name or Password!");
                            UNameLbl.Text = "";
                            PasswordLbl.Text = "";
                        }
                        con.Close();
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
