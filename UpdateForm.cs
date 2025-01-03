using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace WindowsFormsApp1
{
    public partial class UpdateForm : Form
    {
        private Baza dbContext;
        private Customers currentCustomer;

        public UpdateForm()
        {
            InitializeComponent();
            groupBoxCustomer.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text))
            {
                MessageBox.Show("Please enter a Customer ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtCustomerId.Text, out int customerId))
            {
                MessageBox.Show("Please enter a valid numeric ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (dbContext = new Baza())
                {
                    dbContext.Configuration.AutoDetectChangesEnabled = false;
                    currentCustomer = await dbContext.Customers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CustomersId == customerId);

                    if (currentCustomer == null)
                    {
                        MessageBox.Show("Customer not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        groupBoxCustomer.Enabled = false;
                        btnUpdate.Enabled = false;
                        return;
                    }

                    // Populate the form fields
                    txtName.Text = currentCustomer.CustomerName;
                    txtPhone.Text = currentCustomer.CustomerPhone;
                    txtArrivalTime.Text = currentCustomer.ArrivalTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtDepartureTime.Text = currentCustomer.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss");

                    // Enable the form fields and update button
                    groupBoxCustomer.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentCustomer == null)
            {
                MessageBox.Show("Please search for a customer first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text) || 
                string.IsNullOrWhiteSpace(txtPhone.Text) || 
                string.IsNullOrWhiteSpace(txtArrivalTime.Text) || 
                string.IsNullOrWhiteSpace(txtDepartureTime.Text))
            {
                MessageBox.Show("All fields are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Parse dates
                if (!DateTime.TryParse(txtArrivalTime.Text, out DateTime arrivalTime) ||
                    !DateTime.TryParse(txtDepartureTime.Text, out DateTime departureTime))
                {
                    MessageBox.Show("Please enter valid dates in the format: yyyy-MM-dd HH:mm:ss", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (dbContext = new Baza())
                {
                    var customerToUpdate = await dbContext.Customers
                        .FirstOrDefaultAsync(c => c.CustomersId == currentCustomer.CustomersId);

                    if (customerToUpdate == null)
                    {
                        MessageBox.Show("Customer no longer exists in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update customer details
                    customerToUpdate.CustomerName = txtName.Text;
                    customerToUpdate.CustomerPhone = txtPhone.Text;
                    customerToUpdate.ArrivalTime = arrivalTime;
                    customerToUpdate.DepartureTime = departureTime;

                    await dbContext.SaveChangesAsync();
                    MessageBox.Show("Customer updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
