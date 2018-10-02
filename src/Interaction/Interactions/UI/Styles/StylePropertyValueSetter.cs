namespace Pandora.Interactions.UI.Styles
{
    public class StylePropertyValueSetter
    {
        public StylePropertyValueSetter(string name, string value)
        {
            Name = name;

            Value = value;
        }

        public string Name { get; private set; }

        public string Value { get; set; }
    }
}