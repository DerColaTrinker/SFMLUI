using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;

namespace Pandora.Interactions.UI.Animations
{
    public class OpacityAnimation : PropertyAnimation<Color>
    {
        private float _cA;
        private float _fA;

        public OpacityAnimation(BindingProperty<Color> property, byte alphachannel, float time) : base(property, new Color(property.Value.R, property.Value.G, property.Value.B, alphachannel), time)
        { }

        protected override void OnUpdate(float ms, float s)
        {
            _cA += (_fA * ms);

            Property.Value = new Color(TargetValue.R, TargetValue.G, TargetValue.B, (byte)_cA);
        }

        protected override void OnReset()
        {
            _cA = (float)Property.Value.A;

            // Faktoren
            _fA = ((float)TargetValue.A - _cA) / Duration;
        }
    }
}
