using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;

namespace Pandora.Interactions.UI.Controls
{
    public class Button : Control
    {
        private RectElement _rect;
        private TextElement _text;

        public Button()
        {
            _rect = new RectElement
            {
                AutoScaleOnParent = true
            };

            _text = new TextElement
            {
                AutoScaleOnParent = true
            };

            Templates.Add(_rect);
            Templates.Add(_text);

            RegisterBindings();

            Text = "Button";
            TextAlignment = TextAlignment.MiddleCenter;
            FillColor = new Color(60, 0, 0);
            BorderOutlineThickness = 2;
            BorderOutlineColor = Color.Green;
        }

        private void RegisterBindings()
        {
            TextColorBinding = Bindings.Create("TextColor", _text.TextColorBinding);
            TextOutlineColorBinding = Bindings.Create("OutlineColor", _text.OutlineColorBinding);
            TextOutlineThicknessBinding = Bindings.Create("OutlineThickness", _text.OutlineThicknessBinding);

            TextBinding = Bindings.Create("Text", _text.TextBinding);
            FontSizeBinding = Bindings.Create("FontSize", _text.FontSizeBinding);
            FontBinding = Bindings.Create("Font", _text.FontBinding);
            TextAlignmentBinding = Bindings.Create("TextAlignment", _text.TextAlignmentBinding);
            TextStyleBinding = Bindings.Create("TextStyle", _text.TextStyleBinding);

            FillColorBinding = Bindings.Create("FillColor", _rect.FillColorBinding);
            BorderOutlineColorBinding = Bindings.Create("BorderOutlineColorBinding", _rect.OutlineColorBinding);
            BorderOutlineThicknessBinding = Bindings.Create("BorderOutlineThicknessBinding", _rect.OutlineThicknessBinding);
        }

        public BindingProperty<Color> TextColorBinding { get; private set; }

        public BindingProperty<Color> TextOutlineColorBinding { get; private set; }

        public BindingProperty<float> TextOutlineThicknessBinding { get; private set; }

        public BindingProperty<string> TextBinding { get; private set; }

        public BindingProperty<uint> FontSizeBinding { get; private set; }

        public BindingProperty<Font> FontBinding { get; private set; }

        public BindingProperty<TextAlignment> TextAlignmentBinding { get; private set; }

        public BindingProperty<FontStyles> TextStyleBinding { get; private set; }

        public BindingProperty<Color> FillColorBinding { get; private set; }

        public BindingProperty<Color> BorderOutlineColorBinding { get; private set; }

        public BindingProperty<float> BorderOutlineThicknessBinding { get; private set; }

        public Color TextColor
        {
            get { return TextColorBinding.Value; }
            set { TextColorBinding.Value = value; }
        }

        public Color TextOutlineColor
        {
            get { return TextOutlineColorBinding.Value; }
            set { TextOutlineColorBinding.Value = value; }
        }

        public float TextOutlineThickness
        {
            get { return TextOutlineThicknessBinding.Value; }
            set { TextOutlineThicknessBinding.Value = value; }
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

        public Color FillColor
        {
            get { return FillColorBinding.Value; }
            set { FillColorBinding.Value = value; }
        }

        public Color BorderOutlineColor
        {
            get { return BorderOutlineColorBinding.Value; }
            set { BorderOutlineColorBinding.Value = value; }
        }

        public float BorderOutlineThickness
        {
            get { return BorderOutlineThicknessBinding.Value; }
            set { BorderOutlineThicknessBinding.Value = value; }
        }
    }
}
