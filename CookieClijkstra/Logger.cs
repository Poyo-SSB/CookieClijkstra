using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieClijkstra
{
    public static class Logger
    {
        public enum MessageType
        {
            Info,
            Warning,
            Error,
            Debug
        }
        private static readonly object logLock = new object();
        public static void Log(object message, MessageType type = MessageType.Info)
        {
            lock (logLock)
            {
                DateTime time = DateTime.Now;
                switch (type)
                {
                    case MessageType.Info:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[INFO ");
                        break;
                    case MessageType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[WARN ");
                        break;
                    case MessageType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[ERRR ");
                        break;
                    case MessageType.Debug:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("[TEST ");
                        break;
                }

                Console.Write(time.ToString("HH:mm:ss dd/MM/yyyy"));
                Console.Write("] ");

                Console.ResetColor();

                try
                {
                    Console.WriteLine(message.ToString());
                }
                catch (NullReferenceException)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("NULL");
                }

                Console.ResetColor();
            }
        }
    }
}
