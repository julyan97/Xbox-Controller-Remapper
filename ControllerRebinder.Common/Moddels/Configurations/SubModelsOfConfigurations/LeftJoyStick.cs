using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class LeftJoyStick
    {
        public static VirtualKeyCode Up { get; set; } = VirtualKeyCode.VK_W;
        public static VirtualKeyCode Down { get; set; } = VirtualKeyCode.VK_S;
        public static VirtualKeyCode Left { get; set; } = VirtualKeyCode.VK_A;
        public static VirtualKeyCode Right { get; set; } = VirtualKeyCode.VK_D;

        public double StaticArea { get; set; }

        public  double ForwardDown { get; set; }
        public  double LeftRight { get; set; }
        public  int DeadZone { get; set; }

        public  int MaxValController { get; set; }
        public int ThreshHoldAreaCal { get; set; }
    }
}
