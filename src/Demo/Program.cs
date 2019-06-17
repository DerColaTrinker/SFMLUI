namespace Pandora
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtime = new DemoRuntime();

            runtime.Initialize();
            runtime.Start();
        }
    }
}
