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
    public partial class FormData : Form
    {
        public FormData()
        {
            InitializeComponent();
            timer1.Start();
        }
        double irtifa=0;
        private void Timer1_Tick(object sender, EventArgs e)//timer nesnesi her tick olduğunda, interval 100 olarak seçilmiştir
        {
            try//hata yakalanması için
            {
                if (Form1.irtifa != 0)//form1 de irtifa değeri varsa
                {
                    labelVeriDurum.Text = "Anlık Olarak okunuyor";
                    labelEnlem.Text = Form1.enlem.ToString();//değerler ilgili label yazdırılıyor
                    labelBoylam.Text = Form1.boylam.ToString();//değerler ilgili label yazdırılıyor
                    labelUydu.Text = Form1.bagli_uydu.ToString();//değerler ilgili label yazdırılıyor
                    irtifa = Form1.irtifa;

                    irtifa = irtifa - Form1.ref_irtifa;//anlık irtifa değerinden ilk irtifa değeri çıkarılarak gerçek yerden yükseklik hesaplanıyor
                    if (irtifa <= 0)//irtifa negatifse
                        irtifa = 0;
                    irtifa = Math.Round(irtifa, 2);//yuvarlama fonksiyonu
                    labelIrtifa.Text = (irtifa).ToString() + " m";
                    labelHiz.Text = Form1.hiz.ToString()+ " km/h";
                    labelRefIrtifa.Text = Form1.ref_irtifa.ToString() + " m";
                }
                else
                {
                    labelVeriDurum.Text = "Bekleniyor...";//veri gelmiyorsa
                }
            }

            catch (Exception ex)
            {
            }

        }

    }
}
