namespace Pandora.Interactions.UI.Animations
{
    public sealed class StoryboardStep
    {
        public StoryboardStep()
        {
        }

        public float StartTime { get; set; }
        public float Duration { get; set; }
        public Animation Animation { get; set; }
    }
}