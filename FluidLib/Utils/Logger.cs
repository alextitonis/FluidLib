using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Utils
{
    public static class Logger
    {
        public static void Log(string writer, string message, LogType type)
        {
            switch (type)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            Console.WriteLine("[" + writer + "]: " + message);
            Console.ResetColor();
        }
    }

    public enum LogType
    {
        Info = 0,
        Error = 1,
        Warning = 2,
    }
}
