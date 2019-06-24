using Pandora.Interactions.Caching;
using Pandora.Interactions.Dispatcher;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Design;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;
using System.Linq;
using System.Text;

namespace Pandora.Interactions.UI
{
    public sealed class SceneHandler : ObjectPointer
    {
        private ContextSettings _contextsettings;
        private VideoMode _videomode;
        private ControlCollection _scenes;
        private readonly WindowStyle _windowstyle;
        private string _windowtitle;

        public SceneHandler(InteractionService service) : base(IntPtr.Zero)
        {
            Logger.Normal("[Interaction] Create SceneHandler instance");

            Service = service;
            Cache = new CacheHandler();
            Designs = new DesignHandler();

            // Default Configuration
            _contextsettings.DepthBits = 0;
            _contextsettings.StencilBits = 0;
            _contextsettings.AntialiasingLevel = 1;
            _contextsettings.MajorVersion = 2;
            _contextsettings.MinorVersion = 0;

            // Videomode
            _videomode.Width = 1024;
            _videomode.Height = 786;
            _videomode.BitsPerPixel = 32;

            // Windowstyle
            _windowstyle = WindowStyle.Close | WindowStyle.Titlebar;

            Logger.Trace("[Interaction] Context   : " + _contextsettings.ToString());
            Logger.Trace("[Interaction] Videomode :" + _videomode.ToString();
        }

        #region Runtime

        internal bool Initialize()
        {
            try
            {
                // Try to create the RenderWindow. The SceneManager managed the Window
                Pointer = NativeSFML.sfRenderWindow_create(_videomode, _windowtitle, _windowstyle, ref _contextsettings);

                Logger.Debug("[Interaction] Window created");
            }
            catch (Exception ex)
            {
                Logger.Error($"[Interaction] Error on create window : " + ex.Message);
                return false;
            }

            // Event Dispatcher
            Dispatcher = new EventDispatcher(this);
            RegisterEvents();

            // Render-Target erstellen für das Zeichnen
            Renderer = new WindowRenderer(this);

            // Scenes are also Controls.
            _scenes = new ControlCollection(null);

            return true;
        }

        private void RegisterEvents()
        {
            //TODO: Lostfocus = No Rendering
            //Dispatcher.LostFocus += delegate () { Service.Paused = true; };
            //Dispatcher.GetFocus += delegate () { Service.Paused = false; };
            Dispatcher.Closed += delegate () { Service.Runtime.InitiateStopRequest(); };
        }

        internal void SystemUpdate(float ms, float s)
        {
            // Dispatch the next event
            Dispatcher.DispatchEvents();

            // Update all Anmations
            AnimationHandler.SystemUpdate(ms, s);

            // Clear the Display
            Renderer.Clear(Color.Black);

            // Render the First Scene in Pipe.
            if (_scenes.Count > 0) _scenes[0].InternalRenderUpdate(Renderer);

            // Display
            Renderer.Display();
        }

        protected override void Destroy(bool disposing)
        {
            Logger.Normal("[SceneHandler] Detroy render window");
            NativeSFML.sfRenderWindow_destroy(Pointer);
        }

        #endregion

        /// <summary>
        /// The Event Dispatcher for this Window
        /// </summary>
        internal EventDispatcher Dispatcher { get; private set; }

        public WindowRenderer Renderer { get; private set; }

        public string WindowTitle
        {
            get { return _windowtitle; }
            set
            {
                _windowtitle = value;

                // Nur weitermachen wenn das Fenster bereits erstellt wurde.
                if (Pointer == IntPtr.Zero) return;

                // Copy the title to a null-terminated UTF-32 byte array
                byte[] titleAsUtf32 = Encoding.UTF32.GetBytes(value + '\0');

                unsafe
                {
                    fixed (byte* titlePtr = titleAsUtf32)
                    {
                        NativeSFML.sfRenderWindow_setUnicodeTitle(Pointer, (IntPtr)titlePtr);
                    }
                }
            }
        }

        public uint AntialiasingLevel
        {
            get { return _contextsettings.AntialiasingLevel; }
            set
            {
                if (Pointer != IntPtr.Zero) throw new ArgumentException("AntialiasingLevel could not set on runtime.");

                _contextsettings.AntialiasingLevel = value;
            }
        }

        public uint WindowWidth
        {
            get { return _videomode.Width; }
            set
            {
                if (Pointer != IntPtr.Zero) throw new ArgumentException("Width could not set on runtime.");

                _videomode.Width = value;
            }
        }

        public uint WindowHeight
        {
            get { return _videomode.Height; }
            set
            {
                if (Pointer != IntPtr.Zero) throw new ArgumentException("Height could not set on runtime.");

                _videomode.Height = value;
            }
        }

        public uint WindowBitsPerPixel
        {
            get { return _videomode.BitsPerPixel; }
            set
            {
                if (Pointer != IntPtr.Zero) throw new ArgumentException("BitsPerPixel could not set on runtime.");

                _videomode.BitsPerPixel = value;
            }
        }

        public void SetMouseCursor(Cursor cursor)
        {
            NativeSFML.sfRenderWindow_setMouseCursor(Pointer, cursor.Pointer);
        }

        public InteractionService Service { get; }

        public CacheHandler Cache { get; }

        public bool HasFocus
        {
            get { return NativeSFML.sfRenderWindow_hasFocus(Pointer); }
        }

        public bool IsOpen
        {
            get { return NativeSFML.sfRenderWindow_isOpen(Pointer); }
        }

        public void RequestFocus()
        {
            NativeSFML.sfRenderWindow_requestFocus(Pointer);
        }

        #region Scene Control

        public bool HasScene { get => _scenes.Count > 0; }

        public Scene CurrentScene { get { return _scenes.Cast<Scene>().FirstOrDefault(); } }

        public ControlCollection Scenes { get { return _scenes; } }

        public DesignHandler Designs { get; private set; }

        public void Show(Scene scene)
        {
            // Change visibility to hidden
            foreach (var item in _scenes) item.Visibility = Visibilities.Hidden;

            // new scene ist visible
            scene.Visibility = Visibilities.Display;

            // Reset size
            scene.Size = Renderer.Size;

            // Initilize controls
            scene.InternalOnLoad(this);
            scene.InternalOnShow();

            // Insert scene on top
            _scenes.Insert(0, scene);

            Logger.Debug($"[SceneHandler] Show scene '{scene.GetType().Name}'");
        }

        public void Close()
        {
            Logger.Debug($"[SceneHandler] Close current scene");

            if (_scenes.Count > 0)
                _scenes.RemoveAt(0);

            if (_scenes.Count > 0)
                Show((Scene)_scenes.First());
        }

        #endregion
    }
}
