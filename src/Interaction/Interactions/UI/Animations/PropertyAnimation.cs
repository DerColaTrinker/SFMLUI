using Pandora.Interactions.Bindings;

namespace Pandora.Interactions.UI.Animations
{
    public class PropertyAnimation : Animation
    {
        public PropertyAnimation(BindingProperty property, float time) : base(time)
        {
            Property = property;
        }

        public BindingProperty Property { get; }
    }

    public class PropertyAnimation<T> : PropertyAnimation
    {
        public PropertyAnimation(BindingProperty<T> property, T targetvalue, float time) : base(property, time)
        {
            TargetValue = targetvalue;
        }

        public new BindingProperty<T> Property { get { return (BindingProperty<T>)base.Property; } }

        public T TargetValue { get; internal set; }

        protected override void OnFinish()
        {
            Property.Value = TargetValue;
        }
    }
}
