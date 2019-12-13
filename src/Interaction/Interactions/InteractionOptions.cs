using Pandora.Interactions.Abstractions;
using Pandora.Interactions.UI;
using System;
using System.Collections.Generic;

namespace Pandora.Interactions
{
    public class InteractionOptions : IInteractionOptions
    {
        private readonly List<string> _designfiles = new List<string>();

        private InteractionOptions()
        {
            Logger.Trace("Create interaction configuration object");
        }

        internal static InteractionOptions Create()
        {
            return new InteractionOptions();
        }

        public string DefaultFontfile { get; set; }

        public IEnumerable<string> DesignFiles { get { return _designfiles; } }

        internal Type InternalStartScene { get; private set; }

        public void LoadDesign(string filename)
        {
            _designfiles.Add(filename);
        }

        public void StartScene<TScene>() where TScene : Scene
        {
            InternalStartScene = typeof(TScene);
        }
    }
}
