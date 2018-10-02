using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public sealed class StoryboardAnimation : Animation, IComparer<StoryboardStep>
    {
        private List<StoryboardStep> _steps = new List<StoryboardStep>();
        private int _stepindex;

        internal StoryboardAnimation() : base(0)
        { }

        public int Compare(StoryboardStep x, StoryboardStep y)
        {
            if (x.StartTime > y.StartTime) return 1;
            if (x.StartTime < y.StartTime) return -1;

            return 0;
        }

        public void Add(float startime, Animation animation)
        {
            _steps.Add(new StoryboardStep() { StartTime = startime, Duration = animation.Duration, Animation = animation });
            _steps.Sort(this);

            Duration = _steps.Select(s => s.StartTime + s.Duration).Max();
        }

        internal override void InternalReset()
        {
            _steps.ForEach(m => m.Animation.InternalReset());
            _stepindex = 0;

            base.InternalReset();
        }

        internal override void InternalPause()
        {
            for (int index = 0; index < _stepindex; index++)
            {
                if (!_steps[index].Animation.IsComplet) _steps[index].Animation.InternalPause();
            }

            base.InternalPause();
        }

        internal override void InternalStop()
        {
            for (int index = 0; index < _stepindex; index++)
            {
                if (!_steps[index].Animation.IsComplet) _steps[index].Animation.InternalStop();
            }

            base.InternalStop();
        }

        internal override void InternalStart()
        {
            base.InternalStart();
        }

        internal override void InternalUpdate(float ms, float s)
        {
            if (_stepindex > -1 & _stepindex <= _steps.Count)
            {
                var step = _steps[_stepindex];

                if (CurrentTime >= step.StartTime & step.Animation.Status != AnimationStatus.Running)
                {
                    step.Animation.InternalStart();
                    _stepindex++;
                }

                for (int index = 0; index < _stepindex; index++)
                {
                    if (!_steps[index].Animation.IsComplet)
                        _steps[index].Animation.InternalUpdate(ms, s);
                }
            }

            base.InternalUpdate(ms, s);
        }
    }
}
