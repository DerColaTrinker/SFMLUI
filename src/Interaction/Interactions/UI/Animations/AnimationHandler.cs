using Pandora.Interactions.UI.Design;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Animations
{
    public static class AnimationHandler
    {
        private static readonly HashSet<Animation> _animations = new HashSet<Animation>();

        internal static void SystemUpdate(float ms, float s)
        {
            if (_animations.Count == 0) return;

            // Use RemoveWhere for direct remove the animation
            _animations.RemoveWhere(animation =>
            {
                if (animation.Status == AnimationStatus.Running)
                {
                    // Call update
                    animation.InternalUpdate(ms, s);
                }

                // Remove animation if complet
                if (animation.IsComplet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public static void StopRange(IEnumerable<Trigger> animations)
        {
            foreach (var item in animations)
            {
                Stop(item.Animation);
            }
        }

        public static void Start(Animation animation)
        {
            if (_animations.Add(animation))
            {
                animation.InternalStart();
            }
            else
            {
                animation.InternalReset();
            }
        }

        public static void Start(IEnumerable<Animation> animations)
        {
            foreach (var item in animations) Start(item);
        }

        public static void Stop(Animation effect)
        {
            if (_animations.Remove(effect))
                effect.InternalStop();
        }

        public static void Stop(IEnumerable<Animation> animations)
        {
            foreach (var item in animations) Stop(item);
        }

        public static void Pause(Animation effect)
        {
            effect.InternalPause();
        }

        public static void Pause(IEnumerable<Animation> animations)
        {
            foreach (var item in animations) Pause(item);
        }

        public static void Reset(Animation effect)
        {
            effect.InternalReset();
        }

        public static void Reset(IEnumerable<Animation> animations)
        {
            foreach (var item in animations) item.InternalReset();
        }

        public static void Clear()
        {
            _animations.Clear();
        }
    }
}
