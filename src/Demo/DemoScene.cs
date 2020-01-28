﻿using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Controls;
using Pandora.Interactions.UI.Drawing;
using System;

namespace Pandora
{
    class DemoScene : Scene
    {
        private Button _button;

        public DemoScene()
        { }

        protected override void OnLoad()
        {
            BackgroundColor = new Color(0, 0, 10);

            _button = new Button();
            _button.Size = new Vector2F(100, 20);
            _button.Position = new Vector2F(20, 20);

            Controls.Add(_button);

            _button.MouseClick += _button_MouseClick;
        }

        private void _button_MouseClick(ControlElement element)
        {
            throw new System.NotImplementedException();
        }
    }
}
