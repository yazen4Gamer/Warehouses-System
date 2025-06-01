using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warehouses_System
{
    public partial class ItemFrom : Form
    {
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
            // TODO: This line of code loads data into the 'wareHousedbDataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.wareHousedbDataSet.Items);
            // TODO: This line of code loads data into the 'wareHousedbDataSet.ManagerAssignments' table. You can move, or remove it, as needed.
            this.managerAssignmentsTableAdapter.Fill(this.wareHousedbDataSet.ManagerAssignments);

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
