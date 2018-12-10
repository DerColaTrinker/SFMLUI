using Pandora.Engine;
using Pandora.Engine.Services;
using Pandora.Interactions.Abstractions;
using Pandora.Interactions.Caching;
using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Drawing2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions
{
    public sealed class InteractionService : RuntimeService
    {
        private InteractionOptions _options;

        public static Font DefaultFont { get; internal set; }

        public InteractionService(Action<IInteractionOptions> options)
        {
            _options = InteractionOptions.Create();
            options?.Invoke(_options);
        }

        public SceneHandler Scenes { get; private set; }

        protected internal override void Initialize(out bool success)
        {
            Scenes = new SceneHandler(this);
            success = Scenes.Initialize();

            foreach (var item in _options.DesignFiles)
                Scenes.Designs.LoadDesign(item);

            if (!string.IsNullOrEmpty(_options.DefaultFontfile))
                DefaultFont = new Font(_options.DefaultFontfile);
        }

        protected internal override void Start()
        {
            if (_options.StartScene != null)
                Scenes.Show(_options.StartScene);
        }

        protected internal override void Stop()
        {
            Scenes.Dispose();
        }

        protected internal override bool StopRequested()
        {
            return true;
        }

        protected internal override void SystemUpdate(PandoraRuntimeHost pandoraRuntimeHost, RuntimeFrameEventArgs args)
        {
            Scenes.SystemUpdate(args.Milliseconds, args.Secounds);
        }
    }
}
