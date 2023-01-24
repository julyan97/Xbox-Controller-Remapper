using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.DTOs
{
    public class LeftStickDTO
    {
        public double StaticArea { get; set; }

        public double ForwardDown { get; set; }
        public double LeftRight { get; set; }
        public int DeadZone { get; set; }

        public int MaxValController { get; set; }
        public int ThreshHoldAreaCal { get; set; }

        public VirtualKeyCode Up { get; set; }
        public VirtualKeyCode Down { get; set; }
        public VirtualKeyCode Left { get; set; }
        public VirtualKeyCode Right { get; set; }
    }
}
