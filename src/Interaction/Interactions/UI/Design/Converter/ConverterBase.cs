﻿namespace Pandora.Interactions.UI.Design.Converter
{
    public abstract class ConverterBase
    {
        public virtual object ConvertFromString(string value)
        {
            return value;
        }

        public virtual string ConvertToString(object value)
        {
            return value.ToString();
        }
    }
}
