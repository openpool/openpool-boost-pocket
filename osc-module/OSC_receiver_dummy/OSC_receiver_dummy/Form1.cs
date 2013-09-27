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
using System.Threading;
using System.Collections;

namespace OSC_receiver_dummy
{
    public partial class Form1 : Form
    {
        private UdpPacket udpPacket;
        private Osc oscUdp;
        OscMessage oscMsg;
        byte[] buffer = new byte[65536];
        public Form1()
        {
            udpPacket = new UdpPacket();
            udpPacket.RemoteHostName = "127.0.0.1";
            udpPacket.RemotePort = 9000;
            udpPacket.LocalPort = 7000;
            udpPacket.Open();
            oscUdp = new Osc(udpPacket);
            InitializeComponent();
            Console.Write("get");
            backgroundWorker1.RunWorkerAsync();
            
        }

        delegate void add_text_delegate(String str);

        void add_text(String str)
        {
            output_box.Text = str +  Environment.NewLine + output_box.Text;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                int count;
                int i;
                ArrayList messages;
                String message_string;
                Console.Write("get2\n");
                count = udpPacket.ReceivePacket(buffer);
                //Console.Write(Encoding.ASCII.GetString(buffer,0,count)+ "\r\n");
                Console.Write("read:" + count.ToString() + "\n");
                if (count != 0)
                {
                    messages = Osc.PacketToOscMessages(buffer, count);
                    for (i = 0; i < messages.Count; i++)
                    {
                        message_string =  Osc.OscMessageToString((OscMessage)messages[i]);
 //                       output_box.Text = Osc.OscMessageToString((OscMessage)messages[i]) + Environment.NewLine + output_box.Text;
                        Invoke(new add_text_delegate(add_text), message_string);
                        Console.Write(message_string + "\n");
                        Console.Write(messages.Count.ToString() + "\n");
                    }
                }
            }

        }

    }
}
