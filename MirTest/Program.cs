using SharpDX.Windows;
using SharpdxControl;
using SharpdxControl.Controls;
using SharpdxControl.Enums;
using SharpdxControl.Envir;
using SharpdxControl.Librarys;
using SharpdxControl.Scenes;
using System.Runtime.InteropServices;

namespace MirTest
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主要入口点
        /// </summary>
        [STAThread]
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            foreach (KeyValuePair<LibraryFile, string> pair in Libraries.LibraryList)
            {
                if (!File.Exists(@".\" + pair.Value)) continue;

                CEnvir.LibraryList[pair.Key] = new MirLibrary(@".\" + pair.Value);
            }

            CEnvir.Target = new TargetForm();//实例化窗口
            DXManager.Create();//创建画面管理
            CEnvir.Target.MaximizeBox = true;
            CEnvir.Target.FormBorderStyle = FormBorderStyle.Fixed3D;

            DXControl.ActiveScene = new LoginScene(new Size(1024,768));

            RenderLoop.Run(CEnvir.Target, CEnvir.GameLoop);
        }
    }
}