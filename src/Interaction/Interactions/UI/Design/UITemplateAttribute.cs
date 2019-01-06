using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Design
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class UITemplateAttribute : Attribute
    {
        public UITemplateAttribute(string templatename)
        {
            Template = templatename;
        }

        public string Template { get; }
    }
}
