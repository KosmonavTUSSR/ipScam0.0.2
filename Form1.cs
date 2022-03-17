using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ipScan
{

    public partial class Form1 : Form
    {
        readonly string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string ipListSec;
        public string ipList;
        public string locIp;
        public string hostName;
        public string webIp;
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
            timer1.Stop();
            timer1.Start();
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
       
        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            button2.Enabled = false;
            button3.Enabled = false;
            label1.Text = "Данные удалены";
            label1.ForeColor = Color.FromArgb(255,13,0);
            timer1.Stop();
            timer1.Start();
        }
        //Получение ip - new WebClient().DownloadString("http://ipinfo.io/ip")
        //Добавить получение mac
        //Добавить проверку инетрнета
        public void MyIp()
        {
            hostName = System.Net.Dns.GetHostName();
            label4.Text = "Host - " + hostName;
            System.Net.IPAddress ip = Dns.GetHostEntry(hostName).AddressList[1];
            locIp = ip.ToString();
            label2.Text = "Local - " + locIp;
            webIp = new WebClient().DownloadString("http://ipinfo.io/ip");
            label3.Text = "Web - " + webIp;
        }
        public void SaveFile()
        {
            if (textBox1.Text != "" || textBox2.Text != "")
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
                            label1.Text = "Данные сохранены";
                            label1.ForeColor = Color.LimeGreen;
                            timer1.Stop();
                            timer1.Start();
                        }
                        else {
                            label1.Text = "Операция отменена";
                            label1.ForeColor = Color.FromArgb(255,13,0);
                            timer1.Stop();
                            timer1.Start();
                        }
                    }
                    else
                    {
                        File.WriteAllText($@"{savePath}\ipList.txt", textBox1.Text + textBox2.Text);
                        label1.Text = "Данные сохранены";
                        label1.ForeColor = Color.LimeGreen;
                        timer1.Stop();
                        timer1.Start();
                    }
                }else{
                    label1.Text = "Операция отменена";
                    label1.ForeColor = Color.FromArgb(255, 13, 0);
                    timer1.Stop();
                    timer1.Start();
                    }
            } else {
                label1.Text = "Нет данных для сохранения";
                label1.ForeColor = Color.FromArgb(255,13,0);
                timer1.Stop();
                timer1.Start();
            }
        }
        public void Comparison()
        {
            if (textBox1.Text == textBox2.Text)
            {
                label1.ForeColor = Color.LimeGreen;
                label1.Text = "Изменений не обнаружено";
            }
            else if (textBox2.Text == "")
            {
                label1.Text = "";
            }
            else
            {
                label1.ForeColor = Color.FromArgb(192, 0, 0);
                label1.Text = "Данные не совпадают";

            }
            timer1.Stop();
            timer1.Start();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Start();
            Comparison();
            int count = textBox1.Lines.Length - 4;
            label5.Text = count.ToString();
        }
        private void Button2_Click(object sender, EventArgs e)
        {   
            StartSec();
            Comparison();
        }
        //очистка окон
        private void Button3_Click(object sender, EventArgs e)
        {
            Clear();
        }
        //Очистка кэша и окон
        private void Button4_Click(object sender, EventArgs e)
        {
            ClearCash();
        }
        // "Сохранить"
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
        // Мой ip
        private void Button6_Click(object sender, EventArgs e)
        {
            MyIp();
        }
        private void Label2_Click(object sender, EventArgs e)
        {
            if (label2.Text != "") {
                Clipboard.SetText(locIp);
                label1.Text = "Ip Address скопирован";
                label2.ForeColor = Color.LimeGreen;
                
            }
            timer1.Stop();
            timer1.Start();
        }
        private void Label3_Click(object sender, EventArgs e)
        {
            if (label3.Text != "")
            {
                Clipboard.SetText(webIp);
                label1.Text = "Web Ip Address скопирован";
                label3.ForeColor = Color.LimeGreen;
            }
            timer1.Stop();
            timer1.Start();
        }
        private void Label4_Click(object sender, EventArgs e)
        {
            if (label4.Text != "")
            {
                Clipboard.SetText(hostName);
                label1.Text = "Host Name скопирован";
                label4.ForeColor = Color.LimeGreen;
            }
            timer1.Stop();
            timer1.Start();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process clear = new Process();
            clear.StartInfo.FileName = "netsh";
            clear.StartInfo.Arguments = "interface ip delete arpcache";
            clear.StartInfo.UseShellExecute = false;
            clear.StartInfo.CreateNoWindow = true;
            clear.Start();
        }
    }
}
