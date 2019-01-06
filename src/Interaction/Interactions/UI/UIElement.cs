using Pandora.Interactions.Bindings;
using Pandora.Interactions.Caching;
using Pandora.Interactions.Dispatcher;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI
{
    public abstract class UIElement : Visual
    {
        private string _stylekey;
        private bool _isloaded = false;
        private IContainer _container;
        private ITemplate _template;

        protected UIElement(IntPtr pointer) : base(pointer)
        {
            RegisterBindings();
            RegisterInterfaces();
        }

        protected UIElement(UIElement copy) : base(copy)
        {
            RegisterBindings();
            RegisterInterfaces();
        }

        private void RegisterInterfaces()
        {
            if (this is IContainer container)
                _container = container;

            if (this is ITemplate template)
                _template = template;
        }

        #region Bindings

        private void RegisterBindings()
        {
            StyleKeyBinding = Bindings.Create("StyleKey", () => _stylekey, (value) => _stylekey = value);
        }

        public BindingProperty<string> StyleKeyBinding { get; private set; }

        #endregion

        internal virtual void InternalOnLoad(SceneHandler handler)
        {
            if (_isloaded) return;

            Handler = handler;

            OnLoad();

            _template?.Templates.InternalOnLoad(handler);
            _container?.Controls.InternalOnLoad(handler);

            UpdatePosition();

            _isloaded = true;
        }

            internal virtual void InternalRenderUpdate(RenderTargetBase target)
        {
            _template?.Templates.InternalRenderUpdate(target);
            _container?.Controls.InternalRenderUpdate(target);
        }

        internal override void InternalSetParent(Visual parent)
        {
            base.InternalSetParent(parent);

            UpdatePosition();
        }

        protected virtual void ElementDimensionChange()
        { }

        protected virtual void OnLoad()
        { }

        public override Vector2F Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;

                UpdatePosition();
            }
        }

        protected internal override void UpdatePosition()
        {
            base.UpdatePosition();

            _template?.Templates.InternalPositionUpdate();
            _container?.Controls.InternalPositionUpdate();
        }

        internal SceneHandler Handler { get; set; }

        internal EventDispatcher Dispatcher { get { return Handler.Dispatcher; } }

        public InteractionService Manager { get { return Handler.Service; } }

        public new UIElement Parent { get { return (UIElement)base.Parent; } }

        public CacheHandler Cache { get { return Handler.Cache; } }

        #region Style

        public string StyleKey
        {
            get { return _stylekey; }
            set { StyleKeyBinding.Value = value; }
        }

        public string TotelStyleKey { get { return string.IsNullOrEmpty(_stylekey) ? string.Empty : Parent.TotelStyleKey + "/" + _stylekey; } }
        
        #endregion
    }
}
