using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LowLevelInput.Converters;
using LowLevelInput.Hooks;

namespace RepeatSpeaking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Logo);
            Console.WriteLine(Description);

            var inputManager = new InputManager();

            inputManager.OnKeyboardEvent += InputManager_OnKeyboardEvent;
            inputManager.OnMouseEvent += InputManager_OnMouseEvent;

            inputManager.Initialize();
            inputManager.WaitForEvent(VirtualKeyCode.Up, KeyState.Down);

            inputManager.Dispose();
        }

        static bool IsUp = false;
        static DateTime UpDate = DateTime.MinValue;
        static readonly double DoubleClickInterval = double.Parse(ConfigurationManager.AppSettings["doubleClickInterval"]);
        static readonly bool IsAutoEnter = bool.Parse(ConfigurationManager.AppSettings["autoEnter"]);

        private static void InputManager_OnMouseEvent(VirtualKeyCode key, KeyState state, int x, int y)
        {
            if (state == KeyState.Up)
            {
                if (IsUp)
                {
                    if (DateTime.Now.Subtract(UpDate).TotalSeconds < DoubleClickInterval)
                    {
                        SendKeys.SendWait("^{c}");
                        SendKeys.SendWait("{TAB}");
                        SendKeys.SendWait("{TAB}");
                        SendKeys.SendWait("^{v}");
                        if (IsAutoEnter)
                        {
                            SendKeys.SendWait("{ENTER}");
                        }
                    }
                }
                else
                {
                    UpDate = DateTime.Now;
                }
                IsUp = !IsUp;
            }
            if (state == KeyState.Down || state == KeyState.Up)
            {
                Console.WriteLine("鼠标: " + KeyCodeConverter.ToString(key) + " - " + KeyStateConverter.ToString(state) + " - X: " + x + ", Y: " + y);
            }
        }

        private static void InputManager_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            Console.WriteLine("键盘: " + KeyCodeConverter.ToString(key) + " - " + KeyStateConverter.ToString(state));
        }

        const string Description = "双击微信客户端的聊天气泡即可复读";
        const string Logo = @"  _____                       _                              _     
 |  __ \                     | |                            | |    
 | |__) |___ _ __   ___  __ _| |_   ___ _ __   ___  ___  ___| |__  
 |  _  // _ \ '_ \ / _ \/ _` | __| / __| '_ \ / _ \/ _ \/ __| '_ \ 
 | | \ \  __/ |_) |  __/ (_| | |_  \__ \ |_) |  __/  __/ (__| | | |
 |_|  \_\___| .__/ \___|\__,_|\__| |___/ .__/ \___|\___|\___|_| |_|
            | |                        | |                         
            |_|                        |_|                         
                                                                 V0.3";

    }
}
