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
    public partial class DeviceAddForm : Form
    {
        public DeviceAddForm()
        {
            InitializeComponent();
        }

        public string type
        {
            set { textBox1.Text = value; }
            get { return textBox1.Text; }
        }
        public string name
        {
            set { textBox2.Text = value; }
            get { return textBox2.Text; }
        }
        public string responsible
        {
            set { textBox3.Text = value; }
            get { return textBox3.Text; }
        }
        public string placement
        {
            set { textBox4.Text = value; }
            get { return textBox4.Text; }
        }
        public string serial_number
        {
            set { textBox5.Text = value; }
            get { return textBox5.Text; }
        }
        public string verification_interval
        {
            set { textBox6.Text = value; }
            get { return textBox6.Text; }
        }

        public DateTime date_last_ver
        {
            set { dateTimePicker1.Value = value; }
            get { return dateTimePicker1.Value; }
        }
      
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Тип не задан."); return; }
            if (textBox2.Text == "") { MessageBox.Show("Имя не задано."); return; }
            if (textBox3.Text == "") { MessageBox.Show("Ответственный не задан."); return; }
            if (textBox4.Text == "") { MessageBox.Show("Размещение не задано."); return; }
            if (textBox5.Text == "") { MessageBox.Show("Серийный номер не задан."); return; }
            if (textBox6.Text == "") { MessageBox.Show("Интервал поверки не задан."); return; }
            if (dateTimePicker1.Value == null) { MessageBox.Show("Дата последней поверки не задана."); return; }
            
            type = textBox1.Text;
            name = textBox2.Text;
            responsible = textBox3.Text;
            placement = textBox4.Text;
            serial_number = textBox5.Text;
            verification_interval = textBox6.Text;
            date_last_ver = dateTimePicker1.Value;

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

