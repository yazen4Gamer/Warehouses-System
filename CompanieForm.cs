using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warehouses_System
{
    public partial class CompanieForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public CompanieForm()
        {
            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            LoadCompanies();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox3.Visible = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainFrom mainFrom = new MainFrom();
            mainFrom.Show();
        }

        private void CompanieForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Companies' table. You can move, or remove it, as needed.
            this.companiesTableAdapter.Fill(this.wareHousedbDataSet.Companies);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string companyName = textBox1.Text.Trim();
            string contactInfo = textBox2.Text.Trim();
            DateTime contractEndDate = dateTimePicker1.Value.Date;

            if (string.IsNullOrEmpty(companyName))
            {
                MessageBox.Show("Please enter the Company Name.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Companies (CompanyName, ContactInfo, ContractEndDate) VALUES (@name, @contact, @endDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", companyName);
                    cmd.Parameters.AddWithValue("@contact", contactInfo);
                    cmd.Parameters.AddWithValue("@endDate", contractEndDate);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Company added successfully!");
                        LoadCompanies(); // Refresh grid
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void LoadCompanies()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyID, CompanyName, ContactInfo, ContractEndDate FROM Companies";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox10.Text.Trim(), out int companyId))
            {
                DeleteCompanyById(companyId);
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Company ID.");
            }
        }
        private void DeleteCompanyById(int companyId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Companies WHERE CompanyID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", companyId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Company deleted successfully.");
                            LoadCompanies(); // Refresh the grid
                            textBox10.Clear(); // Clear the textbox after deletion
                            LoadCompanies(); // Refresh the grid to show updated data
                        }
                        else
                        {
                            MessageBox.Show("No company found with that ID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Get_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox9.Text.Trim(), out int companyId))
            {
                MessageBox.Show("Please enter a valid numeric Company ID.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName, ContactInfo, ContractEndDate FROM Companies WHERE CompanyID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", companyId);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            textBox7.Text = reader["CompanyName"].ToString();
                            textBox8.Text = reader["ContactInfo"].ToString();

                            if (reader["ContractEndDate"] != DBNull.Value)
                                dateTimePicker2.Value = Convert.ToDateTime(reader["ContractEndDate"]);
                            else
                                dateTimePicker2.Value = DateTime.Today;
                        }
                        else
                        {
                            MessageBox.Show("Company not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox9.Text.Trim(), out int companyId))
            {
                MessageBox.Show("Please enter a valid numeric Company ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Company Name cannot be empty.");
                return;
            }

            string companyName = textBox7.Text.Trim();
            string contactInfo = textBox8.Text.Trim();
            DateTime contractEndDate = dateTimePicker2.Value;

            UpdateCompany(companyId, companyName, contactInfo, contractEndDate);
        }

        private void UpdateCompany(int companyId, string companyName, string contactInfo, DateTime contractEndDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                UPDATE Companies
                SET CompanyName = @name,
                ContactInfo = @contact,
                ContractEndDate = @endDate
                WHERE CompanyID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", companyId);
                    cmd.Parameters.AddWithValue("@name", companyName);
                    cmd.Parameters.AddWithValue("@contact", contactInfo);
                    cmd.Parameters.AddWithValue("@endDate", contractEndDate);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Company updated successfully.");
                            LoadCompanies(); // Refresh your DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No company found with the specified ID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

    }
}
