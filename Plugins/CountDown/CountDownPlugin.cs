using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;

namespace CountDown
{
    public class CountDownPlugin : IPluginWithScheduler
    {
        IScheduler _scheduler;

        public CountDownPlugin(IScheduler scheduler) => _scheduler = scheduler;

        public static string _Id = "count-down";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            int interval;
            if (int.TryParse(input.Message, out interval))
            {
                _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
                return new PluginOutput("Countdown started.");
            }
            else
            {
                return new PluginOutput("Invalid input format. Please provide a valid number.");
            }
        }

        public void OnScheduler(string data)
        {
            Console.WriteLine("Fired.");
        }
    }
}
