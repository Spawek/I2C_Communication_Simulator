using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace I2C_Communication_Simulator
{
    static class Program
    {
        static Thread backgroundWorkerThread;

        // http://ep.com.pl/files/2846.pdf //usefull info

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            backgroundWorkerThread = new Thread(new ThreadStart(BackgroundWorker));
            backgroundWorkerThread.Start();

            Application.Run(new Form1());
        }

        static FixedTimeClockGenerator generator;
        static void BackgroundWorker()
        {
            generator = new FixedTimeClockGenerator(1);
            generator.Start();
            Application.Run();
        }
    }
}
