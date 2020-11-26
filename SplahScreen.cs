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
    public partial class SplahScreen : Form
    {
        
        public SplahScreen()
        {
            InitializeComponent();
        }
        int a = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            if(a<10 && progressBar1.Value<99)
            {
                progressBar1.Value += 10;
            }
            else
            {
                progressBar1.Value = 100;
                timer2.Start();
                timer1.Stop();
                a = 0;
                
            }
            a++;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = 100;
            if(a==6)
            {
                timer2.Stop();
                this.Close();

            }
            a++;

        }
    }
}
