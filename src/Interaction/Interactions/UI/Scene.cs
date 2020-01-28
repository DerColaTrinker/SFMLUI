using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;

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

        public event SceneDelegate SceneShow;
        public event SceneDelegate SceneClose;

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

        public void Close()
        {
            Handler.Close();
        }

        protected override void Destroy(bool disposing)
        { }

        public ControlCollection Controls { get; }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            if (_background == null)
            {
                _background = new RectElement
                {
                    AutoScaleOnParent = true
                };

                RegisterBindings();

                Templates.Add(_background);
            }

            base.InternalOnLoad(handler);
        }

        internal virtual void InternalOnShow()
        {
            OnShow();
        }

        internal virtual void InternalOnClose()
        {
            OnClose();
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            Controls.InternalRenderUpdate(target);

            base.InternalRenderUpdate(target);
        }

        protected virtual void OnShow()
        {
            SceneShow?.Invoke(this);
        }

        protected virtual void OnClose()
        {
            SceneClose?.Invoke(this);
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
