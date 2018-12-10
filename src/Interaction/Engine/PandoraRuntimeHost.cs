using Pandora.Engine.Services;
using Pandora.SFML.System;
using System;
using System.Linq;
using System.Threading;

namespace Pandora.Engine
{
    public abstract class PandoraRuntimeHost
    {
        private int _framelimit;
        private float _maxframetime;

        public PandoraRuntimeHost()
        {
            Services = new ServiceCollection(this);
            FrameLimit = 60;
        }

        public bool IsRunning { get; private set; }

        public float FPS { get; private set; }

        public int FrameLimit
        {
            get { return _framelimit; }
            set { _framelimit = value; _maxframetime = 1000F / (float)value; }
        }

        public ServiceCollection Services { get; private set; }

        public void Start()
        {
            if (IsRunning) return;

            foreach (var item in Services)
            {
                item.Initialize(out bool success);

                if (!success) return;
            }

            RuntimeLoop();
        }

        internal void InternalStopRequest(RuntimeService service)
        {
            if (Services.Any(m => m.StopRequested()))
                Stop();
        }

        public void Stop()
        {
            // Do not stop directly, but set the running status to False, so that the loop is left directly.
            IsRunning = false;
        }

        public void InitiateStopRequest()
        {
            if (GetStopRequestResult())
                Stop();
        }

        public bool GetStopRequestResult()
        {
            return Services.Any(m => m.StopRequested());
        }

        private void RuntimeLoop()
        {
            var clock = new Clock();                    // Uses a time measurement from the SFML library.
            var ms = 1F;                                // Set Initial ms to 1, since exceptions may occur with 0 DivisionByZero.
            var waittime = 0F;                          // Number of milliseconds returned to the system if the maximum frame time was not reached.
            var args = new RuntimeFrameEventArgs();

            IsRunning = true;

            // Allen Diensten ein Start zukommen lassen
            ForEachServices(m => m.Start());

            while (IsRunning)
            {
                clock.Restart();

                args.Milliseconds = ms;
                args.Secounds = ms / 1000F;

                InternalSystemUpdate(args);

                // Perform load balancing by returning system time to the operating system.
                if (_framelimit > 0 && waittime > 0) Thread.Sleep((int)waittime);

                // Measure the time already here and include the compensation time.
                ms = clock.ElapsedTime.AsMilliseconds();

                // Calculate waiting time for load balancing
                if (_framelimit > 0 && ms < _maxframetime)
                {
                    waittime = _maxframetime - ms;
                    ms += waittime; // correction
                }

                // FPS
                FPS = 1000F / (ms);
            }

            IsRunning = false;

            // Alle Dienste ein Stop zukommen lassen
            ForEachServices(m => m.Stop());

        }

        private void InternalSystemUpdate(RuntimeFrameEventArgs args)
        {
            SystemUpdate(args);

            // Forward the system update to all services
            ForEachServices(m => m.SystemUpdate(this, args));
        }

        protected virtual void SystemUpdate(RuntimeFrameEventArgs args)
        { }

        private void ForEachServices(Action<RuntimeService> action)
        {
            foreach (var item in Services) action.Invoke(item);
        }
    }
}