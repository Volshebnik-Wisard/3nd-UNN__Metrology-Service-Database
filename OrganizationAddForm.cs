using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metrologya
{
    public partial class OrganizationAddForm : Form
    {
        public OrganizationAddForm()
        {
            InitializeComponent();
        }

        public string name
        {
            set { textBox1.Text = value; }
            get { return textBox1.Text; }
        }
        public string address
        {
            set { textBox2.Text = value; }
            get { return textBox2.Text; }
        }

        public string Organization;

        public Dictionary<int, string> OrganizationData
        {
            set
            {
                comboBox1.DataSource = value.ToArray();
                comboBox1.DisplayMember = "Value";
            }
        }


        public int OrganizationID
        {
            get
            {
                return ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
            }
            set
            {
                int idx = 0;
                foreach (KeyValuePair<int, string> item in comboBox1.Items)
                {
                    if (item.Key == value)
                    { break; }
                    idx++;
                }
                comboBox1.SelectedIndex = idx;
            }
        }

        


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Название организации не задано."); return; }
            if (textBox2.Text == "") { MessageBox.Show("Адрес не задан."); return; }
            if (comboBox1.Text == "") { MessageBox.Show("Тип не задан."); return; }

            name = textBox1.Text;
            address = textBox2.Text;
            Organization = OrganizationID.ToString();

           
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
