using SharpDX.Windows;
using SharpdxControl;
using SharpdxControl.Control;
using SharpdxControl.Envir;
using SharpdxControl.Scenes;
using System.Runtime.InteropServices;

namespace MirTest
{
    internal static class Program
    {
        /// <summary>
        /// Ӧ�ó������Ҫ��ڵ�
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
            CEnvir.Target = new TargetForm();//ʵ��������
            DXManager.Create();//�����������
            CEnvir.Target.MaximizeBox = true;
            CEnvir.Target.FormBorderStyle = FormBorderStyle.Fixed3D;

            DXControl.ActiveScene = new LoginScene(new Size(640,480));

            RenderLoop.Run(CEnvir.Target, CEnvir.GameLoop);
        }
    }
}