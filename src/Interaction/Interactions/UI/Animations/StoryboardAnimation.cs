using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Interactions.UI.Animations
{
    public sealed class StoryboardAnimation : Animation
    {
        private readonly List<StoryboardStep> _storyboardcollection = new List<StoryboardStep>();
        private readonly Stack<StoryboardStep> _playstack = new Stack<StoryboardStep>();

        public StoryboardAnimation() : base(0)
        { }

        public void Add(float starttime, Animation animation)
        {
            _storyboardcollection.Add(new StoryboardStep() { StartTime = starttime, Animation = animation });
            Duration = _storyboardcollection.Max(m => m.EndTime);
        }

        public void AddRange(float starttime, IEnumerable<Animation> animations)
        {
            foreach (var item in animations)
            {
                _storyboardcollection.Add(new StoryboardStep() { StartTime = starttime, Animation = item });
            }

            Duration = _storyboardcollection.Max(m => m.EndTime);
        }

        /// <summary>
        /// Returns the sum of all animation steps.
        /// </summary>
        public int StepCount => _storyboardcollection.Count;

        internal override void InternalReset()
        {
            _playstack.Clear();
            foreach (var item in _storyboardcollection.OrderByDescending(m => m.StartTime))
            {
                _playstack.Push(item);
            }

            base.InternalReset();
        }

        internal override void InternalStart()
        {
            base.InternalStart();
        }

        internal override void InternalPause()
        {
            base.InternalPause();
        }

        internal override void InternalStop()
        {
            foreach (var item in _storyboardcollection)
            {
                item.Animation.InternalStop();
            }

            base.InternalStop();
        }

        internal override void InternalUpdate(float ms, float s)
        {
            if (_playstack.Count > 0)
            {
                var peekstep = _playstack.Peek();

                if (CurrentTime >= peekstep.StartTime)
                {
                    peekstep.Animation.InternalReset();
                    peekstep.Animation.InternalStart();
                    _playstack.Pop();
                }
            }

            _storyboardcollection.ForEach(step =>
            {
                if (step.Animation.Status == AnimationStatus.Running)
                {
                    step.Animation.InternalUpdate(ms, s);
                }
            });

            base.InternalUpdate(ms, s);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}