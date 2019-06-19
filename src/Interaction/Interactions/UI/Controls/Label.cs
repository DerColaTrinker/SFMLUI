using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;

namespace Pandora.Interactions.UI.Controls
{
    public class Label : Control
    {
        private readonly TextElement _text;

        public Label()
        {
            // Visual Element
            _text = new TextElement
            {
                AutoScaleOnParent = true
            };

            Templates.Add(_text);

            RegisterBindings();
        }

        private void RegisterBindings()
        {
            TextColorBinding = Bindings.Create("TextColor", _text.TextColorBinding);
            OutlineColorBinding = Bindings.Create("OutlineColor", _text.OutlineColorBinding);
            OutlineThicknessBinding = Bindings.Create("OutlineThickness", _text.OutlineThicknessBinding);

            TextBinding = Bindings.Create("Text", _text.TextBinding);
            FontSizeBinding = Bindings.Create("FontSize", _text.FontSizeBinding);
            FontBinding = Bindings.Create("Font", _text.FontBinding);
            TextAlignmentBinding = Bindings.Create("TextAlignment", _text.TextAlignmentBinding);
            TextStyleBinding = Bindings.Create("TextStyle", _text.TextStyleBinding);
        }

        public BindingProperty<Color> TextColorBinding { get; private set; }

        public BindingProperty<Color> OutlineColorBinding { get; private set; }

        public BindingProperty<float> OutlineThicknessBinding { get; private set; }

        public BindingProperty<string> TextBinding { get; private set; }

        public BindingProperty<uint> FontSizeBinding { get; private set; }

        public BindingProperty<Font> FontBinding { get; private set; }

        public BindingProperty<TextAlignment> TextAlignmentBinding { get; private set; }

        public BindingProperty<FontStyles> TextStyleBinding { get; private set; }

        public Color TextColor
        {
            get { return TextColorBinding.Value; }
            set { TextColorBinding.Value = value; }
        }

        public Color OutlineColor
        {
            get { return OutlineColorBinding.Value; }
            set { OutlineColorBinding.Value = value; }
        }

        public float OutlineThickness
        {
            get { return OutlineThicknessBinding.Value; }
            set { OutlineThicknessBinding.Value = value; }
        }

        public virtual string Text
        {
            get { return TextBinding.Value; }
            set { TextBinding.Value = value; }
        }

        public virtual uint FontSize
        {
            get { return FontSizeBinding.Value; }
            set { FontSizeBinding.Value = value; }
        }

        public virtual Font Font
        {
            get { return FontBinding.Value; }
            set { FontBinding.Value = value; }
        }

        public TextAlignment TextAlignment
        {
            get { return TextAlignmentBinding.Value; }
            set { TextAlignmentBinding.Value = value; }
        }

        public FontStyles TextStyle
        {
            get { return TextStyleBinding.Value; }
            set { TextStyleBinding.Value = value; }
        }
    }
}
