using Pandora.Interactions.Bindings;
using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls
{
    public class TextBox : Control
    {
        private RectElement _rect;
        private TextElement _text;

        public TextBox()
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

            AllowFocus = true;

            Text = "";
            TextAlignment = TextAlignment.MiddleCenter;
            FillColor = new Color(60, 0, 0);
            BorderOutlineThickness = 2;
            BorderOutlineColor = Color.Green;
        }

        private void RegisterBindings()
        {
            TextColorBinding = new BindingProperty<Color>("TextColor", _text.TextColorBinding);
            TextOutlineColorBinding = new BindingProperty<Color>("OutlineColor", _text.OutlineColorBinding);
            TextOutlineThicknessBinding = new BindingProperty<float>("OutlineThickness", _text.OutlineThicknessBinding);

            TextBinding = new BindingProperty<string>("Text", _text.TextBinding);
            FontSizeBinding = new BindingProperty<uint>("FontSize", _text.FontSizeBinding);
            FontBinding = new BindingProperty<Font>("Font", _text.FontBinding);
            TextAlignmentBinding = new BindingProperty<TextAlignment>("TextAlignment", _text.TextAlignmentBinding);
            TextStyleBinding = new BindingProperty<FontStyles>("TextStyle", _text.TextStyleBinding);

            FillColorBinding = new BindingProperty<Color>("FillColor", _rect.FillColorBinding);
            BorderOutlineColorBinding = new BindingProperty<Color>("OutlineColor", _rect.OutlineColorBinding);
            BorderOutlineThicknessBinding = new BindingProperty<float>("OutlineThickness", _rect.OutlineThicknessBinding);
        }

        protected override void OnFocusKeyPress(char unicode)
        {
            if (char.IsControl(unicode))
            {
               switch (unicode)
                {
                    case '\b':
                        if (Text.Length > 0)
                            Text = Text.Substring(0, Text.Length - 1);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Text += unicode;
            }
        }

        protected override void OnGetFocus()
        {
            FillColor = new Color(128, 0, 0);
        }

        protected override void OnLostFocus()
        {
            FillColor = new Color(60, 0, 0);
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
