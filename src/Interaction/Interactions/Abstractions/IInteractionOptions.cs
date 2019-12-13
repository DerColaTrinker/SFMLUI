using Pandora.Interactions.UI;

namespace Pandora.Interactions.Abstractions
{
    public interface IInteractionOptions
    {
        string DefaultFontfile { get; set; }

        void LoadDesign(string v);

        void StartScene<TScene>() where TScene : Scene;
    }
}
