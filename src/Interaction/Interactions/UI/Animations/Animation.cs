namespace Pandora.Interactions.UI.Animations
{
    public delegate void AnimationDelegate(Animation animation);

    public abstract class Animation
    {
        public event AnimationDelegate AnimationStart;
        public event AnimationDelegate AnimationStop;
        public event AnimationDelegate AnimationPause;
        public event AnimationDelegate AnimationFinish;

        protected Animation(float duration)
        {
            Duration = duration;
            IsComplet = true;
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
        {
            AnimationStart?.Invoke(this);
        }

        protected virtual void OnPause()
        {
            AnimationPause?.Invoke(this);
        }

        protected virtual void OnStop()
        {
            AnimationStop?.Invoke(this);
        }

        protected virtual void OnFinish()
        {
            AnimationFinish?.Invoke(this);
        }

        protected virtual void OnUpdate(float ms, float s)
        { }

        public override string ToString()
        {
            return $"{CurrentTime:N3}/{Duration:N3}";
        }
    }
}