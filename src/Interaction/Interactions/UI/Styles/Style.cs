using System.Collections.Generic;
using System.Reflection;

namespace Pandora.Interactions.UI.Styles
{
    public class Style
    {
        public PropertyInfo Property { get; internal set; }
        public string Value { get; internal set; }
    }
}