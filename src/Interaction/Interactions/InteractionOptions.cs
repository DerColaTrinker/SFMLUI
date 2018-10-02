using Pandora.Interactions.Abstractions;
using Pandora.Interactions.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions
{
    public class InteractionOptions : IInteractionOptions
    {
        private InteractionOptions()
        { }

        public Scene StartScene { get; set; }

        public string DefaultFontfile { get;  set; }

        internal static InteractionOptions Create()
        {
            return new InteractionOptions();
        }
    }
}
