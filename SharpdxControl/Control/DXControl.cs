


using SharpDX.Direct3D9;
using SharpdxControl.Envir;

namespace SharpdxControl.Control
{
    public class DXControl : IDisposable
    {
        #region 全局变量(static)
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

        public static DXScene ActiveScene //当前激活画质
        {
            get => _ActiveScene;
            set
            {
                if (_ActiveScene == value) return;

                _ActiveScene = value;

                _ActiveScene?.CheckIsVisible();
            }
        }
        private static DXScene _ActiveScene;
        #endregion



        #region Properties(属性)

        protected internal List<DXControl> Controls { get; private set; } = new List<DXControl>();

        #region Size大小

        public virtual Size Size
        {
            get => _Size;
            set
            {
                if (_Size == value) return;

                Size oldValue = _Size;
                _Size = value;

                OnSizeChanged(oldValue, value);
            }
        }
        private Size _Size;

        public event EventHandler<EventArgs> SizeChanged;
        public virtual void OnSizeChanged(Size oValue, Size nValue)
        {
            UpdateDisplayArea();
            UpdateBorderInformation();
            TextureValid = false;

            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region BackColour

        public Color BackColour
        {
            get => _BackColour;
            set
            {
                if (_BackColour == value) return;

                Color oldValue = _BackColour;
                _BackColour = value;

                OnBackColourChanged(oldValue, value);
            }
        }
        private Color _BackColour;
        public event EventHandler<EventArgs> BackColourChanged;
        public virtual void OnBackColourChanged(Color oValue, Color nValue)
        {
            TextureValid = false;
            BackColourChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Text文本

        public string Text
        {
            get => _Text;
            set
            {
                if (_Text == value) return;

                string oldValue = _Text;
                _Text = value;

                OnTextChanged(oldValue, value);
            }
        }
        private string _Text;
        public event EventHandler<EventArgs> TextChanged;
        public virtual void OnTextChanged(string oValue, string nValue)
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region DrawTexture

        public bool DrawTexture
        {
            get => _DrawTexture;
            set
            {
                if (_DrawTexture == value) return;

                bool oldValue = _DrawTexture;
                _DrawTexture = value;

                OnDrawTextureChanged(oldValue, value);
            }
        }
        private bool _DrawTexture;
        public event EventHandler<EventArgs> DrawTextureChanged;
        public virtual void OnDrawTextureChanged(bool oValue, bool nValue)
        {
            TextureValid = false;
            DrawTextureChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Texture文本

        public bool TextureValid { get; set; }//纹理
        public Texture ControlTexture { get; set; }
        public Size TextureSize { get; set; }
        public Surface ControlSurface { get; set; }
        public DateTime ExpireTime { get; protected set; }

        #endregion


        #region ForeColour

        public Color ForeColour
        {
            get => _ForeColour;
            set
            {
                if (_ForeColour == value) return;

                Color oldValue = _ForeColour;
                _ForeColour = value;

                OnForeColourChanged(oldValue, value);
            }
        }
        private Color _ForeColour;
        public event EventHandler<EventArgs> ForeColourChanged;
        public virtual void OnForeColourChanged(Color oValue, Color nValue)
        {
            ForeColourChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region Location本地化

        public Point Location
        {
            get => _Location;
            set
            {
                if (_Location == value) return;

                Point oldValue = _Location;
                _Location = value;

                OnLocationChanged(oldValue, value);
            }
        }
        private Point _Location;
        public event EventHandler<EventArgs> LocationChanged;
        public virtual void OnLocationChanged(Point oValue, Point nValue)
        {
            UpdateDisplayArea();
            LocationChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region IsVisible

        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                if (_IsVisible == value) return;

                bool oldValue = _IsVisible;
                _IsVisible = value;

                OnIsVisibleChanged(oldValue, value);
            }
        }
        private bool _IsVisible;
        public event EventHandler<EventArgs> IsVisibleChanged;
        public virtual void OnIsVisibleChanged(bool oValue, bool nValue)
        {
            if (!IsVisible)
            {
                //if (FocusControl == this)
                //    FocusControl = null;

                //if (MouseControl == this)
                //    MouseControl = null;
            }

            List<DXControl> checks = new List<DXControl>(Controls);

            foreach (DXControl control in checks)
                control.CheckIsVisible();

            IsVisibleChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region Opacity

        public float Opacity
        {
            get => _Opacity;
            set
            {
                if (_Opacity == value) return;

                float oldValue = _Opacity;
                _Opacity = value;

                OnOpacityChanged(oldValue, value);
            }
        }
        private float _Opacity;
        public event EventHandler<EventArgs> OpacityChanged;
        public virtual void OnOpacityChanged(float oValue, float nValue)
        {
            OpacityChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion



        #endregion

        public Action ProcessAction;
        public DXControl()
        {

        }

        #region Methods方法

        public virtual void Process()
        {
            ProcessAction?.Invoke();

            foreach (DXControl control in Controls)
            {
                if (!control.IsVisible) continue;

                control.Process();
            }
        }


        private void UpdateBorderInformation()
        {
            //throw new NotImplementedException();
        }

        private void UpdateDisplayArea()
        {
            //throw new NotImplementedException();
        }



        protected internal virtual void CheckIsVisible()
        {
            //IsVisible = Visible && Parent != null && Parent.IsVisible;
        }
        public virtual void DisposeTexture()
        {
            //if (ControlTexture != null)
            //{
            //    if (!ControlTexture.Disposed)
            //        ControlTexture.Dispose();

            //    ControlTexture = null;
            //}

            //if (ControlSurface != null)
            //{
            //    if (!ControlSurface.Disposed)
            //        ControlSurface.Dispose();

            //    ControlSurface = null;
            //}

            //TextureSize = Size.Empty;
            //ExpireTime = DateTime.MinValue;
            TextureValid = false;

            DXManager.ControlList.Remove(this);
        }


        #region Drawing
        public virtual void Draw()
        {
            DrawControl();
        }

        private void DrawControl()
        {
            if (!DrawTexture) return;
            if (!TextureValid)
            {
                //  CreateTexture();

                if (!TextureValid) return;
            }


            float oldOpacity = 1;

            DXManager.SetOpacity(Opacity);

          //  PresentTexture(ControlTexture, Parent, DisplayArea, IsEnabled ? Color.White : Color.FromArgb(75, 75, 75), this);

            DXManager.SetOpacity(oldOpacity);

            ExpireTime = CEnvir.Now + TimeSpan.FromMinutes(30);
        }

        public static void PresentTexture(Texture texture, DXControl parent, Rectangle displayArea, Color colour, DXControl control, int offX = 0, int offY = 0, bool blend = false, float blendrate = 1f)
        {

        }
        #endregion
        #endregion
        //释放



        public event EventHandler Disposing;
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Dispose(!IsDisposed);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposing?.Invoke(this, EventArgs.Empty);

                IsDisposed = true;
                Disposing = null;



                _BackColour = Color.Empty;
                _DrawTexture = false;
                _Text = null;
                _ForeColour = Color.Empty;
                _Size = Size.Empty;
                SizeChanged = null;
                TextChanged = null;
                DrawTextureChanged = null;
                BackColourChanged = null;
                ForeColourChanged = null;
            }
            ProcessAction = null;
        }

        ~DXControl()
        {
            Dispose(false);
        }
    }
}