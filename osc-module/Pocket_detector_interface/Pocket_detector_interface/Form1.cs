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
using System.IO.Ports;
using System.Threading;
using MakingThings;



namespace Pocket_detector_interface {

    enum boost_type {
        NOT_CONNECTED,
        NOT_RECOGNIZED,
        POCKET_DETECTOR,
        SMART_CUE
    }
    enum boost_version {
        NOT_RECOGNIZED,
        POCKET_DETECTOR_v_1_00,
        SMART_CUE_v_0_10
    }
    //    enum boost__version {
    //        POCKET_DETECTOR

    public partial class Form1 : Form {

        Dictionary<String, boost_type> boost_type_dict = new Dictionary<string, boost_type>();
        Dictionary<String, boost_version> boost_version_dict = new Dictionary<string, boost_version>();
        
        // hard-coded settings
        const int BAUDRATE = 115200;
        const Parity PARITY = Parity.None;
        const int DATABITS = 8;
        const StopBits STOPBITS = StopBits.One;
        const int SERIAL_TIMEOUT_MS = 500;
        const int SAMPLING_PERIOD_MS = 30;
        const String POCKET_DETECTOR_STR = "Pocket_detector";
        const String SMART_CUE_STR = "Smart_cue";
        private UdpPacket udpPacket;
        private Osc oscUdp;
        private SerialPort comport_PD, comport_SC;
        private bool debug_mode_SC, debug_mode_SC_requested, debug_mode_PD, debug_mode_PD_requested;
        public Form1() {
            InitializeComponent();
            // Setting strings that represent boost types and versions
            boost_type_dict["Pocket_detector"] = boost_type.POCKET_DETECTOR;
            boost_type_dict["Smart_cue"] = boost_type.SMART_CUE;
            boost_version_dict["Pocket_detector#1.00"] = boost_version.POCKET_DETECTOR_v_1_00;
            boost_version_dict["Smart_cue#0.10"] = boost_version.SMART_CUE_v_0_10;
            // open OSC over UDP
            udpPacket = new UdpPacket();
            udpPacket.RemoteHostName = "127.0.0.1";
            udpPacket.RemotePort = 7000;
            udpPacket.LocalPort = 9000;
            udpPacket.Open();
            oscUdp = new Osc(udpPacket);

            String[] serialport_names;
            serialport_names = SerialPort.GetPortNames();
            comport_box_PD.Items.Clear();
            comport_PD = null;
            debug_mode_PD = false;
            debug_mode_PD_requested = false;
            comport_box_SC.Items.Clear();
            comport_SC = null;
            debug_mode_SC = false;
            debug_mode_SC_requested = false;
            comport_scan_worker.RunWorkerAsync();
        }


        private byte[] read_serial_port(SerialPort port) {
            if (port == null || port.IsOpen == false) {
                return null;
            }
            int rbyte = port.BytesToRead;
            byte[] buffer = new byte[rbyte];
            int read = 0;
            while (read < rbyte) {
                int length = port.Read(buffer, read, rbyte - read);
                read += length;
            }
            return buffer;
        }


        delegate void add_text_delegate(String str);
        private void add_text_message_PD(String str) {
            message_box_PD.Text = str + Environment.NewLine + message_box_PD.Text;
        }
        private void add_text_message_SC(String str) {
            message_box_SC.Text = str + Environment.NewLine + message_box_SC.Text;
        }

        delegate void status_set_text_delegate(String str);
        private void set_text_status_PD(String str) {
            status_box_PD.Text = str;
        }
        private void set_text_status_SC(String str) {
            status_box_SC.Text = str;
        }
        private void set_text_status_all(String str) {
            status_box_all.Text = str;
        }

