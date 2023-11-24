using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Seminar10Progress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tbTeacherBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            SaveData();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbDataSet.tbCountry' table. You can move, or remove it, as needed.
            this.tbCountryTableAdapter.Fill(this.dbDataSet.tbCountry);
            // TODO: This line of code loads data into the 'dbDataSet.tbTeacher' table. You can move, or remove it, as needed.
            this.tbTeacherTableAdapter.Fill(this.dbDataSet.tbTeacher);
            EnableDisableButtons();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveFirst();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MovePrevious();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveNext();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tbTeacherBindingSource.MoveLast();
        }
        private void EnableDisableButtons()
        {
            //enable and disble navigation buttons
            if (tbTeacherBindingSource.Position == 0)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
            }

            if (tbTeacherBindingSource.Position == tbTeacherBindingSource.Count - 1)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        private void SaveData()
        {
            try
            {
                Validate();
                tbTeacherBindingSource.EndEdit();
                tableAdapterManager.UpdateAll(dbDataSet);

                MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbTeacherBindingSource.Count == 0)
            {
                MessageBox.Show("Nothing to delete");
            }
            else
            {
                var selection = MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo);
                if(selection == DialogResult.Yes)
                {
                    tbTeacherBindingSource.RemoveCurrent();
                    MessageBox.Show("Deleted successfully");
                }
            }
                


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedCountry = ((DataRowView)cbxNewCountry.SelectedItem).Row;

            //add to in-memory dataset
            dbDataSet.tbTeacher.AddtbTeacherRow(
                tbxNewFirstName.Text,
                tbxNewLastName.Text,
                dtpNewDob.Value,
                tbxNewPhone.Text,
                (int)nudNewGrade.Value,
                chbNewIsActive.Checked,
                (dbDataSet.tbCountryRow)selectedCountry
            );

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Validate();
            tbTeacherBindingSource.EndEdit();
            if (dbDataSet.HasChanges())
            {
                if (MessageBox.Show("Do you want to save changes?",
                "Save",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
                    SaveData();
            }

        }

        private void tbxFilter_TextChanged(object sender, EventArgs e)
        {
            tbTeacherBindingSource.Filter = $" lastName LIKE '{tbxFilter.Text}%'";
        }

        private void tbTeacherBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void dtpNewDob_Validating(object sender, CancelEventArgs e)
        {
            if (dtpNewDob.Value.AddYears(18) > DateTime.Now)
            {
                MessageBox.Show("Should be at least 18 years old");
                e.Cancel = true;
            }
        }
    }
}
