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
        public static bool keepGoing = true;
        public static int memoryWarning = 0;
        public static int cpuWarning = 0;

        private static SpeechSynthesizer speechSynth = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            // create text-to-speech object
            speechSynth.SelectVoiceByHints(VoiceGender.Female);
            speechSynth.Speak("Welcome to System Monitor.");

            // create PerformanceCounter objects that grab system info from Performance Monitor
            #region Performance Counters            
            PerformanceCounter perfCPUCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            #endregion

            // main loop that prints system performance info and warns when necessary
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

                // warning if CPU load reaches assigned warning threshold (set in the "if" loop below)
                if (currentCPUPercentage > 90)
                {
                    if (cpuWarning.ToString().EndsWith("0") || cpuWarning.ToString().EndsWith("5"))
                    {
                        speechSynth.Speak(String.Format("Attention. The current CPU load is {0} percent", currentCPUPercentage));
                    }

                    cpuWarning++;
                }

                // warning if memory drops below assigned threshold (set in the "if" loop below)
                if (currentAvailableMemory < 500)
                {
                    if (memoryWarning.ToString().EndsWith("0"))
                    {
                        speechSynth.Speak(String.Format("Warning. Your available memory is {0} megabytes.", currentAvailableMemory));
                    }

                    memoryWarning++;
                }

                Thread.Sleep(1000);
            }
        }// end Main

    }// end Class
}// end Namespace