        delegate void update_comport_list_delegate(String[] serialport_names);
        private void update_comport_list(String[] serialport_names) {
            String selected_item_PD = (String)comport_box_PD.SelectedItem;
            String selected_item_SC = (String)comport_box_SC.SelectedItem;
            ComboBox.ObjectCollection comport_items_PD = comport_box_PD.Items;
            ComboBox.ObjectCollection comport_items_SC = comport_box_SC.Items;
            bool items_changed = false;
            // check if serialport names are updated
            if (serialport_names.Length == comport_items_PD.Count && serialport_names.Length == comport_items_SC.Count) {
                for (int i = 0; i < serialport_names.Length; i++) {
                    if (!(serialport_names[i].Equals((String)comport_items_PD[i])
                        && serialport_names[i].Equals((String)comport_items_SC[i]))) {
                        items_changed = true;
                    }
                }
            } else {
                items_changed = true;
            }
            if (items_changed) {
                comport_box_PD.Items.Clear();
                comport_box_SC.Items.Clear();
                for (int i = 0; i < serialport_names.Length; i++) {
                    comport_box_PD.Items.Add(serialport_names[i]);
                    comport_box_SC.Items.Add(serialport_names[i]);
                    if (selected_item_PD != null && selected_item_PD.Equals(serialport_names[i])) {
                        comport_box_PD.SelectedIndex = i;
                    }
                    if (selected_item_SC != null && selected_item_SC.Equals(serialport_names[i])) {
                        comport_box_SC.SelectedIndex = i;
                    }
                }
                comport_box_PD.Refresh();
                comport_box_SC.Refresh();
            }
        }
        delegate void select_index_delegate(ComboBox combobox, int index);
        private void select_index(ComboBox combobox, int index) {
            if (index < combobox.Items.Count) {
                combobox.SelectedIndex = index;
            }
        }

        private boost_type test_serial_port(String serialport_name) {
            SerialPort temp_port;
            String readstr;
            try {
                temp_port = new SerialPort(serialport_name, BAUDRATE, PARITY, DATABITS, STOPBITS);
                temp_port.Open();
            } catch {
                return boost_type.NOT_CONNECTED;
            }
            try {
                if (!temp_port.IsOpen) {
                    return boost_type.NOT_CONNECTED;
                }
                byte[] writebuf = new byte[] { (byte)'v' };
                temp_port.Write(writebuf, 0, writebuf.Length);
                // try 15 times with 100 msec interval and if nothing responded, just return "not recongnized"
                for (int i = 0; i < 15; i++) {
                    Thread.Sleep(100);
                    if (temp_port.BytesToRead != 0) {
                        break;
                    }
                }
                if (temp_port.BytesToRead == 0) {
                    return boost_type.NOT_RECOGNIZED;
                }
                readstr = temp_port.ReadLine();
                temp_port.Close();
                String boost_type_str = readstr.Split('#')[0].Trim();
                if (boost_type_str.Equals(POCKET_DETECTOR_STR)) {
                    return boost_type.POCKET_DETECTOR;
                } else if (boost_type_str.Equals(SMART_CUE_STR)) {
                    return boost_type.SMART_CUE;
                } else {
                    return boost_type.NOT_RECOGNIZED;
                }
            } catch {
                temp_port.Close();
                return boost_type.NOT_RECOGNIZED;
            }
        }

        private boost_version get_boost_version(SerialPort port) {
            if (port == null || port.IsOpen == false) {
//                Invoke(new add_text_delegate(add_text_message_PD), "not opened");
                return boost_version.NOT_RECOGNIZED;
            }
            byte[] buf = new byte[] { (byte)'v' };
            String serial_str;
            try {
                port.Write(buf, 0, buf.Length);
                Thread.Sleep(1000);
                serial_str = port.ReadLine().TrimEnd('\r', '\n');
                if (boost_version_dict.ContainsKey(serial_str)) {
                    return boost_version_dict[serial_str];
                } else {
                    return boost_version.NOT_RECOGNIZED;
                }
            } catch {
                return boost_version.NOT_RECOGNIZED;
            }
        }

