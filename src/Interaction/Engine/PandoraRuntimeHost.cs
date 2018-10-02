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
            Services =  new ServiceCollection(); 
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

        public void Stop()
        {
            // Nicht direkt anhalten, aber den Running Status auf False setzten, damit wird die Schleife direkt verlassen
            IsRunning = false;
        }

        public bool StopRequest()
        {
            return Services.Any(m => m.StopRequest());
        }

        private void RuntimeLoop()
        {
            var clock = new Clock();                    // Verwendet eine Zeitmessung aus der SFML Bibliothek.
            var ms = 1F;                                // Initial ms auf 1 setzten, da bei 0 ggf. DivisionByZero Ausnahmen auftreten.
            var waittime = 0F;                          // Anzahl an Millisekunden die zurück an das System gegeben werden, wenn die Max. Framezeit nicht erreicht wurde.
            var args = new RuntimeFrameEventArgs();

            IsRunning = true;

            // Allen Diensten ein Start zukommen lassen
            ForEachServices(m => m.Start());

            while (IsRunning)
            {
                clock.Restart();

                args.Milliseconds = ms;
                args.Secounds = ms / 1000F;

                // Das Systemupdate an alle Dienste weiterleiten
                ForEachServices(m => m.SystemUpdate(this, args));

                // Lastenausgleich durchführen, in dem Systemzeit an das Betriebssystem zurück gegeben wird
                if (_framelimit > 0 && waittime > 0) Thread.Sleep((int)waittime);

                // Die Zeit schon hier messen und die Ausgleichzeit mit einbeziehen.
                ms = clock.ElapsedTime.AsMilliseconds();

                // Wartezeit für den Lastenausgleich berechnen
                if (_framelimit > 0 && ms < _maxframetime)
                {
                    waittime = _maxframetime - ms;
                    ms += waittime; // Laufzeit korrektur.
                }

                // FPS berechnen
                FPS = 1000F / (ms);
            }

            IsRunning = false;

            // Alle Dienste ein Stop zukommen lassen
            ForEachServices(m => m.Stop());

        }

        private void ForEachServices(Action<RuntimeService> action)
        {
            foreach (var item in Services) action.Invoke(item);
        }
    }
}