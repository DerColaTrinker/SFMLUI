using Pandora.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Configuration
{
    public class InteractionConfiguration : ConfigurationBase
    {
        public InteractionConfiguration() : base()
        { }

        public override string Section => "Interaction";

        public List<string> DesignFiles { get; internal set; }

        public string DefaultFontfile { get; internal set; }
    }
}
