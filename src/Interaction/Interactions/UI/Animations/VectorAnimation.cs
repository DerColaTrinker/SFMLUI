using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public class VectorAnimation : PropertyAnimation<Vector2F>
    {
        private Vector2F _factor;

        public VectorAnimation(BindingProperty<Vector2F> property, Vector2F target, float time) : base(property, target, time)
        { }

        protected override void OnUpdate(float ms, float s)
        {
            Property.Value += new Vector2F(_factor.X * ms, _factor.Y * ms);
        }
      
        protected override void OnReset()
        {
            _factor = new Vector2F((TargetValue.X - Property.Value.X) / Duration, (TargetValue.Y - Property.Value.Y) / Duration);
        }
    }
}
