using Pandora.Engine;
using Pandora.Engine.Services;
using Pandora.Interactions.Abstractions;
using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Drawing2D;
using System;

namespace Pandora.Interactions
{
    public sealed class InteractionService : RuntimeService
    {
        private readonly InteractionOptions _options;

        public static Font DefaultFont { get; internal set; }

        public InteractionService(Action<IInteractionOptions> options)
        {
            Logger.Normal("[Interaction] Create service instance");
            _options = InteractionOptions.Create();
            options?.Invoke(_options);
        }

        public SceneHandler Scenes { get; private set; }

        protected override void Initialize(out bool success)
        {
            Logger.Debug("[Interaction] Initializing");

            Scenes = new SceneHandler(this);
            success = Scenes.Initialize();

            foreach (var item in _options.DesignFiles)
                Scenes.Designs.Load(item);

            if (!string.IsNullOrEmpty(_options.DefaultFontfile))
                DefaultFont = new Font(_options.DefaultFontfile);
        }

        protected override void Start()
        {
            if (_options.InternalStartScene != null)
                Scenes.Show((Scene)Activator.CreateInstance(_options.InternalStartScene));
        }

        protected override void Stop()
        {
            Scenes.Dispose();
        }

        protected override bool StopRequested()
        {
            return true;
        }

        protected override void SystemUpdate(PandoraRuntimeHost host, float ms, float s)
        {
            Scenes.SystemUpdate(ms, s);
        }
    }
}
