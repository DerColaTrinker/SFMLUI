using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Design.Converter;

namespace Pandora.Interactions.UI.Design
{
    public class DesignHandler
    {
        private readonly Dictionary<string, string> _resources = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private Dictionary<Type, DesignContainer> _controls = new Dictionary<Type, DesignContainer>();

        public void LoadDesign(string filename)
        {
            var xd = new XmlDocument();
            xd.Load(filename);

            ParseResources(xd);
            ParseXmlControls(xd.SelectNodes("controls/control"));
        }

        private void ParseResources(XmlDocument xd)
        {
            foreach (XmlNode resnode in xd.SelectNodes("controls/resource"))
            {
                var name = resnode.Attributes.GetValue("name");
                var value = resnode.Attributes.GetValue("value");

                _resources[name] = value;
            }
        }

        private void ParseXmlControls(XmlNodeList controlnodes)
        {
            foreach (XmlNode controlnode in controlnodes)
            {
                var controlname = controlnode.Attributes.GetValue("name");

                var control = (from a in AppDomain.CurrentDomain.GetAssemblies()
                               from t in a.GetTypes()
                               where t.IsClass && t.IsSubclassOf(typeof(ControlElement)) && t.FullName == controlname
                               select t).FirstOrDefault();
                if (control == null) throw new Exception($"Control '{controlname}' not found");

                if (!_controls.TryGetValue(control, out DesignContainer container))
                {
                    container = new DesignContainer(control);
                    _controls.Add(control, container);
                }

                ParseXmlTemplates(container, controlnode.SelectNodes("templates/element"));
                ParseXmlStyles(container, controlnode.SelectNodes("style"));
                ParseXmlAnimation(container, controlnode.SelectNodes("animation"));
            }
        }

        private void ParseXmlAnimation(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }

        private void ParseXmlStyles(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }

        private void ParseXmlTemplates(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }
    }
}