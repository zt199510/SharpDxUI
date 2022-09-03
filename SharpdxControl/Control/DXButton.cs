
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;
using SharpdxControl.SharpDXs;

namespace SharpdxControl.Control
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class DXButton : DXControl
    {
        #region 属性
       
        #region CanBePressed

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

        public DXLabel Label { get; private set; }
        #endregion
        public DXButton()
        {
            ForeColour = Color.White;
            CanBePressed = true;
            ForeColour = new SharpDX.Color4(0.85F, 0.85F, 0.85F,1f).ToColor();
            Label = new DXLabel
            {
                AutoSize = true,
                DrawFormat = TextFormatFlags.WordBreak,
                IsControl = false,
                Parent = this,
            };
        }
    }
}
