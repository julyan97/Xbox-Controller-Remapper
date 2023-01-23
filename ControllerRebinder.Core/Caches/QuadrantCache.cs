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


    public class Range : IEquatable<Range>
    {

        public double Left { get; set; }
        public double Right { get; set; }

        public List<VirtualKeyCode> Buttons { get; set; }
        public Zone Zone { get; set; }
        public ZoneCalctulationType ZoneCalculationType { get; set; }


        public Range(double left, double right, ZoneCalctulationType positive = ZoneCalctulationType.DefaultZoneCalctulationType, Zone zone = Zone.DefaultZone, List<VirtualKeyCode> buttons = null)
        {
            Left = left;
            Right = right;
            ZoneCalculationType = positive;
            Zone = zone;
            Buttons = buttons;
        }


        public bool Equals(Range other)
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

    public static class QuadrantCache
    {
        public static double ForwardDown { get; set; } = 186883225.143226;
        public static double LeftRight { get; set; } = 146883225.143226;


        public static Dictionary<Quadrant, List<Range>> Quadrants { get; set; }

        public static void Init()
        {
            Quadrants = new Dictionary<Quadrant, List<Range>>();

            Quadrants.Add(Quadrant.TopLeft, new List<Range>());
            Quadrants.Add(Quadrant.TopRight, new List<Range>());
            Quadrants.Add(Quadrant.BottomLeft, new List<Range>());
            Quadrants.Add(Quadrant.BottomRight, new List<Range>());

            //++
            Quadrants[Quadrant.TopRight].Add(new Range(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Upper, new List<VirtualKeyCode>{LeftStckBindings.Up}));
            Quadrants[Quadrant.TopRight].Add(new Range(ForwardDown, LeftRight, ZoneCalctulationType.Between,Zone.Middle, new List<VirtualKeyCode> {LeftStckBindings.Up,LeftStckBindings.Right }));
            Quadrants[Quadrant.TopRight].Add(new Range(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Lower, new List<VirtualKeyCode> {LeftStckBindings.Right }));

            //-+
            Quadrants[Quadrant.TopLeft].Add(new Range(ForwardDown, ForwardDown, ZoneCalctulationType.Positive,Zone.Upper, new List<VirtualKeyCode> { LeftStckBindings.Up }));
            Quadrants[Quadrant.TopLeft].Add(new Range(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Up, LeftStckBindings.Left }));
            Quadrants[Quadrant.TopLeft].Add(new Range(LeftRight, LeftRight, ZoneCalctulationType.Negative,Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Left }));

            //--
            Quadrants[Quadrant.BottomLeft].Add(new Range(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new Range(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new Range(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down}));

            //+-
            Quadrants[Quadrant.BottomRight].Add(new Range(ForwardDown, ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down }));
            Quadrants[Quadrant.BottomRight].Add(new Range(ForwardDown, LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Right }));
            Quadrants[Quadrant.BottomRight].Add(new Range(LeftRight, LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Right }));

        }



    }
}
