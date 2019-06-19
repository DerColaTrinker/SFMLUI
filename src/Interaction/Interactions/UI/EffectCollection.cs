using Pandora.Interactions.UI.Design;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI
{
    public class EffectCollection : ICollection<AnimationEventHook>
    {
        private readonly List<AnimationEventHook> _list = new List<AnimationEventHook>();

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(AnimationEventHook item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(AnimationEventHook item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(AnimationEventHook[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<AnimationEventHook> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(AnimationEventHook item)
        {
            return _list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}