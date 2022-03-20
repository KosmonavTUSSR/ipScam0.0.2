using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
        public static string ipAdr;
        public static string macAdr;
   
        string er404 = "404 - LOL KEK";
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
        }
        public void scanImm()
        {
            Start();

        }
        public void ClearCash()
        {
            Process clear = new Process();
            clear.StartInfo.FileName = "netsh";
            clear.StartInfo.Arguments = "interface ip delete arpcache";
            clear.StartInfo.UseShellExecute = false;
            clear.StartInfo.CreateNoWindow = true;
            clear.Start();
        }

        public void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            button2.Enabled = false;
            button3.Enabled = false;
            label1.Text = "Данные удалены";
            label1.ForeColor = Color.FromArgb(255, 13, 0);
        }
        //Получение ip - new WebClient().DownloadString("http://ipinfo.io/ip")
        //Поправить регулярыне выражение на :
        public void MyIp()
        {
            hostName = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = Dns.GetHostEntry(hostName).AddressList[1];
            locIp = ip.ToString();
            try
            {
                webIp = new WebClient().DownloadString("http://ipinfo.io/ip");
                label3.Text = "Web - " + webIp;
            }
            catch
            {
                label3.Text = er404;
            }
            label4.Text = "Host - " + hostName;
            label2.Text = "Local - " + locIp;
            string GetMACAddress()
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                string macAdr = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (macAdr == string.Empty)//   Считывает только первую строку mac
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        macAdr = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return macAdr;
            }
            string oldmacAdr = GetMACAddress();
            macAdr = Regex.Replace(oldmacAdr, ".{2}", "$0:");

            label6.Text = macAdr;
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
                        DialogResult result2 = MessageBox.Show("Такой файл уже существует \n Заменить?", "Ошибка",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result2 == DialogResult.Yes)
                        {
                            File.WriteAllText($@"{savePath}\ipList.txt", textBox1.Text + textBox2.Text);
                            label1.Text = "Данные сохранены";
                            label1.ForeColor = Color.LimeGreen;
                        }
                        else
                        {
                            label1.Text = "Операция отменена";
                            label1.ForeColor = Color.FromArgb(255, 13, 0);
                        }
                    }
                    else
                    {
                        File.WriteAllText($@"{savePath}\ipList.txt", textBox1.Text + textBox2.Text);
                        label1.Text = "Данные сохранены";
                        label1.ForeColor = Color.LimeGreen;
                    }
                }
                else
                {
                    label1.Text = "Операция отменена";
                    label1.ForeColor = Color.FromArgb(255, 13, 0);
                }
            }
            else
            {
                label1.Text = "Нет данных для сохранения";
                label1.ForeColor = Color.FromArgb(255, 13, 0);
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
            if (textBox1.Text != "" )
            {
                button7.Visible = true;
            }
            
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
            label1.Text = "ARP cash очищен";
        }
        // "Сохранить"
        private void Button5_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            MyIp();
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
        
        private void label2_Click(object sender, EventArgs e)
        {
            if (label2.Text != "")
            {
                Clipboard.SetText(locIp);
                label1.Text = "Ip Address скопирован";
                label2.ForeColor = Color.LimeGreen;
                label1.ForeColor = Color.LimeGreen;

            }
        }
        private void label3_Click(object sender, EventArgs e)
        {
            if (label3.Text != "")
            {
                try
                {
                    Clipboard.SetText(webIp);
                    label1.Text = "Web Ip Address скопирован";
                    label3.ForeColor = Color.LimeGreen;
                    label1.ForeColor = Color.LimeGreen;
                }
                catch
                {
                    Clipboard.SetText(er404);
                    label1.Text = "Зачем?";
                    label3.ForeColor = Color.LimeGreen;
                    label1.ForeColor = Color.LimeGreen;
                }
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            if (label4.Text != "")
            {
                Clipboard.SetText(hostName);
                label1.Text = "Host Name скопирован";
                label4.ForeColor = Color.LimeGreen;
                label1.ForeColor = Color.LimeGreen;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "";
        }
       
        private void label6_Click(object sender, EventArgs e)
        {
            if (label6.Text != "")
            {
                Clipboard.SetText(macAdr);
                label1.Text = "MAC-адрес скопирован";
                label6.ForeColor = Color.LimeGreen;
                label1.ForeColor = Color.LimeGreen;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label1.Text = "Главный блок очищен";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            label1.Text = "Блок очищен";
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

    }
}
