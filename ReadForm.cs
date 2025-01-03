using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ReadForm : Form
    {
        public ReadForm()
        {
            InitializeComponent();
            dataGridViewCustomers.ReadOnly = true;
            dataGridViewCustomers.AllowUserToAddRows = false;
            dataGridViewCustomers.AllowUserToDeleteRows = false;
            dataGridViewCustomers.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new Baza())
                {
                    dataGridViewCustomers.DataSource = context.Customers.ToList();
                    dataGridViewCustomers.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                LoadData();
                return;
            }

            if (int.TryParse(textBox1.Text, out int searchId))
            {
                using (var context = new Baza())
                {
                    dataGridViewCustomers.DataSource = context.Customers
                        .Where(c => c.CustomersId == searchId)
                        .ToList();
                }
            }
        }

        private void dataGridViewCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
