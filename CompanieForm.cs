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
    public partial class CompanieForm : Form
    {
        public CompanieForm()
        {
            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CompanieForm_Load(object sender, EventArgs e)
        {

        }

        private void itmesToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void CompanieForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
