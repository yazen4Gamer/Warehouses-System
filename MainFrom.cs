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
    public partial class MainFrom : Form
    {
        public MainFrom()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemFrom itemsForm = new ItemFrom();
            itemsForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CompanieForm companieForm = new CompanieForm();
            companieForm.Show();
            this.Hide();
        }
    }
}
