

namespace SharpdxControl.Control
{
    public abstract class DXScene:DXControl //窗口资源
    {
        #region 属性Properties

        public DXControl ClickControl;//点击画面

        public DateTime ClickTime;//点击时间

        public MouseButtons Buttons;

        public sealed override Size Size
        {
            get => base.Size;
            set => base.Size = value;
        }


        //重写加载改变
        public override void OnLocationChanged(Point oValue, Point nValue)
        {
            base.OnLocationChanged(oValue, nValue);
        }



        protected DXScene(Size size)
        {
            DrawTexture = false;

            Size = size;

            DXManager.SetResolution(size);
        }


        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                ClickControl = null;
                ClickTime = DateTime.MinValue;
                Buttons = MouseButtons.None;
            }

        }
        #endregion

    }
}
