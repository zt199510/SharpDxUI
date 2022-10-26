using SharpDX.Direct3D9;
using SharpdxControl.Envir;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using SharpdxControl.Controls;
using Blend = SharpDX.Direct3D9.Blend;
using SharpdxControl.SharpDXs;
using SharpDX;
using SharpdxControl.Librarys;
using SharpdxControl.Enums;

namespace SharpdxControl
{

    /// <summary>
    /// DX管理
    /// </summary>
    public static class DXManager
    {
        /// <summary>
        /// 图形样本容器
        /// </summary>
        public static Graphics Graphics { get; private set; }
        //用于设定与将内容呈现到显示器上的相关的参数
        public static PresentParameters Parameters { get; private set; }
        //显示适配器性能
        public static Device Device { get; private set; }
        /// <summary>
        /// 精灵图片
        /// </summary>
        public static Sprite Sprite { get; private set; }

        public static Line Line { get; private set; }
        public static Surface CurrentSurface { get; private set; }
        public static Surface MainSurface { get; private set; }
        public static Texture ScratchTexture;

        public static Surface ScratchSurface;
        public static float Opacity { get; private set; } = 1F;

        public static bool Blending { get; private set; }
        public static float BlendRate { get; private set; } = 1F;
        public static BlendMode BlendMode { get; private set; } = BlendMode.NORMAL;

        public static List<DXControl> ControlList { get; } = new List<DXControl>();
        public static List<MirImage> TextureList { get; } = new List<MirImage>();
        // public static List<DXSound> SoundList { get; } = new List<DXSound>();

   

        public static Texture PoisonTexture;
        static DXManager()
        {
            Graphics = Graphics.FromHwnd(IntPtr.Zero);
            ConfigureGraphics(Graphics);
        }


        /// <summary>
        /// 设置分辨率
        /// </summary>
        /// <param name="size"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void SetResolution(Size size)
        {
            if (CEnvir.Target.ClientSize == size) return;
            Device.Clear(ClearFlags.Target, System.Drawing.Color.Empty.ToRawColorBGRA(), 0, 0);
            Device.Present();
            CEnvir.Target.ClientSize = size;
            CEnvir.Target.MaximizeBox = false;
        }

        public static void ConfigureGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//文本质量
            graphics.CompositingQuality = CompositingQuality.HighQuality;//绘制质量
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//插补模式
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;//偏移方式
            graphics.TextContrast = 0;//灰度校正
        }

