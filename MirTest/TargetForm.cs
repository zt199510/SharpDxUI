using SharpDX.Windows;
using SharpdxControl.Envir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirTest
{
    public partial class TargetForm : RenderForm
    {
        public TargetForm() : base("[ZTUI]")
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
            AutoScaleDimensions = new SizeF(96F, 96F);
            ClientSize = new Size(1024, 768);
            FormBorderStyle =  FormBorderStyle.None ;
        }
    }
}
