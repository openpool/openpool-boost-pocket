/*
 * Openpool.cc
 * The interface between pocket detector and Unity.
 * This program reads the state of pocket detector on a serial communication and sends OSC packets to Unity over UDP.
 * Baud rate (115200) and port number (7000) are hard-coded.
 * 
 * This program uses Makecontroller (http://code.google.com/p/makecontroller/), an Apache Licensed open source library.
 *
*/

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

        const int BAUDRATE = 115200;
        const Parity PARITY = Parity.None;
        const int DATABITS = 8;
        const StopBits STOPBITS = StopBits.One;

        private UdpPacket udpPacket;
        private Osc oscUdp;
        OscMessage oscMsg;
        SerialPort myPort;
        public Form1()
        {
            InitializeComponent();

            // open OSC over UDP
            udpPacket = new UdpPacket();
            udpPacket.RemoteHostName = "127.0.0.1";
            udpPacket.RemotePort = 7000;
            udpPacket.LocalPort = 9000;
            udpPacket.Open();
            oscUdp = new Osc(udpPacket);

            String[] serialport_names;            
            serialport_names = SerialPort.GetPortNames();
            comport_box.Items.Clear();
            myPort = null;
                                    
            backgroundWorker1.RunWorkerAsync();

        }



        private byte[] read_serial_port(SerialPort port)
        {
            if (port == null || port.IsOpen == false)
            {
                return null;
            }
            int rbyte = port.BytesToRead;
            byte[] buffer = new byte[rbyte];
            int read = 0;
            while (read < rbyte)
            {
                int length = port.Read(buffer, read, rbyte - read);
                read += length;
            }
            return buffer;
        }


        private bool check_serial_port(String portname)
        {
            SerialPort temp_port;
            try
            {
                temp_port = new SerialPort(portname, BAUDRATE, PARITY, DATABITS, STOPBITS);
                temp_port.Open();
            }
            catch
            {
                return false;
            }
            try{
                if (!temp_port.IsOpen)
                {
                    return false;
                }
                byte[] writebuf = new byte[] { (byte)'s' };
                temp_port.Write(writebuf, 0, writebuf.Length);
                Thread.Sleep(100);
                if (temp_port.BytesToRead == 0)
                {
                    return false;
                }
                byte[] readbuf = read_serial_port(temp_port);
                writebuf = new byte[] { (byte)'r' };
                temp_port.Write(writebuf, 0, writebuf.Length);
                // try 15 times with 100 msec interval
                for (int i = 0; i < 15; i++)
                {
                    Thread.Sleep(100);
                    if (temp_port.BytesToRead != 0)
                    {
                        break;
                    }
                }
                if (temp_port.BytesToRead == 0)
                {
                    return false;
                }
                readbuf = read_serial_port(temp_port);
                temp_port.Close();
                return true;
            }
            catch
            {
                temp_port.Close();
                return false;
            }
        }

        delegate void add_text_delegate(String str);
        private void add_text(String str)
        {
            output_box.Text = str + Environment.NewLine + output_box.Text;
        }

        delegate void status_set_text_delegate(String str);
        private void status_set_text(String str)
        {
            status_textbox.Text = str;
        }

        delegate void update_serialport_list_delegate();
        private void update_serialport_list()
        {
            // update the list of serial ports keeping selected item if already selected
            String[] serialport_names = SerialPort.GetPortNames();
            String selected_item = (String)comport_box.SelectedItem;
            ComboBox.ObjectCollection comport_box_items = comport_box.Items;

            // check if serialport names are updated
            if ( serialport_names.Length == comport_box_items.Count)
            {
                for(int i=0; i < serialport_names.Length; i++)
                {
                    if ( ! serialport_names[i].Equals((String)comport_box_items[i]))
                    {
                        break;
                    }
                }
                // if all the items are same, return
                return;
            }

            comport_box.Items.Clear();
            for (int i = 0; i < serialport_names.Length; i++)
            {
                comport_box.Items.Add(serialport_names[i]);
                Console.Write("updated:" + serialport_names[i] + "\n");
                if (selected_item != null && selected_item.Equals(serialport_names[i]))
                {
                    comport_box.SelectedIndex = i;
                }
            }
            comport_box.Refresh();
            return;
        }

        delegate void scan_and_open_serialport_delegate();
        private void scan_and_open_serialport()
        {
            // If myPort is opened, stop scanning so that you can avoid unintended comport connection shutdown
            if (myPort != null &&  myPort.IsOpen)
            {
                return;
            }
            String[] serialport_names = SerialPort.GetPortNames();
            comport_box.Items.Clear();
            for (int i = 0; i < serialport_names.Length; i++)
            {
                comport_box.Items.Add(serialport_names[i]);
                if (check_serial_port(serialport_names[i]))
                {
                    comport_box.SelectedIndex = i;
                }
            }
            return;
        }




        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (myPort == null || myPort.IsOpen == false)
                {
                    Invoke(new status_set_text_delegate(status_set_text) , "Comport not connected");
                    Thread.Sleep(1000);
                    Invoke(new scan_and_open_serialport_delegate(scan_and_open_serialport));
                    continue;
                }
                Invoke(new status_set_text_delegate(status_set_text), "Comport connected");
                Invoke(new update_serialport_list_delegate(update_serialport_list));

                byte[] buf = new byte[] { (byte)'r' };
                String serial_str;
                try
                {
                    myPort.Write(buf, 0, buf.Length);
                    Thread.Sleep(30);
                    buf = read_serial_port(myPort);
                    serial_str = Encoding.ASCII.GetString(buf);
                }
                catch
                {
                    myPort = null;
                    continue;
                }
                String[] fallcounts = serial_str.Trim().Split(',');
                String osc_message_str = "/pocket";
                int i;
                for (i = 0; i < 6 && i < fallcounts.Length; i++)
                {
                    osc_message_str += " " + fallcounts[i];
                }
                Console.Write(osc_message_str + "\n");
                try
                {
                    Invoke(new add_text_delegate(add_text), serial_str);
                }
                catch { }

                oscMsg = Osc.StringToOscMessage(osc_message_str);
                oscUdp.Send(oscMsg);
            }
        }


        private void comport_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            String serialport_name = (String)comport_box.SelectedItem;
            add_text("trying to connect " + serialport_name + "...\n");
            if (check_serial_port(serialport_name))
            {
                try
                {
                    myPort = new SerialPort(serialport_name, BAUDRATE, PARITY, DATABITS, STOPBITS);
                    myPort.Open();
                    byte[] buf = new byte[] { (byte)'s' };
                    myPort.Write(buf, 0, buf.Length);
                    Thread.Sleep(500);
                    buf = read_serial_port(myPort);
                    add_text(Encoding.ASCII.GetString(buf));
                    add_text("succeeded to connect " + serialport_name + "\n");
                    return;
                }
                catch { }
            }
            add_text("failed to connect " + serialport_name + "\n");
        }
    }
}
