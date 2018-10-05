using Pandora.Interactions.UI.Design.Converter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Design
{
    internal class DesignContainer
    {
        internal class StyleCollection : IEnumerable<StyleContainer>
        {
            private Dictionary<string, StyleContainer> _properties = new Dictionary<string, StyleContainer>();

            internal void Add(StyleContainer style)
            {
                _properties[style.Property.Name] = style;
            }

            public IEnumerator<StyleContainer> GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public DesignContainer(Type control)
        {
            Control = control;
            Styles = new StyleCollection();
        }

        public Type Control { get; }

        public StyleCollection Styles { get; }
    }
}