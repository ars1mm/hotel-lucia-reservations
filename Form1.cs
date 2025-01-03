using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new Baza())
                {
                    dataGridView1.DataSource = context.Customers.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new Baza())
                {
                    var customer = new Customers
                    {
                        CustomerName = nameTextBox.Text,
                        CustomerPhone = phoneTextBox.Text,
                        ArrivalTime = ArrivalDatePicker.Value,
                        DepartureTime = DepartureDatePicker.Value
                    };

                    context.Customers.Add(customer);
                    context.SaveChanges();

                    nameTextBox.Clear();
                    phoneTextBox.Clear();
                    ArrivalDatePicker.Value = DateTime.Now;
                    DepartureDatePicker.Value = DateTime.Now;

                    LoadData(); // Refresh grid
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadForm readForm = new ReadForm();
            readForm.StartPosition = FormStartPosition.CenterScreen;
            readForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var updateForm = new UpdateForm();
            updateForm.FormClosed += (s, args) => 
            {
                if (updateForm.DialogResult == DialogResult.OK)
                {
                    LoadData(); // Refresh grid after update
                }
            };
            updateForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var deleteForm = new DeleteForm();
            deleteForm.FormClosed += (s, args) => 
            {
                if (deleteForm.DialogResult == DialogResult.OK)
                {
                    LoadData(); // Refresh grid after delete
                }
            };
            deleteForm.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}
