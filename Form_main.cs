using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arrow
{
    public partial class Form_main : Form
    {        
        private DDS _svr;
        private System.Timers.Timer _timer;

        Int64 _lastNumMsgRx = 0;
        Int64 _numMsgRx = 0;

        Int64 _msgRateT0 = 0;
        Int64 _msgRateT1 = 0;
        Int64 _msgRateT2 = 0;
        Int64 _msgRateT3 = 0;
        Int64 _msgRateT4 = 0;

        Int64 _msgRateAvg = 0;
        Int64 _lastPeakMsgRate =0;

        public Form_main()
        {
            InitializeComponent();
             
            _svr = new DDS();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;

            _timer = new System.Timers.Timer(1000);
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {            
            _lastNumMsgRx = _numMsgRx; 
            _numMsgRx = _svr.MsgRxCounter;

            _msgRateT4 = _msgRateT3;
            _msgRateT3 = _msgRateT2;
            _msgRateT2 = _msgRateT1;
            _msgRateT1 = _msgRateT0;
            _msgRateT0 = _numMsgRx - _lastNumMsgRx;

            _msgRateAvg = (_msgRateT0 + _msgRateT1 + _msgRateT2 + _msgRateT3 + _msgRateT4) /5;
            
            this.BeginInvoke((MethodInvoker)delegate { label_msgRate_T4.Text = _msgRateT4.ToString(); });
            this.BeginInvoke((MethodInvoker)delegate { label_msgRate_T3.Text = _msgRateT3.ToString(); });
            this.BeginInvoke((MethodInvoker)delegate { label_msgRate_T2.Text = _msgRateT2.ToString(); });
            this.BeginInvoke((MethodInvoker)delegate { label_msgRate_T1.Text = _msgRateT1.ToString(); });
            this.BeginInvoke((MethodInvoker)delegate { label_msgRate.Text = _msgRateT0.ToString(); });

            this.BeginInvoke((MethodInvoker)delegate { label_msgRateAvg.Text = _msgRateAvg.ToString(); });
            this.BeginInvoke((MethodInvoker)delegate { label_numMsgRx.Text = _numMsgRx.ToString(); });            

            if (_msgRateT0 > _lastPeakMsgRate)
            {
                _lastPeakMsgRate = _msgRateT0;
                this.BeginInvoke((MethodInvoker)delegate { label_Time.Text = System.DateTime.Now.ToString("HH:mm:ss:fffffff"); });
                this.BeginInvoke((MethodInvoker)delegate { label_peakMsgRate.Text = _msgRateT0.ToString(); });
            }
        }
        
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.notifyIcon.Visible = false;
        }

        private void Form_server_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                this.ShowInTaskbar = false;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                notifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        } 

        private void Form_main_Load(object sender, EventArgs e)
        {            
            DDSIP_Port sub = new DDSIP_Port(Properties.Settings.Default.Sub_IP, Properties.Settings.Default.Sub_port, Properties.Settings.Default.Sub_HWM);
            DDSIP_Port pub = new DDSIP_Port(Properties.Settings.Default.Pub_IP, Properties.Settings.Default.Pub_port, Properties.Settings.Default.Pub_HWM);
            DDSIP_Port router = new DDSIP_Port(Properties.Settings.Default.Router_IP, Properties.Settings.Default.Router_port, Properties.Settings.Default.Root_HWM);

            if (Properties.Settings.Default.ConnectToRoot)
            {
                DDSIP_Port root = new DDSIP_Port(Properties.Settings.Default.Root_IP, Properties.Settings.Default.Root_port, Properties.Settings.Default.Root_HWM);

                Task task_DDS = Task.Factory.StartNew(() => _svr.Start(sub, router, pub, root), TaskCreationOptions.LongRunning);

                toolStripStatusLabel_root.Text = "Root - " + root.IP + ":" + root.Port;
            }
            else
            {
                Task task_DDS = Task.Factory.StartNew(() => _svr.Start(sub, router, pub), TaskCreationOptions.LongRunning);
            }

            System.Threading.Thread.Sleep(1000);            

            toolStripStatusLabel_pub.Text = "PUB - " + pub.IP + ":" + pub.Port;
            toolStripStatusLabel_sub.Text = "SUB - " + sub.IP + ":" + sub.Port;            
        }

        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Initializes the variables to pass to the MessageBox.Show method.                                     
            string caption = "Warning!";
            string message = "Close DDS, Confirm?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _timer.Stop();
                _svr.Stop();
                notifyIcon.Dispose();
            }
            else
            {
                e.Cancel = true;
            }            
        }
    }
}
