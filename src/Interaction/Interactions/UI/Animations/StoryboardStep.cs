namespace Pandora.Interactions.UI.Animations
{
    public sealed class StoryboardStep
    {
        public StoryboardStep()
        { }

        public float StartTime { get; set; }

        public float EndTime { get => StartTime + Duration; }

        public float Duration { get => Animation.Duration; }

        public Animation Animation { get; set; }

        public override string ToString()
        {
            return $"Animation {Animation.GetType().Name}";
        }
    }
}