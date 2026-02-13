using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace Metrologya
{
    public partial class VerificationAddForm : Form
    {
        public VerificationAddForm()
        {
            InitializeComponent();
        }

        public Dictionary<int, string> SerialData
        {
            set
            {
                comboBox1.DataSource = value.ToArray();
                comboBox1.DisplayMember = "Value";
            }
        }

        public Dictionary<int, string> OrganizationData
        {
            set
            {
                comboBox2.DataSource = value.ToArray();
                comboBox2.DisplayMember = "Value";
            }
        }

        public int deviceID
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

        public int organizationID
        {
            get
            {
                return ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
            }
            set
            {
                int idx = 0;
                foreach (KeyValuePair<int, string> item in comboBox2.Items)
                {
                    if (item.Key == value)
                    { break; }
                    idx++;
                }
                comboBox2.SelectedIndex = idx;
            }
        }

        public DateTime received
        {
            set { dateTimePicker1.Value = value; }
            get { return dateTimePicker1.Value; }
        }

        public DateTime sent
        {
            set { dateTimePicker2.Value = value; }
            get { return dateTimePicker2.Value; }
        }

        
         private void button1_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") { MessageBox.Show("Устройство не задано."); return; }
           // if (comboBox2.Text == "") { MessageBox.Show("Организация не задана."); return; }
            if (dateTimePicker1.Value == null) { MessageBox.Show("Дата отправки не задана."); return; }
            if (dateTimePicker2.Value == null) { MessageBox.Show("Дата получения не задана."); return; }
            
            received = dateTimePicker1.Value;
            sent = dateTimePicker2.Value;

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
