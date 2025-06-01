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
using System.Configuration;

namespace Warehouses_System
{
    public partial class WarehouseForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public WarehouseForm()
        {
            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = true;
        }

        private void backToMainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainFrom mainFrom = new MainFrom();
            mainFrom.Show();
        }

        private void WarehouseForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.wareHousedbDataSet.Employees);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Companies' table. You can move, or remove it, as needed.
            this.companiesTableAdapter.Fill(this.wareHousedbDataSet.Companies);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Warehouses' table. You can move, or remove it, as needed.
            this.warehousesTableAdapter.Fill(this.wareHousedbDataSet.Warehouses);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string location = textBox1.Text.Trim();
            string MaxCapactiy = textBox2.Text.Trim();

            if (listBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a company.");
                return;
            }

            int companyId = Convert.ToInt32(listBox1.SelectedValue);

            if (string.IsNullOrWhiteSpace(MaxCapactiy) || string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please enter both warehouse name and location.");
                return;
            }

            string query = "INSERT INTO Warehouses (MaxCapacity, Location, CompanyID) VALUES (@MaxCapacity, @location, @companyId)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaxCapacity", MaxCapactiy);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@companyId", companyId);

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Warehouse added successfully.");
                            // Optionally refresh DataGridView if showing warehouses
                            this.warehousesTableAdapter.Fill(this.wareHousedbDataSet.Warehouses);
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Insert failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox10.Text.Trim(), out int warehouseId))
            {
                DeleteWarehouseById(warehouseId);
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Warehouse ID.");
            }
        }
        private void DeleteWarehouseById(int warehouseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Warehouses WHERE WarehouseID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", warehouseId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Warehouse deleted successfully.");
                            textBox10.Clear(); // Optional: Clear input
                            LoadWarehouses(); // Optional: Reload DataGridView or warehouse list
                        }
                        else
                        {
                            MessageBox.Show("No warehouse found with that ID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void LoadWarehouses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Warehouses";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void Get_Click(object sender, EventArgs e)
        {
            int warehouseId;
            if (!int.TryParse(textBox9.Text, out warehouseId))
            {
                MessageBox.Show("Please enter a valid Warehouse ID.");
                return;
            }

            LoadWarehouseData(warehouseId);
        }
        private void LoadWarehouseData(int warehouseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get Location, MaxCapacity, and CompanyID
                string queryWarehouse = "SELECT Location, MaxCapacity, CompanyID FROM Warehouses WHERE WarehouseID = @WarehouseID";
                using (SqlCommand cmd = new SqlCommand(queryWarehouse, conn))
                {
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox7.Text = reader["Location"].ToString();          // Location
                            textBox3.Text = reader["MaxCapacity"].ToString();       // MaxCapacity

                            object companyIdObj = reader["CompanyID"];
                            if (companyIdObj != DBNull.Value)
                            {
                                listBox2.Items.Clear(); // Only clear here, since we're not using DataSource
                                listBox2.Items.Add("Company ID: " + companyIdObj.ToString());
                            }
                            else
                            {
                                listBox2.Items.Clear();
                                listBox2.Items.Add("No Company ID assigned.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Warehouse not found.");
                            return;
                        }
                    }
                }
            }
        }




        private class ItemListBoxItem
        {
            public int ItemID { get; set; }
            public string DisplayName { get; set; }
            public int Quantity { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            int warehouseId;
            if (!int.TryParse(textBox9.Text, out warehouseId))
            {
                MessageBox.Show("Invalid Warehouse ID.");
                return;
            }

            string location = textBox7.Text;
            int maxCapacity;
            if (!int.TryParse(textBox3.Text, out maxCapacity))
            {
                MessageBox.Show("Invalid Max Capacity.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Update warehouse info
                string updateWarehouse = "UPDATE Warehouses SET Location = @Location, MaxCapacity = @MaxCapacity WHERE WarehouseID = @WarehouseID";
                using (SqlCommand cmd = new SqlCommand(updateWarehouse, conn))
                {
                    cmd.Parameters.AddWithValue("@Location", location);
                    cmd.Parameters.AddWithValue("@MaxCapacity", maxCapacity);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                    cmd.ExecuteNonQuery();
                }

                // Optionally, update item quantities here
                // (You need to implement UI to edit quantities; here we'll just demo the update for example)

                foreach (var obj in listBox2.Items)
                {
                    var item = obj as ItemListBoxItem;
                    if (item != null)
                    {
                        string updateItem = "UPDATE Items SET Quantity = @Quantity WHERE ItemID = @ItemID";
                        using (SqlCommand cmd = new SqlCommand(updateItem, conn))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            MessageBox.Show("Warehouse and items updated successfully.");
        }
    }
}
