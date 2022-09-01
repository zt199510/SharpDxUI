

using SharpDX.Direct3D9;
using SharpdxControl.Envir;
using SharpdxControl.SharpDXs;

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
        public virtual void OnBackColourChanged(System.Drawing.Color oValue, Color nValue)
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
        protected virtual void CreateTexture()
        {
            if (ControlTexture == null || DisplayArea.Size != TextureSize)
            {
                DisposeTexture();
                TextureSize = DisplayArea.Size;
                ControlTexture = new Texture(DXManager.Device, TextureSize.Width, TextureSize.Height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
                ControlSurface = ControlTexture.GetSurfaceLevel(0);
                DXManager.ControlList.Add(this);
            }

            Surface previous = DXManager.CurrentSurface;
            DXManager.SetSurface(ControlSurface);

            DXManager.Device.Clear(ClearFlags.Target, BackColour.ToRawColorBGRA(), 0, 0);

            OnClearTexture();

            DXManager.SetSurface(previous);
            TextureValid = true;
            ExpireTime = CEnvir.Now + Config.CacheDuration;
        }
        protected virtual void OnClearTexture()
        {
        }
        #endregion


        #region ForeColour文本颜色

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

        #region Enabled



        public bool Enabled
        {
            get => _Enabled;
            set
            {
                if (_Enabled == value) return;

                bool oldValue = _Enabled;
                _Enabled = value;

                OnEnabledChanged(oldValue, value);
            }
        }
        private bool _Enabled;
        public event EventHandler<EventArgs> EnabledChanged;
        public virtual void OnEnabledChanged(bool oValue, bool nValue)
        {
            CheckIsEnabled();
            EnabledChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Location坐标位置

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

        #region IsVisible是否隐藏

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

        #region IsEnabled

        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                if (_IsEnabled == value) return;

                bool oldValue = _IsEnabled;
                _IsEnabled = value;

                OnIsEnabledChanged(oldValue, value);
            }
        }
        private bool _IsEnabled;
        public event EventHandler<EventArgs> IsEnabledChanged;
        public virtual void OnIsEnabledChanged(bool oValue, bool nValue)
        {
            foreach (DXControl control in Controls)
                control.CheckIsEnabled();

            IsEnabledChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Opacity透明度

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

        #region DisplayArea

        public Rectangle DisplayArea
        {
            get => _DisplayArea;
            set
            {
                if (_DisplayArea == value) return;

                Rectangle oldValue = _DisplayArea;
                _DisplayArea = value;

                OnDisplayAreaChanged(oldValue, value);
            }
        }
        private Rectangle _DisplayArea;
        public event EventHandler<EventArgs> DisplayAreaChanged;
        public virtual void OnDisplayAreaChanged(Rectangle oValue, Rectangle nValue)
        {
            if (Controls == null) return;
            foreach (DXControl control in Controls)
                control.UpdateDisplayArea();

            UpdateBorderInformation();
            DisplayAreaChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion


        #region Parent父类

        public DXControl Parent
        {
            get => _Parent;
            set
            {
                if (_Parent == value) return;

                DXControl oldValue = _Parent;
                _Parent = value;

                OnParentChanged(oldValue, value);
            }
        }
        private DXControl _Parent;
        public event EventHandler<EventArgs> ParentChanged;
        public virtual void OnParentChanged(DXControl oValue, DXControl nValue)
        {
            oValue?.Controls.Remove(this);
            Parent?.Controls.Add(this);

            CheckIsVisible();
            CheckIsEnabled();

            UpdateDisplayArea();

            ParentChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region IsMoving

        public bool IsMoving
        {
            get => _IsMoving;
            set
            {
                if (_IsMoving == value) return;

                bool oldValue = _IsMoving;
                _IsMoving = value;

                OnIsMovingChanged(oldValue, value);
            }
        }
        private bool _IsMoving;
        public event EventHandler<EventArgs> IsMovingChanged;
        public virtual void OnIsMovingChanged(bool oValue, bool nValue)
        {
            if (IsMoving)
                CEnvir.Target.SuspendLayout();
            else
                CEnvir.Target.ResumeLayout();

            IsMovingChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region AllowDragOut

        public bool AllowDragOut
        {
            get => _AllowDragOut;
            set
            {
                if (_AllowDragOut == value) return;

                bool oldValue = _AllowDragOut;
                _AllowDragOut = value;

                OnAllowDragOutChanged(oldValue, value);
            }
        }
        private bool _AllowDragOut;
        public event EventHandler<EventArgs> AllowDragOutChanged;
        public virtual void OnAllowDragOutChanged(bool oValue, bool nValue)
        {
            AllowDragOutChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #endregion

        public Action ProcessAction;
        public DXControl()
        {
            BackColour = Color.Empty;
            Enabled = true;
            Opacity = 1F;
            ForeColour = Color.White;
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
            
        }

        private void UpdateDisplayArea()
        {
            Rectangle area = new Rectangle(Location, Size);
            if (Parent != null)
                area.Offset(Parent.DisplayArea.Location);

            DisplayArea = area;
        }

        protected internal virtual void CheckIsVisible()
        {
           
        }
        public virtual void DisposeTexture()
        {
            TextureValid = false;
            DXManager.ControlList.Remove(this);
        }
        protected internal virtual void CheckIsEnabled()
        {
            try
            {
                IsEnabled = Enabled && (Parent == null || Parent.IsEnabled);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Drawing
        public virtual void Draw()
        {
            DrawControl();
            DrawChildControls();
        }
        protected virtual void DrawChildControls()
        {
            foreach (DXControl control in Controls)
                control.Draw();
        }
        protected  virtual void DrawControl()
        {
            if (!DrawTexture) return;
            if (!TextureValid)
            {
                  CreateTexture();

                if (!TextureValid) return;
            }


            float oldOpacity = 1;

            DXManager.SetOpacity(Opacity);

            PresentTexture(ControlTexture, Parent, DisplayArea, IsEnabled ? Color.White : Color.FromArgb(75, 75, 75), this);

            DXManager.SetOpacity(oldOpacity);

            ExpireTime = CEnvir.Now + TimeSpan.FromMinutes(30);
        }

        public static void PresentTexture(Texture texture, DXControl parent, Rectangle displayArea, Color colour, DXControl control, int offX = 0, int offY = 0, bool blend = false, float blendrate = 1f)
        {
            Rectangle bounds = ActiveScene.DisplayArea;
            Rectangle textureArea = Rectangle.Intersect(bounds, displayArea);

            if (!control.IsMoving || !control.AllowDragOut)
                while (parent != null)
                {
                    if (parent.IsMoving && parent.AllowDragOut)
                    {
                        bounds = ActiveScene.DisplayArea;
                        textureArea = Rectangle.Intersect(bounds, displayArea);
                        break;
                    }

                    bounds = parent.DisplayArea;
                    textureArea = Rectangle.Intersect(bounds, textureArea);

                    if (bounds.IntersectsWith(displayArea))
                    {
                        parent = parent.Parent;
                        continue;
                    }

                    return;
                }

            if (textureArea.IsEmpty) return;

            textureArea.Location = new Point(textureArea.X - displayArea.X, textureArea.Y - displayArea.Y);

            DXManager.Sprite.Draw(texture, textureArea, SharpDX.Vector3.Zero, new SharpDX.Vector3(displayArea.X + textureArea.Location.X + offX, displayArea.Y + textureArea.Location.Y + offY, 0), colour);
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