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

        static void Main(string[] args)
        {            
            SpeechSynthesizer speechSynth = new SpeechSynthesizer();
            speechSynth.Speak("Welcome to System Monitor. Initializing. Cordinitts reeseeved. Reeseeving sistim data."); // english is a rough language for text to speech

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

                string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCPUPercentage);
                string memoryAvailableVocalMessage = String.Format("The current available memory is {0} megabytes.", currentAvailableMemory);
                speechSynth.Speak(cpuLoadVocalMessage);
                speechSynth.Speak(memoryAvailableVocalMessage);

                Thread.Sleep(1000);
                
            }

        }// end Main


    }// end Class
}// end Namespace
