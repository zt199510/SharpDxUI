﻿using SharpdxControl.Controls;
using SharpdxControl.Envir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Scenes
{
    public class LoginScene : DXScene
    {
        public LoginScene(Size size) : base(size)
        {
            BackColour = Color.Black;
            CreateLogin();
        }

        public void CreateLogin()
        {
            var Title = new DXLabel
            {
                Location = new Point(10, 10),
                Text = "聊天设置",
                Parent = this,
                OutlineColour = Color.White,
            };

            new DXLabel
            {
                Location = new Point(Title.Location.X + Title.DisplayArea.Width + 20, 20),
                Text = "聊天设置123123",
                Parent = this,
            };

            new DXButton
            {
                Location = new Point(Title.Location.X + Title.DisplayArea.Width + 20, 50),
                Label = { Text = "小蝌蚪找妈妈" },
                Size = new Size(100, DefaultHeight),
                Parent = this,
            };


            

        }

        public override void Process()
        {
            base.Process();
        }
    }
}
