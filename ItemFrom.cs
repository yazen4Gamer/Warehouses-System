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
    public partial class ItemFrom : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public ItemFrom()
        {
            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;

        }
        private void addItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
        }
        private void removeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            groupBox1.Visible = false;
            groupBox3.Visible = false;

        }

        private void editItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
           
        }

        private void backToMainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainFrom mainFrom = new MainFrom();
            mainFrom.Show();
        }

        private void ItemFrom_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Warehouses' table. You can move, or remove it, as needed.
            this.warehousesTableAdapter.Fill(this.wareHousedbDataSet.Warehouses);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Companies' table. You can move, or remove it, as needed.
            this.companiesTableAdapter.Fill(this.wareHousedbDataSet.Companies);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.wareHousedbDataSet.Items);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.ManagerAssignments' table. You can move, or remove it, as needed.
            this.managerAssignmentsTableAdapter.Fill(this.wareHousedbDataSet.ManagerAssignments);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string itemName = textBox1.Text.Trim();
            string itemType = textBox3.Text.Trim();
            int quantity;

            // Validate quantity input
            if (string.IsNullOrEmpty(itemName) || !int.TryParse(textBox2.Text, out quantity))
            {
                MessageBox.Show("Please enter valid item name and quantity.");
                return;
            }

            // Get selected WarehouseID and CompanyID
            if (listBox2.SelectedValue == null || listBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid warehouse and company.");
                return;
            }

            int warehouseId = Convert.ToInt32(listBox2.SelectedValue);
            int companyId = Convert.ToInt32(listBox1.SelectedValue);

            // Insert into Items table
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string insertQuery = @"INSERT INTO Items (ItemName, ItemType, Quantity, WarehouseID, CompanyID)
                               VALUES (@ItemName, @ItemType, @Quantity, @WarehouseID, @CompanyID)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemName", itemName);
                    cmd.Parameters.AddWithValue("@ItemType", string.IsNullOrEmpty(itemType) ? (object)DBNull.Value : itemType);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                    cmd.Parameters.AddWithValue("@CompanyID", companyId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item added successfully!");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Item addition failed.");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == null)
            {
                MessageBox.Show("Please select an item to remove.");
                return;
            }

            int itemId = Convert.ToInt32(textBox6.Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string deleteQuery = "DELETE FROM Items WHERE ItemID = @ItemID";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item removed successfully.");
                        // Refresh item list here if needed
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove item.");
                    }
                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            string newItemName = textBox1.Text.Trim();
            string newItemType = textBox3.Text.Trim();
            int newQuantity;

            if (!int.TryParse(textBox2.Text, out newQuantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            if (textBox9.Text == null)
            {
                MessageBox.Show("Please select an item to update.");
                return;
            }

            int itemId = Convert.ToInt32(textBox9.Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string updateQuery = @"UPDATE Items
                               SET ItemName = @ItemName,
                                   ItemType = @ItemType,
                                   Quantity = @Quantity
                               WHERE ItemID = @ItemID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemName", newItemName);
                    cmd.Parameters.AddWithValue("@ItemType", string.IsNullOrEmpty(newItemType) ? (object)DBNull.Value : newItemType);
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item updated successfully.");
                        // Optionally refresh listBox3 or clear inputs
                    }
                    else
                    {
                        MessageBox.Show("Item update failed.");
                    }
                }
            }
        }

        private void Get_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox9.Text, out int selectedItemId))
            {
                LoadItemDetails(selectedItemId);
            }
            else
            {
                MessageBox.Show("Please enter a valid Item ID.");
            }
        }
        private void LoadItemDetails(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ItemName, ItemType, Quantity FROM Items WHERE ItemID = @ItemID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string itemName = reader["ItemName"].ToString();
                            string itemType = reader["ItemType"].ToString();
                            string quantity = reader["Quantity"].ToString();

                            // Fill textboxes
                            textBox1.Text = itemName;
                            textBox3.Text = itemType;
                            textBox2.Text = quantity;

                        }
                        else
                        {
                            MessageBox.Show("Item not found.");
                        }
                    }
                }
            }
        }


    }
}
