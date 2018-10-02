using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public class FloatAnimation : PropertyAnimation<float>
    {
        private float _factor;

        public FloatAnimation(BindingProperty<float> property, float target, float time) : base(property, target, time)
        { }

        protected override void OnUpdate(float ms, float s)
        {
            Property.Value += _factor * ms;
        }

        protected override void OnReset()
        {
            _factor = (Property.Value - TargetValue) / Duration;
        }
    }
}