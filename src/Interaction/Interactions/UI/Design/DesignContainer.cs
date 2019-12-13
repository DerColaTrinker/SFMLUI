﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Design
{
    internal class DesignContainer
    {
        internal class StyleCollection : IEnumerable<PropertySetterContainer>
        {
            private readonly Dictionary<string, PropertySetterContainer> _properties = new Dictionary<string, PropertySetterContainer>();

            internal void Add(PropertySetterContainer style)
            {
                _properties[style.BindingName] = style;
            }

            internal void AddRange(IEnumerable<PropertySetterContainer> enumerable)
            {
                foreach (var item in enumerable)
                {
                    _properties[item.BindingName] = item;
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
            private readonly Dictionary<TriggerEvents, AnimationContainer> _animations = new Dictionary<TriggerEvents, AnimationContainer>();

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

        internal class TemplateCollection : IEnumerable<DesignContainer>
        {
            private readonly List<DesignContainer> _controls = new List<DesignContainer>();

            internal void Add(DesignContainer control)
            {
                _controls.Add(control);
            }

            internal void AddRange(IEnumerable<DesignContainer> enumerable)
            {
                _controls.AddRange(enumerable);
            }

            public IEnumerator<DesignContainer> GetEnumerator()
            {
                return _controls.GetEnumerator();
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
            Animations = new AnimationCollection();
            Templates = new TemplateCollection();
        }

        public Type Control { get; }

        public StyleCollection Styles { get; }

        public AnimationCollection Animations { get; }

        public TemplateCollection Templates { get; }
    }
}