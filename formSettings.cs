using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Simurg_Arayuz
{
    public partial class formSettings : Form
    {
        string[] ports = SerialPort.GetPortNames();
        public formSettings()
        {
            InitializeComponent();
            
        }

        private void FormSettings_Load(object sender, EventArgs e)//form yüklenince
        {
            foreach(string port in ports)//tüm portlar comboboxa kaydedilir
            {
                comboBoxPort.Items.Add(port);
            }
            comboBoxBaud.Items.Add("2400");//baud değerleri eklenir
            comboBoxBaud.Items.Add("4800");//baud değerleri eklenir
            comboBoxBaud.Items.Add("9600");//baud değerleri eklenir
            comboBoxBaud.Items.Add("19200");//baud değerleri eklenir
            comboBoxBaud.Items.Add("115200");//baud değerleri eklenir
           // comboBoxBaud.SelectedIndex = 2;
            //comboBoxPort.SelectedIndex = 0;

        }

        private void Button1_Click(object sender, EventArgs e)//kaydet butonu
        {
            string a = comboBoxBaud.Text;
            int b = Convert.ToInt16(a);
            Form1.port_set(comboBoxPort.Text, b);//form1 deki port set fonksiyonuna gerekli veriler gönderilir
        }

       
    }
}
