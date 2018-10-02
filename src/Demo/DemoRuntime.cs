using Pandora.Engine;
using Pandora.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            });

            Services.Add(Interaction);
        }

        public InteractionService Interaction { get; private set; }
    }
}
