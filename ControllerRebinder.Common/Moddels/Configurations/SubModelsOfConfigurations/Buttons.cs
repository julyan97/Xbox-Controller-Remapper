using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class Buttons
    {
        public bool On { get; set; }
        public bool Log { get; set; }

        public VirtualKeyCode X { get; set; }
        public VirtualKeyCode Y { get; set; }
        public VirtualKeyCode A { get; set; }
        public VirtualKeyCode B { get; set; }

        public VirtualKeyCode DPadUp { get; set; }
        public VirtualKeyCode DPadDown { get; set; }
        public VirtualKeyCode DPadLeft { get; set; }
        public VirtualKeyCode DPadRight { get; set; }
    }
}
