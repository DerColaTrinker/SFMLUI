﻿using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Controls;
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
        private Button _button;

        public DemoScene()
        { }

        protected override void OnLoad()
        {
            BackgroundColor = new Color(0, 0, 80);

            _label1 = new Label
            {
                Text = "Hello World...",
                TextColor = Color.Red,
                Position = new Vector2F(5, 5)
            };

            _button = new Button
            {
                Position = new Vector2F(20, 20),
                Size = new Vector2F(150, 40),
                Text = "Demo Button",
                FillColor = new Color(20, 20, 20)
            };

            Controls.Add(_label1);
            Controls.Add(_button);

            _button.MouseButtonUp += _button_MouseButtonUp;
        }

        private void _button_MouseButtonUp(ControlElement element, Interactions.Controller.MouseButton button, float x, float y)
        {
            if (element is Button b)
            {
                b.TextColor = Color.Yellow;
            }
        }
    }
}
