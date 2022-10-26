using SharpdxControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Controls
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/26 9:40:52 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class DXImageControl : DXControl
    {
        #region Properties属性

        #region Blend工具
        public bool Blend
        {
            get => _Blend;
            set
            {
                if (_Blend == value) return;

                bool oldValue = _Blend;
                _Blend = value;

                OnBlendChanged(oldValue, value);
            }
        }
        private bool _Blend;
        public event EventHandler<EventArgs> BlendChanged;
        public virtual void OnBlendChanged(bool oValue, bool nValue)
        {
            BlendChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region BlendMode效果类型

        public BlendMode BlendMode
        {
            get => _BlendMode;
            set
            {
                if (_BlendMode == value) return;

                BlendMode oldValue = _BlendMode;
                _BlendMode = value;

                OnBlendModeChanged(oldValue, value);
            }
        }
        private BlendMode _BlendMode = BlendMode.NORMAL;
        public event EventHandler<EventArgs> BlendModeChanged;
        public virtual void OnBlendModeChanged(BlendMode oValue, BlendMode nValue)
        {
            BlendModeChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region DrawImage绘制图片
        public bool DrawImage
        {
            get => _DrawImage;
            set
            {
                if (_DrawImage == value) return;

                bool oldValue = _DrawImage;
                _DrawImage = value;

                OnDrawImageChanged(oldValue, value);
            }
        }
        private bool _DrawImage;
        public event EventHandler<EventArgs> DrawImageChanged;
        public virtual void OnDrawImageChanged(bool oValue, bool nValue)
        {
            DrawImageChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region FixedSize像素大小
        public bool FixedSize
        {
            get => _FixedSize;
            set
            {
                if (_FixedSize == value) return;

                bool oldValue = _FixedSize;
                _FixedSize = value;

                OnFixedSizeChanged(oldValue, value);
            }
        }
        private bool _FixedSize;
        public event EventHandler<EventArgs> FixedSizeChanged;
        public virtual void OnFixedSizeChanged(bool oValue, bool nValue)
        {
            TextureValid = false;
            UpdateDisplayArea();
            FixedSizeChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Scale比例

        public float Scale
        {
            get => _Scale;
            set
            {
                if (_Scale == value) return;

                float oldValue = _Scale;
                _Scale = value;

                OnScaleChanged(oldValue, value);
            }
        }
        private float _Scale = 1.0f;
        public event EventHandler<EventArgs> ScaleChanged;
        public virtual void OnScaleChanged(float oValue, float nValue)
        {
            TextureValid = false;
            ScaleChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ImageOpacity图片透明度
        public float ImageOpacity
        {
            get => _ImageOpacity;
            set
            {
                if (_ImageOpacity == value) return;

                float oldValue = _ImageOpacity;
                _ImageOpacity = value;

                OnImageOpacityChanged(oldValue, value);
            }
        }
        private float _ImageOpacity;
        public event EventHandler<EventArgs> ImageOpacityChanged;
        public virtual void OnImageOpacityChanged(float oValue, float nValue)
        {
            ImageOpacityChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Index图片编号
        public int Index
        {
            get => _Index;
            set
            {
                if (_Index == value) return;

                int oldValue = _Index;
                _Index = value;

                OnIndexChanged(oldValue, value);
            }
        }
        private int _Index;
        public event EventHandler<EventArgs> IndexChanged;
        public virtual void OnIndexChanged(int oValue, int nValue)
        {
            TextureValid = false;
            UpdateDisplayArea();
            IndexChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #endregion
    }
}
