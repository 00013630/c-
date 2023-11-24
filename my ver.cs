using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtpModuleYear.Format = DateTimePickerFormat.Custom;
            dtpModuleYear.CustomFormat = "yyyy";
        }

        private void tb_ModuleBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tb_ModuleBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.universityDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'universityDataSet.tb_ModuleType' table. You can move, or remove it, as needed.
            this.tb_ModuleTypeTableAdapter.Fill(this.universityDataSet.tb_ModuleType);
            // TODO: This line of code loads data into the 'universityDataSet.tb_Module' table. You can move, or remove it, as needed.
            this.tb_ModuleTableAdapter.Fill(this.universityDataSet.tb_Module);
            enable_disable_buttons();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.tb_ModuleBindingSource.MoveFirst();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.tb_ModuleBindingSource.MovePrevious();

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.tb_ModuleBindingSource.MoveNext();

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.tb_ModuleBindingSource.MoveLast();
        }

        private void enable_disable_buttons()
        {
            if(tb_ModuleBindingSource.Position == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
            } else
            {
                btnFirst.Enabled = true;
                btnPrev.Enabled = true;
            }

            if (tb_ModuleBindingSource.Position == tb_ModuleBindingSource.Count - 1)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            } else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

        }

        private void tb_ModuleBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            enable_disable_buttons();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save_data();
        }

        private void save_data()
        {

            if (string.IsNullOrWhiteSpace(tbxModuleName.Text) || string.IsNullOrWhiteSpace(tbxModuleCode.Text))
            {
                MessageBox.Show("Name and Code are required.");
                return;
            }

            try
            {
                Validate();
                tbModuleBindingSource.EndEdit();
                tableAdapterManager.UpdateAll(universityDataSet);
                MessageBox.Show("Saved");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(tb_ModuleBindingSource.Count == 0)
            {
                MessageBox.Show("Nothing to delete");
            } else
            {
                var result = MessageBox.Show("Are you sure you want to delete?", "Detele", MessageBoxButtons.YesNo);

                if(result == DialogResult.Yes)
                {
                    tbModuleBindingSource.RemoveCurrent();
                    MessageBox.Show("Deleted successfully");
                }
            }
        }

        private void btnAddModule_Click(object sender, EventArgs e)
        {
            var selected = ((DataRowView)cbxModuleType.SelectedItem).Row;

            if (string.IsNullOrWhiteSpace(tbxModuleName.Text) || string.IsNullOrWhiteSpace(tbxModuleCode.Text) || string.IsNullOrWhiteSpace(dtpModuleYear.Value.Year.ToString()))
            {
                MessageBox.Show("Name, Code and Year are required.");
                return;
            }

            universityDataSet.tb_Module.Addtb_ModuleRow(
                tbxModuleCode.Text, tbxModuleName.Text, (int)dtpModuleYear.Value.Year, (UniversityDataSet.tb_ModuleTypeRow)selected
            );
        }
    }
}
