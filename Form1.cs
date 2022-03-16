using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ipScan
{

    public partial class Form1 : Form
    {
        readonly string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string ipListSec;
        public string ipList;
        static public string ipAdr;
        static public string macAdr;
        public Form1()
         
        {
            InitializeComponent();
        }
        public void Start()
        {
            Process scan = new Process();
            scan.StartInfo.FileName = "arp";
            scan.StartInfo.Arguments = "-a";
            scan.StartInfo.RedirectStandardOutput = true;
            scan.StartInfo.UseShellExecute = false;
            scan.StartInfo.CreateNoWindow = true;
            scan.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            scan.Start();
            ipList = scan.StandardOutput.ReadToEnd();
            textBox1.Text = ipList;
            button2.Enabled = true; //Scam+
            button3.Enabled = true; //Clear
        }
        public void ClearCash()
        {
            if (checkBox1.Checked)
            {
                Process clear = new Process();
                clear.StartInfo.FileName = "netsh";
                clear.StartInfo.Arguments = "interface ip delete arpcache";
                clear.StartInfo.UseShellExecute = false;
                clear.StartInfo.CreateNoWindow = true;
                clear.Start();
            }
            if (checkBox2.Checked)
            {
                textBox1.Text = "";
            }
            if (checkBox3.Checked)
            {
                textBox2.Text = "";
            }
            Comparison();
        }
        public void StartSec()
        {
            Process process = new Process();
            process.StartInfo.FileName = "arp";
            process.StartInfo.Arguments = "-a";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            ipListSec = process.StandardOutput.ReadToEnd();
            textBox2.Text = ipListSec;
        }
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            button2.Enabled = false;
            label2.Text = "Данные удалены";
        }
        public void SaveFile()
        {
            if (textBox1.Text != "")
            {
                DialogResult result = MessageBox.Show("Ваш результат будет сохранен в файл", "Сохранение",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    File.Exists($@"{savePath}\ipList.txt");
                    if (File.Exists($@"{savePath}\ipList.txt"))
                    {
                        DialogResult result2 = MessageBox.Show("ipList.txt уже существует \n Заменить?", "Сохранение",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result2 == DialogResult.Yes)
                        {
                            File.WriteAllText($@"{savePath}\ipList.txt", textBox1.Text + textBox2.Text);
                        }
                        else { }
                    }
                    else
                    {
                        File.WriteAllText($@"{savePath}\ipList.txt", textBox1.Text + textBox2.Text);
                    }
                }
            }
        }
        public void Comparison()
        {
            if (textBox1.Text == textBox2.Text)
            {
                label2.ForeColor = Color.LimeGreen;
                label2.Text = "Изменений не обнаружено";
            }
            else if (textBox2.Text == "")
            {
                label2.Text = "";
            }
            else
            {
                label2.ForeColor = Color.Firebrick;
                label2.Text = "Данные не совпадают";
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Start();
            Comparison();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            StartSec();
            Comparison();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            Clear();
            
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            ClearCash();
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        // Cкролбары
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Size sz = TextRenderer.MeasureText(textBox1.Text, Font);
            textBox1.ScrollBars = sz.Height > textBox1.Height ? ScrollBars.Vertical : ScrollBars.None;
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            Size sz = TextRenderer.MeasureText(textBox2.Text, Font);
            textBox2.ScrollBars = sz.Height > textBox2.Height ? ScrollBars.Vertical : ScrollBars.None;
        }
    }
}
