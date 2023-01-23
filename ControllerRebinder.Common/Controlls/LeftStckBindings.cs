using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Core.Controlls
{
    public static class LeftStckBindings
    {
        public static VirtualKeyCode Up { get; set; } = VirtualKeyCode.VK_W;
        public static VirtualKeyCode Down { get; set; } = VirtualKeyCode.VK_S;
        public static VirtualKeyCode Left { get; set; } = VirtualKeyCode.VK_A;
        public static VirtualKeyCode Right { get; set; } = VirtualKeyCode.VK_D;


    }
}
