using Pandora.Interactions.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtime = new DemoRuntime();

            runtime.Initialize();

            StyleHandler.LoadStyle("styletest.xml");

            runtime.Start();
        }
    }
}
