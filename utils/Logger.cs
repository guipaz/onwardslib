using System;
using System.Diagnostics;
using System.IO;

namespace onwards.utils
{
    public static class Logger
    {
        public static string logDir = Environment.CurrentDirectory + "/log.txt";
        static StringWriter writer = new StringWriter();

        static Logger()
        {
            Initialize();
        }

        public static void Initialize()
        {
            Console.SetWindowSize(90, 25);
            Trace.Listeners.Clear();

            TextWriterTraceListener twtl = new TextWriterTraceListener(writer);
            twtl.Name = "TextLogger";
            twtl.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;

            ConsoleTraceListener ctl = new ConsoleTraceListener(false);
            ctl.TraceOutputOptions = TraceOptions.DateTime;

            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);
            Trace.AutoFlush = true;
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.WriteLine("[X] " + message);
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Trace.WriteLine("[!] " + message);
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Trace.WriteLine("[*] " + message);
        }

        public static void WaitToClose()
        {
            writer.WriteLine("---------------------------");
            writer.Close();

            Trace.WriteLine("");
            Info("Type 'D' to dump the log or anything else to close ", ConsoleColor.Yellow);
            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.D)
            {
                Console.Write("\n");
                Info("Dumping log...", ConsoleColor.Yellow);

                File.WriteAllText(logDir, writer.ToString());

                Info("Log dumped to " + logDir, ConsoleColor.Green);
                Info("Type anything to close", ConsoleColor.Green);
                Console.ReadKey();
            }
        }

        public static void Info(string message, ConsoleColor? color = null)
        {
            if (color == null)
                color = ConsoleColor.White;

            Console.ForegroundColor = (ConsoleColor)color;
            Trace.WriteLine("[-] " + message);
        }
    }
}