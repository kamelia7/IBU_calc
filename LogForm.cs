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
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_manageAc_Click(object sender, EventArgs e)
        {
            if (rb_create.Checked)
            {
                logInCSV();
                this.Close();
            }
            else
            {
                if (rb_delete.Checked)
                {
                    deleteFromCSV();
                    this.Close();
                }
            }
            
        }
        
        void logInCSV()
        {
           try
           {
               if ((string.IsNullOrEmpty(textBox1.Text.Trim()) || string.IsNullOrEmpty(textBox2.Text.Trim()) || string.IsNullOrEmpty(textBox3.Text.Trim())))
                   MessageBox.Show("Некорректно введены данные!", "Создание учетной записи", MessageBoxButtons.OK, MessageBoxIcon.None);

                else if (File.Exists(Application.StartupPath + @"\users.csv") && File.ReadAllText(Application.StartupPath + @"\users.csv").Contains(textBox1.Text))
                        MessageBox.Show("Такое имя пользователя уже есть.\nПридумайте другое.", "Создание учетной записи", MessageBoxButtons.OK, MessageBoxIcon.None);

               else if (textBox2.Text != textBox3.Text)
                   MessageBox.Show("Набранные пароли не совпадают. Повторите попытку.", "Создание учетной записи", MessageBoxButtons.OK, MessageBoxIcon.None);

                else
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + @"\users.csv", true, Encoding.GetEncoding(1251)))
                    {
                        FileInfo fileInfo = new FileInfo(Application.StartupPath + @"\users.csv");

                        if (fileInfo.Length == 0)
                        {
                            writer.WriteLine(@"""user"";""password""");
                        }
                            writer.WriteLine(@"""" + textBox1.Text + @""";""" + textBox2.Text + @"""");
                            MessageBox.Show("Новая учетная запись успешно создана!", "Создание учетной записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
         }

        void deleteFromCSV()
        {
            
                bool authorized = false;  

                string user = textBox1.Text;
                string password = textBox2.Text;

                if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите логин и пароль для удаления учетной записи!", "Удаление учетной записи", MessageBoxButtons.OK, MessageBoxIcon.None);
                    textBox1.Focus();
                }
                else if (string.IsNullOrEmpty(textBox2.Text.Trim()) || string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Некорректно введены данные!", "Удаление учетной записи", MessageBoxButtons.OK, MessageBoxIcon.None);
                    textBox1.Focus();
                }
                else
                {
                    try
                    {
                        if (MessageBox.Show("Вы уверены, что хотите удалить эту учетную запись и все сохраненные под ней результаты?", "Удаление учетной записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (File.Exists(Application.StartupPath + @"\users.csv"))
                            {
                                string[] lines = File.ReadAllLines(Application.StartupPath + @"\users.csv", Encoding.Default);

                                foreach (string line in lines)
                                {
                                    if (line.Contains(user) && line.Contains(password) && (line.Length - (user.Length + password.Length) < 6))
                                    {
                                        authorized = true;
                                        break;
                                    }
                                }

                                if (authorized == true)
                                {
                                    IEnumerable<string> remainingLines = lines.Where(x => !x.Contains(textBox1.Text));
                                    string[] newlines = remainingLines.ToArray();
                                    File.WriteAllLines(Application.StartupPath + @"\users.csv", newlines);

                                    //удаляем всю инфу из data.csv
                                    if (File.Exists(Application.StartupPath + @"\data.csv"))
                                    {
                                        string[] lines1 = File.ReadAllLines(Application.StartupPath + @"\data.csv", Encoding.Default);
                                        IEnumerable<string> remainingLines1 = lines1.Where(x => !x.Contains(textBox1.Text));
                                        string[] newlines1 = remainingLines1.ToArray();
                                        File.WriteAllLines(Application.StartupPath + @"\data.csv", newlines1);
                                    }

                                    MessageBox.Show("Учетная запись и все ее данные успешно удалены!", "Удаление учетной записи", MessageBoxButtons.OK,MessageBoxIcon.Information);
                                }
                             
                                else MessageBox.Show("Данной учетной записи не существует!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);

                            }
                            else MessageBox.Show("Данной учетной записи не существует!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
        }

        private void rb_create_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
            lbl_title.Text = "Создание учетной записи";
            label4.Visible = true;
            label1.Text = "Придумайте имя пользователя*";
            btn_manageAc.Text = "Создать учетную запись";
            label3.Visible = true;
            textBox3.Visible = true;
        }

        private void rb_delete_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
            lbl_title.Text = "Удаление учетной записи";
            label4.Visible = false;
            label1.Text = "Имя пользователя";
            btn_manageAc.Text = "Удалить учетную запись";
            label3.Visible = false;
            textBox3.Visible = false;
        }
    }
}