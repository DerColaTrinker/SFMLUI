using Pandora.Interactions.UI.Design.Converter;
using System;

namespace Pandora.Interactions.UI.Design
{
    internal class DesignContainer
    {
        public DesignContainer(Type control)
        {
            Control = control;
        }

        public Type Control { get; }

        private static ConverterBase Converter(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    switch (type.Name)
                    {
                        case "Color":
                            return ColorConverter.Converter;

                        case "Vector2":
                        case "Vector2F":
                        case "Vector2U":
                            return VectorConverter.Converter;

                        default:
                            throw new Exception($"Binding type '{type.Name}' not supported");
                    }

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return new DefaultConverter(type);

                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    throw new Exception($"Binding type '{type.Name}' not supported");
            }
        }
    }
}