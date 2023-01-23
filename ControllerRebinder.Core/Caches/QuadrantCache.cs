using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;
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
        private static Configurations Configurations { get; set; }
        public static Dictionary<Quadrant, List<ZoneRange>> Quadrants { get; set; }
        

        /// <summary>
        /// The Init method caches hte information about eache Quadrant and its zones att the start of the application
        /// so we can move between them. Each Quadrant has the Zones and eaches zone needs to be calculated diffrently 
        /// thats why we use the ZoneCalctulationType enum to indicate the calculation type.
        /// </summary>
        public static void Init()
        {
            Configurations = ConfigCache.Configurations;

            Quadrants = new Dictionary<Quadrant, List<ZoneRange>>();

            Quadrants.Add(Quadrant.TopLeft, new List<ZoneRange>());
            Quadrants.Add(Quadrant.TopRight, new List<ZoneRange>());
            Quadrants.Add(Quadrant.BottomLeft, new List<ZoneRange>());
            Quadrants.Add(Quadrant.BottomRight, new List<ZoneRange>());

            //++
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.ForwardDown, ZoneCalctulationType.Positive, Zone.Upper, new List<VirtualKeyCode>{LeftStckBindings.Up}));
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Between,Zone.Middle, new List<VirtualKeyCode> {LeftStckBindings.Up,LeftStckBindings.Right }));
            Quadrants[Quadrant.TopRight].Add(new ZoneRange(Configurations.LeftJoyStick.LeftRight, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Negative, Zone.Lower, new List<VirtualKeyCode> {LeftStckBindings.Right }));

            //-+
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.ForwardDown, ZoneCalctulationType.Positive,Zone.Upper, new List<VirtualKeyCode> { LeftStckBindings.Up }));
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Up, LeftStckBindings.Left }));
            Quadrants[Quadrant.TopLeft].Add(new ZoneRange(Configurations.LeftJoyStick.LeftRight, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Negative,Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Left }));

            //--
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(Configurations.LeftJoyStick.LeftRight, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Left }));
            Quadrants[Quadrant.BottomLeft].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down}));

            //+-
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.ForwardDown, ZoneCalctulationType.Positive, Zone.Lower, new List<VirtualKeyCode> { LeftStckBindings.Down }));
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(Configurations.LeftJoyStick.ForwardDown, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Between, Zone.Middle, new List<VirtualKeyCode> { LeftStckBindings.Down, LeftStckBindings.Right }));
            Quadrants[Quadrant.BottomRight].Add(new ZoneRange(Configurations.LeftJoyStick.LeftRight, Configurations.LeftJoyStick.LeftRight, ZoneCalctulationType.Negative, Zone.Upper, new List<VirtualKeyCode> {  LeftStckBindings.Right }));

        }



    }
}
