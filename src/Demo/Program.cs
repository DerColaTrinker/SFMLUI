using Pandora.Runtime.Logging;

namespace Pandora
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerManager.IsEnabled = true;
            LoggerManager.MinLevel = LoggerMessageType.Trace;
            LoggerManager.MaxLevel = LoggerMessageType.Error;
            LoggerManager.Add("console", new Runtime.Logging.Targets.ConsoleLoggerTarget());

            var runtime = new DemoRuntime();

            runtime.Initialize();
            runtime.Start();
        }
    }
}
