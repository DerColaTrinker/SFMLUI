using System.Collections.Generic;
using System.Linq;

namespace Pandora.Interactions.UI.Animations
{
    public sealed class GroupAnimation : Animation
    {
        private readonly HashSet<Animation> _animations = new HashSet<Animation>();

        public GroupAnimation() : base(0)
        { }

        public void Add(Animation effect)
        {
            _animations.Add(effect);
            Duration = _animations.Max(m => m.Duration);
        }

        public void AddRange(params Animation[] animations)
        {
            foreach (var animation in animations) _animations.Add(animation);
            Duration = _animations.Max(m => m.Duration);
        }

        public void AddRange(IEnumerable<Animation> animations)
        {
            foreach (var animation in animations) _animations.Add(animation);
            Duration = _animations.Max(m => m.Duration);
        }

        public void Remove(Animation animations)
        {
            _animations.Remove(animations);
            Duration = _animations.Max(m => m.Duration);
        }

        internal override void InternalUpdate(float ms, float s)
        {
            foreach (var animation in _animations)
            {
                if (!animation.IsComplet && animation.Status == AnimationStatus.Running)
                    animation.InternalUpdate(ms, s);
            }

            base.InternalUpdate(ms, s);
        }

        internal override void InternalStart()
        {
            foreach (var animation in _animations)
            {
                animation.InternalStart();
            }

            base.InternalStart();
        }

        internal override void InternalPause()
        {
            foreach (var animation in _animations)
            {
                animation.InternalStart();
            }

            base.InternalPause();
        }

        internal override void InternalStop()
        {
            foreach (var animation in _animations)
            {
                animation.InternalStop();
            }

            base.InternalStop();
        }

        internal override void InternalReset()
        {
            foreach (var animation in _animations)
            {
                animation.InternalReset();
            }

            base.InternalReset();
        }
    }
}
