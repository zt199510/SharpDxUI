
using Library;
using SharpdxControl.Envir;
using System.Windows.Forms;

namespace SharpdxControl.Control
{
    public abstract class DXScene:DXControl //窗口资源
    {
        #region Properties
        public DXControl ClickControl;
        public DateTime ClickTime;
        public MouseButtons Buttons;

        public sealed override Size Size
        {
            get => base.Size;
            set => base.Size = value;
        }

        public override void OnLocationChanged(Point oValue, Point nValue)
        {
            base.OnLocationChanged(oValue, nValue);

            if (DebugLabel == null || PingLabel == null || AttackModeLabel == null || JJLabel == null || PetModeLabel == null) return;

            DebugLabel.Location = new Point(Location.X + 5, 0);
            PingLabel.Location = new Point(Location.X + 5, 0);
            JJLabel.Location = new Point(Location.X + 5, 0);
            AttackModeLabel.Location = new Point(Location.X + 5, 0);
            PetModeLabel.Location = new Point(Location.X + 5, 0);
            // NowLabel.Location = new Point(Location.X + 5, Location.Y + 5);

        }

        public override void OnIsVisibleChanged(bool oValue, bool nValue)
        {
            base.OnIsVisibleChanged(oValue, nValue);

            if (!IsVisible) return;

            //foreach (DXComboBox box in DXComboBox.ComboBoxes)
            //    box.ListBox.Parent = this;
        }
        #endregion

        protected DXScene(Size size)
        {
            DrawTexture = false;

            Size = size;

            DXManager.SetResolution(size);
        }

        #region Methods

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!IsEnabled) return;

            if (MouseControl != null && MouseControl != this)
                MouseControl.OnMouseDown(e);
            else
                base.OnMouseDown(e);


            //DXControl listbox = MouseControl;

            //while (listbox != null)
            //{
            //    if (listbox is DXListBox) break;

            //    listbox = listbox.Parent;
            //}

            //foreach (DXComboBox box in DXComboBox.ComboBoxes)
            //{
            //    if (box.ListBox != listbox)
            //        box.Showing = false;
            //}
        }
        public override void OnMouseUp(MouseEventArgs e)
        {
            if (!IsEnabled) return;

            if (MouseControl != null && MouseControl != this)
                MouseControl.OnMouseUp(e);
            else
                base.OnMouseUp(e);
        }
        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsEnabled) return;

            //if (FocusControl != null && FocusControl != this && FocusControl is MapControl)
            //    FocusControl.OnMouseMove(e);
            else if (MouseControl != null && MouseControl != this && (MouseControl.IsMoving || MouseControl.IsResizing))
                MouseControl.OnMouseMove(e);
            else
                base.OnMouseMove(e);
        }
        public override void OnMouseClick(MouseEventArgs e)
        {
            if (!IsEnabled) return;

            if (Buttons == e.Button)
            {
                if (ClickTime.AddMilliseconds(SystemInformation.DoubleClickTime) >= Time.Now)
                {
                    OnMouseDoubleClick(e);
                    return;
                }
            }
            else ClickTime = DateTime.MinValue;

            if (MouseControl != null && MouseControl != this)
            {
                if (MouseControl == FocusControl)
                    MouseControl.OnMouseClick(e);
            }
            else
                base.OnMouseClick(e);

            ClickControl = MouseControl;

            ClickTime = CEnvir.Now;
            Buttons = e.Button;
        }
        public override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (!IsEnabled) return;


            if (MouseControl != null && MouseControl != this)
            {
                if (MouseControl == ClickControl)
                {
                    MouseControl.OnMouseDoubleClick(e);
                    ClickTime = DateTime.MinValue;
                }
                else
                {
                    MouseControl.OnMouseClick(e);
                    ClickTime = CEnvir.Now;
                }
            }

            ClickControl = MouseControl;
        }
        public override void OnMouseWheel(MouseEventArgs e)
        {
            if (!IsEnabled) return;

            if (MouseControl != null && MouseControl != this)
                MouseControl.OnMouseWheel(e);
            else
                base.OnMouseWheel(e);
        }


        protected override void OnAfterDraw()
        {
            base.OnAfterDraw();

            //if (!string.IsNullOrEmpty(HintLabel.Text))
            //    HintLabel.Draw();

            //if (GameScene.Game != null)
            //{
            //    DebugLabel.Draw();

            //    if (!string.IsNullOrEmpty(PingLabel.Text))
            //        PingLabel.Draw();

            //    if (!string.IsNullOrEmpty(PetModeLabel.Text))
            //        AttackModeLabel.Draw();

            //    if (!string.IsNullOrEmpty(PetModeLabel.Text))
            //        PetModeLabel.Draw();

            //    if (!string.IsNullOrEmpty(JJLabel.Text))
            //        JJLabel.Draw();
            //}
        }

        protected internal sealed override void CheckIsVisible()
        {
            IsVisible = Visible && ActiveScene == this;

            foreach (DXControl control in Controls)
                control.CheckIsVisible();

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
