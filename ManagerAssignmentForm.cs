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
    public partial class ManagerAssignmentForm : Form
    {
        public ManagerAssignmentForm()
        {
            InitializeComponent();
        }

        private void backToMainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFrom mainFrom = new MainFrom();
            mainFrom.Show();
            this.Hide();
        }
    }
}
