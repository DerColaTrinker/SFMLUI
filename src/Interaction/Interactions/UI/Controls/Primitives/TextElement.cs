using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Native;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class TextElement : UIElement
    {
        private TextAlignment _textalignment = TextAlignment.TopLeft;
        private Font _font;

        public TextElement() : base(IntPtr.Zero)
        {
            Pointer = NativeSFML.sfText_create();

            RegisterBindings();
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfText_destroy(Pointer);
        }

        #region Bindings

        private void RegisterBindings()
        {
            TextColorBinding = Bindings.Create("TextColor", () => NativeSFML.sfText_getFillColor(Pointer), (value) => NativeSFML.sfText_setFillColor(Pointer, value), Color.White);
            OutlineColorBinding = Bindings.Create("OutlineColor", () => NativeSFML.sfText_getOutlineColor(Pointer), (value) => NativeSFML.sfText_setOutlineColor(Pointer, value), Color.Transparent);
            OutlineThicknessBinding = Bindings.Create("OutlineThickness", () => NativeSFML.sfText_getOutlineThickness(Pointer), (value) => NativeSFML.sfText_setOutlineThickness(Pointer, value), 0F);

            TextBinding = Bindings.Create("Text", InternalTextGetter, InternalTextSetter, "");
            FontSizeBinding = Bindings.Create<uint>("FontSize", () => NativeSFML.sfText_getCharacterSize(Pointer), (value) => NativeSFML.sfText_setCharacterSize(Pointer, value), 12);
            FontBinding = Bindings.Create("Font", () => _font, (value) => { _font = value; NativeSFML.sfText_setFont(Pointer, value.Pointer); }, InteractionService.DefaultFont);
            TextAlignmentBinding = Bindings.Create("TextAlignment", () => _textalignment, (value) => _textalignment = value, TextAlignment.MiddleLeft);
            TextStyleBinding = Bindings.Create("TextStyle", () => NativeSFML.sfText_getStyle(Pointer), (value) => NativeSFML.sfText_setStyle(Pointer, value), FontStyles.Regular);

            SizeBinding.BindingPropertyChanged += delegate (BindingProperty sender, Vector2F value) { ResetTextPosition(); };
            TextBinding.BindingPropertyChanged += delegate (BindingProperty sender, string value) { ResetTextPosition(); };
            FontSizeBinding.BindingPropertyChanged += delegate (BindingProperty sender, uint value) { ResetTextPosition(); };
            FontBinding.BindingPropertyChanged += delegate (BindingProperty sender, Font value) { ResetTextPosition(); };
            TextAlignmentBinding.BindingPropertyChanged += delegate (BindingProperty sender, TextAlignment value) { ResetTextPosition(); };
            TextStyleBinding.BindingPropertyChanged += delegate (BindingProperty sender, FontStyles value) { ResetTextPosition(); };
        }

        private void InternalTextSetter(string value)
        {
            // Copy the string to a null-terminated UTF-32 byte array
            byte[] utf32 = Encoding.UTF32.GetBytes(value + '\0');

            // Pass it to the C API
            unsafe
            {
                fixed (byte* ptr = utf32)
                {
                    NativeSFML.sfText_setUnicodeString(Pointer, (IntPtr)ptr);
                }
            }
        }

        private string InternalTextGetter()
        {
            // Get a pointer to the source string (UTF-32)
            IntPtr source = NativeSFML.sfText_getUnicodeString(Pointer);

            // Find its length (find the terminating 0)
            uint length = 0;
            unsafe
            {
                for (uint* ptr = (uint*)source.ToPointer(); *ptr != 0; ++ptr)
                    length++;
            }

            // Copy it to a byte array
            byte[] sourceBytes = new byte[length * 4];
            Marshal.Copy(source, sourceBytes, 0, sourceBytes.Length);

            // Convert it to a C# string
            return Encoding.UTF32.GetString(sourceBytes);
        }

        public BindingProperty<Color> TextColorBinding { get; private set; }

        public BindingProperty<Color> OutlineColorBinding { get; private set; }

        public BindingProperty<float> OutlineThicknessBinding { get; private set; }

        public BindingProperty<string> TextBinding { get; private set; }

        public BindingProperty<uint> FontSizeBinding { get; private set; }

        public BindingProperty<Font> FontBinding { get; private set; }

        public BindingProperty<TextAlignment> TextAlignmentBinding { get; private set; }

        public BindingProperty<FontStyles> TextStyleBinding { get; private set; }

        #endregion

        private void ResetTextPosition()
        {
            if (Font == null || FontSize == 0 || string.IsNullOrEmpty(Text)) return;

            TextBounds = GetLocalBounds();

            switch (TextAlignment)
            {
                case TextAlignment.TopLeft: PositionOffset = new Vector2F(TextBounds.Left, TextBounds.Top); break;
                case TextAlignment.TopCenter: PositionOffset = new Vector2F((Size.X / 2F) - ((TextBounds.Left + TextBounds.Width) / 2F), TextBounds.Top); break;
                case TextAlignment.TopRight: PositionOffset = new Vector2F(Size.X - (TextBounds.Left + TextBounds.Width), TextBounds.Top); break;

                case TextAlignment.MiddleLeft: PositionOffset = new Vector2F(TextBounds.Left, (Size.Y / 2F) - ((TextBounds.Top + TextBounds.Height) / 2F)); break;
                case TextAlignment.MiddleCenter: PositionOffset = new Vector2F((Size.X / 2F) - ((TextBounds.Left + TextBounds.Width) / 2F), (Size.Y / 2F) - ((TextBounds.Top + TextBounds.Height) / 2F)); break;
                case TextAlignment.MiddleRight: PositionOffset = new Vector2F(Size.X - (TextBounds.Left + TextBounds.Width), (Size.Y / 2F) - ((TextBounds.Top + TextBounds.Height) / 2F)); break;

                case TextAlignment.BottomLeft: PositionOffset = new Vector2F(TextBounds.Left, Size.Y - (TextBounds.Top + TextBounds.Height)); break;
                case TextAlignment.BottomCenter: PositionOffset = new Vector2F((Size.X / 2F) - (TextBounds.Width / 2F), Size.Y - (TextBounds.Top + TextBounds.Height)); break;
                case TextAlignment.BottomRight: PositionOffset = new Vector2F(Size.X - TextBounds.Width, Size.Y - (TextBounds.Top + TextBounds.Height)); break;
            }
        }

        protected RectangleF GetGlobalBounds()
        {
            return Transform.TransformRect(GetLocalBounds());
        }

        protected RectangleF GetLocalBounds()
        {
            return NativeSFML.sfText_getLocalBounds(Pointer);
        }

        public Vector2F FindCharacterPos(uint index)
        {
            return NativeSFML.sfText_findCharacterPos(Pointer, index);
        }

        public RectangleF TextBounds { get; private set; }

        public float LetterSpacing

        {
            get { return NativeSFML.sfText_getLetterSpacing(Pointer); }
            set { NativeSFML.sfText_setLetterSpacing(Pointer, value); }
        }

        public float LineSpacing
        {
            get { return NativeSFML.sfText_getLineSpacing(Pointer); }
            set { NativeSFML.sfText_setLineSpacing(Pointer, value); }
        }

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
            get { return _font; }
            set { FontBinding.Value = value; }
        }

        public TextAlignment TextAlignment
        {
            get { return _textalignment; }
            set { TextAlignmentBinding.Value = value; }
        }

        public FontStyles TextStyle
        {
            get { return TextStyleBinding.Value; }
            set { TextStyleBinding.Value = value; }
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            var states = RenderStates.Default;
            states.Transform *= Transform;
            var marshaledStates = states.Marshal();

            target.DrawText(Pointer, ref marshaledStates);
        }
    }
}
