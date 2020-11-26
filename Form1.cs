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
using System.Windows.Forms.DataVisualization.Charting;

namespace Simurg_Arayuz
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            Gizle();//menü göstergelerinin gizlenmesi
            Panel_container_form_ekle(new formSettings());//ana forma ayarlar pencerisinin yüklenmesi
            onSettings.Show();//menüdeki ayarlar kısmının göstergesi
            


        }
        StreamWriter SW = File.AppendText(Application.StartupPath + "\\veriler_roket.txt");//gelen verilerin kaydedilmesi için oluşturulan txt dosyası
        private void Form1_Load(object sender, EventArgs e)
        {
            enlemList = new List<double>();//enlem listesi
            boylamList = new List<double>();//boylam listesi
            SW.WriteLine("Program Başlama Tarihi: " + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"));//txt dosyasına programın başlama tarihi yazdırılıyor
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)//form kapatılırken
        {
            
            if(SerialPort1.IsOpen==false)//port açıksa
            {
                SerialPort1.Close();//açık olan port kapanıyor
            }
            if(enlemList.Count>0)
            {
                double gecici = 0;
                gecici = enlemList[enlemList.Count - 1] - enlemList[0];//son elem ile ilk enlem değerlerinin farkı alınıyor
                gecici *= 86;//1 enlem değeri türkiye de yaklaşık olarak 86 km denk geldiği için bu değerle çarpıldı
                SW.WriteLine("");
                gecici = Math.Round(gecici, 3);//yuvaralama işlemi
                if (gecici < 0)//değer negatif ise
                {
                    gecici *= -1;//pozitife çevrilir
                    SW.WriteLine("Roketiniz  " + gecici.ToString() + "km Güneye,");//güneye düştüğünün bilgisi txt dosyasına yazılır

                }
                else//pozitifse
                {
                    SW.WriteLine("Roketiniz  " + gecici.ToString() + "km Kuzeye,");//kuzeye düştüğünün bilgisi txt dosyasına yazılır
                }
                gecici = boylamList[boylamList.Count - 1] - boylamList[0];//son boylam ile ilk boylam değerlerinin farkı alınıyor
                gecici *= 111;//1 boylam değeri 111 km denk geldiği için bu değerle çarpıldı
                gecici = Math.Round(gecici, 3);//yuvaralama işlemi
                if (gecici < 0)//değer negatif ise
                {
                    gecici *= -1;//pozitife çevrilir
                    SW.WriteLine("Roketiniz  " + gecici.ToString() + "km Batıya Düşmüştür.");//batıya düştüğünün bilgisi txt dosyasına yazılır
                }
                else//pozitifse
                {
                    SW.WriteLine("Roketiniz  " + gecici.ToString() + "km Doğuya Düşmüştür.");//doğuya düştüğünün bilgisi txt dosyasına yazılır
                }
            }
            
            SW.WriteLine("------------------------------------");
            SW.WriteLine("Program Kapatılma Tarihi: " + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"));//program kapatılma anı txt dosyasına yazılır
            SW.Close();//txt doysası kapatılır
        }

        static string port; static int braud; int a = 0;//ayarlar için olan değişkenler
        static Boolean cont_set = false;//portun açıklığını kontrol eden boolen değişken
        char[] yildiz = { '*' };
        public static double enlem = 0, boylam = 0, ref_irtifa = 0, irtifa = 0;//enlem,boylam,ilk irtifa,anlık irtifa değerleri için olan değişkenler
        public static string hiz, bagli_uydu;//hız,bağlı uydu sayısı değerleri için kullanılacak olan değişkenler
        public static List<double> enlemList;//enlemleri tutacak olan liste
        public static List<double> boylamList;//boylamları tutacak olan liste






        SerialPort SerialPort1 = new SerialPort();//seri port nesnesi üretiliyor

        public static void port_set(string G_port, int G_baud)//ayarlar kısmından kullanılacak olan fonksiyon
        {
            port = G_port;//gelen değer port değişkenine aktarılıyor
            braud = G_baud;//gelen değer braud değişkenine aktarılıyor
            cont_set = true;//ayarların yapıldığını belli etmek için kontrol değişkeni true yapılıyor
            
        }

       


        private void port_Ac()//ayarlar kısmında port ayarları yapıldıktan sonra gelen verileri okumak için port açılır
        {
            if(SerialPort1.PortName!=port || SerialPort1.BaudRate!=braud)//eğer işlem yapıcalak port aynı değilse veya baudrate değeride aynı değilse
            {
                if(SerialPort1.IsOpen==false)//port açık değilse
                {
                    SerialPort1.PortName = port;//port ayarı
                    SerialPort1.BaudRate = braud;//baudrate ayarı
                    SerialPort1.Open();//port açılır
                }
                else//port ayarları farklı ise yeni port ayarları atanması için
                {
                    SerialPort1.Close();//port kapatılır
                    SerialPort1.PortName = port;//yeni port ayarı
                    SerialPort1.BaudRate = braud;//yeni baudrate ayarı
                    SerialPort1.Open();//port açılır
                }
                
            }
        }
        
        

        public void Gizle()//menülerin yanındaki belirteçlerin gösterilmemesi
        {
            onData.Hide();//tüm belirteçler tek tek kapanır
            onInfo.Hide();//tüm belirteçler tek tek kapanır
            onMap.Hide();//tüm belirteçler tek tek kapanır
            onSettings.Hide();//tüm belirteçler tek tek kapanır

        }
        
        private void port_closed()//formun ayarlar kısmına kapanmadan önce bu fonksiyon eklendi
        {
            if(cont_set==true)//eğer kontrol şartı true ise
            {
                if (SerialPort1.IsOpen == true)//port açıksa 
                {
                    SerialPort1.Close();//port kapanır
                    timer1.Stop();//timer kapanır
                }
            }
        }

        private void Panel_container_form_ekle(object form_gelen)//formda menüye göre değişecek olan kısım
        {
            if (this.panelContainer.Controls.Count > 0)
                this.panelContainer.Controls.RemoveAt(0);

            Form fh = form_gelen as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panelContainer.Controls.Add(fh);
            this.panelContainer.Tag = fh;
            fh.Show();
        }


       

        private void PictureBox1_Click(object sender, EventArgs e)//kapatma butonu görsellik açısından fotoğraf arkasına yazılmıştır
        {
            port_closed();//port kapatma fonksiyonu
            Application.Exit();//uygulamayı kapat

        }

        private void BtnSettings_Click(object sender, EventArgs e)//menüden ayarlar butonuna basıldığında
        {
            Gizle();//tüm belirteçler gizlenir
            onSettings.Show();//ayarlar belirteci açılır
            Panel_container_form_ekle(new formSettings());//sağ kısımdaki panele ayarlar formu eklenir
            
        }


        private void ButtonData_Click(object sender, EventArgs e)//menüden veri akışı butonuna basıldığında
        {
            Gizle();
            onData.Show();
            Panel_container_form_ekle(new FormData());
        }

        private void Button2_Click(object sender, EventArgs e)//menüden harita butonuna basıldığında
        {
            Gizle();
            onMap.Show();
            Panel_container_form_ekle(new FormMap());
        }


        private void Button3_Click(object sender, EventArgs e)//menüden hakkında butonuna basıldığında
        {
            Gizle();
            onInfo.Show();
            Panel_container_form_ekle(new FormInfo());
        }

        public void Veri_yaz(string yaz)//txt dosyasına veri yazan fonksiyon
        {
            SW.WriteLine( DateTime.Now.ToString("MM/dd/yyyy H:mm:ss")+"     "+yaz);//tarih ve saatin arkasına istenilen veriyi yazar
        }

        public void Timer1_Tick(object sender, EventArgs e)//timer nesnesi her tick olduğunda, interval 100 olarak seçilmiştir
        {
            try//hata olması durumunda yakalanması için
            {
                
                if (cont_set == true)//ayarlar yapılmışsa
                {
                    port_Ac();//port açılır
                    
                    
                    string gelen = SerialPort1.ReadExisting();//seri porta gelen metin gelen nesnesine aktarılır
                    
                    gelen = gelen.Replace("*", "");//yıldızı kaldırmak
                    gelen = gelen.Trim();//boşlukları kaldıran fonksiyon
                    gelen = gelen.TrimStart();//baştaki boşlukları kaldıran fonksiyon
                    gelen = gelen.TrimEnd();//sondaki boşlukları kaldıran fonksiyon

                    if (-1!=gelen.IndexOf("Lt:"))//gelen stringte Lt: değeri varsa
                    {
                        Veri_yaz(gelen);//txt dosyasına gelen veri yazılır
                        string temizle = "Lt:";
                        gelen = gelen.Replace(".", "");//işlem yapılması için nokta kaldırıldı
                        enlem = Convert.ToDouble(gelen.Trim(temizle.ToCharArray()));//gelen değer enlem değişkenine aktarılır
                        enlem /= 1000000;//gerçek enlem değeri

                        if(boylam!=0 && enlem>1 && boylam>1)//enlem ve boylam boş değilse
                        {
                            enlemList.Add(enlem);//listeye eklenir
                            boylamList.Add(boylam);//listeye eklenir
                        }
                    }

                    else if (-1 != gelen.IndexOf("Lg:"))//gelen stringte Lg: değeri varsa
                    {
                        Veri_yaz(gelen);//txt dosyasına gelen veri yazılır
                        string temizle = "Lg:";
                        gelen = gelen.Replace(".", "");//işlem yapılması için nokta kaldırıldı
                        boylam = Convert.ToDouble(gelen.Trim(temizle.ToCharArray()));//gelen değer boylam değişkenine aktarılır
                        boylam /= 1000000;//gerçek boylam değeri
                        if (enlem != 0 && enlem > 1 && boylam > 1)//enlem ve boylam boş değilse
                        {
                            enlemList.Add(enlem);//listeye eklenir
                            boylamList.Add(boylam);//listeye eklenir
                        }
                    }

                    else if (-1 != gelen.IndexOf("Speed:"))//gelen stringte Speed: değeri varsa
                    {
                        Veri_yaz(gelen);//txt dosyasına gelen veri yazılır
                        string temizle = "Speed:";
                        hiz = gelen.Trim(temizle.ToCharArray());
                    }

                    else if (-1 != gelen.IndexOf("Alt:"))//gelen stringte Alt: değeri varsa
                    {
                        Veri_yaz(gelen);//txt dosyasına gelen veri yazılır
                        string temizle = "Alt:";
                        gelen = gelen.Trim(temizle.ToCharArray());
                        gelen = gelen.Replace(".", "");//işlem yapılması için nokta kaldırıldı
                        irtifa = Convert.ToDouble(gelen);
                        irtifa /= 100;
                        if(ref_irtifa==0)//daha önceden hiç irtifa verisi gelmemişse
                        {
                            Veri_yaz("Referans irtifa: "+irtifa.ToString());//referans irtifa verisi txt dosyasına yazdırıldı
                            ref_irtifa = irtifa;//referans irtifa değişkene aktarıldı
                        }
                    }

                    else if (-1 != gelen.IndexOf("Sat:"))//gelen stringte Sat: değeri varsa
                    {
                        Veri_yaz(gelen);//txt dosyasına gelen veri yazılır
                        string temizle = "Sat:";
                        bagli_uydu = gelen.Trim(temizle.ToCharArray());
                    }

                }
            }

            catch (Exception ex)//hata varsa
            {

            }
        }

    }
}
