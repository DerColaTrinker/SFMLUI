using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Controls;
using Pandora.Interactions.UI.Controls.Primitives;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora
{
    class DemoScene : Scene
    {
        private Label _label1;
        private ButtonTest _button;
        private TextBox _textbox;
        private RectElement _testbox;
        private StoryboardAnimation _story;

        public DemoScene()
        { }

        protected override void OnLoad()
        {
            BackgroundColor = new Color(0, 0, 10);

            //_label1 = new Label
            //{
            //    Text = "Hello World...",
            //    TextColor = Color.Red,
            //    Position = new Vector2F(5, 5)
            //};

            _button = new ButtonTest
            {
                Position = new Vector2F(120, 120),
                Size = new Vector2F(150, 40)
            };

            _button.Bindings.Accessor.TextColor = new Color(255, 0, 255);
            

            _button.MouseClick += _button_MouseClick;

            //_textbox = new TextBox
            //{
            //    Position = new Vector2F(0, 60),
            //    Size = new Vector2F(150, 40),
            //    Text = "",
            //    FillColor = new Color(20, 20, 20)
            //};

            //_testbox = new RectElement(new Vector2F(50, 50))
            //{
            //    Position = new Vector2F(200, 200),
            //    Origin = new Vector2F(25, 25),
            //    FillColor = Color.Blue
            //};

            //_story = new StoryboardAnimation();
            //_story.Add(0, new ColorAnimation(_testbox.FillColorBinding, Color.Red, 2500));
            //_story.Add(2000, new FloatAnimation(_testbox.RotationBinding, 90, 1000));
            //_story.Add(2500, new ColorAnimation(_testbox.FillColorBinding, Color.Yellow, 5000));
            //_story.Add(4000, new FloatAnimation(_testbox.RotationBinding, 180, 1000));
            //_story.Add(5000, new ColorAnimation(_testbox.FillColorBinding, Color.Green, 7500));
            //_story.Add(6000, new FloatAnimation(_testbox.RotationBinding, 270, 1000));
            //_story.Add(7500, new ColorAnimation(_testbox.FillColorBinding, Color.Blue, 10000));
            //_story.Add(8000, new FloatAnimation(_testbox.RotationBinding, 360, 1000));

            //Controls.Add(_label1);
            Controls.Add(_button);
            //Controls.Add(_textbox);

            //Templates.Add(_testbox);
        }

        private void _button_MouseClick(ControlElement element)
        {
            AnimationHandler.Start(_story);
        }
    }
}
