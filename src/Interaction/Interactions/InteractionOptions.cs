﻿using Pandora.Interactions.Abstractions;
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
        private List<string> _designfiles = new List<string>();

        private InteractionOptions()
        { }

        internal static InteractionOptions Create()
        {
            return new InteractionOptions();
        }

        public Scene StartScene { get; set; }

        public string DefaultFontfile { get; set; }

        public IEnumerable<string> DesignFiles { get { return _designfiles; } }

        public void LoadDesign(string filename)
        {
            _designfiles.Add(filename);
        }
    }
}
