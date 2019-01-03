﻿using Pandora.Interactions.UI.Design.Converter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Design
{
    internal class ControlContainer
    {
        internal class StyleCollection : IEnumerable<PropertySetterContainer>
        {
            private Dictionary<string, PropertySetterContainer> _properties = new Dictionary<string, PropertySetterContainer>();

            internal void Add(PropertySetterContainer style)
            {
                _properties[style.Property.Name] = style;
            }

            internal void AddRange(IEnumerable<PropertySetterContainer> enumerable)
            {
                foreach (var item in enumerable)
                {
                    _properties[item.Property.Name] = item;
                }
            }

            public IEnumerator<PropertySetterContainer> GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        internal class AnimationCollection : IEnumerable<AnimationContainer>
        {
            private Dictionary<DesignAnimationEvents, AnimationContainer> _animations = new Dictionary<DesignAnimationEvents, AnimationContainer>();

            internal void Add(AnimationContainer animation)
            {
                _animations[animation.Event] = animation;
            }

            internal void AddRange(IEnumerable<AnimationContainer> enumerable)
            {
                foreach (var item in enumerable)
                {
                    _animations[item.Event] = item;
                }
            }

            public IEnumerator<AnimationContainer> GetEnumerator()
            {
                return _animations.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        internal class TemplateCollection : IEnumerable<ControlContainer>
        {
            private List<ControlContainer> _controls = new List<ControlContainer>();

            internal void Add(ControlContainer control)
            {
                _controls.Add(control);
            }

            internal void AddRange(IEnumerable<ControlContainer> enumerable)
            {
                _controls.AddRange(enumerable);
            }

            public IEnumerator<ControlContainer> GetEnumerator()
            {
                return _controls.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        public ControlContainer(Type control)
        {
            Control = control;
            Styles = new StyleCollection();
            Animations = new AnimationCollection();
            Teamplates = new TemplateCollection();
        }

        public Type Control { get; }

        public StyleCollection Styles { get; }

        public AnimationCollection Animations { get; }

        public TemplateCollection Teamplates { get; }
    }
}