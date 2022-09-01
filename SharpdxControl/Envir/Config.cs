using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpdxControl.Envir
{
    public static class Config
    {
        public static TimeSpan TimeOutDuration = TimeSpan.FromSeconds(15);

        public static TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(30);
    }
}
