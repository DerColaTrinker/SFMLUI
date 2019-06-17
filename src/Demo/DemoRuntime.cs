using Pandora.Engine;
using Pandora.Interactions;
using System;
using System.IO;

namespace Pandora
{
    public sealed class DemoRuntime : PandoraRuntimeHost
    {
        public void Initialize()
        {
            Interaction = new InteractionService((option) =>
            {
                option.DefaultFontfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                option.StartScene = new DemoScene();
                option.LoadDesign("styletest.xml");
                option.LoadDesign("templatetest.xml");
            });

            Services.Add(Interaction);
        }

        public InteractionService Interaction { get; private set; }
    }
}
