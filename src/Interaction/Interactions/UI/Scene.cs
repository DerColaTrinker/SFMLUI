using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.Bindings;

namespace Pandora.Interactions.UI
{
    public delegate void SceneDelegate(Scene scene);
    public delegate void SceneResizeDelegate(Scene scene, uint x, uint y);

    public class Scene : ControlElement, IContainer
    {
        private RectElement _background;

        public event SceneDelegate WindowMouseEnter;
        public event SceneDelegate WindowMouseLeft;
        public event SceneResizeDelegate Resize;

        protected Scene()
        {
            Controls = new ControlCollection(this);
        }

        private void RegisterBindings()
        {
            BackgroundColorProperty = Bindings.Create("BackgroundColor", () => _background.FillColor, (value) => _background.FillColor = value, Color.Transparent);
        }

        public BindingProperty<Color> BackgroundColorProperty { get; private set; }

        public void Show(Scene scene)
        {
            Handler.Show(scene);
        }

        protected override void Destroy(bool disposing)
        { }

        public ControlCollection Controls { get; }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            // Do not call, since this comes from the UIElement.
            //Controls.InternalOnLoad(handler);

            _background = new RectElement
            {
                AutoScaleOnParent = true
            };

            RegisterBindings();
                 
            Templates.Add(_background);

            base.InternalOnLoad(handler);
        }

        internal virtual void InternalOnShow()
        {
            OnShow();
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            Controls.InternalRenderUpdate(target);

            base.InternalRenderUpdate(target);
        }

        protected virtual void OnShow()
        {

        }

        public Color BackgroundColor
        {
            get { return BackgroundColorProperty.Value; }
            set { BackgroundColorProperty.Value = value; }
        }

        #region Events

        private void RegisterDispatcherEvents()
        {
            Dispatcher.WindowMouseEnter += RegisterWindowMouseEnterEvent;
            Dispatcher.WindowMouseLeft += RegisterWindowLeftEvent;
            Dispatcher.WindowResized += RegisterWindowResizeEvent;
        }

        #region MouseEnter & MouseLeft

        private void RegisterWindowLeftEvent()
        {
            WindowMouseLeft?.Invoke(this);
        }

        private void RegisterWindowMouseEnterEvent()
        {
            WindowMouseEnter?.Invoke(this);
        }

        #endregion

        #region Resize

        private void RegisterWindowResizeEvent(uint x, uint y)
        {
            Resize?.Invoke(this, x, y);
        }

        #endregion

        #endregion
    }
}
