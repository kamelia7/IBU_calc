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
    public partial class IBUform : Form
    {
        private int mark = 0;
        private string user;
        private string password;

        public string User 
        {
            get
            {
                return user;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

        public IBUform()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox1.Text.Trim())))
            this.label1.Visible = false;
            else
                this.label1.Visible = true;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox2.Text.Trim())))
                this.label2.Visible = false;
            else
                this.label2.Visible = true;
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox3.Text.Trim())))
                this.label3.Visible = false;
            else
                this.label3.Visible = true;
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox4.Text.Trim())))
                this.label4.Visible = false;
            else
                this.label4.Visible = true;
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox5.Text.Trim())))
                this.label5.Visible = false;
            else
                this.label5.Visible = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox6.Text.Trim())))
                this.label6.Visible = false;
            else
                this.label6.Visible = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox7.Text.Trim())))
                this.label8.Visible = false;
            else
                this.label8.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            textBox3.Focus();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            textBox5.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            textBox6.Focus();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            textBox7.Focus();
        }

        private void btn_calc_Click(object sender, EventArgs e)
        {
            try
            {
                double wortSugarPercent = Convert.ToDouble(textBox1.Text);
                double wortBoilingTime = Convert.ToDouble(textBox2.Text);
                double hopAlphaAcidsPercent = Convert.ToDouble(textBox3.Text);
                double hopWeight = Convert.ToDouble(textBox4.Text);
                double drinkVolume = Convert.ToDouble(textBox5.Text);

                if (wortSugarPercent > 0 && wortBoilingTime > 0 && hopAlphaAcidsPercent > 0 && hopWeight > 0 && drinkVolume > 0)
                {
                    Drink drink = new Drink(wortSugarPercent, wortBoilingTime, hopAlphaAcidsPercent, hopWeight, drinkVolume);
                    double IBU = drink.calculateIBU();
                    lbl_IBU.Text = Math.Round(IBU, 2).ToString() + " IBU";
                }
                else if (wortSugarPercent < 0 || wortBoilingTime < 0 || hopAlphaAcidsPercent < 0 || hopWeight < 0 || drinkVolume < 0)
                {
                    lbl_IBU.Text = "0,00 IBU";
                    MessageBox.Show("Введите числа больше нуля!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    lbl_IBU.Text = "0,00 IBU";
                    MessageBox.Show("Некорректный ввод данных!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            catch (Exception)
            {
                lbl_IBU.Text = "0,00 IBU";
                MessageBox.Show("Некорректный ввод данных!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void piсBox_mark_MouseEnter(object sender, EventArgs e)
        {
            piсBox_mark.Image = IBUcalc.Properties.Resources.Thumb_up_icon_3_svg;
        }

        private void piсBox_mark_MouseLeave(object sender, EventArgs e)
        {
            piсBox_mark.Image = IBUcalc.Properties.Resources.Thumb_up_icon_1_svg;
        }

        private void piсBox_mark_Click(object sender, EventArgs e)
        {
            if (lbl_IBU.Text != "0,00 IBU")
            {
                mark += 1;
                if (mark == 6) mark = 0;
                lbl_mark.Text = mark.ToString();
            }
            else
                MessageBox.Show("Нет данных для оценки!", "Оценка результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void saveData()
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + @"\data.csv", true))
                {
                    FileInfo fileInfo = new FileInfo(Application.StartupPath + @"\data.csv");

                    if (fileInfo.Length == 0)
                    {
                        writer.WriteLine(@"""user"";""IBU"";""mark"";""date""");
                    }
                    if (lbl_IBU.Text != "0,00 IBU")
                    {
                        writer.WriteLine(@"""" + user + @""";""" + lbl_IBU.Text.Remove(lbl_IBU.Text.Length - 4) + @""";""" + lbl_mark.Text + @""";""" + Convert.ToString(date) + @"""");
                        MessageBox.Show("Результаты успешно сохранены!", "Сохранение результатов", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else MessageBox.Show("Нет данных для сохранения!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                } 
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nЗакройте файл с базой данных на время доступа к нему других процессов.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            bool authorized = false;
            user = textBox7.Text;
            password = textBox6.Text;
                    
            if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите свой логин и пароль для сохранения результатов!", "Сохранение результатов", MessageBoxButtons.OK, MessageBoxIcon.None);
                textBox7.Focus();
            }
            else if (string.IsNullOrEmpty(user.Trim()) || string.IsNullOrEmpty(password.Trim()))
            {
                MessageBox.Show("Некорректно введены данные!", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.None);
                textBox7.Focus();
            }
            else
            {
                try
                {
                    if (File.Exists(Application.StartupPath + @"\users.csv"))
                    {
                        foreach (string line in File.ReadAllLines(Application.StartupPath + @"\users.csv"))
                        {
                            if (line.Contains(user) && line.Contains(password) && (line.Length - (user.Length + password.Length) < 6))
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
             if (authorized) saveData();
        }

        private void btn_new_user_Click(object sender, EventArgs e)
        {
            LogForm logForm = new LogForm();
            logForm.Show();
        }

        private void btn_pre_Click(object sender, EventArgs e)
        {
            user = textBox7.Text;
            password = textBox6.Text;
            PreResultsForm preForm = new PreResultsForm(this);
            preForm.Show();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }
    }
}