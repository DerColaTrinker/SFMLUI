using Pandora.Interactions.UI;

namespace Pandora.Interactions.Abstractions
{
    public interface IInteractionOptions
    {
        Scene StartScene { get; set; }

        string DefaultFontfile { get; set; }

        void LoadDesign(string v);
    }
}
