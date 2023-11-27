using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tb_ModuleBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'universityDataSet.tb_ModuleType' table. You can move, or remove it, as needed.
            this.tb_ModuleTypeTableAdapter.Fill(this.universityDataSet.tb_ModuleType);
            // TODO: This line of code loads data into the 'universityDataSet.tb_Module' table. You can move, or remove it, as needed.
            this.tb_ModuleTableAdapter.Fill(this.universityDataSet.tb_Module);

            if(universityDataSet != null && universityDataSet.tb_Module.Columns.Count > 0)
            {
                DataTable moduleTable = this.universityDataSet.tb_Module;

                foreach (DataColumn dataColumn in moduleTable.Columns)
                {
                    cbxFilterByColumn.Items.Add(dataColumn.ColumnName);
                }

                if (cbxFilterByColumn.Items.Count > 0)
                {
                    cbxFilterByColumn.SelectedIndex = 0;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.InputValidations())
            {
                this.SaveData();
            }
        }

        private void SaveData()
        {
            try
            {
                this.Validate();
                this.tb_ModuleBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.universityDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want this delete?", "Delete", MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)
            {
                tb_ModuleBindingSource.RemoveCurrent();
                MessageBox.Show("Successfully deleted!");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedType = ((DataRowView) cbxNewModuleType.SelectedItem).Row;

            string newModuleCode = tbxNewModuleCode.Text;
            string newModuleName = tbxNewModuleName.Text;
            int newModuleType = Convert.ToInt32(tbxNewModuleYear.Text);

            universityDataSet.tb_Module.Addtb_ModuleRow(newModuleCode, newModuleName, newModuleType, (UniversityDataSet.tb_ModuleTypeRow)selectedType);
        }

        private void tbxFilter_TextChanged(object sender, EventArgs e)
        {
            string selectedColumn = cbxFilterByColumn.SelectedItem.ToString();


            if (!string.IsNullOrEmpty(selectedColumn))
            {
                DataColumn dataColumn = universityDataSet.tb_Module.Columns[selectedColumn];

                if(dataColumn.DataType == typeof(string))
                {
                    tb_ModuleBindingSource.Filter = $"{dataColumn} LIKE '%{tbxFilter.Text}%'";
                } else if(dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(double))
                {
                    if (int.TryParse(tbxFilter.Text, out int _) || double.TryParse(tbxFilter.Text, out double _))
                    {
                        tb_ModuleBindingSource.Filter = $"{selectedColumn} = {tbxFilter.Text}";
                    }
                }
            } else
            {
                tb_ModuleBindingSource.Filter = $"ModuleName LIKE '%{tbxFilter.Text}%'";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Validate();
            tb_ModuleBindingSource.EndEdit();
            if(this.universityDataSet.HasChanges())
            {
                var response = MessageBox.Show("Do you want to save changes?", "Save", MessageBoxButtons.YesNo);
                if(response == DialogResult.Yes)
                {
                    SaveData();
                    MessageBox.Show("Saved!");
                }
                
            } 
           
        }

        private bool InputValidations()
        {
            if (string.IsNullOrEmpty(tbxModuleCode.Text))
            {
                MessageBox.Show("Code cannot be empty");
                return false;
            }

            if (string.IsNullOrEmpty(tbxModuleName.Text))
            {
                MessageBox.Show("Name cannot be empty");
                return false;
            }

            return true;
        }

        private void tb_ModuleBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(tb_ModuleBindingSource.Count == 0) {
                btnDelete.Enabled = false;
            } else
            {
                btnDelete.Enabled = true;
            }
        }
    }
}
