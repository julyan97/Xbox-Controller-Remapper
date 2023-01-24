using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;
using ControllerRebinder.Core.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Helpers
{
    public static class CircleHelper
    {
        private static Configurations _configuration = ConfigCache.Configurations;

        public static bool isInDeadZone(int leftStickX, int leftStickY)
        {
            return Math.Abs(leftStickX) <= _configuration.LeftJoyStick.DeadZone && Math.Abs(leftStickY) <= _configuration.LeftJoyStick.DeadZone;
        }

        public static List<ZoneRange> InitCurrentZonezForQuadrant(double currentXArea, Quadrant _currentQuadrant, ZoneRange _prevZone, ZoneRange _currentZone, ref bool InitZones)
        {
            List<ZoneRange> zones = QuadrantCache.Quadrants[_currentQuadrant];
            if(InitZones)
            {
                var temp = QuadrantHelper.WhereAmI(zones, currentXArea);
                _prevZone = temp;
                _currentZone = temp;
                InitZones = false;
            }

            return zones;
        }

        /// <summary>
        /// Keep track of the previous and current zone and if it changes _didZoneChange will become true 
        /// </summary>
        public static void DetectZoneChange(double currentXArea, List<ZoneRange> zones, ZoneRange _currentZone, ZoneRange _prevZone, ref bool _didZoneChange)
        {
            var tempZone = QuadrantHelper.WhereAmI(zones, currentXArea);
            if(_currentZone != null && tempZone != _currentZone)
            {
                _prevZone = _currentZone;
                _currentZone = tempZone;
                _didZoneChange = true;
            }
        }

        /// <summary>
        /// Keep track of the previous and current zone and if it changes _didQuadrantChange will become true 
        /// </summary>
        public static void DetectQuadrantChange(int leftStickX, int leftStickY, Quadrant _currentQuadrant, Quadrant _prevQuadrant, ref bool _didQuadrantChange)
        {
            var tempQuadrant = QuadrantHelper.WhereAmI(leftStickX, leftStickY);
            if(tempQuadrant != _currentQuadrant)
            {
                _prevQuadrant = _currentQuadrant;
                _currentQuadrant = tempQuadrant;
                _didQuadrantChange = true;
            }
        }

        public static void FindArea(int threshold, int leftStickX, int leftStickY, out double angle, out double area)
        {
            angle = Math.Atan2(leftStickY, leftStickX);
            angle = angle * (180 / Math.PI);
            area = (angle / 360) * Math.PI * Math.Pow(threshold, 2);
        }
    }
}
