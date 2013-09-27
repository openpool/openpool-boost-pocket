using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakingThings;
using System.IO.Ports;
using System.Threading;

namespace Pocket_detector_interface
{
    public partial class Form1 : Form
    {
        private UdpPacket udpPacket;
        private Osc oscUdp;
        OscMessage oscMsg;
        SerialPort myPort;   
        public Form1()
        {
            udpPacket = new UdpPacket();
            udpPacket.RemoteHostName = "127.0.0.1";
            udpPacket.RemotePort = 7000;
            udpPacket.LocalPort = 9000;
            udpPacket.Open();
            oscUdp = new Osc(udpPacket);

            string PortName = "COM8";
            int BaudRate = 115200;
            Parity Parity = Parity.None;
            int DataBits = 8;
            StopBits StopBits = StopBits.One;
            
            myPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
            myPort.Open();
            byte[] buf = new byte[] { (byte)'s' };
            myPort.Write(buf, 0, buf.Length);


            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();

        }

        private byte[] read_serial_port()
        {
            int rbyte = myPort.BytesToRead;
            byte[] buffer = new byte[rbyte];
            int read = 0;
            while (read < rbyte)
            {
                int length = myPort.Read(buffer, read, rbyte - read);
                read += length;
            }
            return buffer;
        }

        delegate void add_text_delegate(String str);

        void add_text(String str)
        {
            output_box.Text = str + Environment.NewLine + output_box.Text;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                byte[] buf = new byte[] { (byte)'r' };
                myPort.Write(buf, 0, buf.Length);
                Thread.Sleep(500);
                buf = read_serial_port();
                String serial_str = Encoding.ASCII.GetString(buf);
                Invoke(new add_text_delegate(add_text),serial_str);
                String[] fallcounts = serial_str.Trim().Split(',');
                String osc_message_str = "/pocket";
                int i;
                osc_message_str += " " + fallcounts.Length.ToString();
                for (i = 0; i < 6 && i < fallcounts.Length; i++)
                {
                    osc_message_str += " " + fallcounts[i];
                }

                oscMsg = Osc.StringToOscMessage(osc_message_str);
                oscUdp.Send(oscMsg);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("send data");
            //パスはアプリの仕様で適当に、今回はウインドウの幅を送ります。
            oscMsg = Osc.StringToOscMessage("/event/click " + Width.ToString());
            oscUdp.Send(oscMsg);
//            byte[] buf = new byte[] { (byte)'t', (byte)'s', (byte)'\r', (byte)'\n' };


        }
    }
}
