using ControllerRebinder.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels
{
    public class ZoneRange : IEquatable<ZoneRange>
    {

        public double Left { get; set; }
        public double Right { get; set; }

        public List<VirtualKeyCode> Buttons { get; set; }
        public Zone Zone { get; set; }
        public ZoneCalctulationType ZoneCalculationType { get; set; }


        public ZoneRange(double left, double right, ZoneCalctulationType positive = ZoneCalctulationType.DefaultZoneCalctulationType, Zone zone = Zone.DefaultZone, List<VirtualKeyCode> buttons = null)
        {
            Left = left;
            Right = right;
            ZoneCalculationType = positive;
            Zone = zone;
            Buttons = buttons;
        }


        public bool Equals(ZoneRange other)
        {
            return this.Left == other.Left && this.Right == other.Right;
        }

        public double BiggerNumber()
        {
            return this.Left > this.Right ? Left : Right;
        }

        public double SmallerNumber()
        {
            return this.Left < this.Right ? Left : Right;
        }
    }

}
