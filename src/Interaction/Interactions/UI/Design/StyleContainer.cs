using System.Reflection;

namespace Pandora.Interactions.UI.Design
{
    internal class StyleContainer
    {
        public StyleContainer()
        { }

        public int Duration { get;  set; }

        public PropertyInfo Property { get; set; }

        public string Value { get; set; }
    }
}