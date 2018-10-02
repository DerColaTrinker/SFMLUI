using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public static class AnimationHandler
    {
        private static HashSet<Animation> _animations = new HashSet<Animation>();

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

        public static void Start(Animation effect)
        {
            if (_animations.Add(effect))
            {
                effect.InternalStart();
            }
            else
            {
                effect.InternalReset();
            }
        }

        public static void Start(IEnumerable<Animation> effects)
        {
            foreach (var item in effects) Start(item);
        }

        public static void Stop(Animation effect)
        {
            if (_animations.Remove(effect))
                effect.InternalStop();
        }

        public static void Stop(IEnumerable<Animation> effects)
        {
            foreach (var item in effects) Stop(item);
        }

        public static void Pause(Animation effect)
        {
            effect.InternalPause();
        }

        public static void Pause(IEnumerable<Animation> effects)
        {
            foreach (var item in effects) Pause(item);
        }

        public static void Reset(Animation effect)
        {
            effect.InternalReset();
        }

        public static void Reset(IEnumerable<Animation> effects)
        {
            foreach (var item in effects) item.InternalReset();
        }
    }
}
