using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Control
{
    public class DXLabel : DXControl
    {

        public DXLabel()
        {
            BackColour = Color.Empty;
            DrawTexture = true;
            AutoSize = true;
            Font = new Font("宋体", FontSize(8.5f), FontStyle.Regular);
            ForeColour = Color.White;
            OutlineColour = Color.FromArgb(8, 8, 8);
            Outline = true;
            DrawFormat = TextFormatFlags.ExternalLeading | TextFormatFlags.ExpandTabs | TextFormatFlags.WordBreak;

        }

        #region Properties属性

        #region AutoSize 大小自动变化

        public bool AutoSize
        {
            get => _AutoSize;
            set
            {
                if (_AutoSize == value) return;

                bool oldValue = _AutoSize;
                _AutoSize = value;

                OnAutoSizeChanged(oldValue, value);
            }
        }
        private bool _AutoSize;
        public event EventHandler<EventArgs> AutoSizeChanged;
        public virtual void OnAutoSizeChanged(bool oValue, bool nValue)
        {
            TextureValid = false;
            CreateSize();

            AutoSizeChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region DrawFormat绘制字体格式
        public TextFormatFlags DrawFormat
        {
            get => _DrawFormat;
            set
            {
                if (_DrawFormat == value) return;

                TextFormatFlags oldValue = _DrawFormat;
                _DrawFormat = value;

                OnDrawFormatChanged(oldValue, value);
            }
        }
        private TextFormatFlags _DrawFormat;
        public event EventHandler<EventArgs> DrawFormatChanged;
        public virtual void OnDrawFormatChanged(TextFormatFlags oValue, TextFormatFlags nValue)
        {
            TextureValid = false;

            DrawFormatChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Font字体

        public Font Font
        {
            get => _Font;
            set
            {
                if (_Font == value) return;

                Font oldValue = _Font;
                _Font = value;

                OnFontChanged(oldValue, value);
            }
        }
        private Font _Font;
        public event EventHandler<EventArgs> FontChanged;
        public virtual void OnFontChanged(Font oValue, Font nValue)
        {
            FontChanged?.Invoke(this, EventArgs.Empty);

            TextureValid = false;
            CreateSize();
        }

        #endregion

        #region Outline



        public bool Outline
        {
            get => _Outline;
            set
            {
                if (_Outline == value) return;

                bool oldValue = _Outline;
                _Outline = value;

                OnOutlineChanged(oldValue, value);
            }
        }
        private bool _Outline;
        public event EventHandler<EventArgs> OutlineChanged;
        public virtual void OnOutlineChanged(bool oValue, bool nValue)
        {
            TextureValid = false;
            CreateSize();

            OutlineChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region OutlineColour

        public Color OutlineColour
        {
            get => _OutlineColour;
            set
            {
                if (_OutlineColour == value) return;

                Color oldValue = _OutlineColour;
                _OutlineColour = value;

                OnOutlineColourChanged(oldValue, value);
            }
        }



        private Color _OutlineColour;
        public event EventHandler<EventArgs> OutlineColourChanged;
        public virtual void OnOutlineColourChanged(Color oValue, Color nValue)
        {
            TextureValid = false;

            OutlineColourChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
        #endregion


        #region Methods方法
        //创建自动大小
        private void CreateSize()
        {
            if (!AutoSize) return;
            Size = GetSize(Text, Font, Outline);
        }
        #endregion


        #region 全局静态方法
        //文本拆分
        static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }

        //获取字体大小
        public static Size GetSize(string text, Font font, bool outline)
        {
            //如果空就返回0,0
            if (string.IsNullOrEmpty(text))
                return Size.Empty;

            try
            {
                ///计算字符串文本所占的尺寸
                Size tempSize = TextRenderer.MeasureText(DXManager.Graphics, text, font);
                if (outline && tempSize.Width > 0 && tempSize.Height > 0)
                {
                    tempSize.Width += 2;
                    tempSize.Height += 2;
                }

                int c = 0;
                int g = 0;

                c = SubstringCount(text, "\n");
                //判断是否需要换行
                g = c == 0 ? c : c + 1;

                return new Size(tempSize.Width, tempSize.Height + g * 2);

            }
            catch (Exception)
            {
                return Size.Empty; ;
            }   
        }
        #endregion


        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _AutoSize = false;
                _DrawFormat = TextFormatFlags.Default;
                _Font?.Dispose();
                _Font = null;
                _Outline = false;
                _OutlineColour = Color.Empty;

                AutoSizeChanged = null;
                DrawFormatChanged = null;
                FontChanged = null;
                OutlineChanged = null;
                OutlineColourChanged = null;
            }
        }
        #endregion
    }
}
