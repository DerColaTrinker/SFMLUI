using Pandora.Engine;

namespace Pandora.Interactions.Components
{
    public delegate void TimerTickDelegate();

    public sealed class TimerComponent : ComponentBase
    {
        private float _counter = 0F;

        public event TimerTickDelegate Tick;

        public TimerComponent(float interval, bool enabled)
        {
            Interval = interval;
            Enabled = enabled;
        }

        public TimerComponent(float interval, bool enabled, TimerTickDelegate tickevent)
        {
            Interval = interval;
            Enabled = enabled;
            Tick += tickevent;
        }

        public float Interval { get; }

        public bool Enabled { get; set; }

        protected internal override void OnSystemUpdate(PandoraRuntimeHost host, float ms, float s)
        {
            if (!Enabled) return;

            _counter += ms;

            if (_counter >= Interval)
            {
                _counter = 0F;
                Tick?.Invoke();
            }
        }
    }
}
