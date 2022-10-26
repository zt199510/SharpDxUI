using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Enums
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/26 9:44:57 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public enum BlendMode : sbyte
    {
        NONE = -1,
        NORMAL = 0,
        LIGHT = 1,
        LIGHTINV = 2,
        INVNORMAL = 3,
        INVLIGHT = 4,
        INVLIGHTINV = 5,
        INVCOLOR = 6,
        INVBACKGROUND = 7,
        COLORFY = 8,
        MASK = 9,
        HIGHLIGHT = 10,
        EFFECTMASK = 11
    }
}
