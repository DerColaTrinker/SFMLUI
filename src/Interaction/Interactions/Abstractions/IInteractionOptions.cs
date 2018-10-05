using Pandora.Interactions.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Abstractions
{
    public interface IInteractionOptions
    {
        Scene StartScene { get; set; }

        string DefaultFontfile { get; set; }

        void LoadDesign(string v);
    }
}
