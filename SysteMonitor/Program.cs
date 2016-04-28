using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Synthesis;

namespace SysteMonitor
{
    class Program
    {
        private static bool keepGoing = true;
        private static int memoryWarning = 0;
        private static int cpuWarning = 0;

        static void Main(string[] args)
        {            
            SpeechSynthesizer speechSynth = new SpeechSynthesizer();
            speechSynth.Speak("Welcome to System Monitor.");

            #region Performance Counters            
            PerformanceCounter perfCPUCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            #endregion

            while (keepGoing)
            {
                float currentCPUPercentage = (int)perfCPUCount.NextValue();
                float currentAvailableMemory = (int)perfMemCount.NextValue();

                // print load every second
                Console.WriteLine("CPU Load:         {0}%", currentCPUPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);
                Console.WriteLine("System Uptime: {0} seconds", (int)perfUptimeCount.NextValue());
                Console.WriteLine();
                Console.WriteLine();

                if (currentCPUPercentage > 90)
                {
                    if (cpuWarning.ToString().EndsWith("0") || cpuWarning.ToString().EndsWith("5"))
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCPUPercentage);
                        speechSynth.Speak(cpuLoadVocalMessage);
                    }

                    cpuWarning++;
                }

                if (currentAvailableMemory < 800)
                {
                    if (memoryWarning.ToString().EndsWith("0"))
                    {
                        string memoryAvailableVocalMessage = String.Format("Warning. Your available memory is {0} megabytes.", currentAvailableMemory);
                        speechSynth.Speak(memoryAvailableVocalMessage);
                    }

                    memoryWarning++;
                }

                Thread.Sleep(1000);
                
            }

        }// end Main


    }// end Class
}// end Namespace
