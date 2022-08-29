using SharpdxControl.Control;
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
                AutoSize = false,
                Size = new Size(100, 20),
                Location=new Point(10,10),
                BackColour = Color.Wheat,
                Outline = false,
                Text = "聊天设置",
                Font = new Font("宋体", CEnvir.FontSize(9f), FontStyle.Bold),
                DrawFormat = (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter)
            };
        }


        public override void Process()
        {
            base.Process();
        }
    }
}
