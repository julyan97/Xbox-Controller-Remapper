using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Caches
{
    public static class ConfigCache
    {
        public static double ForwardDown { get; set; } = 186883225.143226;
        public static double LeftRight { get; set; } = 146883225.143226;
        public static int DeadZone { get; set; } = 21_815;
        public static int MaxValController { get; set; }  = 32_767; 
    }
}
