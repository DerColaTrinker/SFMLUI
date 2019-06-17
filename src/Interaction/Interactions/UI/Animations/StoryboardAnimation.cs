using System.Collections.Generic;
using System.Linq;

namespace Pandora.Interactions.UI.Animations
{
    public sealed class StoryboardAnimation : Animation, IComparer<StoryboardStep>
    {
        private List<StoryboardStep> _storyboardcollection = new List<StoryboardStep>();
        private Stack<StoryboardStep> _playstack = new Stack<StoryboardStep>();

        public StoryboardAnimation() : base(0)
        { }

        public int Compare(StoryboardStep x, StoryboardStep y)
        {
            if (x.StartTime > y.StartTime) return 1;
            if (x.StartTime < y.StartTime) return -1;

            return 0;
        }

        public void Add(float startime, Animation animation)
        {
            // Add the Animation and create a step
            _storyboardcollection.Add(new StoryboardStep() { StartTime = startime, Animation = animation });
            _storyboardcollection.Sort(this);

            // The sum of all durations is the total duration of the storyboard.
            Duration = _storyboardcollection.Max(m => m.EndTime);
        }

        public void AddRange(float starttime, IEnumerable<Animation> animations)
        {
            foreach (var item in animations)
            {
                Add(starttime, item);
            }
        }

        /// <summary>
        /// Returns the sum of all animation steps.
        /// </summary>
        public int StepCount => _storyboardcollection.Count;

        internal override void InternalReset()
        {
            // Reorder the startparameters
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
            base.InternalStop();
        }

        internal override void InternalUpdate(float ms, float s)
        {
            if (_playstack.Count > 0)
            {
                var peekstep = _playstack.Peek();

                if (CurrentTime >= peekstep.StartTime)
                {
                    peekstep.Animation.InternalStart();
                    peekstep.Animation.InternalReset();
                    _playstack.Pop();
                }
            }

            _storyboardcollection.ForEach(step =>
            {
                if (step.Animation.Status == AnimationStatus.Running)
                {
                    step.Animation.InternalUpdate(ms, s);

                    if (step.Animation.IsComplet)
                        step.Animation.InternalStop();
                }
            });

            base.InternalUpdate(ms, s);
        }
    }
}