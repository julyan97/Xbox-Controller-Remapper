using ControllerRebinder.Core.Controlls;
using ControllerRebinder.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Core.Caches
{
    public static class BindingsCache
    {

        public static Dictionary<(Quadrant, Zone), List<VirtualKeyCode>> LeftStickBindings { get; set; }

        public static void Init()
        {
            LeftStickBindings = new Dictionary<(Quadrant, Zone), List<VirtualKeyCode>>();

            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Upper), new List<VirtualKeyCode>());
        }

        private static void LeftStickBinding()
        {
            LeftStickBindings = new Dictionary<(Quadrant, Zone), List<VirtualKeyCode>>();

            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Middle), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopLeft, Zone.Lower), new List<VirtualKeyCode>());


            LeftStickBindings.Add((Quadrant.TopRight, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopRight, Zone.Middle), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.TopRight, Zone.Lower), new List<VirtualKeyCode>());


            LeftStickBindings.Add((Quadrant.BottomLeft, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.BottomLeft, Zone.Middle), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.BottomLeft, Zone.Lower), new List<VirtualKeyCode>());


            LeftStickBindings.Add((Quadrant.BottomRight, Zone.Upper), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.BottomRight, Zone.Middle), new List<VirtualKeyCode>());
            LeftStickBindings.Add((Quadrant.BottomRight, Zone.Lower), new List<VirtualKeyCode>());

        } 
    }
}
