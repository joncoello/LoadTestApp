using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestApp.PerfCounters.Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteErrorMessage();
            }
            else
            {
                if (args[0] == "/install")
                {
                    Console.WriteLine("Installing performance counters...");
                    PerformanceCounterLocator.Instance.CreateCounters();
                    Console.WriteLine("Installed performance counters.");
                }
                else if (args[0] == "/uninstall")
                {
                    Console.WriteLine("Uninstalling performance counters...");
                    PerformanceCounterLocator.Instance.DeleteCounters();
                    Console.WriteLine("Uninstalled performance counters.");
                }
                else
                {
                    WriteErrorMessage();
                }
            }
        }

        private static void WriteErrorMessage()
        {
            Console.Error.WriteLine("Specify either '/install' or '/uninstall' to add or remove perfmon counters.");
        }
    }
}
