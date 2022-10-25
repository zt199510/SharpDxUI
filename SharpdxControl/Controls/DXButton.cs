
using SharpDX.Direct3D9;
using SharpdxControl.Envir;
using SharpdxControl.SharpDXs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharpdxControl.Controls
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class DXButton : DXControl
    {
        #region Properites属性

        #region HasFocus

        public bool HasFocus
        {
            get => _HasFocus;
            set
            {
                if (_HasFocus == value) return;

                bool oldValue = _HasFocus;
                _HasFocus = value;

                OnHasFocusChanged(oldValue, value);
            }
        }
        private bool _HasFocus;
        public event EventHandler<EventArgs> HasFocusChanged;
        public virtual void OnHasFocusChanged(bool oValue, bool nValue)
        {
            UpdateDisplayArea();
            HasFocusChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Pressed点击

        public bool Pressed
        {
            get => _Pressed;
            set
            {
                if (_Pressed == value) return;

                bool oldValue = _Pressed;
                _Pressed = value;

                OnPressedChanged(oldValue, value);
            }
        }
        private bool _Pressed;
        public event EventHandler<EventArgs> PressedChanged;
        public virtual void OnPressedChanged(bool oValue, bool nValue)
        {
            UpdateForeColour();

            PressedChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region CanBePressed//是否可以点击
        public bool CanBePressed
        {
            get => _CanBePressed;
            set
            {
                if (_CanBePressed == value) return;

                bool oldValue = _CanBePressed;
                _CanBePressed = value;

                OnCanBePressedChanged(oldValue, value);
            }
        }
        private bool _CanBePressed;
        public event EventHandler<EventArgs> CanBePressedChanged;
        public virtual void OnCanBePressedChanged(bool oValue, bool nValue)
        {
            CanBePressedChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region RightAligned右对齐

        public bool RightAligned
        {
            get => _RightAligned;
            set
            {
                if (_RightAligned == value) return;

                bool oldValue = _RightAligned;
                _RightAligned = value;

                OnRightAlignedChanged(oldValue, value);
            }
        }
        private bool _RightAligned;
        public event EventHandler<EventArgs> RightAlignedChanged;
        public virtual void OnRightAlignedChanged(bool oValue, bool nValue)
        {
            RightAlignedChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ButtonType按钮类型

        public ButtonType ButtonType
        {
            get => _ButtonType;
            set
            {
                if (_ButtonType == value) return;

                ButtonType oldValue = _ButtonType;
                _ButtonType = value;

                OnButtonTypeChanged(oldValue, value);
            }
        }
        private ButtonType _ButtonType;
        public event EventHandler<EventArgs> ButtonTypeChanged;
        public virtual void OnButtonTypeChanged(ButtonType oValue, ButtonType nValue)
        {
            if (Label == null) return;
            switch (nValue)
            {
                case ButtonType.SmallButton:
                    Label.Location = new Point(0, -1);
                    break;
                default:
                    Label.Location = new Point(0, 0);
                    break;
            }

            ButtonTypeChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region HoverIndex鼠标悬停使用图片

        public int HoverIndex
        {
            get => _HoverIndex;
            set
            {
                if (_HoverIndex == value) return;

                int oldValue = _HoverIndex;
                _HoverIndex = value;

                OnHoverIndexChanged(oldValue, value);
            }
        }
        private int _HoverIndex;
        public event EventHandler<EventArgs> HoverIndexChanged;
        public virtual void OnHoverIndexChanged(int oValue, int nValue)
        {
            TextureValid = false;
            UpdateDisplayArea();
            HoverIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region PressedIndex点击图片

        public int PressedIndex
        {
            get => _PressedIndex;
            set
            {
                if (_PressedIndex == value) return;

                int oldValue = _PressedIndex;
                _PressedIndex = value;

                OnPressedIndexChanged(oldValue, value);
            }
        }
        private int _PressedIndex;
        public event EventHandler<EventArgs> PressedIndexChanged;
        public virtual void OnPressedIndexChanged(int oValue, int nValue)
        {
            TextureValid = false;
            UpdateDisplayArea();
            PressedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public DXLabel Label { get; private set; }

        #region Override重写属性方法
        public override void OnIsEnabledChanged(bool oValue, bool nValue)
        {
            base.OnIsEnabledChanged(oValue, nValue);

            UpdateForeColour();
            UpdateDisplayArea();
        }
        public override void OnDisplayAreaChanged(Rectangle oValue, Rectangle nValue)
        {
            base.OnDisplayAreaChanged(oValue, nValue);

            if (Label == null) return;

            Label.Size = DisplayArea.Size;
        }
        public override void OnOpacityChanged(float oValue, float nValue)
        {
            base.OnOpacityChanged(oValue, nValue);

            if (Label == null) return;

            Label.Opacity = Opacity;
        }


        #endregion

        #endregion

        public DXButton()
        {
            ForeColour = Color.White;
         //   Sound = SoundIndex.ButtonA;
            CanBePressed = true;
            ForeColour = new SharpDX.Color4(0.85F, 0.85F, 0.85F,0).ToColor();
            Label = new DXLabel
            {
                Location = new Point(0, -1),
                AutoSize = false,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                IsControl = false,
                Parent = this,
            };
        }

        #region methods
        public void UpdateForeColour()
        {
            if (!IsEnabled)
                ForeColour = new SharpDX.Color4(0.2F, 0.2F, 0.2F,0.2F).ToColor();
            else
                ForeColour = MouseControl == this || Pressed ? new SharpDX.Color4(1F, 1F, 1F,1f).ToColor() : new SharpDX.Color4(0.85F, 0.85F, 0.85F,0.85F).ToColor();
        }
      

        #region Overridel
        protected internal override void UpdateDisplayArea()
        {
            Rectangle area = new Rectangle(Location, Size);

            if (Parent != null)
                area.Offset(Parent.DisplayArea.Location);

            if (HasFocus && MouseControl == this && !Pressed && IsEnabled && CanBePressed) area.Y++;

            DisplayArea = area;
        }
        
        public override void OnFocus()
        {
            base.OnFocus();

            HasFocus = true;
        }
        public override void OnLostFocus()
        {
            base.OnFocus();

            HasFocus = false;
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            UpdateForeColour();
            UpdateDisplayArea();
        }
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            UpdateForeColour();
            UpdateDisplayArea();
        }

        #endregion

        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _HasFocus = false;
                _Pressed = false;
                _CanBePressed = false;
                _RightAligned = false;
                _ButtonType = 0;

                _HoverIndex = 0;
                _PressedIndex = 0;

                if (Label != null)
                {
                    if (!Label.IsDisposed)
                        Label.Dispose();

                    Label = null;
                }

                HasFocusChanged = null;
                CanBePressedChanged = null;
                PressedChanged = null;
                RightAlignedChanged = null;
                ButtonTypeChanged = null;
                HoverIndexChanged = null;
                PressedIndexChanged = null;
            }
        }
        #endregion
    }

    public enum ButtonType
    {
        Default,
        SelectedTab,
        DeselectedTab,
        SmallButton
    }
}
