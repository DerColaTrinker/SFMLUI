using Pandora.Interactions.Bindings;

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
            _factor = (TargetValue - Property.Value) / Duration;
        }
    }
}