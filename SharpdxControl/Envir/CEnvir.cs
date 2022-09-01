using Library;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using SharpdxControl.Control;

using SharpdxControl.SharpDXs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Envir
{
    public static class CEnvir
    {
        public static string TargetText; //目标文本

        public static RenderForm Target;//目标窗口

        /// <summary>
        /// FPS时间
        /// </summary>
        private static DateTime _FPSTime;
        private static int FPSCounter;
        public static int FPSCount;
        public static int Blends;
        public static int DPSCounter;
        private static int DPSCount;
        #region 平滑移动
        public static int Loopdelay;
        public static DateTime LastLoopTime;
        #endregion


        public static DateTime Now;
        /// <summary>
        /// 鼠标移动坐标
        /// </summary>
        public static Point MouseLocation;
        /// <summary>
        /// 更新画面状态
        /// </summary>
        static void UpdateGame()
        {
            Now = Time.Now;
            ///DXControl.ActiveScene?.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, MouseLocation.X, MouseLocation.Y, 0));
            if (Time.Now >= _FPSTime)
            {
                _FPSTime = Time.Now.AddSeconds(1);
                FPSCount = FPSCounter;
                FPSCounter = 0;
                DPSCount = DPSCounter;
                DPSCounter = 0;
                DXManager.MemoryClear();
            }

            DXControl.ActiveScene?.Process();

            //if (Config.Mode == 0)
            //{
            //    count = FPSCount >= 100 ? 100 : FPSCount;
            //    debugText = $"[FPS: {count}]";
            //}
            //else
            //{
            //    debugText = $"[FPS: {FPSCount}]";
            //}
        }

        /// <summary>
        /// 渲染画面
        /// </summary>
        static void RenderGame()
        {
            if (Target.ClientSize.Width == 0 || Target.ClientSize.Height == 0)
            {
                Thread.Sleep(1);
                return;
            }

            DXManager.Device.Clear(ClearFlags.Target, Color.Black.ToRawColorBGRA(), 1, 0);
            DXManager.Device.BeginScene();
            DXManager.Sprite.Begin(SpriteFlags.AlphaBlend);
            DXControl.ActiveScene?.Draw();
            DXManager.Sprite.End();
            DXManager.Device.EndScene();
            DXManager.Device.Present();
            FPSCounter++;
        }
        public static void GameLoop()
        {
            UpdateGame();
            RenderGame();
            if (true)
                Thread.Sleep(1);

            CEnvir.LastLoopTime = Time.Now;
            CEnvir.Loopdelay = (int)(CEnvir.LastLoopTime - CEnvir.Now).TotalMilliseconds;
        }



        public static float FontSize(float size)
        {
            try
            {
                return (size - 0.0F) * (96F / DXManager.Graphics.DpiX);
            }
            catch (Exception)
            {

                return (size - 0.0F) * (96F / 96);
            }
        }
    }
}
