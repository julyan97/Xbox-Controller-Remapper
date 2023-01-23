using ControllerRebinder.Common.Moddels;
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

    public static class QuadrantCache
    {
        public static double ForwardDown { get; set; } = 186883225.143226;
        public static double LeftRight { get; set; } = 146883225.143226;


        public static Dictionary<Quadrant, List<ZoneRange>> Quadrants { get; set; }

        public static void Init()
        {
            Quadrants = new Dictionary<Quadrant, List<ZoneRange>>();

            Quadrants.Add(Quadrant.TopLeft, new List<ZoneRange>());
            Quadrants.Add(Quadrant.TopRight, new List<ZoneRange>());
            Quadrants.Add(Quadrant.BottomLeft, new List<ZoneRange>());
            Quadrants.Add(Quadrant.BottomRight, new List<ZoneRange>());

            //++
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Upper, new List<VirtualKeyCode>{LeftStckBindings.Up}));
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(ForwardDown, LeftRight, ZoneCalctulationType.Between,Zone.Middle, new List<VirtualKeyCode> {LeftStckBindings.Up,LeftStckBindings.Right }));
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Lower, new List<VirtualKeyCode> {LeftStckBindings.Right }));

            //-+
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(ForwardDown, ForwardDown, ZoneCalctulationType.Positive,Zone.Upper, new List<VirtualKeyCode> { LeftStckBindings.Up }));
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Up, LeftStckBindings.Left }));
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(LeftRight, LeftRight, ZoneCalctulationType.Negative,Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Left }));

            //--
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down}));

            //+-
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down }));
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Right }));
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Right }));

        }



    }
}
