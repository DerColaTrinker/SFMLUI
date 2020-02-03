﻿using Pandora.Engine.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Pandora.Engine
{
    public delegate void RuntimeStopRequestDelegate(PandoraRuntimeHost host, bool services, ref bool cancel);
    public delegate void RuntimeSystemUpdateDelegate(PandoraRuntimeHost host, float ms, float s);

    public class PandoraRuntimeHost : IDisposable
    {
        private int _framelimit;
        private float _maxframetime;

        public event RuntimeStopRequestDelegate RuntimeStopRequest;
        public event RuntimeSystemUpdateDelegate RuntimeSystemUpdate;

        public PandoraRuntimeHost()
        {
            Logger.Normal($"Starting Host '{GetType().Name}'");
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

            Logger.Normal("[Runtime] start");

            foreach (var item in Services.Services)
            {
                item.Initialize(out bool success);

                if (!success) return;
            }

            RuntimeLoop();
        }

        internal void InternalStopRequest(RuntimeService service)
        {
            Logger.Debug("[Runtime] call stop request");
            var serviceresult = Services.Services.All(m => m.StopRequested());
            Logger.Debug($"[Runtime] Stop request service result : {serviceresult}");

            var cancel = false;
            RuntimeStopRequest?.Invoke(this, serviceresult, ref cancel);
            if (cancel | !serviceresult) return;

            Stop();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            Logger.Normal("[Runtime] stop");

            // Do not stop directly, but set the running status to False, so that the loop is left directly.
            IsRunning = false;
        }

        public void InitiateStopRequest()
        {
            Logger.Normal("[Runtime] Initiate stop request");
            if (GetStopRequestResult())
                Stop();
        }

        public bool GetStopRequestResult()
        {
            return Services.Services.Any(m => m.StopRequested());
        }

        private void RuntimeLoop()
        {
            var clock = new Stopwatch();
            var ms = 1F;                                // Set Initial ms to 1, since exceptions may occur with 0 DivisionByZero.
            var waittime = 0F;                          // Number of milliseconds returned to the system if the maximum frame time was not reached.

            IsRunning = true;

            // Allen Diensten ein Start zukommen lassen
            ForEachServices(m => m.Start());

            Logger.Debug("[Runtime] Enter loop.");

            while (IsRunning)
            {
                clock.Restart();

                InternalSystemUpdate(ms, ms / 1000F);

                // Perform load balancing by returning system time to the operating system.
                if (_framelimit > 0 && waittime > 0) Thread.Sleep((int)waittime);

                // Measure the time already here and include the compensation time.
                ms = clock.ElapsedMilliseconds;

                // Calculate waiting time for load balancing
                if (_framelimit > 0 && ms < _maxframetime)
                {
                    waittime = _maxframetime - ms;
                    ms += waittime; // correction
                }

                // FPS
                FPS = 1000F / (ms);
            }

            Logger.Debug("[Runtime] Leave loop");

            IsRunning = false;

            // Alle Dienste ein Stop zukommen lassen
            ForEachServices(m => m.Stop());
        }

        private void InternalSystemUpdate(float ms, float s)
        {
            SystemUpdate(ms, s);
            RuntimeSystemUpdate?.Invoke(this, ms, s);

            // Forward the system update to all services
            ForEachServices(m => m.SystemUpdate(this, ms, s));
        }

        protected virtual void SystemUpdate(float ms, float s)
        { }

        private void ForEachServices(Action<RuntimeService> action)
        {
            foreach (var item in Services.Services)
            {
                action.Invoke(item);
            }
        }

        public void Dispose()
        {
            Logger.Debug("[Runtime] Dispose");
            ForEachServices(m => m.Dispose());
        }
    }
}