        // comport_scan_worker continuously update serialport and make boost workers work 
        // 
        private void comport_scan_worker_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                Thread.Sleep(1000);
                Console.Write("comscanner called\n");
                if (comport_scan_worker.CancellationPending) {
                    Console.Write("cancellation requested\n");
                    e.Cancel = true;
                    return;
                }
                // always update comports box for all the devices
                // update the list of serial ports keeping selected item if already selected
                String[] serialport_names = SerialPort.GetPortNames();
                // if updated, update comport list
                Invoke(new update_comport_list_delegate(update_comport_list), (Object)serialport_names);            
                // scan all the comports and open a port if a valid port is detected and no port is already opend for the device
                for (int i = 0; i < serialport_names.Length; i++) {
                    boost_type boost_type_temp = test_serial_port(serialport_names[i]);
                    // if port is valid pocket detector and pocket detector is not yet opened,
                    // select the index to make selectedindexchanged() run, in which the port is opened
                    if (boost_type_temp == boost_type.POCKET_DETECTOR && (comport_PD == null || ! comport_PD.IsOpen)) {
                        Invoke(new select_index_delegate(select_index), comport_box_PD,i);
                    } else if (boost_type_temp == boost_type.SMART_CUE && (comport_SC == null || !comport_SC.IsOpen)) {
                        Invoke(new select_index_delegate(select_index), comport_box_SC,i);
                    } 
                }
                String status_text = "";
                if (comport_PD != null && comport_PD.IsOpen && pocketdetector_worker.IsBusy) {
                    status_text += "Pocket detector: running ";
                }else{
                    status_text += "Pocket detector: not running ";
                }
                if (comport_SC != null && comport_SC.IsOpen && pocketdetector_worker.IsBusy) {
                    status_text += "Smart cue: running ";
                } else {
                    status_text += "Smart cue: not running ";
                }
                Invoke(new status_set_text_delegate(set_text_status_all), status_text);

            }
        }


        private void pocketdetector_worker_DoWork(object sender, DoWorkEventArgs e) {
            boost_version version;
            // if the comport is not opened, do nothiing and return
            if (comport_PD == null || comport_PD.IsOpen == false) {
                Invoke(new status_set_text_delegate(set_text_status_PD), "A thread for pocket detector called, but port not opened");
                return;
            } else {
                Invoke(new add_text_delegate(add_text_message_PD), "trying to get boost version");
                version = get_boost_version(comport_PD);
            }
            Invoke(new add_text_delegate(add_text_message_PD), version.ToString());

            if (version == boost_version.POCKET_DETECTOR_v_1_00) {
                Invoke(new status_set_text_delegate(set_text_status_PD), "Pocket detector v1.00 running");
                // initialize by sending "s"
                byte[] buf = new byte[] { (byte)'s' };
                try {
                    comport_PD.Write(buf, 0, buf.Length);
                    Thread.Sleep(1000);
                    Invoke(new add_text_delegate(add_text_message_PD), comport_PD.ReadLine());
                } catch {
                    Invoke(new add_text_delegate(add_text_message_PD), "disconnected");
                    Invoke(new status_set_text_delegate(set_text_status_PD), "Pocket detector not connected");
                    return;
                }
                while (true) {
                    Thread.Sleep(SAMPLING_PERIOD_MS);
                    if (pocketdetector_worker.CancellationPending) {
                        e.Cancel = true;
                        return;
                    }
                    if (comport_PD == null || comport_PD.IsOpen == false) {
                        Invoke(new add_text_delegate(add_text_message_PD), "disconnected");
                        Invoke(new status_set_text_delegate(set_text_status_PD), "Pocket detector not connected");
                        return;
                    }

                    buf = new byte[] { (byte)'r' };
                    String serial_str;
                    try {
                        comport_PD.Write(buf, 0, buf.Length);
                        serial_str = comport_PD.ReadLine().TrimEnd('\r','\n');
                    } catch (TimeoutException) {
                        continue;
                    } catch (Exception) {
                        comport_PD = null;
                        continue;
                    }
                    String[] fallcounts = serial_str.Split(',');
                    String osc_message_str = "/pocket";
                    int i;
                    if (fallcounts.Length != 6) {
                        continue;
                    }
                    // if ball count is more than zero, send packet
                    for (i = 0; i < 6 ; i++) {
                        int ball_count;
                        try {
                            ball_count = int.Parse(fallcounts[i]);
                        } catch {
                            ball_count = 0;
                        }
                        if (ball_count > 0) {
                            osc_message_str = "/pocket " + i.ToString() + " 0";
                            Invoke(new add_text_delegate(add_text_message_PD), osc_message_str);
                            oscUdp.Send(Osc.StringToOscMessage(osc_message_str));
                        }
                    }
                }
            } else { // If the version number is not valid, show the error message and return
                String serial_str;
                try {
                    byte[] buf = new byte[] { (byte)'v' };
                    comport_PD.Write(buf, 0, buf.Length);
                    Thread.Sleep(100);
                    serial_str = comport_PD.ReadLine().TrimEnd('\r', '\n');
                } catch {
                    serial_str = "(unable to read)";
                }
                Invoke(new add_text_delegate(add_text_message_PD), "Unrecognizable version");
                Invoke(new status_set_text_delegate(set_text_status_PD), "Unrecognizable version:" + serial_str);
                return;
            }
        }

        private void Smartcue_worker_DoWork(object sender, DoWorkEventArgs e) {
            boost_version version;
            // if the comport is not opened, do nothiing and return
            if (comport_SC == null || comport_SC.IsOpen == false) {
                Invoke(new status_set_text_delegate(set_text_status_SC), "A thread for smart cue called, but port not opened");
                return;
            } else {
                Invoke(new add_text_delegate(add_text_message_SC), "trying to get boost version");
                version = get_boost_version(comport_SC);
            }
            Invoke(new add_text_delegate(add_text_message_SC), version.ToString());
            /*
            byte[] debugbuf = new byte[] { (byte)'d' };
            try {
                comport_SC.Write(debugbuf, 0, debugbuf.Length);
                Thread.Sleep(200);
                Invoke(new add_text_delegate(add_text_message_SC), comport_SC.ReadLine());

            } catch {
                Invoke(new add_text_delegate(add_text_message_SC), "disconnected");
                Invoke(new status_set_text_delegate(set_text_status_SC), "Smart cue not connected");
                return;
            }
            */
            if (version == boost_version.SMART_CUE_v_0_10) {
                Invoke(new status_set_text_delegate(set_text_status_SC), "Smart cue v0.10 running");
                // initialize by sending "s"
                byte[] buf = new byte[] { (byte)'s' };
                try {
                    comport_SC.Write(buf, 0, buf.Length);
                    Thread.Sleep(1000);
                    Invoke(new add_text_delegate(add_text_message_SC), comport_SC.ReadLine());
                } catch {
                    Invoke(new add_text_delegate(add_text_message_SC), "disconnected");
                    Invoke(new status_set_text_delegate(set_text_status_SC), "Smart cue not connected");
                    return;
                }
                while (true) {
                    Thread.Sleep(SAMPLING_PERIOD_MS);
                    if(smartcue_worker.CancellationPending) {
                        e.Cancel = true;
                        return;
                    }
                    if (comport_SC == null || comport_SC.IsOpen == false) {
                        Invoke(new add_text_delegate(add_text_message_SC), "disconnected");
                        Invoke(new status_set_text_delegate(set_text_status_SC), "Smart cue not connected");
                        return;
                    }

                    buf = new byte[] { (byte)'r' };
                    String serial_str;
                    try {
                        comport_SC.Write(buf, 0, buf.Length);
                        serial_str = comport_SC.ReadLine().TrimEnd('\r', '\n').Trim();
                    } catch (TimeoutException) {
                        continue;
                    } catch (Exception) {
                        comport_SC = null;
                        continue;
                    }
                    String osc_message_str = "";
                    int cue_count;
                    int i;
                    // if ball count is more than zero, send packet
                    try {
                        cue_count = int.Parse(serial_str);
                    } catch {
                        cue_count = 0;
                    }
                    if (cue_count > 0) {
                        osc_message_str = "/cue";
                        Invoke(new add_text_delegate(add_text_message_SC), osc_message_str);
                        oscUdp.Send(Osc.StringToOscMessage(osc_message_str));
                    }
                }
            } else { // If the version number is not valid, show the error message and return
                String serial_str;
                try {
                    byte[] buf = new byte[] { (byte)'v' };
                    comport_SC.Write(buf, 0, buf.Length);
                    Thread.Sleep(100);
                    serial_str = comport_SC.ReadLine().TrimEnd('\r', '\n');
                } catch {
                    serial_str = "(unable to read)";
                }
                Invoke(new add_text_delegate(add_text_message_SC), "Unrecognizable version");
                Invoke(new status_set_text_delegate(set_text_status_SC), "Unrecognizable version:" + serial_str);
                return;
            }
        }

        private void comport_box_PD_SelectedIndexChanged(object sender, EventArgs e) {

            String serialport_name = (String)comport_box_PD.SelectedItem;
            // if selected item is same as currently connected, do nothing and return
            if (comport_PD != null && comport_PD.IsOpen && comport_PD.PortName == serialport_name) {
                return;
            }
            add_text_message_PD("trying to connect " + serialport_name + "...\n");
            boost_type boost_type_temp = test_serial_port(serialport_name);
                          
            if (boost_type_temp == boost_type.POCKET_DETECTOR) {
                try {
                    comport_PD = new SerialPort(serialport_name, BAUDRATE, PARITY, DATABITS, STOPBITS);
                    comport_PD.ReadTimeout = SERIAL_TIMEOUT_MS;
                    comport_PD.Open();
                    add_text_message_PD("succeeded to connect " + serialport_name + "\n");
                    pocketdetector_worker.RunWorkerAsync();
                    return;
                } catch { }
            } else if (boost_type_temp == boost_type.SMART_CUE) {
            } else if (boost_type_temp == boost_type.NOT_RECOGNIZED){
                add_text_message_PD("unrecognizable device: " + serialport_name + "\n");
            } else {
                add_text_message_PD("unable to connect " + serialport_name + "\n");
            }

        }

        private void comport_box_SC_SelectedIndexChanged(object sender, EventArgs e) {
            String serialport_name = (String)comport_box_SC.SelectedItem;
            // if selected item is same as currently connected, do nothing and return
            if (comport_SC != null && comport_SC.IsOpen && comport_SC.PortName == serialport_name) {
                return;
            }
            add_text_message_SC("trying to connect " + serialport_name + "...\n");
            boost_type boost_type_temp = test_serial_port(serialport_name);

            if (boost_type_temp == boost_type.POCKET_DETECTOR) {
            } else if (boost_type_temp == boost_type.SMART_CUE) {
                try {
                    comport_SC = new SerialPort(serialport_name, BAUDRATE, PARITY, DATABITS, STOPBITS);
                    comport_SC.ReadTimeout = SERIAL_TIMEOUT_MS;
                    comport_SC.Open();
                    add_text_message_SC("succeeded to connect " + serialport_name + "\n");
                    smartcue_worker.RunWorkerAsync();
                    return;
                } catch { }
            } else if (boost_type_temp == boost_type.NOT_RECOGNIZED) {
                add_text_message_SC("unrecognizable device: " + serialport_name + "\n");
            } else {
                add_text_message_SC("unable to connect " + serialport_name + "\n");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            comport_scan_worker.CancelAsync();
            while (comport_scan_worker.IsBusy) {
                Thread.Sleep(100);
                Application.DoEvents();
            }
            pocketdetector_worker.CancelAsync();
            smartcue_worker.CancelAsync();
            while (pocketdetector_worker.IsBusy || smartcue_worker.IsBusy) {
                Thread.Sleep(100);
                Application.DoEvents();
            }
        }

    }
}
