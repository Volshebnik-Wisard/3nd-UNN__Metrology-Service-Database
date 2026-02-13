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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectTab(0);
        }

        //string connection = "Data Source=192.168.9.5;User ID=sa;Password=sa";
        string connection = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Управление данными\Metrologya\Metrologya\Database1.mdf;Integrated Security=True";




        private void RefreshDGV(int index)
        {
            string SQLRequest = "SELECT * FROM [Order]";
            switch (index)
            {
                case 0: SQLRequest = "SELECT * FROM [Guschin_device]"; break;
                case 1: SQLRequest = "SELECT * FROM [Guschin_organization] JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id"; break;
                case 2:
                    SQLRequest = "SELECT * FROM [Guschin_device]";
                    SQLRequest += " JOIN [Guschin_ver_procedure] ON [Guschin_device].Id = [Guschin_ver_procedure].device_id";
                    SQLRequest += " JOIN [Guschin_organization] ON [Guschin_ver_procedure].organization_id = [Guschin_organization].Id";
                    SQLRequest += " JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id";
                    break;

            }

            //Создаем объект адаптера
            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

            //Создаем объект-таблицу
            DataTable dataTable = new DataTable();

            //Заполняем таблицу посредством адаптера
            adapter.Fill(dataTable);

            DataGridView dgv = new DataGridView();

            switch (index)
            {
                case 0: dgv = dataGridView1; break;
                case 1: dgv = dataGridView2; break;
                case 2: dgv = dataGridView3; break;
            }

            dgv.DataSource = dataTable;

            switch (index)
            {
                case 0:
                    dgv.Columns["Id"].Visible = false;
                    dgv.Columns["type"].HeaderText = "Тип";
                    dgv.Columns["name"].HeaderText = "Наименование";
                    dgv.Columns["responsible"].HeaderText = "Ответственный";
                    dgv.Columns["placement"].HeaderText = "Размещение";
                    dgv.Columns["serial_number"].HeaderText = "Серийный номер";
                    dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
                    dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";
                    break;
                case 1:
                    dgv.Columns["Id"].Visible = false;
                    dgv.Columns["Id1"].Visible = false;
                    dgv.Columns["type_id"].Visible = false;
                    dgv.Columns["name"].HeaderText = "Название организации";
                    dgv.Columns["address"].HeaderText = "Адрес";
                    dgv.Columns["type"].HeaderText = "Типы поверяемых приборов";
                    break;

                case 2:
                    dgv.Columns["Id"].Visible = false;
                    dgv.Columns["Id1"].Visible = false;
                    dgv.Columns["Id2"].Visible = false;
                    dgv.Columns["organization_id"].Visible = false;
                    dgv.Columns["device_id"].Visible = false;
                    dgv.Columns["type_id"].Visible = false;
                    dgv.Columns["type"].HeaderText = "Тип";
                    dgv.Columns["name"].HeaderText = "Наименование";
                    dgv.Columns["responsible"].HeaderText = "Ответственный";
                    dgv.Columns["placement"].Visible = false;
                    dgv.Columns["serial_number"].HeaderText = "Серийный номер";
                    dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
                    dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";
                    dgv.Columns["received"].HeaderText = "Получено";
                    dgv.Columns["sent"].HeaderText = "Отправлено";
                    dgv.Columns["name1"].HeaderText = "Название организации";
                    dgv.Columns["address"].Visible = false;
                    dgv.Columns["type1"].Visible = false;
                    break;

            }


            dgv.Show();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            RefreshDGV(e.TabPageIndex);
        }

        private void buttonAdd1_Click(object sender, EventArgs e)
        {
            DeviceAddForm f = new DeviceAddForm();
            f.ShowDialog();

            if (f.DialogResult == DialogResult.OK)
            {
                string SQLRequest = "";


                SQLRequest += "INSERT INTO [Guschin_device] (type,name,responsible,placement,serial_number,verification_interval,date_last_ver)";
                SQLRequest += "VALUES ('" + f.type + "','" + f.name + "','" + f.responsible + "','" + f.placement + "','" + f.serial_number + "','" + f.verification_interval + "','" + f.date_last_ver.ToString("yyyy-MM-dd") + "')";

                ExecuteRequest2(SQLRequest);
            }

            RefreshDGV(0);
        }

        private void buttonChange1_Click(object sender, EventArgs e)
        {
            DeviceAddForm f = new DeviceAddForm();

            var dgv = dataGridView1;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0) { MessageBox.Show("Не выбрана строка."); return; }
            else if (selected.Count > 1) { MessageBox.Show("Выбрано больше одной строки."); return; };
            int deviceID = (int)dgv["Id", selected[0].Index].Value;

            f.type = selected[0].Cells[1].Value.ToString();
            f.name = selected[0].Cells[2].Value.ToString();
            f.responsible = selected[0].Cells[3].Value.ToString();
            f.placement = selected[0].Cells[4].Value.ToString();
            f.serial_number = selected[0].Cells[5].Value.ToString();
            f.verification_interval = selected[0].Cells[6].Value.ToString();
            f.date_last_ver = DateTime.Parse(selected[0].Cells[7].Value.ToString());


            f.ShowDialog();

            if (f.DialogResult == DialogResult.OK)
            {
                string SQLRequest = "";

                SQLRequest += "UPDATE [Guschin_device] ";
                SQLRequest += "SET type = '" + f.type + "', name = '" + f.name + "', responsible = '" + f.responsible + "', placement = '" + f.placement + "', serial_number = '" + f.serial_number + "', verification_interval = '" + f.verification_interval + "', date_last_ver = '" + f.date_last_ver.ToString("yyyy-MM-dd") + "'";
                SQLRequest += "WHERE Id = " + deviceID.ToString();

                ExecuteRequest2(SQLRequest);
            }

            RefreshDGV(0);
        }

        private void buttonDelete1_Click(object sender, EventArgs e)
        {
            var dgv = dataGridView1;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0) { MessageBox.Show("Не выбрана строка."); return; }
            else if (selected.Count > 1) { MessageBox.Show("Выбрано больше одной строки."); return; };
            int deviceID = (int)dgv["Id", selected[0].Index].Value;

            string SQLRequest = "DELETE [Guschin_device] WHERE Id = " + deviceID.ToString();
            ExecuteRequest2(SQLRequest);

            RefreshDGV(0);
        }





        private void buttonAdd2_Click(object sender, EventArgs e)
        {
            OrganizationAddForm f = new OrganizationAddForm();

            string SQLRequest = "SELECT * FROM [Guschin_type]";

            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            var dict = new System.Collections.Generic.Dictionary<int, string>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dict.Add((int)dataRow[0], dataRow[1].ToString());
            }
            f.OrganizationData = dict;

            f.ShowDialog();

            if (f.DialogResult == DialogResult.OK)
            {
                string SQLRequest2 = "";

                SQLRequest2 += "INSERT INTO [Guschin_organization] (name,address,type_id)";
                SQLRequest2 += "VALUES ('" + f.name + "','" + f.address + "','" + f.Organization + "')";

                ExecuteRequest2(SQLRequest2);
            }
            RefreshDGV(1);
        }

        private void buttonChange2_Click(object sender, EventArgs e)
        {
            OrganizationAddForm f = new OrganizationAddForm();


            string SQLRequest = "SELECT * FROM [Guschin_type]";

            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            var dict = new System.Collections.Generic.Dictionary<int, string>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dict.Add((int)dataRow[0], dataRow[1].ToString());
            }
            f.OrganizationData = dict;


            var dgv = dataGridView2;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0) { MessageBox.Show("Не выбрана строка."); return; }
            else if (selected.Count > 1) { MessageBox.Show("Выбрано больше одной строки."); return; };
            int OrgID = (int)dgv["Id", selected[0].Index].Value;

            f.name = selected[0].Cells["name"].Value.ToString();
            f.address = selected[0].Cells["address"].Value.ToString();
            f.OrganizationID = (int)selected[0].Cells["type_id"].Value;

            f.ShowDialog();


            if (f.DialogResult == DialogResult.OK)
            {
                string SQLRequest2 = "";

                SQLRequest2 += "UPDATE [Guschin_organization]  ";
                SQLRequest2 += "SET name = " + f.name + ", address = '" + f.address + "', type_id = " + f.OrganizationID + " ";
                SQLRequest2 += "WHERE Id = " + OrgID;

                ExecuteRequest2(SQLRequest2);
            }
            RefreshDGV(1);
        }

        private void buttonDelete2_Click(object sender, EventArgs e)
        {
            var dgv = dataGridView2;
            var selected = dgv.SelectedRows;

            if (selected.Count == 0) { MessageBox.Show("Не выбрана строка."); return; }
            else if (selected.Count > 1) { MessageBox.Show("Выбрано больше одной строки."); return; };
            int OrgID = (int)dgv["Id", selected[0].Index].Value;

            string SQLRequest = "";

            SQLRequest += "DELETE [Guschin_organization] ";
            SQLRequest += "WHERE Id = " + OrgID.ToString();

            ExecuteRequest2(SQLRequest);


            RefreshDGV(1);
        }

        
        private void buttonAdd3_Click(object sender, EventArgs e)
        {
            VerificationAddForm f = new VerificationAddForm();


            string SQLRequest1 = "SELECT * FROM [Guschin_device]";

            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest1, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            var dict1 = new System.Collections.Generic.Dictionary<int, string>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dict1.Add((int)dataRow["Id"],  dataRow["serial_number"].ToString() + ";  Type: " + dataRow["type"].ToString());
            }
            f.SerialData = dict1;

            string SQLRequest2 = "SELECT * FROM [Guschin_organization] JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id";

            SqlDataAdapter adapter2 = new SqlDataAdapter(SQLRequest2, connection);
            DataTable dataTable2 = new DataTable();
            adapter2.Fill(dataTable2);

            var dict2 = new System.Collections.Generic.Dictionary<int, string>();

            foreach (DataRow dataRow in dataTable2.Rows)
            {
                dict2.Add((int)dataRow["Id"], dataRow["name"].ToString() + ";" + "  Type: " + dataRow["type"]);
            }
            f.OrganizationData = dict2;

            f.ShowDialog();


            if (f.DialogResult == DialogResult.OK)
            {
                string SQLRequest3 = "";


                SQLRequest3 += "INSERT INTO [Guschin_ver_procedure] (device_id,sent,received,organization_id)";
                SQLRequest3 += "VALUES ('" + f.deviceID + "','" + f.sent.ToString("yyyy-MM-dd") + "','" + f.received.ToString("yyyy-MM-dd") + "'," + f.organizationID + ")";
                ExecuteRequest2(SQLRequest3);
            }

            RefreshDGV(2);
        }

        private void buttonDelete3_Click(object sender, EventArgs e)
        {
            var dgv = dataGridView3;
            var selected = dgv.SelectedRows;
            if (selected.Count == 0) { MessageBox.Show("Не выбрана строка."); return; }
            else if (selected.Count > 1) { MessageBox.Show("Выбрано больше одной строки."); return; };
            int deviceID = (int)dgv["device_id", selected[0].Index].Value;
            int organizationID = (int)dgv["organization_id", selected[0].Index].Value;

            string SQLRequest = "";

            SQLRequest += "DELETE [Guschin_ver_procedure] ";
            SQLRequest += "WHERE device_id = " + deviceID + " AND organization_id = " + organizationID;

            ExecuteRequest2(SQLRequest);

            RefreshDGV(2);
        }

       private void ExecuteRequest2(string SQLRequest)
        {

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection;

            conn.Open();//Открываем соединение

            //Создаем команду, ассоциированную с открытым соединением
            SqlCommand command = conn.CreateCommand();

            //Определяем саму команду и ее параметры
            command.CommandText = SQLRequest;

            //Выдаем команду, рез. команды помещаем в специальный объект
            SqlDataReader результат = command.ExecuteReader();
            conn.Close();	//Закрываем соединение	

        }



        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            DeviceFind1();
        }

        private void DeviceFind1()
        {
            string SQLRequest = "";
            SQLRequest += "SELECT * FROM [Guschin_device]\n";

            SQLRequest += "WHERE [Guschin_device].type LIKE '%" + textBox11.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].name LIKE '%" + textBox12.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].responsible LIKE '%" + textBox13.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].placement LIKE '%" + textBox14.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].serial_number LIKE '%" + textBox15.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].verification_interval LIKE '%" + textBox16.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].date_last_ver LIKE '%" + textBox17.Text + "%' \n";

            //Создаем объект адаптера
            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

            //Создаем объект-таблицу
            DataTable dataTable = new DataTable();

            //Заполняем таблицу посредством адаптера
            adapter.Fill(dataTable);

            DataGridView dgv = dataGridView1;

            dgv.DataSource = dataTable;

            dgv.Columns["Id"].Visible = false;
            dgv.Columns["type"].HeaderText = "Тип";
            dgv.Columns["name"].HeaderText = "Наименование";
            dgv.Columns["responsible"].HeaderText = "Ответственный";
            dgv.Columns["placement"].HeaderText = "Размещение";
            dgv.Columns["serial_number"].HeaderText = "Серийный номер";
            dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
            dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";

            dgv.Show();
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            DeviceFind2();
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            DeviceFind2();
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            DeviceFind2();
        }


        private void DeviceFind2()
        {
            string SQLRequest = "";
            SQLRequest += "SELECT * FROM [Guschin_organization] JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id\n";


            SQLRequest += "WHERE [Guschin_type].type LIKE '%" + textBox21.Text + "%' \n";
            SQLRequest += "  AND [Guschin_organization].name LIKE '%" + textBox22.Text + "%' \n";
            SQLRequest += "  AND [Guschin_organization].address LIKE '%" + textBox23.Text + "%' \n";

            //Создаем объект адаптера
            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

            //Создаем объект-таблицу
            DataTable dataTable = new DataTable();

            //Заполняем таблицу посредством адаптера
            adapter.Fill(dataTable);

            DataGridView dgv = dataGridView2;

            dgv.DataSource = dataTable;

            dgv.Columns["Id"].Visible = false;
            dgv.Columns["Id1"].Visible = false;
            dgv.Columns["type_id"].Visible = false;
            dgv.Columns["name"].HeaderText = "Название организации";
            dgv.Columns["address"].HeaderText = "Адрес";
            dgv.Columns["type"].HeaderText = "Типы поверяемых приборов";


            dgv.Show();
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {
            DeviceFind3();
        }

        private void DeviceFind3()
        {

            string SQLRequest = "";
            SQLRequest += "SELECT * FROM [Guschin_device]\n";
            SQLRequest += " JOIN [Guschin_ver_procedure] ON [Guschin_device].Id = [Guschin_ver_procedure].device_id\n";
            SQLRequest += " JOIN [Guschin_organization] ON [Guschin_ver_procedure].organization_id = [Guschin_organization].Id\n";
            SQLRequest += " JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id\n";


            SQLRequest += "WHERE [Guschin_device].type LIKE '%" + textBox31.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].name LIKE '%" + textBox32.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].responsible LIKE '%" + textBox33.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].serial_number LIKE '%" + textBox34.Text + "%' \n";
            SQLRequest += "  AND [Guschin_device].date_last_ver LIKE '%" + textBox35.Text + "%' \n";
            SQLRequest += "  AND [Guschin_ver_procedure].sent LIKE '%" + textBox36.Text + "%' \n";
            SQLRequest += "  AND [Guschin_ver_procedure].received LIKE '%" + textBox37.Text + "%' \n";
            SQLRequest += "  AND [Guschin_organization].name LIKE '%" + textBox38.Text + "%' \n";

            //Создаем объект адаптера
            SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

            //Создаем объект-таблицу
            DataTable dataTable = new DataTable();

            //Заполняем таблицу посредством адаптера
            adapter.Fill(dataTable);

            DataGridView dgv = dataGridView3;

            dgv.DataSource = dataTable;

            dgv.Columns["Id"].Visible = false;
            dgv.Columns["Id1"].Visible = false;
            dgv.Columns["Id2"].Visible = false;
            dgv.Columns["organization_id"].Visible = false;
            dgv.Columns["device_id"].Visible = false;
            dgv.Columns["type_id"].Visible = false;
            dgv.Columns["type"].HeaderText = "Тип";
            dgv.Columns["name"].HeaderText = "Наименование";
            dgv.Columns["responsible"].HeaderText = "Ответственный";
            dgv.Columns["placement"].Visible = false;
            dgv.Columns["serial_number"].HeaderText = "Серийный номер";
            dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
            dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";
            dgv.Columns["received"].HeaderText = "Получено";
            dgv.Columns["sent"].HeaderText = "Отправлено";
            dgv.Columns["name1"].HeaderText = "Название организации";
            dgv.Columns["address"].Visible = false;
            dgv.Columns["type1"].Visible = false;


            dgv.Show();
        }

        private void checkVer1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkVer1.Checked == true)
            {
                string SQLRequest = "";
                SQLRequest += "SELECT * FROM [Guschin_device]\n";
                SQLRequest += "WHERE [Guschin_device].date_last_ver NOT BETWEEN @date1 and @date2\n";

                //Создаем объект адаптера
                SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

                //Создаем объект-таблицу
                DataTable dataTable = new DataTable();
 
                var dateOnly1 = DateTime.Now;
                var dateOnly2 = DateTime.Now;
                var dateOnly3 = dateOnly2.AddYears(-1).AddDays(14);

                adapter.SelectCommand.Parameters.AddWithValue("@date1", dateOnly3);
                adapter.SelectCommand.Parameters.AddWithValue("@date2", dateOnly1);

                //Заполняем таблицу посредством адаптера
                adapter.Fill(dataTable);

                DataGridView dgv = dataGridView1;

                dgv.DataSource = dataTable;

                dgv.Columns["Id"].Visible = false;
                dgv.Columns["type"].HeaderText = "Тип";
                dgv.Columns["name"].HeaderText = "Наименование";
                dgv.Columns["responsible"].HeaderText = "Ответственный";
                dgv.Columns["placement"].HeaderText = "Размещение";
                dgv.Columns["serial_number"].HeaderText = "Серийный номер";
                dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
                dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";

                dgv.Show();
            }

            else RefreshDGV(0);
        }

        private void checkVer2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkVer2.Checked == true)
            {
                string SQLRequest = "";
                SQLRequest += "SELECT * FROM [Guschin_device]\n";
                SQLRequest += " JOIN [Guschin_ver_procedure] ON [Guschin_device].Id = [Guschin_ver_procedure].device_id\n";
                SQLRequest += " JOIN [Guschin_organization] ON [Guschin_ver_procedure].organization_id = [Guschin_organization].Id\n";
                SQLRequest += " JOIN [Guschin_type] ON [Guschin_organization].type_id = [Guschin_type].Id\n";

                SQLRequest += "WHERE [Guschin_device].date_last_ver NOT BETWEEN @date1 and @date2\n";


                //Создаем объект адаптера
                SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest, connection);

                //Создаем объект-таблицу
                DataTable dataTable = new DataTable();

                var dateOnly1 = DateTime.Now;
                var dateOnly2 = DateTime.Now;
                var dateOnly3 = dateOnly2.AddYears(-1).AddDays(14);

                adapter.SelectCommand.Parameters.AddWithValue("@date1", dateOnly3);
                adapter.SelectCommand.Parameters.AddWithValue("@date2", dateOnly1);

                //Заполняем таблицу посредством адаптера
                adapter.Fill(dataTable);

                DataGridView dgv = dataGridView3;

                dgv.DataSource = dataTable;

                dgv.Columns["Id"].Visible = false;
                dgv.Columns["Id1"].Visible = false;
                dgv.Columns["Id2"].Visible = false;
                dgv.Columns["organization_id"].Visible = false;
                dgv.Columns["device_id"].Visible = false;
                dgv.Columns["type_id"].Visible = false;
                dgv.Columns["type"].HeaderText = "Тип";
                dgv.Columns["name"].HeaderText = "Наименование";
                dgv.Columns["responsible"].HeaderText = "Ответственный";
                dgv.Columns["placement"].Visible = false;
                dgv.Columns["serial_number"].HeaderText = "Серийный номер";
                dgv.Columns["verification_interval"].HeaderText = "Интервал поверки (мес.)";
                dgv.Columns["date_last_ver"].HeaderText = "Дата последней поверки";
                dgv.Columns["received"].HeaderText = "Получено";
                dgv.Columns["sent"].HeaderText = "Отправлено";
                dgv.Columns["name1"].HeaderText = "Название организации";
                dgv.Columns["address"].Visible = false;
                dgv.Columns["type1"].Visible = false;


                dgv.Show();
            }

            else RefreshDGV(2);
        }
    }
}
