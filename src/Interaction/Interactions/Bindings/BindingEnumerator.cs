using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Bindings
{
    public static class BindingEnumerator
    {
        public static IEnumerable<BindingProperty> GetPropertyBindings(object obj)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsAssignableFrom(typeof(BindingProperty)))
                    yield return (BindingProperty)property.GetValue(obj);
            }
        }
    }
}
