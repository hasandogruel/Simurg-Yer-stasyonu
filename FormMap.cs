using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simurg_Arayuz
{
    public partial class FormMap : Form
    {
        public FormMap()
        {
            InitializeComponent();
            buttonGenel.Enabled = false;

        }

        double hassasiyet = 50;
        int alinan_veri = 0;
        int gecici = 0;

        private void FormMap_Load(object sender, EventArgs e)//form yüklenince
        {
            hassasiyet = 100 - hassasiyet;//slider nesnesinden gelen değerden 100 çıkarılır
            hassasiyet /= 1700;//haritaya yakın bakmak için 
            hassasiyet = Math.Round(hassasiyet, 5);//yuvarlama
            timer_chart.Start();//timer nesnesi başlatılıyor
        }

        private void ButtonAnlik_Click(object sender, EventArgs e)//anlık takip butonu
        {
            buttonAnlik.Enabled = false;
            buttonGenel.Enabled = true;
        }

        private void ButtonGenel_Click(object sender, EventArgs e)//genel takip butonu
        {
            buttonGenel.Enabled = false;
            buttonAnlik.Enabled = true;
        }


        private void BunifuSlider1_ValueChanged(object sender, EventArgs e)//sliderda veri değişirse yani hassasiyet değeri değiştirilse
        {
            if (bunifuSlider1.Value == 0)
                bunifuSlider1.Value = 1;
            hassasiyet = bunifuSlider1.Value;
            hassasiyet = 100 - hassasiyet;
            hassasiyet /= 1700;
            hassasiyet = Math.Round(hassasiyet, 5);
        }

        private void Timer_chart_Tick(object sender, EventArgs e)
        {
            try
            {

                if (Form1.enlemList.Count <= 0)
                {
                    return;
                }

                if (buttonGenel.Enabled == false && Form1.enlemList.Count > 0)
                {
                    chart1.ChartAreas[0].AxisX.Minimum = Form1.enlemList[0] - hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisX.Maximum = Form1.enlemList[0] + hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisY.Minimum = Form1.boylamList[0] - hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisY.Maximum = Form1.boylamList[0] + hassasiyet;//grafik ayarları
                }

                else if (buttonAnlik.Enabled == false)
                {
                    chart1.ChartAreas[0].AxisX.Minimum = Form1.enlemList[Form1.enlemList.Count - 1] - hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisX.Maximum = Form1.enlemList[Form1.enlemList.Count - 1] + hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisY.Minimum = Form1.boylamList[Form1.boylamList.Count - 1] - hassasiyet;//grafik ayarları
                    chart1.ChartAreas[0].AxisY.Maximum = Form1.boylamList[Form1.boylamList.Count - 1] + hassasiyet;//grafik ayarları

                }

                for (int i = 0; i < Form1.enlemList.Count() - alinan_veri; i++, gecici = Form1.enlemList.Count)
                {
                    chart1.Series[0].Points.AddXY(Form1.enlemList[i + alinan_veri], Form1.boylamList[i + alinan_veri]);//grafik ayarları
                }
                alinan_veri = gecici;

                chart1.SaveImage(Application.StartupPath + "\\sonmap_roket.png", System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);//grafiğin her an png olarak kaydedilir
            }

            catch (Exception ex)
            {

            }
        }
    }
}
