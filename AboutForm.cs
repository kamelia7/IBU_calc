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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + @"\readMeIBU.txt"))
            {
                string[] textt = File.ReadAllLines(Application.StartupPath + @"\readMeIBU.txt", Encoding.GetEncoding(1251));
                foreach (string line in textt)
                {
                    listBox1.Items.Add(line);
                }
            }
            else MessageBox.Show("Файл readMeIBU.txt с информацией о программе отсутствует в папке с исполняемым файлом!");
        }    
    }
}
