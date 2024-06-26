﻿using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;
using ControllerRebinder.Core.Caches;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ControllerRebinder.Core.Helpers
{
    public static class CircleHelper
    {
        /// <summary>
        /// Use Distance formula for a circle to mesure if you are in the deadZone
        /// </summary>
        public static bool isInDeadZone(int stickX, int stickY, int deadZone, ILogger logger = null)
        {
            var x = Math.Pow(stickX - 0, 2);
            var y = Math.Pow(stickY - 0, 2);
            var currentPositionInTheCircle = Math.Round(Math.Sqrt(x + y));
            logger?.LogInformation($"Current Position: {currentPositionInTheCircle}");
            return currentPositionInTheCircle <= deadZone;
        }



        public static List<ZoneRange> InitCurrentZonezForQuadrant(double currentXArea, ref Quadrant _currentQuadrant, ref ZoneRange _currentZone, ref ZoneRange _prevZone, ref bool InitZones)
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
        public static void DetectZoneChange(double currentXArea, List<ZoneRange> zones, ref ZoneRange _currentZone, ref ZoneRange _prevZone, ref bool _didZoneChange)
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
        public static void DetectQuadrantChange(int leftStickX, int leftStickY, ref Quadrant _currentQuadrant, ref Quadrant _prevQuadrant, ref bool _didQuadrantChange)
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

        public static void ExtractCurrentArea(int leftStickX, int leftStickY, out double currentXArea, Configurations _configuration)
        {
            CircleHelper.FindArea(_configuration.LeftJoyStick.ThreshHoldAreaCal, Math.Abs(leftStickX), Math.Abs(leftStickY), out double CurrenrtAngle, out currentXArea);
        }

    }
}
