using System;
using Pandora.Interactions.Bindings;

namespace Pandora.Interactions.UI.Animations
{
    public abstract class Animation
    {
        protected Animation(float duration)
        {
            Duration = duration;
            IsComplet = true;
            //InternalReset();
        }

        internal virtual void InternalReset()
        {
            CurrentTime = 0F;
            IsComplet = false;
            OnReset();
        }

        internal virtual void InternalStart()
        {
            if (Status == AnimationStatus.Running) return;
            if (IsComplet) InternalReset();

            Status = AnimationStatus.Running;

            OnStart();
        }

        internal virtual void InternalPause()
        {
            if (Status != AnimationStatus.Running) return;

            Status = AnimationStatus.Pause;

            OnPause();
        }

        internal virtual void InternalStop()
        {
            if (Status == AnimationStatus.Stopped) return;

            Status = AnimationStatus.Stopped;
            IsComplet = true;

            OnStop();
        }

        internal virtual void InternalUpdate(float ms, float s)
        {
            CurrentTime += ms;

            OnUpdate(ms, s);

            if (CurrentTime >= Duration)
            {
                InternalStop();
                OnFinish();
            }
        }

        public bool IsComplet { get; private set; }

        public AnimationStatus Status { get; private set; }

        public float Duration { get; internal set; }

        public float CurrentTime { get; private set; }

        protected virtual void OnReset()
        { }

        protected virtual void OnStart()
        { }

        protected virtual void OnPause()
        { }

        protected virtual void OnStop()
        { }

        protected virtual void OnFinish()
        { }

        protected virtual void OnUpdate(float ms, float s)
        { }
    }
}