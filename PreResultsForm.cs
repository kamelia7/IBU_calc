using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace IBUcalc
{
    public partial class PreResultsForm : Form
    {
        private bool authorized = false;
        private string user;
        private string password;

        public PreResultsForm(IBUform ibuform)
        {
            InitializeComponent();
            textBox2.Focus();
            user = ibuform.User;
            password = ibuform.Password;
            if (user != "" && password != "")
            {
                textBox2.Text = user;
                textBox1.Text = password;

                checkLoginAndPassword();
                dataGridView1.Rows.Clear();
                if (authorized) outputUserData();
            }
        }

        private void outputUserData()
        {
            dataGridView1.Rows.Clear();
            string user = textBox2.Text;

            try
            {
                if (File.Exists(Application.StartupPath + @"\data.csv"))
                {
                    int c = 0;
                    foreach (string line in File.ReadAllLines(Application.StartupPath + @"\data.csv"))
                    {
                        string[] mas = line.Split(';');
                       
                        if (line.Contains(user))
                        {                                                      
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[c].Cells[0].Value = mas[1].Trim('\"');
                            dataGridView1.Rows[c].Cells[1].Value = mas[2].Trim('\"');
                            dataGridView1.Rows[c].Cells[2].Value = mas[3].Trim('\"');
                            
                            c += 1;                         
                        }
                    }
                    if (c == 0) MessageBox.Show("Нет сохраненных результатов пользователя", "Просмотр результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        void checkLoginAndPassword()
        {
            authorized = false;

            string user = textBox2.Text;
            string password = textBox1.Text;

            if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите свой логин и пароль для просмотра результатов!", "Просмотр результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                textBox2.Focus();
            }
            else if (string.IsNullOrEmpty(user.Trim()) || string.IsNullOrEmpty(password.Trim()))
            {
                MessageBox.Show("Некорректно введены данные!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                textBox2.Focus();
            }
            else
            {
                try
                {
                    if (File.Exists(Application.StartupPath + @"\users.csv"))
                    {
                        foreach (string line in File.ReadAllLines(Application.StartupPath + @"\users.csv"))
                        {
                            if (line.Contains(user) && line.Contains(password) && (line.Length - (user.Length + password.Length) < 6) )
                            {
                                authorized = true;
                                break;
                            }
                        }
                        if (authorized == false) MessageBox.Show("Данной учетной записи не существует!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);

                    }
                    else MessageBox.Show("Данной учетной записи не существует!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }


        private void btn_enter_Click(object sender, EventArgs e)
        {
            checkLoginAndPassword();
            dataGridView1.Rows.Clear();
            if (authorized) outputUserData();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox2.Text.Trim())))
                this.label2.Visible = false;
            else
                this.label2.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox1.Text.Trim())))
                this.label1.Visible = false;
            else
                this.label1.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void delAllFromDG()
        {
            string user = textBox2.Text;
            string password = textBox1.Text;

            checkLoginAndPassword();
            if (authorized)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить все результаты, сохраненные под этой учетной записью?", "Удаление результатов", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {              
                    try
                    {
                        if (File.Exists(Application.StartupPath + @"\data.csv"))
                        {
                            string[] lines = File.ReadAllLines(Application.StartupPath + @"\data.csv", Encoding.Default);
                            IEnumerable<string> remainingLines = lines.Where(x => !x.Contains(textBox2.Text));
                            string[] newlines = remainingLines.ToArray();
                            File.WriteAllLines(Application.StartupPath + @"\data.csv", newlines);

                            MessageBox.Show("Все результаты удалены!", "Удаление результатов", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else MessageBox.Show("Нет данных для удаления!", "Удаление результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
        }

        private void btn_delAllFromDG_Click(object sender, EventArgs e)
        {
            delAllFromDG();
            if (authorized) dataGridView1.Rows.Clear();
        }

        private void btn_delFromDG_Click(object sender, EventArgs e)
        {
            bool exeption = false;

            string user = textBox2.Text;
            string password = textBox1.Text;
            checkLoginAndPassword();

            if (authorized)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить выделенные результаты, сохраненные под этой учетной записью?", "Удаление результатов", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        int index = dataGridView1.SelectedCells[0].RowIndex;
                        dataGridView1.Rows.RemoveAt(index);
                    }
                    catch (Exception)
                    {
                        exeption = true;
                        MessageBox.Show("Удаление пустой строки невозможно! \nНет данных для удаления.", "Удаление результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }

                    if (!exeption)
                    {
                        try
                        {
                            if (File.Exists(Application.StartupPath + @"\data.csv"))
                            {
                                string[] lines = File.ReadAllLines(Application.StartupPath + @"\data.csv", Encoding.Default);
                                IEnumerable<string> remainingLines = lines.Where(x => !x.Contains(textBox2.Text));

                                string[] newlines = remainingLines.ToArray();
                                File.WriteAllLines(Application.StartupPath + @"\data.csv", newlines);
                            }

                            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + @"\data.csv", true))
                            {
                                FileInfo fileInfo = new FileInfo(Application.StartupPath + @"\data.csv");

                                if (fileInfo.Length == 0)
                                {
                                    writer.WriteLine(@"""user"";""IBU"";""mark"";""date""");
                                }


                                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                                {
                                    writer.Write('\"' + textBox2.Text + '\"' + ';');
                                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                                    {

                                        writer.Write('\"' + dataGridView1.Rows[i].Cells[j].Value.ToString() + '\"' + ';');
                                    }
                                    writer.Write('\n');
                                }
                            }
                            MessageBox.Show("Выделенные результаты удалены!", "Удаление результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
        }
    }
}
