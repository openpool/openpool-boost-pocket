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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakingThings;

namespace Pocket_detector_interface
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
