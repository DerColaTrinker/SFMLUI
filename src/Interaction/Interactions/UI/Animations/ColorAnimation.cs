using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public class ColorAnimation : PropertyAnimation<Color>
    {
        private float _cR;
        private float _cG;
        private float _cB;
        private float _cA;
        private float _fR;
        private float _fG;
        private float _fB;
        private float _fA;

        public ColorAnimation(BindingProperty<Color> property, Color target, float time) : base(property, target, time)
        { }

        protected override void OnUpdate(float ms, float s)
        {
            _cR += (_fR * ms);
            _cG += (_fG * ms);
            _cB += (_fB * ms);
            _cA += (_fA * ms);

            Property.Value = new Color((byte)_cR, (byte)_cG, (byte)_cB, (byte)_cA);
        }

        protected override void OnReset()
        {
            // Startwerte
            _cR = (float)Property.Value.R;
            _cG = (float)Property.Value.G;
            _cB = (float)Property.Value.B;
            _cA = (float)Property.Value.A;

            // Faktoren
            _fR = ((float)TargetValue.R - _cR) / Duration;
            _fG = ((float)TargetValue.G - _cG) / Duration;
            _fB = ((float)TargetValue.B - _cB) / Duration;
            _fA = ((float)TargetValue.A - _cA) / Duration;
        }
    }
}