        public static void Create()
        {
            PresentParameters obj = new PresentParameters
            {
                BackBufferFormat = Format.X8R8G8B8,
                PresentFlags = PresentFlags.LockableBackBuffer,
                SwapEffect = SwapEffect.Discard,
                Windowed = true,//窗口化
            };
            ///性能模式选择
            obj.PresentationInterval = PresentInterval.Immediate;
            obj.BackBufferWidth = CEnvir.Target.ClientSize.Width;
            obj.BackBufferHeight = CEnvir.Target.ClientSize.Height;
            obj.PresentFlags = PresentFlags.LockableBackBuffer;
            Parameters = obj;
            Direct3D direct3D = new Direct3D();

            Device = new Device(direct3D, direct3D.Adapters.First().Adapter, DeviceType.Hardware, CEnvir.Target.Handle, CreateFlags.HardwareVertexProcessing, Parameters);
            LoadTextures();
            //此方法允许在全屏模式应用程序中使用GDI对话框。
            Device.DialogBoxMode = true;
        }
        private static unsafe void LoadTextures()
        {
            Sprite = new Sprite(Device);
            Line = new Line(Device) { Width = 1F };
            MainSurface = Device.GetBackBuffer(0, 0);
            CurrentSurface = MainSurface;
            Device.SetRenderTarget(0, MainSurface);
            PoisonTexture = new Texture(Device, 6, 6, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            int* data  = (int*)PoisonTexture.LockRectangle(0, LockFlags.Discard).DataPointer;


            for (int y = 0; y < 6; y++)
                for (int x = 0; x < 6; x++)
                    data[y * 6 + x] = x == 0 || y == 0 || x == 5 || y == 5 ? -16777216 : -1;

            ScratchTexture = new Texture(Device, Parameters.BackBufferWidth, Parameters.BackBufferHeight, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            ScratchSurface = ScratchTexture.GetSurfaceLevel(0);
        }
    



        private static void CleanUp()
        {
            //if (Sprite != null)
            //{
            //    if (!Sprite.Disposed)
            //        Sprite.Dispose();

            //    Sprite = null;
            //}

            //if (Line != null)
            //{
            //    if (!Line.Disposed)
            //        Line.Dispose();

            //    Line = null;
            //}

            //if (CurrentSurface != null)
            //{
            //    if (!CurrentSurface.Disposed)
            //        CurrentSurface.Dispose();

            //    CurrentSurface = null;
            //}

            //if (_ColourPallete != null)
            //{
            //    if (!_ColourPallete.Disposed)
            //        _ColourPallete.Dispose();

            //    _ColourPallete = null;
            //}

            //if (ScratchTexture != null)
            //{
            //    if (!ScratchTexture.Disposed)
            //        ScratchTexture.Dispose();

            //    ScratchTexture = null;
            //}

            //if (ScratchSurface != null)
            //{
            //    if (!ScratchSurface.Disposed)
            //        ScratchSurface.Dispose();

            //    ScratchSurface = null;
            //}

            //if (PoisonTexture != null)
            //{
            //    if (!PoisonTexture.Disposed)
            //        PoisonTexture.Dispose();

            //    PoisonTexture = null;
            //}


            //if (_LightTexture != null)
            //{
            //    if (!_LightTexture.Disposed)
            //        _LightTexture.Dispose();

            //    _LightTexture = null;
            //}


            //if (_LightSurface != null)
            //{
            //    if (!_LightSurface.Disposed)
            //        _LightSurface.Dispose();

            //    _LightSurface = null;
            //}


            for (int i = ControlList.Count - 1; i >= 0; i--)
                ControlList[i].DisposeTexture();

            for (int i = TextureList.Count - 1; i >= 0; i--)
                TextureList[i].DisposeTexture();
        }

        /// <summary>
        /// 资源销毁
        /// </summary>
        public static void MemoryClear()
        {
            for (int i = ControlList.Count - 1; i >= 0; i--)
            {
                if (CEnvir.Now < ControlList[i].ExpireTime) continue;

                ControlList[i].DisposeTexture();
            }

            for (int i = TextureList.Count - 1; i >= 0; i--)
            {
                if (CEnvir.Now < TextureList[i].ExpireTime) continue;

                TextureList[i].DisposeTexture();
            }

        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="opacity"></param>
        public static void SetOpacity(float opacity)
        {
            if (Opacity == opacity)
                return;

            Sprite.Flush();

            Device.SetRenderState(RenderState.AlphaBlendEnable, true);

            if (opacity >= 1 || opacity < 0)
            {
                Device.SetRenderState(RenderState.SourceBlend, Blend.BlendFactor);
                Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
                Device.SetRenderState(RenderState.SourceBlendAlpha, Blend.One);
                Device.SetRenderState(RenderState.BlendFactor, System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb());
            }
            else
            {
                Device.SetRenderState(RenderState.SourceBlend, Blend.BlendFactor);
                Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseBlendFactor);
                Device.SetRenderState(RenderState.SourceBlendAlpha, Blend.SourceAlpha);
                Device.SetRenderState(RenderState.BlendFactor, System.Drawing.Color.FromArgb((byte)(255 * opacity), (byte)(255 * opacity),
                    (byte)(255 * opacity), (byte)(255 * opacity)).ToArgb());
            }

            Opacity = opacity;
            Sprite.Flush();
        }



        public static void SetSurface(Surface surface)
        {
            if (CurrentSurface == surface) return;

            Sprite.Flush();
            CurrentSurface = surface;
            Device.SetRenderTarget(0, surface);
        }


        public static void SetBlend(bool value, float rate = 1F, BlendMode mode = BlendMode.NORMAL)
        {
            if (Blending == value && BlendRate == rate && BlendMode == mode) return;

            Blending = value;
            BlendRate = rate;
            BlendMode = mode;

            Sprite.Flush();
            Sprite.End();

            if (Blending)
            {
                Sprite.Begin(SpriteFlags.DoNotSaveState);
                Device.SetRenderState(RenderState.AlphaBlendEnable, true);
                Device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.Modulate);
                Device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);

                switch (BlendMode)
                {
                    case BlendMode.INVLIGHT:
                        Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
                        Device.SetRenderState(RenderState.SourceBlend, Blend.BlendFactor);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceColor);
                        break;
                    case BlendMode.COLORFY:
                        Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                        break;
                    case BlendMode.MASK:
                        Device.SetRenderState(RenderState.SourceBlend, Blend.Zero);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
                        break;
                    case BlendMode.EFFECTMASK:
                        Device.SetRenderState(RenderState.SourceBlend, Blend.DestinationAlpha);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                        break;
                    case BlendMode.HIGHLIGHT:
                        Device.SetRenderState(RenderState.SourceBlend, Blend.BlendFactor);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                        break;
                    default:
                        Device.SetRenderState(RenderState.SourceBlend, Blend.InverseDestinationColor);
                        Device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                        break;
                }

                Device.SetRenderState(RenderState.BlendFactor, System.Drawing.Color.FromArgb((byte)(255 * rate), (byte)(255 * rate), (byte)(255 * rate), (byte)(255 * rate)).ToArgb());
            }
            else
            {
                Sprite.Begin(SpriteFlags.AlphaBlend);
            }

            Device.SetRenderTarget(0, CurrentSurface);
        }
    }
}
